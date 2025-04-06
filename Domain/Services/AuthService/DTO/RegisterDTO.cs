using System.ComponentModel.DataAnnotations;

namespace Domain.Services.AuthService.DTO
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "First name must be between 3 and 20 characters.")]
        public string FirstName { get; set; }

        [Required]
        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120.")]
        public int? Age { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Last name must be between 3 and 20 characters.")]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [MaxLength(16)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$", ErrorMessage = "The password must contain at least one uppercase letter, one number and one special character.")]
        public string Password { get; set; }
    }
}
