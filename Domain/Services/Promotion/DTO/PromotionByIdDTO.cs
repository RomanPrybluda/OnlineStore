using DAL;

namespace Domain
{
    public class PromotionByIdDTO
    {

        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public List<Guid> CategoryIds { get; set; } = new List<Guid>();

        public List<Guid> ProductIds { get; set; } = new List<Guid>();


        public static PromotionByIdDTO FromPromotion(Promotion promotion)
        {
            return new PromotionByIdDTO
            {
                Id = promotion.Id,
                Name = promotion.Name,
                Description = promotion.Description,
                IsActive = promotion.IsActive,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                StartTime = promotion.StartTime,
                EndTime = promotion.EndTime,
                CategoryIds = promotion.CategoryIds ?? new List<Guid>(),
                ProductIds = promotion.ProductIds ?? new List<Guid>()
            };
        }

    }
}
