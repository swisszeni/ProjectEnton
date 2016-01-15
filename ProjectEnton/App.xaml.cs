using ProjectEnton.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ProjectEnton
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {

        private ApplicationTheme OSTheme;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);

            // Save the OS Theme before we override the value
            OSTheme = this.RequestedTheme;

            // Only set requested theme when color is defined. Otherwise use system value
            ElementTheme theme = Models.Settings.Instance.AppTheme;
            if (theme == ElementTheme.Dark)
            {
                this.RequestedTheme = ApplicationTheme.Dark;
            }
            else if (theme == ElementTheme.Light)
            {
                this.RequestedTheme = ApplicationTheme.Light;
            }

            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// author: Raphael Zenhäusern
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (e.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                bool loadState = (e.PreviousExecutionState == ApplicationExecutionState.Terminated);
                Shell shell = InitializeShell(e);
                Splash extendedSplash = new Splash(e.SplashScreen, shell, loadState);
                Window.Current.Content = extendedSplash;
            }

            Window.Current.Activate();
        }

        private Shell InitializeShell(LaunchActivatedEventArgs e)
        {
            // Create a Frame to act as the navigation context and navigate to the first page
            Shell shell = new Shell();

            shell.AppFrame.NavigationFailed += OnNavigationFailed;

            if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }

            // Pass the OS theme to the shell
            shell.OSTheme = OSTheme;

            if (shell.AppFrame.Content == null)
            {
                // Loading the Shell containing the Navigation and Titlebar
                shell.AppFrame.Navigate(typeof(MyDrugsListPage), e.Arguments);
            }

            return shell;
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
