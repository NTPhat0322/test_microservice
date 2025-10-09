using DotNetEnv;
using InventoryService.API.EventHandler;
using InventoryService.API.Grpc;
using InventoryService.Application.Interfaces;
using InventoryService.Domain.Repositories;
using InventoryService.Infrastructure.Data;
using InventoryService.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

//db config
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
builder.Services.AddDbContext<InventoryServiceDbContext>(options => {
    options.UseNpgsql(connectionString);
});

//DI
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IInventoryService, InventoryService.Application.Services.InventoryService>();

//mass transit
builder.Services.AddMassTransit(busConfigurator => { 
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<OrderCreatedConsumer>();

    busConfigurator.UsingRabbitMq((context, configurator) => {
        configurator.Host(new Uri(Environment.GetEnvironmentVariable("MESSAGE_BROKER_HOST")!), h => {
            h.Username(Environment.GetEnvironmentVariable("MESSAGE_BROKER_USERNAME")!);
            h.Password(Environment.GetEnvironmentVariable("MESSAGE_BROKER_PASSWORD")!);
        });
        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.AddGrpc();

//Health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapGrpcService<InventoryGrpcService>();

app.MapHealthChecks("/health");

app.Run();
