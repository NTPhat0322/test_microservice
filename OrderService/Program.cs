using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using OrderService.API.GRPC;
using OrderService.Application.Interfaces;
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
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService.Application.Services.OrderService>();

//DB configuration
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
builder.Services.AddDbContext<OrderServiceDbContext>(options =>
               options.UseNpgsql(connectionString));

//Health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapGrpcService<OrderGrpcService>();
app.MapHealthChecks("/health");

app.Run();
