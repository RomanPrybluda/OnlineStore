using DAL;

namespace BSExpPhotos.Metadata;

public class ImageMetadata
{
    public Guid EntityId { get; set; }
    public Photo.EntityType EntityType { get; set; }
    public List<string> FileNames { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}