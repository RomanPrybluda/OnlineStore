using BSExpPhotos.Metadata;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSExpPhotos.Interfaces
{
    public interface IImageUploadMetadataService
    {
        Task SaveImageInfoToDb(string fileName, DAL.Photo.EntityType entityType, Guid entityId, DateTime uploadedAt);
    }
}


/*
 * FileName = imageUploadMetadata.FileNames,
                EntityId = imageUploadMetadata.EntityId,
                UploadedAt = imageUploadMetadata.CreatedAt,
                Type = imageUploadMetadata.EntityType
 */