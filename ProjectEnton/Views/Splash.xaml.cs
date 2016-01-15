using ProjectEnton.Datahandling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ProjectEnton.Views
{
    /// <summary>
    /// View for mimicking the original splashscreen. Allows to do further preparing in the back.
    /// Partially based on Microsoft UWP resource about this topic.
    /// author: Raphael Zenhäusern
    /// </summary>
    public sealed partial class Splash
    {
        internal Rect splashImageRect; // Rect to store splash screen image coordinates.
        internal bool dismissed = false; // Variable to track splash screen dismissal status.
        internal Frame rootFrame;

        private Shell shell;

        private SplashScreen splash; // Variable to hold the splash screen object.
        private double ScaleFactor; //Variable to hold the device scale factor (use to determine phone screen resolution)

        public Splash(SplashScreen splashscreen, Shell shell, bool loadState)
        {
            this.InitializeComponent();

            this.shell = shell;

            // Subscribe to changed size
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(Splash_OnResize);

            ScaleFactor = (double)DisplayInformation.GetForCurrentView().ResolutionScale / 100;

            splash = splashscreen;

            if (splash != null)
            {
                // Register an event handler to be executed when the splash screen has been dismissed.
                splash.Dismissed += new TypedEventHandler<SplashScreen, Object>(DismissedEventHandler);

                // Retrieve the window coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();
            }

            CheckMedDatabaseUpdateAsync();
        }

        void PositionImage()
        {
            extendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.Left);
            extendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Top);
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                extendedSplashImage.Height = splashImageRect.Height / ScaleFactor;
                extendedSplashImage.Width = splashImageRect.Width / ScaleFactor;
            }
            else
            {
                extendedSplashImage.Height = splashImageRect.Height;
                extendedSplashImage.Width = splashImageRect.Width;
            }
        }

        void Splash_OnResize(Object sender, WindowSizeChangedEventArgs e)
        {
            // Safely update the extended splash screen image coordinates. This function will be fired in response to snapping, unsnapping, rotation, etc...
            if (splash != null)
            {
                // Update the coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();
            }
        }

        async void ContinueToAppInterfaceAsync()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                Window.Current.Content = shell;
                Window.Current.Activate();
            });
            
        }

        // Include code to be executed when the system has transitioned from the splash screen to the extended splash screen (application's first view).
        void DismissedEventHandler(SplashScreen sender, object e)
        {
            dismissed = true;

            // Navigate away from the app's extended splash screen after completing setup operations here...
            // This sample navigates away from the extended splash screen when the "Learn More" button is clicked.
        }

        private async void CheckMedDatabaseUpdateAsync()
        {
            ShowProgressInformationAsync(true);
            await DatabaseStore.IfNeededUpdateMedDatabaseAsync();
            ShowProgressInformationAsync(false);
            ContinueToAppInterfaceAsync();
        }

        private async void ShowProgressInformationAsync(bool show)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                ProgressInformation.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
                ProgressIndicator.IsActive = show;
            });
            
        }
    }
}
