using DAL;

namespace BSExpPhotos.Interfaces;

public interface IImageInfoExtractor
{
    Task<List<string>> ExtractImageFileNames(Guid entityId, Photo.EntityType type);
}