using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class AppUser : IdentityUser
    {

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int? Age { get; set; }

        public string HashedPassword { get; set; } = string.Empty;

        public string? Role { get; set; } = "User";
       
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }      
       
        public List<Review> Reviews { get; set; } = new();

        public List<Order> Orders { get; set; } = new();

        public string? PasswordResetToken { get; set; }

        public DateTime? ResetTokenExpires { get; set; }

        public List<Product> FavoriteProducts { get; set; } = new();
    }
}
