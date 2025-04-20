using DAL;
using Domain;
using Microsoft.AspNetCore.Identity;

public class AuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly TokenService _tokenService;

    public AuthService(
        UserManager<AppUser> userManager,
        TokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResult> Register(RegisterDTO request, string role)
    {
        if (string.IsNullOrEmpty(request.Email))
            throw new CustomException(CustomExceptionType.NotFound, "No users found.");

        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
            throw new CustomException(CustomExceptionType.UserIsAlreadyExists, "User is already exists.");

        if (request.Password.Length < 6)
            throw new CustomException(CustomExceptionType.PasswordLength, "Password length must be at least 6 symbols.");

        var appUser = new AppUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Age = request.Age,
            UserName = request.UserName,
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

}
