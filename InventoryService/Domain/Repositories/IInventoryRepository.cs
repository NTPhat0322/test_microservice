using InventoryService.Domain.Entities;

namespace InventoryService.Domain.Repositories
{
    public interface IInventoryRepository
    {
        Task<bool> AddInventory(Inventory inventory);
        Task<List<Inventory>> GetAll();
        Task<Inventory?> GetByProductId(Guid id);
        Task<Inventory?> GetById(Guid id);
        Task<bool> UpdateInventory(Inventory inventory);
    }
}
