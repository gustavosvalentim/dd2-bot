using System;
using Microsoft.Graphics.Canvas;
using System.Threading.Tasks;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX;
using Windows.Storage;

namespace DD2Bot.UI.Services
{
    public class ScreenCaptureService : IScreenCaptureService
    {
        public const DirectXPixelFormat PixelFormat = DirectXPixelFormat.B8G8R8A8UIntNormalized;
        public CanvasBitmap CurrentFrame { get; private set; }
        public string CurrentFramePath { get; private set; }
        private GraphicsCaptureItem _item;
        private CanvasDevice _canvasDevice;
        private GraphicsCaptureSession _session;
        private Direct3D11CaptureFramePool _framePool;

        public ScreenCaptureService()
        {
            _canvasDevice = new CanvasDevice();
        }

        private void StartCaptureInternal(GraphicsCaptureItem item)
        {
            StopCapture();

            _item = item;
            _framePool = Direct3D11CaptureFramePool.Create(
                _canvasDevice,
                PixelFormat,
                1,
                _item.Size);

            _framePool.FrameArrived += (s, a) =>
            {
                using (var frame = _framePool.TryGetNextFrame())
                {
                    ProcessFrame(frame);
                }
            };

            _item.Closed += (s, a) =>
            {
                StopCapture();
            };

            _session = _framePool.CreateCaptureSession(_item);
            _session.StartCapture();
        }

        private void ProcessFrame(Direct3D11CaptureFrame frame)
        {
            CurrentFrame = CanvasBitmap.CreateFromDirect3D11Surface(
                _canvasDevice,
                frame.Surface);
        }

        public async Task SaveImageAsync(string filename)
        {
            StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(Configuration.StoragePath);
            StorageFile file = await storageFolder.CreateFileAsync(
                filename,
                CreationCollisionOption.ReplaceExisting);

            using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                await CurrentFrame.SaveAsync(fileStream, CanvasBitmapFileFormat.Png, 1f);
            }
            CurrentFramePath = file.Path.Replace(storageFolder.Name, storageFolder.DisplayName);
        }

        public async Task StartCaptureAsync()
        {
            var picker = new GraphicsCapturePicker();
            GraphicsCaptureItem item = await picker.PickSingleItemAsync();
            if (item != null)
            {
                StartCaptureInternal(item);
            }
        }

        public void StopCapture()
        {
            _session?.Dispose();
            _framePool?.Dispose();
            _item = null;
            _session = null;
            _framePool = null;
        }
    }
}
