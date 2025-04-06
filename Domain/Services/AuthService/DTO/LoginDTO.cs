using System.ComponentModel.DataAnnotations;


namespace Domain.Services.AuthService.DTO
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress(ErrorMessage ="Invalid email adress")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
