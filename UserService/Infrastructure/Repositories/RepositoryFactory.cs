using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly UserServiceDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public RepositoryFactory(UserServiceDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            //if (!_repositories.ContainsKey(typeof(T)))
            //{
            //    var repositoryInstance = new GenericRepository<T>(_context);
            //    _repositories.Add(typeof(T), repositoryInstance);
            //}

            if (!_repositories.ContainsKey(typeof(T)))
            {
                object? repositoryInstance = null;
                switch (typeof(T).Name)
                {
                    case nameof(User):
                        repositoryInstance = new UserRepository(_context);
                        break;
                    //case nameof(Role):
                    //    repositoryInstance = new RoleRepository(_context);
                    //    break;
                    //case nameof(Permission):
                    //    repositoryInstance = new PermissionRepository(_context);
                    //    break;
                    default:
                        repositoryInstance = new GenericRepository<T>(_context.Set<T>());
                        break;
                }
                _repositories.Add(typeof(T), repositoryInstance);
            }

            //_repositories[typeof(T)] --> GenericRepository<T>
            return (IGenericRepository<T>)_repositories[typeof(T)];
        }
    }
}
