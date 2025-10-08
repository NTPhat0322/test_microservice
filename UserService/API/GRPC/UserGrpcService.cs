
using Grpc.Core;
using UserGrpc.Protos;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;

namespace UserService.API.GRPC
{
    public class UserGrpcService(IUserService userService) : UserGrpc.Protos.UserService.UserServiceBase
    {
        public override async Task<UserList> GetUsers(EmptyRequest emptyRequest, ServerCallContext context)
        {
            var users = await userService.GetAllUserAsync();
            var result = new UserList();
            foreach(var user in users)
            {
                result.UserList_.Add(new UserDto()
                {
                    UserId = user.Id.ToString(),
                    Email = user.Email,
                });
            }
            return result;
        }
        public override async Task<UserDto> GetById(UserIdRequest request, ServerCallContext context)
        {
            var user = await userService.GetUserByIdAsync(Guid.Parse(request.Id));
            if(user is null)
            {
                throw new KeyNotFoundException("User not found");
            }
            return new UserDto()
            {
                UserId = user.Id.ToString(),
                Email = user.Email,
            };
        }
        public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            RegisterUserRequestDTO dto = new()
            {
                Email = request.Email,
                Password = request.Password
            };
            var result = await userService.RegisterUserAsync(dto);
            return new CreateUserResponse()
            {
                Email = result.Email,
                UserId = result.UserId,
                Token = result.Token
            };
        }
        public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            LoginRequestDTO dto = new()
            {
                Email = request.Email,
                Password = request.Password
            };
            var token = await userService.Login(dto);
            return new LoginResponse()
            {
                Token = token
            };
        }

    }
}
