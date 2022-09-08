namespace Core
{
    public interface IImageRepository
    {
        Task SaveImageAsync(string filepath, byte[] content);
        Task<byte[]> GetImageAsync(string source);
    }
}