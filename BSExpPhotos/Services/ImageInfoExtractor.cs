using BSExpPhotos.Interfaces;
using DAL;

namespace BSExpPhotos.Services;

public class ImageInfoExtractor : IImageInfoExtractor
{
    public List<string> ExtractImageFileNames(object entity)
    {
        return entity switch
        {
            Product p => ExtractFromProduct(p),
            Promotion promo => new List<string> { promo.ImageBannerName }
                .Where(x => !string.IsNullOrWhiteSpace(x)).ToList(),
            Category cat => new List<string> { cat.ImageBaseName }
                .Where(x => !string.IsNullOrWhiteSpace(x)).ToList(),
            _ => new List<string>()
        };
    }

    private List<string> ExtractFromProduct(Product product)
    {
        var result = new List<string>();

        if (!string.IsNullOrWhiteSpace(product.MainImageBaseName))
            result.Add(product.MainImageBaseName);

        if (product.ImageBaseNames?.Any() == true)
            result.AddRange(product.ImageBaseNames.Where(x => !string.IsNullOrWhiteSpace(x)));

        return result;
    }
}