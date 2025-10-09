using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;
using InventoryService.Infrastructure.Repositories;

namespace InventoryService.Application.Services
{
    public class InventoryService(InventoryRepository inventoryRepository) : IInventoryService
    {
        public async Task<bool> AddInventoryAsync(Inventory inventory)
        {
            return await inventoryRepository.AddInventory(inventory);
        }

        public async Task<Inventory?> GetInventoryByIdAsync(Guid id)
        {
            return await inventoryRepository.GetById(id);
        }

        public async Task<Inventory?> GetInventoryByProductIdAsync(Guid id)
        {
            return await inventoryRepository.GetByProductId(id);
        }

        public async Task<bool> UpdateInventory(Inventory inventory)
        {
            return await inventoryRepository.UpdateInventory(inventory);
        }
    }
}
