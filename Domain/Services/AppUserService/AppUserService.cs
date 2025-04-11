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

        public async Task<IEnumerable<UserInfoDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();

            if (!users.Any())
                throw new CustomException(CustomExceptionType.NotFound, "No categories found");

            var usersDTOs = users.Select(UserInfoDTO.FromAppUser).ToList();
            return usersDTOs;

        }

        public async Task<UserInfoDTO> GetUserByIdAsync(Guid id)
        {
            var userById = await _context.Users.FirstOrDefaultAsync(u => u.Id == id.ToString());

            if (userById == null)
                throw new CustomException(CustomExceptionType.NotFound, $"User not found with ID {id}");

            var userDTO = UserInfoDTO.FromAppUser(userById);

            return userDTO;
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id.ToString());

            if (user == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No category found with ID {id}");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserInfoDTO> UpdateUserAsync(string id, UserUpdateDTO updateRequest)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new CustomException(CustomExceptionType.NotFound, $"User not found with ID:{id}.");

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var userDTO = UserInfoDTO.FromAppUser(user);

            return userDTO;
        }
    }
}
