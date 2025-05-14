using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSExpPhotos.Metadata
{
    public class ImageMetadata
    {
        public Guid EntityId { get; set; }
        public Photo.EntityType EntityType { get; set; }
        public List<string> FileNames { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
    }
}
