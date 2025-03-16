namespace Domain.Services.User.DTO;

public class UserDTO
{
    public required string FirstName { get; set; }
 
    public required int? Age { get; set; }
 
    public required string LastName { get; set; }
 
    public required string Email { get; set; }

    public required string Password { get; set; }

    public string Role { get; set; }
}