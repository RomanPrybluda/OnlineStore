using DAL;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class PromotionInitializer
    {
        private readonly OnlineStoreDbContext _context;

        public PromotionInitializer(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public async Task InitializePromotions()
        {
            if (_context.Promotions.Any())
                return;

            var categories = await _context.Categories.ToListAsync();
            var products = await _context.Products.ToListAsync();

            var categoryGuids = categories.Select(c => c.Id).ToList();
            var productGuids = products.Select(p => p.Id).ToList();

            var promotions = new List<Promotion>
            {
                new Promotion
                {
                    Id = Guid.Parse("88888888-1111-1111-1111-111111111001"),
                    Name = "Summer Sale",
                    Description = "30% off on all sweets until the end of August!",
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(10),
                    StartTime = new TimeSpan(8, 0, 0),
                    EndTime = new TimeSpan(22, 0, 0),
                    IsActive = true,
                    ImageBannerName = "summer_sale",
                    CategoryIds = new List<Guid>
                    {
                        categoryGuids.ElementAtOrDefault(0),
                        categoryGuids.ElementAtOrDefault(1),
                        categoryGuids.ElementAtOrDefault(2)
                    }
                },
                new Promotion
                {
                    Id = Guid.Parse("88888888-1111-1111-1111-111111111002"),
                    Name = "Chocolate Week",
                    Description = "Enjoy chocolate with 20% off all week long!",
                    StartDate = DateTime.Today.AddDays(1),
                    EndDate = DateTime.Today.AddDays(7),
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(21, 0, 0),
                    IsActive = true,
                    ImageBannerName = "chocolate_week",
                    CategoryIds = new List<Guid>
                    {
                        categoryGuids.ElementAtOrDefault(4),
                    }
                },
                new Promotion
                {
                    Id = Guid.Parse("88888888-1111-1111-1111-111111111003"),
                    Name = "Sweet Morning",
                    Description = "Get a 15% discount on pastries before 12:00!",
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(30),
                    StartTime = new TimeSpan(6, 0, 0),
                    EndTime = new TimeSpan(12, 0, 0),
                    IsActive = true,
                    ImageBannerName = "sweet_morning",
                    ProductIds = new List<Guid>
                    {
                        productGuids.ElementAtOrDefault(0),
                        productGuids.ElementAtOrDefault(1),
                        productGuids.ElementAtOrDefault(2)
                    }
                },
                new Promotion
                {
                    Id = Guid.Parse("88888888-1111-1111-1111-111111111004"),
                    Name = "Super Weekend",
                    Description = "Only on Saturday and Sunday — 25% off everything!",
                    StartDate = DateTime.Today.AddDays(5),
                    EndDate = DateTime.Today.AddDays(7),
                    StartTime = new TimeSpan(10, 0, 0),
                    EndTime = new TimeSpan(20, 0, 0),
                    IsActive = false,
                    ImageBannerName = "weekend_special",
                    ProductIds = new List<Guid>
                    {
                        productGuids.ElementAtOrDefault(50),
                        productGuids.ElementAtOrDefault(58),
                        productGuids.ElementAtOrDefault(80)
                    }
                },
                new Promotion
                {
                    Id = Guid.Parse("88888888-1111-1111-1111-111111111005"),
                    Name = "Gifts for Loved Ones",
                    Description = "Buy gift sets with a 20% discount",
                    StartDate = DateTime.Today.AddDays(3),
                    EndDate = DateTime.Today.AddDays(14),
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(23, 0, 0),
                    IsActive = true,
                    ImageBannerName = "gifts_love",
                    ProductIds = new List<Guid>
                    {
                        productGuids.ElementAtOrDefault(25),
                        productGuids.ElementAtOrDefault(75),
                        productGuids.ElementAtOrDefault(85)
                    }
                }
            };

            _context.Promotions.AddRange(promotions);
            await _context.SaveChangesAsync();
        }
    }
}
