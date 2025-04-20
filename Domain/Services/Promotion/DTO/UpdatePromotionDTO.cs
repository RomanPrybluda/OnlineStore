using DAL;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class UpdatePromotionDTO
    {

        [StringLength(100, MinimumLength = 3, ErrorMessage = "The name of the promotion must be from 3 to 100 characters.")]
        public string Name { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public IFormFile Banner { get; set; }

        public List<int> CategoryIds { get; set; } = new List<int>();

        public List<int> ProductIds { get; set; } = new List<int>();

        public static Promotion ToPromotion(UpdatePromotionDTO request, string imageBaseName)
        {
            return new Promotion
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                ImageBannerName = imageBaseName
            };
        }

    }
}
