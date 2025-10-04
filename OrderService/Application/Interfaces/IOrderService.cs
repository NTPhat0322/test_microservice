using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces
{
    public interface IOrderService
    {
        Task<bool> CreateOrderAsync(Guid productId, int quantity);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(Guid id);
    }
}
