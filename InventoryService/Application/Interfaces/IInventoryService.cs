using InventoryService.Domain.Entities;

namespace InventoryService.Application.Interfaces
{
    public interface IInventoryService
    {
        Task<bool> AddInventoryAsync(Inventory inventory);
        Task<Inventory?> GetInventoryByIdAsync(Guid id);
        Task<Inventory?> GetInventoryByProductIdAsync(Guid id);
        Task<bool> UpdateInventory(Inventory inventory);
    }
}
