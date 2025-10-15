using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;
using System.Runtime.InteropServices;

namespace InventoryService.Application.Services
{
    public class InventoryService(IUnitOfWork unitOfWork) : IInventoryService
    {
        public async Task<bool> AddInventoryAsync(Inventory inventory)
        {
            await unitOfWork.Inventories.AddInventory(inventory);
            return await unitOfWork.Complete() > 0;
        }

        public async Task<List<Inventory>> GetAllInventoriesAsync()
        {
            return await unitOfWork.Inventories.GetAll();
        }

        public async Task<Inventory?> GetInventoryByIdAsync(Guid id)
        {
            return await unitOfWork.Inventories.GetById(id);
        }

        public async Task<Inventory?> GetInventoryByProductIdAsync(Guid id)
        {
            return await unitOfWork.Inventories.GetByProductId(id);
        }

        public async Task<bool> UpdateInventory(Inventory inventory)
        {
            await unitOfWork.Inventories.UpdateInventory(inventory);
            return await unitOfWork.Complete() > 0;
        }
    }
}
