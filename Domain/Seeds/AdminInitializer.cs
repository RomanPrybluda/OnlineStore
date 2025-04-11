using DAL;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AdminInitializer
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminInitializer(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public void InitializeAdmin()
        {
            var adminEmail = "Admin@gmail.com";
            var adminPassword = "700402_QweAsdZxc";

            var adminCurrent = _userManager.FindByEmailAsync(adminEmail).Result;

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

                var result = _userManager.CreateAsync(admin, adminPassword).Result;
                if (!result.Succeeded)
                {
                    throw new Exception();
                }

                result = _userManager.AddToRoleAsync(admin, Roles.ADMIN).Result;
                if (!result.Succeeded)
                {
                    throw new Exception();
                }
            }
        }
    }
}
