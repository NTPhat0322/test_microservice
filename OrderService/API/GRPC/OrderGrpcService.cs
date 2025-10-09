using Grpc.Core;
using MassTransit;
using OrderGrpc.Protos;
using OrderService.Application.Interfaces;
using OrderService.Shared.EventContracts;

namespace OrderService.API.GRPC
{
    public class OrderGrpcService(IOrderService orderService, IPublishEndpoint publishEndpoint) : OrderGrpc.Protos.OrderService.OrderServiceBase
    {
        public override async Task<OrderList> GetOrders(EmptyRequest request, ServerCallContext context)
        {
            var orders = await orderService.GetAllOrdersAsync();
            // Map domain orders to OrderDto
            OrderList result = new();
            foreach (var order in orders)
            {
                result.Items.Add(new OrderDto
                {
                    OrderId = order.Id.ToString(),
                    Quantity = order.Quantity,
                    ProductId = order.ProductId.ToString(),
                    PriceCents = (long)(order.TotalPrice)
                });
            }
            return result;
        }
        public override async Task<OrderDto> GetById(OrderIdRequest request, ServerCallContext context)
        {
            Guid orderId = Guid.Parse(request.Id);
            var order = await orderService.GetOrderByIdAsync(orderId);
            OrderDto orderDto = new();
            if (order is not null)
            {
                orderDto.OrderId = order.Id.ToString();
                orderDto.Quantity = order.Quantity;
                orderDto.ProductId = order.ProductId.ToString();
                orderDto.PriceCents = (long)(order.TotalPrice * 100m);
            }
            return orderDto;
        }
        public override async Task<BoolReply> CreateOrder(OrderInfo request, ServerCallContext context)
        {
            var result = await orderService.CreateOrderAsync(Guid.Parse(request.ProductId), request.Quantity);
            if (result)
            {
                //pubish event to message broker
                await publishEndpoint.Publish(new OrderCreatedEvent
                {
                    ProductId = Guid.Parse(request.ProductId),
                    Quantity = request.Quantity
                });
            }
            BoolReply rs = new()
            {
                Success = result
            };
            return rs;
        }
    }
}
