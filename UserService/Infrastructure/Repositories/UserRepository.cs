using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.Repositories
{
    public class UserRepository(UserServiceDbContext context) : IUserRepository
    {
        public async Task<bool> Add(User user)
        {
            await context.Users.AddAsync(user);
            int change = await context.SaveChangesAsync();
            if(change > 0) return true;
            return false;
        }

        public async Task<List<User>> GetAll() => await context.Users.ToListAsync();

        public async Task<User?> GetByEmail(string email)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetById(Guid id)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
