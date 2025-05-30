using DAL;

namespace Domain
{
    public class PopularProductDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string? MainImageBaseName { get; set; }

        public string SortDescription { get; set; } = string.Empty;

        public double Rating { get; set; }

        public int Views { get; set; }

        public int FavoritesCount { get; set; }

        public int ReviewCount { get; set; }

        public double PopularityScore { get; set; }

        public static PopularProductDTO FromProduct(Product product, double popularityScore)
        {
            return new PopularProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                MainImageBaseName = product.MainImageBaseName,
                SortDescription = product.SortDescription,
                Rating = product.Rating,
                Views = product.Views,
                FavoritesCount = product.FavoritedByUsers.Count,
                ReviewCount = product.Reviews.Count,
                PopularityScore = Math.Round(popularityScore, 2)
            };
        }
    }
}
