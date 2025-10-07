using System.ComponentModel.DataAnnotations;

namespace UserService.Application.DTOs
{
    public class RegisterUserRequestDTO
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
