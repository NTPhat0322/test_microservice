using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;
using InventoryService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace InventoryService.Infrastructure.Repositories
{
    public class InventoryRepository(InventoryServiceDbContext context) : IInventoryRepository
    {
        public async Task<bool> AddInventory(Inventory inventory)
        {
            await context.AddAsync(inventory);
            int rs = await context.SaveChangesAsync();
            if(rs > 0) return true;
            return false;
        }

        public Task<List<Inventory>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Inventory?> GetById(Guid id)
        {
            return await context.Inventories.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Inventory?> GetByProductId(Guid id)
        {
            return await context.Inventories.FirstOrDefaultAsync(i => i.ProductId == id);
        }

        public Task<bool> UpdateInventory(Inventory inventory)
        {
            throw new NotImplementedException();
        }
    }
}
