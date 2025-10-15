using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;
using OrderService.Infrastructure.Data;

namespace OrderService.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderServiceDbContext context;
        public IGenericRepository<Order> Orders { get; }
        public UnitOfWork(IGenericRepository<Order> orderRepository, OrderServiceDbContext context)
        {
            Orders = orderRepository;
            this.context = context;
        }

        public async Task<int> Complete()
        {
            return await context.SaveChangesAsync();
        }
    }
}
