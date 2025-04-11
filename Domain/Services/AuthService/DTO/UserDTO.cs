using System.ComponentModel.DataAnnotations;

namespace Domain.Services.AuthService.DTO;

public class UserDTO
{
    [Required]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "First name must be between 3 and 20 characters.")]
    public string FirstName { get; set; }

    [Required]
    [Range(1, 120, ErrorMessage = "Age must be between 1 and 120.")]
    public int? Age { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 20 characters.")]
    public string LastName { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    

    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [MaxLength(16, ErrorMessage = "password must be up to 16 characters")]
    public string Password { get; set; }

    public string Role { get; set; }

    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [MaxLength(16, ErrorMessage = "password must be up to 16 characters")]
    public string NewPassword { get; set; }

    public string Token { get; set; }
}