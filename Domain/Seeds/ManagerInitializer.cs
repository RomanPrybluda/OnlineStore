using DAL;
using DAL.Models;
using Domain;
using Microsoft.AspNetCore.Identity;

public class ManagerInitializer
{
    private readonly AuthService _authService;
    private readonly UserManager<AppUser> _userManager;

    public ManagerInitializer(
        AuthService authService,
        UserManager<AppUser> userManager)
    {
        _authService = authService;
        _userManager = userManager;
    }

    public async Task InitializeManagerAsync()
    {
        var managerEmail = "manager@example.com";

        var existingUser = await _userManager.FindByEmailAsync(managerEmail);

        if (existingUser != null)
        {
            Console.WriteLine($"Manager with email {managerEmail} already exists.");
            return;
        }

        var dto = new RegisterDTO
        {
            FirstName = "Default",
            LastName = "Manager",
            Age = 35,
            Email = managerEmail,
            Password = "Manager@123"
        };

        var result = await _authService.Register(dto, Roles.Manager);

        if (result.Succeeded)
        {
            Console.WriteLine($"Manager account created: {dto.Email}");
        }
        else
        {
            Console.WriteLine($"Failed to create manager: {string.Join(", ", result.Errors)}");
        }
    }
}
