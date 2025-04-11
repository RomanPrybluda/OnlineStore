using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class RoleInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleInitializer(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public void InitializeRoles()
        {
            if (!_roleManager.RoleExistsAsync(Roles.ADMIN).Result)
            {
                var result = _roleManager.CreateAsync(new IdentityRole
                {
                    Name = Roles.ADMIN
                }).Result;

                if (!result.Succeeded)
                {
                    throw new Exception($"Error while creating role {Roles.ADMIN}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                Console.WriteLine($"Role {Roles.ADMIN} created.");
            }

            if (!_roleManager.RoleExistsAsync(Roles.USER).Result)
            {
                var result = _roleManager.CreateAsync(new IdentityRole
                {
                    Name = Roles.USER
                }).Result;

                if (!result.Succeeded)
                {
                    throw new Exception($"Error while creating role {Roles.USER}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                Console.WriteLine($"Role {Roles.USER} created.");
            }
        }
    }
}
