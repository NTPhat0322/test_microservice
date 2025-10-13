using ApiGateway.Middleware;
using DotNetEnv;
using InventoryGrpc.Protos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderGrpc.Protos;
using System.Text;
using UserGrpc.Protos;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

// Swagger cho client
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "API Gateway", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập JWT token vào đây (ví dụ: Bearer eyJhbGciOi...)"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


// gRPC clients
var productServiceAddress = Environment.GetEnvironmentVariable("GRPCENDPOINTS__PRODUCTSERVICE");
var orderServiceAddress = Environment.GetEnvironmentVariable("GRPCENDPOINTS__ORDERSERVICE");
var userServiceAddress = Environment.GetEnvironmentVariable("GRPCENDPOINTS__USERSERVICE");
var inventoryServiceAddress = Environment.GetEnvironmentVariable("GRPCENDPOINTS__INVENTORYSERVICE");
//var productServiceAddress = Environment.GetEnvironmentVariable("GRPCENDPOINTS_PRODUCTSERVICE");
builder.Services
    .AddGrpcClient<Shared.Protos.ProductService.ProductServiceClient>(/*"ProductService",*/ o =>
    {
        //o.Address = new Uri(builder.Configuration["GrpcEndpoints:ProductService"]!);
        o.Address = new Uri(productServiceAddress!);
    });
//.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
//{
//});
//.AddPolicyHandler(HttpPolicyExtensions
//    .HandleTransientHttpError()
//    .OrResult(msg => (int)msg.StatusCode == 429)
//    .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(200 * attempt)));


builder.Services.AddGrpcClient<OrderService.OrderServiceClient>(o => { 
    o.Address = new Uri(orderServiceAddress!);
});
builder.Services.AddGrpcClient<UserService.UserServiceClient>(o =>
{
    o.Address = new Uri(userServiceAddress!);
});
builder.Services.AddGrpcClient<InventoryService.InventoryServiceClient>(o =>
{
    o.Address = new Uri(inventoryServiceAddress!);
});

builder.Services.AddControllers();
builder.Services.AddHealthChecks();

//authentication schema
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();


var app = builder.Build();

//pipeline
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
//pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");
app.MapControllers(); //controller routing
app.Run();




//app.MapGet("/api/products", async (IHttpClientFactory httpClientFactory,
//                                   GrpcClientFactory grpcFactory) =>
//{
//    var client = grpcFactory.CreateClient<Shared.Protos.ProductService.ProductServiceClient>("ProductService");
//    var reply = await client.GetProductsAsync(new EmptyRequest());
//    return Results.Ok(reply.Items);
//});

//app.MapGet("/api/products/{id}", async (string id, GrpcClientFactory grpcFactory) =>
//{
//    var client = grpcFactory.CreateClient<Shared.Protos.ProductService.ProductServiceClient>("ProductService");
//    var reply = await client.GetByIdAsync(new ProductIdRequest { Id = id });
//    return Results.Ok(reply);
//});