using Grpc.Core;
using ProductService.Entities;
using ProductService.Repositories;
using Shared.Protos;

namespace ProductService.Services
{
    public class ProductGrpcService(IProductRepository productRepository) : Shared.Protos.ProductService.ProductServiceBase
    {
        //private static readonly List<ProductDto> _seed =
        //    [
        //        new() { Id = "p1", Name = "iPhone 15", Price = 999 },
        //        new() { Id = "p2", Name = "Galaxy S24", Price = 899 },
        //        new() { Id = "p3", Name = "Xiaomi 14", Price = 699 },
        //    ];

        //public override Task<ProductList> GetProducts(EmptyRequest request, ServerCallContext context)
        //{
        //    var list = new ProductList();
        //    list.Items.AddRange(_seed);
        //    return Task.FromResult(list);
        //}

        public override async Task<ProductList> GetProducts(EmptyRequest request, ServerCallContext context)
        {
            ProductList result = new();
            List<Entities.Product> list = await productRepository.getAll();
            foreach (var item in list)
            {
                result.Items.Add(new ProductDto
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Price = (double)item.Price
                });
            }
            return result;
        }

        //public override Task<ProductDto> GetById(ProductIdRequest request, ServerCallContext context)
        //{
        //    var found = _seed.FirstOrDefault(x => x.Id == request.Id);
        //    if (found is null) throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
        //    return Task.FromResult(found);
        //}

        public override async Task<ProductDto> GetById(ProductIdRequest request, ServerCallContext context)
        {
            Product found = await productRepository.getById(Guid.Parse(request.Id));
            if (found is null) throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
            ProductDto result = new()
            {
                Id = found.Id.ToString(),
                Name = found.Name,
                Price = (double)found.Price
            };
            return result;
        }
    }
}
