using System.Data.Common;
using UserService.Application.DTOs;
using UserService.Application.Helpers;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;

namespace UserService.Application.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public async Task<List<User>> GetAllUserAsync()
        {
            return await userRepository.GetAll();
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await userRepository.GetById(id);
        }

        public async Task<string> Login(LoginRequestDTO request)
        {
            //check if email exists
            var user = await userRepository.GetByEmail(request.Email);
            if (user is null)
                throw new KeyNotFoundException("User has not been registered yet");
            //check if password is correct
            var isPasswordValid = PasswordHasher.VerifyPassword(request.Password, user.PasswordHash);
            if(!isPasswordValid)
                throw new UnauthorizedAccessException("Password is incorrect");
            //generate token
            var token = "coming soon";
            return token;
        }

        public async Task<RegisterUserResponseDTO> RegisterUserAsync(RegisterUserRequestDTO request)
        {
            //check if email already exists
            var existingUser = await userRepository.GetByEmail(request.Email);
            if (existingUser != null)
            {
                throw new Exception("Email already exists");
            }
            var hasedPassword = PasswordHasher.HashPassword(request.Password);
            var newUser = new User()
            {
                Email = request.Email,
                PasswordHash = hasedPassword,
            };
            var rs = await userRepository.Add(newUser);
            if (rs)
                return new RegisterUserResponseDTO()
                {
                    Email = newUser.Email,
                    UserId = newUser.Id.ToString(),
                    Token = "coming soon"
                };
            else
            {
                throw new Exception("Storing user to db is failed");
            }
        }
    
    }
}
