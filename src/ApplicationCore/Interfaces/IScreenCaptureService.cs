namespace DD2Bot.ApplicationCore.Interfaces
{
    public interface IScreenCaptureService
    {
        byte[] CaptureDesktop();
        byte[] CaptureScreen(IntPtr handle);
    }
}
