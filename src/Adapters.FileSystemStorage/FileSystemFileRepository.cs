using DD2Bot.ApplicationCore.Interfaces;

namespace DD2Bot.Adapters.FileSystemStorage
{
    public class FileSystemFileRepository : IFileRepository
    {
        public async Task SaveAsync(string filepath, byte[] content)
        {
            await File.WriteAllBytesAsync(filepath, content);
        }

        public async Task SaveAsync(string filepath, string content)
        {
            await File.WriteAllTextAsync(filepath, content);
        }

        public async Task<byte[]> ReadAsync(string filepath)
        {
            return await File.ReadAllBytesAsync(filepath);
        }
    }
}