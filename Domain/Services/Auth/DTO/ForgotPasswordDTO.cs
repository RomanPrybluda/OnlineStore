using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class ForgotPasswordDTO
    {
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
    }
}
