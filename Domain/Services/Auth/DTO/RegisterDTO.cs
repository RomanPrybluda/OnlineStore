using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "First name must be between 3 and 20 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Last name must be between 3 and 20 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 16 characters.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,16}$", ErrorMessage = "The password must contain at least one uppercase letter, one number and one special character.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Range(1, 120, ErrorMessage = "Age must be between 18 and 120.")]
        public int? Age { get; set; }
    }
}
