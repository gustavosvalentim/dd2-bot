using Microsoft.Graphics.Canvas;
using System.Threading.Tasks;

namespace DD2Bot.UI.Services
{
    public interface IScreenCaptureService
    {
        string CurrentFramePath { get; }
        CanvasBitmap CurrentFrame { get; }
        Task SaveImageAsync(string filename);
        Task StartCaptureAsync();
        void StopCapture();
    }
}
