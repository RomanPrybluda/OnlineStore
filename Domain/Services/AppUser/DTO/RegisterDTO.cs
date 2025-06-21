using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;

public class RegisterDTO
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string UserName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [Range(1, 120)]
    public int Age { get; set; }
}
