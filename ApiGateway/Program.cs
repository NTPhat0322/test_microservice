
//namespace ApiGateway
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.

//            builder.Services.AddControllers();
//            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();

//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();

//            app.UseAuthorization();


//            app.MapControllers();

//            app.Run();
//        }
//    }
//}


using Grpc.Net.ClientFactory;
using Polly;
using Polly.Extensions.Http;
using Shared.Protos;


var builder = WebApplication.CreateBuilder(args);

// Swagger cho client
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// gRPC clients via factory + resilience
builder.Services
    .AddGrpcClient<Shared.Protos.ProductService.ProductServiceClient>("ProductService", o =>
    {
        o.Address = new Uri(builder.Configuration["GrpcEndpoints:ProductService"]!);
    })
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
    });
    //.AddPolicyHandler(HttpPolicyExtensions
    //    .HandleTransientHttpError()
    //    .OrResult(msg => (int)msg.StatusCode == 429)
    //    .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(200 * attempt)));

builder.Services.AddControllers();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("/health");

app.MapGet("/api/products", async (IHttpClientFactory httpClientFactory,
                                   GrpcClientFactory grpcFactory) =>
{
    var client = grpcFactory.CreateClient<Shared.Protos.ProductService.ProductServiceClient>("ProductService");
    var reply = await client.GetProductsAsync(new EmptyRequest());
    return Results.Ok(reply.Items);
});

app.MapGet("/api/products/{id}", async (string id, GrpcClientFactory grpcFactory) =>
{
    var client = grpcFactory.CreateClient<Shared.Protos.ProductService.ProductServiceClient>("ProductService");
    var reply = await client.GetByIdAsync(new ProductIdRequest { Id = id });
    return Results.Ok(reply);
});

app.Run();
