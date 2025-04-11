using DAL;
using Microsoft.AspNetCore.Identity;


namespace Domain.Services.AuthService
{
    public class AuthService
    {
        private readonly OnlineStoreDbContext _context;
        private readonly TokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly AuthService _authService;
        public AuthService(OnlineStoreDbContext context, TokenService tokenService, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<string> Register(RegisterDTO registerDto)
        {
            
            if (string.IsNullOrEmpty(registerDto.Email))
            {
                throw new ArgumentException("Email cannot be empty.");
            }

            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists.");
            }

            var appUser = RegisterDTO.FromRegisterDTO(registerDto);

            
            var result = await _userManager.CreateAsync(appUser, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User creation failed: {errors}");
            }

            
            
            return _tokenService.GenerateJwtToken(appUser);
        }

        public async Task<string> Validate(LoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            return _tokenService.GenerateJwtToken(user);
        }

        public async Task<string> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return "User not found";

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            
            return token;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return false;

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            return result.Succeeded;
        }
    }
}
