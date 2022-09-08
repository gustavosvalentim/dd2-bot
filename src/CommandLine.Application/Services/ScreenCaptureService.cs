using Core;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace CommandLine.Application.Services
{
    public class ScreenCaptureService : IScreenCaptureService
    {
        private readonly IImageRepository _imageRepository;

        public ScreenCaptureService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        /// <summary>
        /// Capture the screen and returns a buffer with the image contents.
        /// </summary>
        /// <returns>A buffer with image contents.</returns>
        public byte[] CaptureScreen()
        {
            IntPtr handle = User32.GetDesktopWindow();
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest, hOld);
            // clean up
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);

            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);

            var buffer = new MemoryStream();
            img.Save(buffer, System.Drawing.Imaging.ImageFormat.Bmp);

            return buffer.ToArray();
        }

        public async Task CaptureScreenToFileAsync(string filepath)
        {
            await _imageRepository.SaveImageAsync(filepath, CaptureScreen());
        }
    }
}
