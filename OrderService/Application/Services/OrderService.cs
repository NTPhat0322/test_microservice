using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;
using Shared.Protos;

namespace OrderService.Application.Services
{
    public class OrderService(ProductService.ProductServiceClient productClient, IUnitOfWork unitOfWork) : IOrderService
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
            //await orderRepository.AddAsync(order);

            await unitOfWork.BeginTransactionAsync();
            await unitOfWork.Orders.AddAsync(order);
            await unitOfWork.CommitAsync();
            
            return true;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            //return await orderRepository.GetAllAsync() as List<Order> ?? new List<Order>();
            return await unitOfWork.Orders.GetAllAsync() as List<Order> ?? new List<Order>();
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            //return await orderRepository.GetByIdAsync(id);
            return await unitOfWork.Orders.GetByIdAsync(id);
        }
    }
}
