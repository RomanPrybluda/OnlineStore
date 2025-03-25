using System;
using System.Threading.Tasks;
using DAL;
using Domain.Services.User.DTO;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Domain.Services.AuthService
{
    public class AuthService
    {
        private readonly OnlineStoreDbContext _context;
        private readonly TokenService _tokenService;

        public AuthService(OnlineStoreDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Регистрирует нового пользователя.
        /// </summary>
        public async Task<string> Register(RegisterDTO registerDto)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists.");
            }

            
            var appUser = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                Age = registerDto.Age,
                HashedPassword = HashPassword(registerDto.Password),
            };

            _context.Users.Add(appUser);
            await _context.SaveChangesAsync();

            return _tokenService.GenerateJwtToken(appUser);
        }

        /// <summary>
        /// Аутентифицирует пользователя.
        /// </summary>
        public async Task<string> Validate(LoginDTO loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || !VerifyPassword(loginDto.Password, user.HashedPassword))
            {
                return null;
            }

            return _tokenService.GenerateJwtToken(user);
        }

        /// <summary>
        /// Генерирует токен для сброса пароля (Forgot Password).
        /// </summary>
        public async Task<string> ForgotPasswordAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return "User not found";

            var token = Guid.NewGuid().ToString(); 
            user.PasswordResetToken = token;
            user.ResetTokenExpires = DateTime.UtcNow.AddMinutes(30);

            await _context.SaveChangesAsync();

            return token; 
        }

        /// <summary>
        /// Сбрасывает пароль (Reset Password).
        /// </summary>
        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || user.PasswordResetToken != token || user.ResetTokenExpires < DateTime.UtcNow)
            {
                return false;
            }

            user.HashedPassword = HashPassword(newPassword);
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _context.SaveChangesAsync();
            return true;
        }


        /// <summary>
        /// Изменяет пароль.
        /// </summary>
        public async Task<bool> ChangePassword(string userId, string oldPassword, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null || !VerifyPassword(oldPassword, user.HashedPassword))
            {
                return false; 
            }

            user.HashedPassword = HashPassword(newPassword); 
            await _context.SaveChangesAsync(); 
            return true;
        }

        private string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        private bool VerifyPassword(string password, string passwordHash) => BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
