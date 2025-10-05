using Grpc.Core;
using ProductService.Entities;
using ProductService.Repositories;
using Shared.Protos;

namespace ProductService.Services
{
    public class ProductGrpcService(IProductRepository productRepository) : Shared.Protos.ProductService.ProductServiceBase
    {

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


        public override async Task<ProductDto> GetById(ProductIdRequest request, ServerCallContext context)
        {
            var found = await productRepository.getById(Guid.Parse(request.Id));
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
