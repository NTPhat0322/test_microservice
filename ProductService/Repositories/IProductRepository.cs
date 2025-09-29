using ProductService.Entities;

namespace ProductService.Repositories
{
    public interface IProductRepository
    {
       Task<List<Product>> getAll();

    }
}
