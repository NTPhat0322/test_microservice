using Microsoft.EntityFrameworkCore;
using ProductService.Entities;

namespace ProductService.Repositories
{
    public class ProductRepository(DemoMicroContext dbContext)
    {
        public async Task<List<Product>> getAll() =>
            await dbContext.Products.ToListAsync();
        
        public async Task<Product?> getById(Guid id) =>
            await dbContext.Products.SingleOrDefaultAsync(p => p.Id == id);
    }

}
