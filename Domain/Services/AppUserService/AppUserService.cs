using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;

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

        public async Task<List<UserInfoDTO>> GetAllUsersAsync()
        {
            var users =  await _userManager.Users.ToListAsync();

            if (!users.Any()) 
            {
                throw new NotFoundException();
            }

            var usersDTOs = users.Select(UserInfoDTO.FromAppUser).ToList();
            return usersDTOs;

        }

        public async Task<UserInfoDTO?> GetUserByIdAsync(Guid userId)
        {
            var userById = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId.ToString());
            var userDTO = UserInfoDTO.FromAppUser(userById);
            return userDTO;
        }

        public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> DeleteUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            return await _userManager.DeleteAsync(user);
        }

        public async Task<UserInfoDTO> GetUserInfoAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }


            var role = await _userManager.GetRolesAsync(user);
            var roleName = role.FirstOrDefault();

            return new UserInfoDTO
            {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Age = user.Age,
                    Id  = user.Id,
                    UserName = user.UserName
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
            user.UserName = updateRequest.UserName ?? user.UserName;
            user.Email = updateRequest.Email ?? user.Email;   

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Error Update");
            }

            return new UserUpdateResult
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                UserName = user.UserName
            };
        }
    }
}
