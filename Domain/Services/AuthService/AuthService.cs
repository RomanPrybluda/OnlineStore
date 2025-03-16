using DAL;
using Domain.Services.AuthService.Login.DTO;
using Domain.Services.User.DTO;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services.AuthService;

public class AuthService
{
    private readonly OnlineStoreDbContext _context;
    private readonly TokenService _tokenService;

    public AuthService(OnlineStoreDbContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public string Register(UserDTO user)
    {
        var appuser = new AppUser
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Age = user.Age,
            HashedPassword = HashPassword(user.Password),
            Role = user.Role
        };



        var Token = _tokenService.GenerateJwtToken(appuser);

        _context.Users.Add(appuser);
        _context.SaveChanges();

        return Token;

    }

    public async Task<string> Validate(LoginDTO login)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);

        if (!VerifyPassword(login.Password, user.HashedPassword))
        {
            return null;
        }
        return _tokenService.GenerateJwtToken(user);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}