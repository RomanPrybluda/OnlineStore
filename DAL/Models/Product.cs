﻿namespace DAL
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string SortDescription { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string? MainImageBaseName { get; set; } = string.Empty;

        public List<string>? ImageBaseNames { get; set; }

        public string Sku { get; set; } = string.Empty;

        public double Rating { get; set; }

        public int TotalVotes { get; set; }

        public int Views { get; set; }

        public int StockQuantity { get; set; }

        public int SoldQuantity { get; set; }

        public bool IsActive { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public List<Review> Reviews { get; set; } = new();

        public List<AppUser> FavoritedByUsers { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        public string? Composition { get; set; }

        public string? Allergens { get; set; }

        public double WeightInGrams { get; set; }

        public bool IsSugarFree { get; set; }

        public bool IsGlutenFree { get; set; }


    }
}
