using ApiGateway.Middleware;
using DotNetEnv;
using OrderGrpc.Protos;
using Shared.Protos;
using UserGrpc.Protos;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

// Swagger cho client
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// gRPC clients
var productServiceAddress = Environment.GetEnvironmentVariable("GRPCENDPOINTS__PRODUCTSERVICE");
var orderServiceAddress = Environment.GetEnvironmentVariable("GRPCENDPOINTS__ORDERSERVICE");
var userServiceAddress = Environment.GetEnvironmentVariable("GRPCENDPOINTS__USERSERVICE");
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

builder.Services.AddControllers();
builder.Services.AddHealthChecks();

var app = builder.Build();

//pipeline
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
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