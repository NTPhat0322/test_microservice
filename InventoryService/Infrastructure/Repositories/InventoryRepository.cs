using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;
using InventoryService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace InventoryService.Infrastructure.Repositories
{
    public class InventoryRepository(InventoryServiceDbContext context) : IInventoryRepository
    {
        public async Task AddInventory(Inventory inventory)
        {
            await context.AddAsync(inventory);
        }

        public async Task<List<Inventory>> GetAll()
        {
            return await context.Inventories.ToListAsync();
        }

        public async Task<Inventory?> GetById(Guid id)
        {
            return await context.Inventories.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Inventory?> GetByProductId(Guid id)
        {
            return await context.Inventories.FirstOrDefaultAsync(i => i.ProductId == id);
        }

        public Task UpdateInventory(Inventory inventory)
        {
            context.Inventories.Update(inventory);
            return Task.CompletedTask;
        }

    }
}
