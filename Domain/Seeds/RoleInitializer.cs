using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class RoleInitializer
    {

        public static async Task InitializeRole(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Roles.ADMIN))
            {
                await roleManager.CreateAsync(
                    new IdentityRole
                    {
                        Name = Roles.ADMIN
                    }
                );
            }

            if (!await roleManager.RoleExistsAsync(Roles.USER))
            {
                await roleManager.CreateAsync(
                    new IdentityRole()
                    {
                        Name = Roles.USER
                    }
                );
            }

        }
    }
}

