using DAL;
using Microsoft.EntityFrameworkCore;

namespace Domain
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

        public async Task<string> Register(RegisterDTO registerDto)
        {
            if (string.IsNullOrEmpty(registerDto.Email))
            {
                throw new ArgumentException("Email cannot be empty.");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists.");
            }

            if (registerDto.Password.Length < 6)
            {
                throw new ArgumentException("Password must be at least 6 characters long.");
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

        public async Task<string> Validate(LoginDTO loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Email))
            {
                throw new ArgumentException("Email cannot be empty.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || !VerifyPassword(loginDto.Password, user.HashedPassword))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            return _tokenService.GenerateJwtToken(user);
        }

        public async Task<string> ForgotPasswordAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email cannot be empty.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return "User not found";

            var token = Guid.NewGuid().ToString();
            user.PasswordResetToken = token;
            user.ResetTokenExpires = DateTime.UtcNow.AddMinutes(30);

            await _context.SaveChangesAsync();

            return token;
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentException("Invalid input data.");
            }

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

        private string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        private bool VerifyPassword(string password, string passwordHash) => BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
