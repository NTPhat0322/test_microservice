using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Repositories;
using OrderService.Infrastructure.Data;

namespace OrderService.Infrastructure.Repositories
{
    public class GenericRepository<T>(DbSet<T> dbSet) : IGenericRepository<T> where T : class
    {
        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync (Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

    }
}
