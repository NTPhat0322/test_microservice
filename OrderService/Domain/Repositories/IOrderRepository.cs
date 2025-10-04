using OrderService.Domain.Entities;

namespace OrderService.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task<Order?> GetOrderByIdAsync(Guid id);
        Task<List<Order>> GetAllOrdersAsync();
    }
}
