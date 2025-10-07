using UserService.Domain.Entities;

namespace UserService.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<User?> GetById(Guid id);
        Task<User?> GetByEmail(string email);
        Task<bool> Add(User user);
    }
}
