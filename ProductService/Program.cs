using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using ProductService.Entities;
using ProductService.Repositories;
using ProductService.Services;

// cài đặt thư viện DotNetEnv: dotnet add package DotNetEnv
// Tải biến môi trường từ file .env
Env.Load(); // Đọc file .env và nạp vào biến môi trường
var builder = WebApplication.CreateBuilder(args);

//DI
//builder.Services.AddScoped<ProductGrpcService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

//===========Database Configuration===========
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

//builder.Services.AddDbContext<DemoMicroContext>();
builder.Services.AddDbContext<DemoMicroContext>(options =>
               options.UseNpgsql(connectionString));

// gRPC
builder.Services.AddGrpc();

// Health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapGrpcService<ProductGrpcService>();
app.MapGet("/", () => "ProductService gRPC up. Use a gRPC client.");
app.MapHealthChecks("/health");

app.Run();

