using System.Data.Common;
using UserService.Application.DTOs;
using UserService.Application.Helpers;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;

namespace UserService.Application.Services
{
    public class UserService(IUnitOfWork unitOfWork) : IUserService
    {
        public async Task<List<User>> GetAllUserAsync()
        {
            //return await userRepository.GetAll();
            var repo = unitOfWork.Repository<User>();
            return await repo.GetAllAsync() as List<User> ?? new List<User>();
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await unitOfWork.Repository<User>().GetByIdAsync(id);
        }

        public async Task<LoginResponseDTO?> Login(LoginRequestDTO request)
        {
            await unitOfWork.BeginTransactionAsync();

            var userRepository = (IUserRepository)unitOfWork.Repository<User>();
            //check if email exists
            var user = await userRepository.GetByEmail(request.Email);
            if (user is null)
                return null;
            //check if password is correct
            var isPasswordValid = PasswordHasher.VerifyPassword(request.Password, user.PasswordHash);
            if (!isPasswordValid)
                return null;
            //generate token
            var accessToken = JwtHelper.CreateToken(user);
            var refreshToken = RefreshTokenHelper.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            userRepository.Update(user);

            await unitOfWork.CommitAsync();

            return new LoginResponseDTO() { 
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<RegisterUserResponseDTO> RegisterUserAsync(RegisterUserRequestDTO request)
        {
            await unitOfWork.BeginTransactionAsync();
            var userRepository = (IUserRepository)unitOfWork.Repository<User>();    
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
            var accessToken = JwtHelper.CreateToken(newUser);
            var refreshToken = RefreshTokenHelper.GenerateRefreshToken();
            newUser.RefreshToken = refreshToken;
            newUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await userRepository.AddAsync(newUser);
            var rs = await unitOfWork.CommitAsync() > 0;
            if (rs)
                return new RegisterUserResponseDTO()
                {
                    Email = newUser.Email,
                    UserId = newUser.Id.ToString(),
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            else
            {
                throw new Exception("Storing user to db is failed");
            }
        }
    
    }
}
