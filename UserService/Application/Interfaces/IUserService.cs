using UserService.Application.DTOs;
using UserService.Domain.Entities;

namespace UserService.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUserAsync();
        Task<User?> GetUserByIdAsync(Guid id);
        Task<RegisterUserResponseDTO> RegisterUserAsync(RegisterUserRequestDTO request);
        Task<string> Login(LoginRequestDTO request);
    }
}
