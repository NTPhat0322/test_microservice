using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using UserService.API.GRPC;
using UserService.Application.Interfaces;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Repositories;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

//database
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
builder.Services.AddDbContext<UserServiceDbContext>(options => options.UseNpgsql(connectionString));

//health checks
builder.Services.AddHealthChecks();

//grpc
builder.Services.AddGrpc();

//DI
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService.Application.Services.UserService>();


var app = builder.Build();

app.MapHealthChecks("/health");
app.MapGrpcService<UserGrpcService>();

app.Run();
