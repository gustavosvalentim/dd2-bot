namespace Core
{
    public interface IScreenCaptureService
    {
        byte[] CaptureScreen();
        Task CaptureScreenToFileAsync(string filepath);
    }
}
