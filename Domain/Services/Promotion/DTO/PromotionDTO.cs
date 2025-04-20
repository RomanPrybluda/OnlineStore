using DAL;

namespace Domain
{
    public class PromotionDTO
    {

        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public string ImageBannerName { get; set; } = string.Empty;

        public List<int> CategoryIds { get; set; } = new List<int>();

        public List<int> ProductIds { get; set; } = new List<int>();

        public static PromotionDTO FromPromotion(Promotion promotion)
        {
            return new PromotionDTO
            {
                Id = promotion.Id,
                Name = promotion.Name,
                Description = promotion.Description
            };
        }

    }
}
