using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;
using OrderService.Infrastructure.Data;

namespace OrderService.Infrastructure.Repositories
{
    public class OrderRepository(OrderServiceDbContext dbContext) : IOrderRepository
    {
        public async Task AddOrderAsync(Order order)
        {
            await dbContext.Orders.AddAsync(order);
        }

        public async Task<List<Order>> GetAllOrdersAsync() => await dbContext.Orders.ToListAsync();

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            return await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
