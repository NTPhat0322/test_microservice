using ProductService.Entities;

namespace ProductService.Repositories
{
    public interface IProductRepository
    {
       Task<List<Product>> getAll();
       Task<Product?> getById(Guid id);
    }
}
