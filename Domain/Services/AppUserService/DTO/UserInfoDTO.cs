using DAL;
using System.ComponentModel.DataAnnotations;


namespace Domain
{
    public class UserInfoDTO
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Id { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string UserName { get; set; }
        
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
