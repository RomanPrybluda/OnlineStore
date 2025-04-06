using System.ComponentModel.DataAnnotations;

namespace Domain.Services.User.DTO
{
    public class ResetPasswordDTO
    {
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [MaxLength(16, ErrorMessage = "password must be up to 16 characters")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$", ErrorMessage = "The password must contain at least one uppercase letter, one number and one special character.")]
        public string NewPassword { get; set; }
        [Required]
        public string Token { get; set; }  
    }
}
