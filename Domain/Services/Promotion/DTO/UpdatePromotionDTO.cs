using DAL;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class UpdatePromotionDTO
    {

        [StringLength(100, MinimumLength = 3, ErrorMessage = "The name of the promotion must be from 3 to 100 characters.")]
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [StringLength(500, MinimumLength = 3, ErrorMessage = "The name of the promotion must be from 3 to 500 characters.")]
        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public IFormFile Banner { get; set; }

        public List<string>? CategoryIds { get; set; } = new();

        public List<string>? ProductIds { get; set; } = new();

        public static void ToPromotion(UpdatePromotionDTO request, string imageBaseName, Promotion promotion)
        {
            promotion.Name = request.Name;
            promotion.StartDate = request.StartDate;
            promotion.EndDate = request.EndDate;
            promotion.Description = request.Description;
            promotion.IsActive = request.IsActive;
            promotion.ImageBannerName = imageBaseName;
            promotion.CategoryIds = request.CategoryIds?
                    .Where(id => !string.IsNullOrWhiteSpace(id) && Guid.TryParse(id, out _))
                    .Select(Guid.Parse)
                    .ToList();
            promotion.ProductIds = request.ProductIds?
                    .Where(id => !string.IsNullOrWhiteSpace(id) && Guid.TryParse(id, out _))
                    .Select(Guid.Parse)
                    .ToList();
        }

    }
}
