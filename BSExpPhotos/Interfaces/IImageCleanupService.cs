using DAL;

namespace BSExpPhotos.Interfaces;

public interface IImageCleanupService
{
    Task MarkRemovedImagesAsDeletedAsync(
        Guid entityId,
        Photo.EntityType entityType,
        List<string> newFileNames,
        CancellationToken cancellationToken = default);

    Task CleanUpOutdatedPhotosAsync();
}