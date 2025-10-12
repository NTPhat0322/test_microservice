using Grpc.Core;
using InventoryGrpc.Protos;
using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;

namespace InventoryService.API.Grpc
{
    public class InventoryGrpcService(IInventoryService inventoryService) : InventoryGrpc.Protos.InventoryService.InventoryServiceBase
    {
        public override async Task<BoolReply> CreateInventory(CreateRequest request, ServerCallContext context)
        {
            var inventory = new Inventory()
            {
                ProductId = Guid.Parse(request.ProductId),
                QuantityInStock = request.QuantityInStock
            };
            var rs = await inventoryService.AddInventoryAsync(inventory);
            return new BoolReply { Success = rs };
        }

        public override async Task<InventoryList> GetAllInventory(EmptyRequest request, ServerCallContext context)
        {
            var inventories = await inventoryService.GetAllInventoriesAsync();
            var inventoryList = new InventoryList();
            foreach (var item in inventories)
            {
                inventoryList.Items.Add(new InventoryDto() { 
                    Id = item.Id.ToString(),
                    ProductId = item.ProductId.ToString(),
                    QuantityInStock = item.QuantityInStock
                });
            }
            return inventoryList;
        }
    }
}
