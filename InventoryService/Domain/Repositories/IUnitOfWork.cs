namespace InventoryService.Domain.Repositories
{
    public interface IUnitOfWork
    {
        public IInventoryRepository Inventories { get; }
        Task<int> Complete();
    }
}
