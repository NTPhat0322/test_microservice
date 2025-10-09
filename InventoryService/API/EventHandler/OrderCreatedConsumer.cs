using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;
using InventoryService.Shared.EventContracts;
using MassTransit;

namespace InventoryService.API.EventHandler
{
    public class OrderCreatedConsumer(IInventoryService inventoryService) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var inventory = await inventoryService.GetInventoryByProductIdAsync(context.Message.ProductId);
            if (inventory is null) throw new Exception("Product id is not valid");
            var rs = await inventoryService.UpdateInventory(inventory);
            if (!rs) throw new Exception("Update inventory failed");
        }
    }
}
