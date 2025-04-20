using DAL;

namespace Domain
{
    public class UpdateAppUserDTO
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public int? Age { get; set; }

        public void UpdateAppUser(AppUser user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            UserName = user.UserName;
            Age = user.Age;

        }

    }
}
