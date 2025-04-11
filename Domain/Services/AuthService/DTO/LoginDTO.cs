using DAL;
using System.ComponentModel.DataAnnotations;


namespace Domain
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email adress")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }


        public static AppUser FromLoginDTO(LoginDTO loginDto)
        {
            return new AppUser
            {
                Email = loginDto.Email,
            };
        }
    }
}
