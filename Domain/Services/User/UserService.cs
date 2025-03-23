using DAL;
using Domain.Services.AuthService;
using Domain.Services.User.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Domain.Services.User
{
    public  class UserService
    {
        private readonly OnlineStoreDbContext _dbContext;
        private readonly TokenService _tokenService;
        private readonly string _jwtKey;
        public UserService(OnlineStoreDbContext dbContext, TokenService tokenService, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _jwtKey = configuration["JwtSettings:SecretKey"];
        }

        public async Task<string> FargotPassword( string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if(user == null)
            {
                return "user not found";
            }

            string resetToken = GenerateResetToken(email);

            return resetToken;
        }


        public async Task<bool> ResetPassword(string token, string newPassword)
        {
            var email = ValidateResetToken(token);

            if(email == null)
            {
                return false;
            }


            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return false;
            }


            user.HashedPassword = HashPassword(newPassword);
            await _dbContext.SaveChangesAsync();


            return true;
        }

        private string  GenerateResetToken(string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey)); 
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourapp",
                audience: "yourapp",
                claims: new[] { new Claim(ClaimTypes.Email, email) },
                expires: DateTime.UtcNow.AddMinutes(30), 
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string ValidateResetToken(string token)
        {
           
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtKey); 

                var parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken validatedToken);
                return principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            }
            catch
            {
                return null; 
            }
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
