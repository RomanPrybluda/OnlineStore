using DAL;


namespace Domain
{
    public class UserInfoDTO
    {

        public string? Email { get; set; } = string.Empty;

        public string? Id { get; set; } = string.Empty;

        public string? FirstName { get; set; } = string.Empty;

        public string? LastName { get; set; } = string.Empty;

        public string? UserName { get; set; } = string.Empty;

        public int? Age { get; set; }

        public static UserInfoDTO FromAppUser(AppUser appUser)
        {
            return new UserInfoDTO
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                UserName = appUser.UserName,
                Email = appUser.Email,
                Age = appUser.Age
            };
        }
    }
}
