using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure.Data;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

//DB configuration
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
builder.Services.AddDbContext<OrderServiceDbContext>(options =>
               options.UseNpgsql(connectionString));


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
