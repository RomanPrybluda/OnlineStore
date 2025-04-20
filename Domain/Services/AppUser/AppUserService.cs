using DAL;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class AppUserService
    {
        private readonly OnlineStoreDbContext _context;

        public AppUserService(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppUserInfoDTO>> GetAllUsersAsync()
        {

            var users = await _context.Users.ToListAsync();

            if (!users.Any())
                throw new CustomException(CustomExceptionType.NotFound, "No users found");

            var userDTOs = new List<AppUserInfoDTO>();

            foreach (var user in users)
            {
                var userDTO = AppUserInfoDTO.FromUser(user);
                userDTOs.Add(userDTO);
            }

            return userDTOs;
        }

        public async Task<AppUserDTO> GetUserByIdAsync(Guid id)
        {
            var userById = await _context.Users.FindAsync(id);

            if (userById == null)
                throw new CustomException(CustomExceptionType.NotFound, $"User not found with ID:{id}");

            var userDTO = AppUserDTO.FromUser(userById);

            return userDTO;
        }

        public async Task<AppUserInfoDTO> GetUserInfoAsync(Guid id)
        {
            var userById = await _context.Users.FindAsync(id);

            if (userById == null)
                throw new CustomException(CustomExceptionType.NotFound, $"User not found with ID:{id}");

            var userDTO = AppUserInfoDTO.FromUser(userById);

            return userDTO;
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No user found with ID {id}");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<AppUserDTO> UserUpdateAsync(Guid id, UpdateAppUserDTO request)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new CustomException(CustomExceptionType.NotFound, $"User not found with ID {id}");

            request.UpdateAppUser(user);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var userDTO = AppUserDTO.FromUser(user);

            return userDTO;

        }
    }
}
