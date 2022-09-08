using Core;

namespace Persistence.FolderStorage
{
    public class FolderStorageImageRepository : IImageRepository
    {
        public async Task SaveImageAsync(string filepath, byte[] content)
        {
            using (var stream = File.OpenWrite(filepath))
            {
                await stream.WriteAsync(content);
            }
        }

        public async Task<byte[]> GetImageAsync(string filepath)
        {
            return File.ReadAllBytes(filepath);
        }
    }
}