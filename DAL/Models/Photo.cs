using Microsoft.EntityFrameworkCore;
namespace DAL;

public class Photo
{
    public required Guid EntityId { get; set; }
    public required string FileName { get; set; }
    public EntityType Type{ get; set;} = EntityType.None;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public enum EntityType
    {
        Promotion,
        Product,
        Category,
        None
    }
}