using InventoryService.Domain.Repositories;
using InventoryService.Infrastructure.Data;

namespace InventoryService.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InventoryServiceDbContext context;

        public IInventoryRepository Inventories { get; }
        public UnitOfWork(IInventoryRepository inventoryRepository, InventoryServiceDbContext context)
        {
            Inventories = inventoryRepository;
            this.context = context;
        }
        public async Task<int> Complete()
        {
            return await context.SaveChangesAsync();
        }
    }
}
