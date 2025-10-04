using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;
using Shared.Protos;

namespace OrderService.Application.Services
{
    public class OrderService(IOrderRepository orderRepository, ProductService.ProductServiceClient productClient) : IOrderService
    {
        public async Task<bool> CreateOrderAsync(Guid productId, int quantity)
        {
            var product = await productClient.GetByIdAsync(new ProductIdRequest { Id = productId.ToString() });
            if(product is null) return false;
            var order = new Order()
            {
                ProductId = productId,
                Quantity = quantity,
                TotalPrice = Convert.ToDecimal(product.Price) * quantity
            };
            await orderRepository.AddOrderAsync(order);
            return true;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await orderRepository.GetAllOrdersAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            return await orderRepository.GetOrderByIdAsync(id);
        }
    }
}
