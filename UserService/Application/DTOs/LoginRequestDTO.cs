using System.ComponentModel.DataAnnotations;

namespace UserService.Application.DTOs
{
    public class LoginRequestDTO
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
