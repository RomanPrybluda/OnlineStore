namespace BSExpPhotos.Interfaces;

public interface IImageInfoExtractor
{
    List<string> ExtractImageFileNames(object entity);
}