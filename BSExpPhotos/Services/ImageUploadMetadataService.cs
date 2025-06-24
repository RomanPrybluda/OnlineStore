using BSExpPhotos.Interfaces;
using DAL;

namespace BSExpPhotos.Services;

public class ImageUploadMetadataService(OnlineStoreDbContext dbContext) : IImageUploadMetadataService
{
    public async Task SaveImageInfoToDb(string fileName, Photo.EntityType entityType, Guid entityId,
        DateTime uploadedAt)
    {
        var record = new Photo
        {
            FileName = fileName,
            EntityId = entityId,
            UploadedAt = uploadedAt,
            Type = entityType,
            IsDeleted = true
        };

        dbContext.Photos.Add(record);
        await dbContext.SaveChangesAsync();
    }
}