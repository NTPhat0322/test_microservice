using DotNetEnv;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.API.GRPC;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Repositories;
using Shared.Protos;

//env
Env.Load();
var builder = WebApplication.CreateBuilder(args);

//gRPC
builder.Services.AddGrpc();

var productServiceAddress = Environment.GetEnvironmentVariable("GRPCENDPOINTS__PRODUCTSERVICE");
builder.Services.AddGrpcClient<ProductService.ProductServiceClient>(o =>
{
    o.Address = new Uri(productServiceAddress!);
});

//DI
builder.Services.AddScoped<IOrderService, OrderService.Application.Services.OrderService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
//builder.Services.AddScoped<IGenericRepository<Order>, GenericRepository<Order>>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//DB configuration
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
builder.Services.AddDbContext<OrderServiceDbContext>(options =>
               options.UseNpgsql(connectionString));

//Health checks
builder.Services.AddHealthChecks();

//MassTransit with RabbitMQ
builder.Services.AddMassTransit(busConfigurator => {
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.UsingRabbitMq((context, configurator) => 
    {
        var host = Environment.GetEnvironmentVariable("MESSAGE_BROKER_HOST");
        var vHost = Environment.GetEnvironmentVariable("MESSAGE_BROKER_VHOST");
        var username = Environment.GetEnvironmentVariable("MESSAGE_BROKER_USERNAME");
        var password = Environment.GetEnvironmentVariable("MESSAGE_BROKER_PASSWORD");
        configurator.Host(host, vHost, h => 
        {
            h.Username(username!);
            h.Password(password!);
        });
    });
});

var app = builder.Build();

app.MapGrpcService<OrderGrpcService>();
app.MapHealthChecks("/health");

app.Run();
