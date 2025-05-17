using DAL;

namespace BSExpPhotos.Interfaces;

public interface IImageUploadMetadataService
{
    Task SaveImageInfoToDb(string fileName, Photo.EntityType entityType, Guid entityId, DateTime uploadedAt);
}


/*
* FileName = imageUploadMetadata.FileNames,
            EntityId = imageUploadMetadata.EntityId,
            UploadedAt = imageUploadMetadata.CreatedAt,
            Type = imageUploadMetadata.EntityType
*/