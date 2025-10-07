namespace UserService.Application.Helpers
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
            => BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);

        public static bool VerifyPassword(string password, string hashedPassword)
            => BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}
