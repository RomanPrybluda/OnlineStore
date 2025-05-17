namespace BSExpPhotos.Interfaces;

public interface IImageInfoExtractor
{
    Task<List<string>> ExtractImageFileNames(object entity);
}