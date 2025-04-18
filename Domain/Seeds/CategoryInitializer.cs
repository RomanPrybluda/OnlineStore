﻿using DAL;

namespace Domain
{
    public class CategoryInitializer
    {
        private readonly OnlineStoreDbContext _context;

        public CategoryInitializer(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public void InitializeCategories()
        {
            if (!_context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Cakes",
                        Description = "Freshly baked cakes with various flavors",
                        ImageBaseName = "cakes"
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Boxes sweets",
                        Description = "Assorted sweets in elegant gift boxes",
                        ImageBaseName = "boxes_sweets"
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Caramel and candies",
                        Description = "A variety of caramel and hard candies",
                        ImageBaseName = "caramel_candies"
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Chocolate bar",
                        Description = "Delicious chocolate bars with different fillings",
                        ImageBaseName = "chocolate_bar"
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Biscuits",
                        Description = "Crunchy and flavorful biscuits",
                        ImageBaseName = "biscuits"
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Marshmallow",
                        Description = "Soft and fluffy marshmallows",
                        ImageBaseName = "marshmallow"
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Healthy sweets",
                        Description = "Sweet treats made with natural ingredients",
                        ImageBaseName = "healthy_sweets"
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Gift sets",
                        Description = "Beautifully packaged sweet gift sets",
                        ImageBaseName = "gift_sets"
                    },
                };

                _context.Categories.AddRange(categories);
                _context.SaveChanges();
            }
        }
    }
}
