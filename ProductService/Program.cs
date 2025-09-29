//namespace ProductService
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);
//            var app = builder.Build();

//            app.MapGet("/", () => "Hello World!");

//            app.Run();
//        }
//    }
//}

using Microsoft.EntityFrameworkCore;
using ProductService.Entities;
using ProductService.Services;

var builder = WebApplication.CreateBuilder(args);

//===========Database Configuration===========
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

builder.Services.AddDbContext<DemoMicroContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// gRPC
builder.Services.AddGrpc();

// Health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapGrpcService<ProductGrpcService>();
app.MapGet("/", () => "ProductService gRPC up. Use a gRPC client.");
app.MapHealthChecks("/health");

app.Run();

