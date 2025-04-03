using DAL;
using Domain.Services.AppUser.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using System.Data;

namespace Domain
{
    public class AppUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AppUserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<AppUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<AppUser?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            return await _userManager.DeleteAsync(user);
        }

        public async Task<ResultUserInfo> GetUserInfoAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }


            var role = await _userManager.GetRolesAsync(user);
            var roleName = role.FirstOrDefault();

            return new ResultUserInfo
            {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Age = user.Age,
                    Reviews = user.Reviews,
                    Orders = user.Orders,
                    Id  = user.Id
            };
        }

        public async Task<UserUpdateResult> UserUpdateAsync(string userId, UserUpdateRequest updateRequest)
        {
            var user =  await _userManager.FindByIdAsync(userId);
            if(user is null)
            {
                throw new NotFoundException("User not found");
            }

            user.FirstName = updateRequest.FirstName ?? user.FirstName;
            user.LastName = updateRequest.LastName ?? user.LastName;
            user.Age = updateRequest.Age ?? user.Age;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Error Update");
            }

            return new UserUpdateResult
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age
            };
        }
    }
}
