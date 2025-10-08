using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserGrpc.Protos;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(UserService.UserServiceClient userServiceClient) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllUserAsync()
        {
            var result = await userServiceClient.GetUsersAsync(new EmptyRequest());
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] string id)
        {
            var result = await userServiceClient.GetByIdAsync(new UserIdRequest { Id = id });
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest createUserRequest)
        {
            var result = await userServiceClient.CreateUserAsync(createUserRequest);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await userServiceClient.LoginAsync(request);
            return Ok(result);
        }
    }
}
