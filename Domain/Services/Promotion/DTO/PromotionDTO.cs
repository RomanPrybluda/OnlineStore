using DAL;

namespace Domain
{
    public class PromotionDTO
    {

        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public static PromotionDTO FromPromotion(Promotion promotion)
        {
            return new PromotionDTO
            {
                Id = promotion.Id,
                Name = promotion.Name,
                Description = promotion.Description,
                IsActive = promotion.IsActive
            };
        }

    }
}
