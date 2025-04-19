using DAL;
using Domain;
using Microsoft.AspNetCore.Identity;

public class AuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly TokenService _tokenService;

    public AuthService(UserManager<AppUser> userManager, TokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<AppUser> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<AuthResult> Register(RegisterDTO request, string role)
    {
        if (string.IsNullOrEmpty(request.Email))
        {
            throw new ArgumentException("Email cannot be empty.");
        }

        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        if (request.Password.Length < 6)
        {
            throw new ArgumentException("Password must be at least 6 characters long.");
        }

        var appUser = new AppUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Age = request.Age,
            UserName = request.UserName,
            HashedPassword = HashPassword(request.Password),
        };

        var result = await _userManager.CreateAsync(appUser, request.Password);
        if (!result.Succeeded)
        {
            return new AuthResult
            {
                Succeeded = false,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        await _userManager.AddToRoleAsync(appUser, role);

        var token = _tokenService.GenerateJwtToken(appUser);
        return new AuthResult
        {
            Succeeded = true,
            Token = token
        };
    }

    public async Task<string> Validate(LoginDTO loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null || !VerifyPassword(loginDto.Password, user.HashedPassword))
        {
            throw new InvalidOperationException("Invalid credentials");
        }

        return _tokenService.GenerateJwtToken(user);
    }

    public async Task<string> ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        return token;
    }

    public async Task ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException("Failed to reset password");
        }
    }

    private string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

    private bool VerifyPassword(string password, string passwordHash) => BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
