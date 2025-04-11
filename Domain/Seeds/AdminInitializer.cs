using DAL;
using Domain.Services.AuthService;
using Microsoft.AspNetCore.Identity;
using System.Linq;


namespace Domain
{
    public class AdminInitializer
    {

        private readonly AuthService _authService;

        public AdminInitializer(AuthService authService) 
        {
            _authService = authService;
        }

        public static async Task InitializeRole(UserManager<AppUser> userManager)
        {

            var adminEmail = "Admin@gmail.com";
            var adminPassword = "700402_QweAsdZxc";

            var adminCurrent = await userManager.FindByEmailAsync(adminEmail);

            if (adminCurrent == null)
            {
                var registerDto = new RegisterDTO
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = adminEmail,
                    Age = 20,
                    UserName = "AdminName",
                    Password = adminPassword
                };
                var admin = RegisterDTO.FromRegisterDTO(registerDto);

                var result = await userManager.CreateAsync(admin,adminPassword);

                if (!result.Succeeded)
                {
                    throw new Exception();
                }

                result = await userManager.AddToRoleAsync(admin, Roles.ADMIN);

                var roles = await userManager.GetRolesAsync(admin);
                if (!result.Succeeded)
                {
                    throw new Exception();
                }

            }

        }
    }
}
    
    


 
    


