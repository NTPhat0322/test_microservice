using OrderService.Domain.Entities;

namespace OrderService.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        //public IGenericRepository<Order> Orders { get; }
        public IOrderRepository Orders { get; }
        Task BeginTransactionAsync();
        Task<int> CommitAsync();
        //Task RollbackAsync();
        //Task<int> SaveChangesAsync();
    }
}
