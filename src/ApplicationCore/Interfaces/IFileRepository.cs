namespace DD2Bot.ApplicationCore.Interfaces
{
    public interface IFileRepository
    {
        Task SaveAsync(string filepath, byte[] content);
        Task SaveAsync(string filepath, string content);
        Task<byte[]> ReadAsync(string source);
    }
}