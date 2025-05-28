using DAL;
using Microsoft.EntityFrameworkCore;

namespace BSExpPhotos.Initial;

// for one use in first deploy on prod
public class ImageInitializer(OnlineStoreDbContext dbContext)
{
    public async Task InitializeAsync()
    {
        var storedImages = new List<Photo>();

        // Product Main Image
        var products = await dbContext.Products.ToListAsync();
        foreach (var product in products)
        {
            if (!string.IsNullOrEmpty(product.MainImageBaseName))
                storedImages.Add(new Photo
                {
                    FileName = product.MainImageBaseName,
                    EntityId = product.Id,
                    UploadedAt = product.UpdateAt,
                    Type = Photo.EntityType.Product
                });

            if (product.ImageBaseNames != null)
                foreach (var image in product.ImageBaseNames)
                    storedImages.Add(new Photo
                    {
                        FileName = image,
                        EntityId = product.Id,
                        UploadedAt = product.UpdateAt,
                        Type = Photo.EntityType.Product
                    });
        }

        // Promotion Image
        var promotions = await dbContext.Promotions.ToListAsync();
        foreach (var promo in promotions)
            if (!string.IsNullOrEmpty(promo.ImageBannerName))
                storedImages.Add(new Photo
                {
                    FileName = promo.ImageBannerName,
                    EntityId = promo.Id,
                    Type = Photo.EntityType.Promotion
                });
        
        // Category images
        var categories = await dbContext.Categories.ToListAsync();
        foreach (var category in categories)
            if (!string.IsNullOrEmpty(category.ImageBaseName))
                storedImages.Add(new Photo
                    {
                        FileName = category.ImageBaseName,
                        EntityId = category.Id,
                        Type = Photo.EntityType.Category
                    }
                    );

        // Добавим в БД
        var batchSize = 50;
        for (int i = 0; i < storedImages.Count; i += batchSize)
        {
            var batch = storedImages.Skip(i).Take(batchSize).ToList();
            dbContext.Photos.AddRange(batch);
            await dbContext.SaveChangesAsync();
        }

        //_dbContext.Photos.AddRange(storedImages);
        //await _dbContext.SaveChangesAsync();
    }
}