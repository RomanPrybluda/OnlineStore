using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BSExpPhotos
{
    // for one use in first deploy
    public class ImageInitializer(OnlineStoreDbContext _dbContext)
    {
      

        public async Task InitializeAsync()
        {
            var storedImages = new List<Photo>();

            // Product Main Image
            var products = await _dbContext.Products.ToListAsync();
            foreach (var product in products)
            {
                if (!string.IsNullOrEmpty(product.MainImageBaseName))
                {
                    storedImages.Add(new Photo
                    {
                        FileName = product.MainImageBaseName,
                        EntityId = product.Id,
                        UploadedAt = product.UpdateAt,
                        Type = Photo.EntityType.Product,

                    });
                }

                if (product.ImageBaseNames != null)
                {
                    foreach (var image in product.ImageBaseNames)
                    {
                        storedImages.Add(new Photo
                        {
                            FileName = image,                           
                            EntityId = product.Id,
                            UploadedAt = product.UpdateAt,
                            Type = Photo.EntityType.Product,

                        });
                    }
                }
            }

            // Promotion Image
            var promotions = await _dbContext.Promotions.ToListAsync();
            foreach (var promo in promotions)
            {
                if (!string.IsNullOrEmpty(promo.ImageBannerName))
                {
                    storedImages.Add(new Photo
                    {
                        FileName = promo.ImageBannerName,
                        EntityId = promo.Id,   
                        Type = Photo.EntityType.Promotion,
                    });
                }
            }

            // Добавим в БД
            _dbContext.Photos.AddRange(storedImages);
            await _dbContext.SaveChangesAsync();
        }
    }

}
