using ProjectEnton.Datahandling;
using ProjectEnton.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.System.Display;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using ZXing;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace ProjectEnton.Views
{
    /// <summary>
    /// This page is responsible for the drug search. The user can either enter a text or scan a barcode to get the needed drug.
    /// author: Florian Schnyder, Dominique Walter, Raphael Zenhäusern
    /// </summary>
    public sealed partial class SearchDrug : Page
    {
        private MediaCapture _mediaCapture;
        private bool _isInitialized;
        private bool _isPreviewing;
        private bool _externalCamera;
        private bool _mirroringPreview;
        private bool _torchActive;

        // Request to keep the screen awake
        private readonly DisplayRequest _displayRequest = new DisplayRequest();

        // Trackers for screen rotation
        private readonly DisplayInformation _displayInformation = DisplayInformation.GetForCurrentView();
        private DisplayOrientations _displayOrientation = DisplayOrientations.Portrait;

        private readonly SimpleOrientationSensor _orientationSensor = SimpleOrientationSensor.GetDefault();
        private SimpleOrientation _deviceOrientation = SimpleOrientation.NotRotated;

        private static readonly Guid RotationKey = new Guid("C380465D-2271-428C-9B83-ECEA3B4A85C1");

        private byte[] imageBuffer;
        public int exit = 0;

        public SearchDrug()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// Removes all entered characters from the input field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelDrugButton_Click(object sender, RoutedEventArgs e)
        {
            DrugsearchTextBox.Text = "";
        }

        /// <summary>
        /// Search all drugs with the given user input in the "DrugsearchTextBox" Field. Navigates the user to a new page. The new page shows all the drugs which could be found with the given input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchDrugButton_Click(object sender, RoutedEventArgs e)
        {
            if(DrugsearchTextBox.Text == "pummeluff:enable")
            {
                Shell.Current.EnablePummeluffMode(true);
            } else if(DrugsearchTextBox.Text == "pummeluff:disable")
            {
                Shell.Current.EnablePummeluffMode(false);
            } else
            {
                var result = DatabaseStore.GetDrugsForNameSubstring(DrugsearchTextBox.Text);
                Frame.Navigate(typeof(SearchDrugResults), result);
            }
        }

        private void InitiateBarcodescanButton_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush semitransBrush = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));
            CameraOverlay.Background = semitransBrush;
            CameraOverlay.Visibility = Visibility.Visible;
            InitializeCameraAsync();
        }

        private void torchButton_Click(object sender, RoutedEventArgs e)
        {
            var torch = _mediaCapture.VideoDeviceController.TorchControl;
            torch.Enabled = !_torchActive;
            _torchActive = !_torchActive;
        }

        private void rotateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        #region CameraStream

        private async void InitializeCameraAsync()
        {
            if (_mediaCapture == null)
            {
                // Find all webcams of the Device
                var webcamList = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

                // Get the rear Camera if available
                DeviceInformation webcam = webcamList.FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back);

                // Check if webcam is set. If no, try to find another one (maybe front?)
                webcam = webcam ?? webcamList.FirstOrDefault();

                // Check if set now
                if (webcam == null)
                {
                    // No... fuck there seems to be no cam
                    // TODO: Failhandling
                    Debug.WriteLine("no webcam");
                    var dialog = new MessageDialog("Keine Webcam vorhanden");
                    await dialog.ShowAsync();
                    return;
                }

                // Now create media capture
                _mediaCapture = new MediaCapture();
                var mediaCaptureInitSettings = new MediaCaptureInitializationSettings
                {
                    VideoDeviceId = webcam.Id,
                    AudioDeviceId = "",
                    StreamingCaptureMode = StreamingCaptureMode.Video,
                    PhotoCaptureSource = PhotoCaptureSource.VideoPreview
                };

                try
                {
                    await _mediaCapture.InitializeAsync(mediaCaptureInitSettings);
                    _isInitialized = true;
                }
                catch (UnauthorizedAccessException)
                {
                    // Fuck, access to cam denied :(
                    Debug.WriteLine("no webcam access");
                    var dialog = new MessageDialog("Keine Zugriffsberechtigung auf Kamera.");
                    await dialog.ShowAsync();
                }
                catch (Exception e)
                {
                    // Crap, unknown exception
                    Debug.WriteLine("Exception when accessing the webcam: {0}", e.ToString());
                    var dialog = new MessageDialog("Unerwarteter Fehler während Zugriff auf Kamera aufgetreten.");
                    await dialog.ShowAsync();
                }

                // OK We seem to have Access to the cam, now lets init the Stream
                if (_isInitialized)
                {
                    // Find out what cam we're accessing
                    if (webcam.EnclosureLocation == null || webcam.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Unknown)
                    {
                        // No info available... maybe it is an external. Atleast we assume
                        _externalCamera = true;
                    }
                    else
                    {
                        // Info available. So it is internal
                        _externalCamera = false;

                        // Find out if we need to mirror the Image (only if it is the cam on the front
                        _mirroringPreview = (webcam.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front);
                    }

                    // now start the preview
                    await StartPreviewAsync();

                    UpdateCaptureControls();
                }
            }
        }

        private async Task StartPreviewAsync()
        {
            // Now that we start the preview, prevent the device from sleeping
            _displayRequest.RequestActive();

            // Now connect the source to the UI
            captureElement.Source = _mediaCapture;
            captureElement.FlowDirection = _mirroringPreview ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            // Start the Preview
            try
            {
                await _mediaCapture.StartPreviewAsync();
                _isPreviewing = true;
            }
            catch (Exception e)
            {
                // Ok, something fucked up
                Debug.WriteLine("Exception when starting the preview: {0}", e.ToString());
                var dialog = new MessageDialog("Unerwarteter Fehler während Zugriff auf Kamera aufgetreten.");
                await dialog.ShowAsync();
            }

            // Now adjust the Preview to the current rotation
            if (_isPreviewing)
            {
                await SetPreviewRotationAsync();
                await SetAutofocusAsync();
            }
        }

        private async Task SetPreviewRotationAsync()
        {
            // Only need to update the orientation if the camera is mounted on the device
            if (_externalCamera) return;

            // Populate orientation variables with the current state
            _displayOrientation = _displayInformation.CurrentOrientation;

            // Calculate which way and how far to rotate the preview
            int rotationDegrees = ConvertDisplayOrientationToDegrees(_displayOrientation);

            // The rotation direction needs to be inverted if the preview is being mirrored
            if (_mirroringPreview)
            {
                rotationDegrees = (360 - rotationDegrees) % 360;
            }

            // Add rotation metadata to the preview stream to make sure the aspect ratio / dimensions match when rendering and getting preview frames
            var props = _mediaCapture.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview);
            props.Properties.Add(RotationKey, rotationDegrees);
            await _mediaCapture.SetEncodingPropertiesAsync(MediaStreamType.VideoPreview, props, null);
        }

        private async Task SetAutofocusAsync()
        {
            await _mediaCapture.VideoDeviceController.FocusControl.UnlockAsync();
            var focusSettings = new FocusSettings();
            focusSettings.AutoFocusRange = AutoFocusRange.FullRange;
            focusSettings.Mode = FocusMode.Continuous;
            focusSettings.WaitForFocus = true;
            focusSettings.DisableDriverFallback = false;
            _mediaCapture.VideoDeviceController.FocusControl.Configure(focusSettings);
            await _mediaCapture.VideoDeviceController.FocusControl.FocusAsync();
        }



        private static int ConvertDisplayOrientationToDegrees(DisplayOrientations orientation)
        {
            switch (orientation)
            {
                case DisplayOrientations.Portrait:
                    return 90;
                case DisplayOrientations.LandscapeFlipped:
                    return 180;
                case DisplayOrientations.PortraitFlipped:
                    return 270;
                case DisplayOrientations.Landscape:
                default:
                    return 0;
            }
        }

        private void UpdateCaptureControls()
        {
            var torch = _mediaCapture.VideoDeviceController.TorchControl;
            torchButton.Visibility = torch.Supported ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

        #region Barcodescan

        private async void ScanBarCode()
        {
            var imgProp = new ImageEncodingProperties { Subtype = "BMP", Width = 380, Height = 380 };
            var bcReader = new BarcodeReader();

            while (exit == 0)
            {
                var stream = new InMemoryRandomAccessStream();
                await _mediaCapture.CapturePhotoToStreamAsync(imgProp, stream);

                stream.Seek(0);
                var wbm = new WriteableBitmap(380, 380);
                await wbm.SetSourceAsync(stream);
                var result = bcReader.Decode(wbm);

                if (result != null)
                {
                    exit = 1;
                    int swissmedicNr = 0;
                    try
                    {
                        swissmedicNr = Int32.Parse(result.Text.Substring(3, 5));
                    }
                    catch { }
                    
                    if(swissmedicNr > 0)
                    {
                        Drug med = DatabaseStore.GetDrugForSwissMedicNumber(swissmedicNr);
                        List<Drug> list = new List<Drug>();
                        if(med != null)
                        {
                            list.Add(med);
                        }
                        Frame.Navigate(typeof(SearchDrugResults), list);
                    }

                    
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        #endregion
    }
}
