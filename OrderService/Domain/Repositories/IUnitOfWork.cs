using OrderService.Domain.Entities;

namespace OrderService.Domain.Repositories
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Order> Orders { get; }
        Task<int> Complete();
    }
}
