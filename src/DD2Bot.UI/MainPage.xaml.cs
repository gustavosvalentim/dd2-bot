using DD2Bot.UI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.Graphics.Capture;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DD2Bot.UI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly IScreenCaptureService _screenCaptureService;
        private readonly PhaseCatalogService _phaseCatalogService;
        private bool _isCapturingScreen = false;
        private bool _isBuildPhase = false;
        private int _clickCounter = 0;

        public MainPage()
        {
            if (!GraphicsCaptureSession.IsSupported())
            {
                CaptureButton.Visibility = Visibility.Collapsed;
            }

            Configuration.Bootstrap();

            _screenCaptureService = new ScreenCaptureService();
            _phaseCatalogService = new PhaseCatalogService();

            this.InitializeComponent();
        }

        private async Task CaptureButtonHandler()
        {
            if (!_isCapturingScreen || _screenCaptureService.CurrentFrame == null)
            {
                await _screenCaptureService.StartCaptureAsync();
                _isCapturingScreen = true;
            }
            if (_screenCaptureService.CurrentFrame != null)
            {
                await _screenCaptureService.SaveImageAsync(Guid.NewGuid().ToString() + ".png");
                _isBuildPhase = _phaseCatalogService.IsBuildPhase(_screenCaptureService.CurrentFramePath);
            }
        }

        private async void CaptureButton_Click(object sender, RoutedEventArgs e)
        {
            CaptureButton.IsEnabled = false;
            await CaptureButtonHandler();
            //await CaptureButtonTest_Click();
            _clickCounter++;
            IsMatchingCaptureBlock.Text = _isBuildPhase ? "Is a match" : "Not a match";
            CaptureButton.IsEnabled = true;
        }

        private void ImageStoragePathBlock_Loaded(object sender, RoutedEventArgs e)
        {
            ImageStoragePathBlock.Text = Configuration.StoragePath;
        }
    }
}
