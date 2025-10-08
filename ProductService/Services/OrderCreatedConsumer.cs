using MassTransit;
using ProductService.Shared.EventContracts;

namespace ProductService.Services
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var msg = context.Message.Msg;
            Console.WriteLine($"Message nhan tu order service: {msg}");
            await Task.CompletedTask;
        }
    }
}
