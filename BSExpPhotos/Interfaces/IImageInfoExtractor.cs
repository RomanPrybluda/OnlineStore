using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSExpPhotos.Interfaces
{
    public interface IImageInfoExtractor
    {
        List<string> ExtractImageFileNames(object entity);
    }
}
