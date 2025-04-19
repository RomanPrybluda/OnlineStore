using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class RoleInitializer
    {
        private readonly OnlineStoreDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleInitializer(OnlineStoreDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task InitializeRolesAsync()
        {

            var roles = new[] { Roles.Admin, Roles.Manager, Roles.User };

            foreach (var roleName in roles)
            {
                var existingRole = await _roleManager.RoleExistsAsync(roleName);
                if (!existingRole)
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        Console.WriteLine($"Failed to create role '{roleName}': {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                    }
                    else
                    {
                        Console.WriteLine($"Role '{roleName}' created.");
                    }
                }
                else
                {
                    Console.WriteLine($"Role '{roleName}' already exists.");
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
