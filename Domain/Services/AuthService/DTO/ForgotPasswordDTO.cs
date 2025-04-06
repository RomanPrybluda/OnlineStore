using System.ComponentModel.DataAnnotations;

namespace Domain.Services.User.DTO
{
    public class ForgotPasswordDTO
    {
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
    }
}
