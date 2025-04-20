using DAL;

namespace Domain
{
    public class AppUserDTO
    {

        public string Email { get; set; }

        public string Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string UserName { get; set; }

        public int? Age { get; set; }

        public static AppUserDTO FromUser(AppUser user)
        {
            return new AppUserDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Age = user.Age
            };
        }
    }
}
