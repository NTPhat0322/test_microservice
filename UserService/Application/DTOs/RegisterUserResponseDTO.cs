﻿namespace UserService.Application.DTOs
{
    public class RegisterUserResponseDTO
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
