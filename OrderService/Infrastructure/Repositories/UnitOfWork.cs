using Microsoft.EntityFrameworkCore.Storage;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;
using OrderService.Infrastructure.Data;

namespace OrderService.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderServiceDbContext _context;
        private IDbContextTransaction? _transaction;
        //public IGenericRepository<Order> Orders { get; }
        public IOrderRepository Orders { get; }
        //public UnitOfWork(IGenericRepository<Order> orderRepository, OrderServiceDbContext context)
        //{
        //    Orders = orderRepository;
        //    _context = context;
        //}

        public UnitOfWork(IOrderRepository orderRepository, OrderServiceDbContext context)
        {
            Orders = orderRepository;
            _context = context;
        }

        //public async Task<int> SaveChangesAsync()
        //{
        //    return await _context.SaveChangesAsync();
        //}

        //public async Task BeginTransactionAsync()
        //{
        //    _transaction = await _context.Database.BeginTransactionAsync();
        //}

        //public async Task CommitAsync()
        //{
        //    if(_transaction == null)
        //    {
        //        throw new InvalidOperationException("No transaction started.");
        //    }
        //    await _transaction.CommitAsync();
        //}

        //public async Task RollbackAsync()
        //{
        //    if(_transaction == null)
        //    {
        //        throw new InvalidOperationException("No transaction started.");
        //    }
        //    await _transaction!.RollbackAsync();
        //}

        //public void Dispose()
        //{
        //    _transaction?.Dispose();
        //    _context?.Dispose();
        //}

        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
            }
        }

        public async Task<int> CommitAsync()
        {
            try
            {
                //await AddAuditLogsAsync(performedBy);

                int result = await _context.SaveChangesAsync();

                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }

                return result;
            }
            catch
            {   
                await RollbackAsync();
                throw;
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public async Task RollbackAsync()
        {
            if  (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await DisposeTransactionAsync();
            }
        }

        private async Task DisposeTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose() => _context.Dispose();
        //khi bạn đăng kí UnitOfWork trong DI với scoped
        //thì ASP.NET Core sẽ tự động gọi Dispose() sau khi request kết thúc
        //-> DbContext cũng được dispose theo → kết nối DB đóng lại
    }
}
