using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Domain.Entities;

namespace UserService.Application.Helpers
{
    public static class JwtHelper
    {
        private static readonly string JwtKey = Environment.GetEnvironmentVariable("JWT_KEY")!;
        private static readonly string JwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")!;
        private static readonly string JwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")!;

        public static string CreateToken(User user)
        {
            //what is claims?
            //Claims are pieces of information about the user that are encoded in the JWT token.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };
           
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            //jwt key is at least 512 bits long
            var tokenDescriptor = new JwtSecurityToken(
               issuer: JwtIssuer,
               audience : JwtAudience,
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(double.Parse(Environment.GetEnvironmentVariable("TOKEN_LIFETIME_MINUTES") ?? "60")),
               signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
