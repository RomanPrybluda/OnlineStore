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
    /// ������������ ������ ������������.
    /// </summary>
    /// <param name="user">������ ������������.</param>
    /// <returns>JWT-����� ��� ��������������.</returns>
    public async Task<string> Register(UserDTO user)
    {
        
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        // �������� ������ ������������
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
    /// ��������������� ������������.
    /// </summary>
    /// <param name="email">Email ������������.</param>
    /// <param name="password">������ ������������.</param>
    /// <returns>JWT-����� ��� �������������� ��� null, ���� �������������� �� �������.</returns>
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
    /// �������� ������.
    /// </summary>
    /// <param name="password">������.</param>
    /// <returns>������������ ������.</returns>
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12); 
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    /// <param name="password">������.</param>
    /// <param name="passwordHash">������������ ������.</param>
    /// <returns>True, ���� ������ ������, ����� False.</returns>
    private bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}