namespace UserService.Application.DTOs
{
    public class RegisterUserResponseDTO
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;   
    }
}
