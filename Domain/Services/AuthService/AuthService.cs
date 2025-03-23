using DAL;
using Domain.Services.User;
using Domain.Services.User.DTO;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Data;

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

    /// <summary>
    /// Регистрирует нового пользователя.
    /// </summary>
    /// <param name="user">Данные пользователя.</param>
    /// <returns>JWT-токен для аутентификации.</returns>
    public async Task<string> Register(UserDTO user)
    {
        
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        // Создание нового пользователя
        var appUser = new AppUser
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Age = user.Age,
            HashedPassword = HashPassword(user.Password) 
        };

        
        _context.Users.Add(appUser);
        await _context.SaveChangesAsync();

        
        return _tokenService.GenerateJwtToken(appUser);
    }

    /// <summary>
    /// Аутентифицирует пользователя.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    /// <returns>JWT-токен для аутентификации или null, если аутентификация не удалась.</returns>
    public async Task<string> Validate(LoginDTO loginDto)
    {
        if(loginDto == null)
        {
            throw new ArgumentNullException(nameof(loginDto), "LoginDTO cannot be null.");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (user == null)
        {
            return null; 
        }

        
        if (!VerifyPassword(loginDto.Password, user.HashedPassword))
        {
            return null; 
        }

        
        return _tokenService.GenerateJwtToken(user);
    }

    /// <summary>
    /// Хеширует пароль.
    /// </summary>
    /// <param name="password">Пароль.</param>
    /// <returns>Хешированный пароль.</returns>
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12); 
    }

    /// <summary>
    /// Проверяет пароль.
    /// </summary>
    /// <param name="password">Пароль.</param>
    /// <param name="passwordHash">Хешированный пароль.</param>
    /// <returns>True, если пароль верный, иначе False.</returns>
    private bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}