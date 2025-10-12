using Contracts;
using InventoryService.Application.Interfaces;
using MassTransit;

namespace InventoryService.API.EventHandler
{
    public class OrderCreatedConsumer(IInventoryService inventoryService) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var inventory = await inventoryService.GetInventoryByProductIdAsync(context.Message.ProductId);
            if (inventory is null) throw new Exception("Product id is not valid");
            inventory.QuantityInStock -= context.Message.Quantity;
            var rs = await inventoryService.UpdateInventory(inventory);
            if (!rs) throw new Exception("Update inventory failed");
        }
    }
}
