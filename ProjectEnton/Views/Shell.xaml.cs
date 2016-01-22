using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Automation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ProjectEnton.Controls;
using ProjectEnton.Views;
using ProjectEnton.Models;
using Windows.UI.ViewManagement;
using Windows.UI;
using System.Threading.Tasks;

namespace ProjectEnton.Views
{
    /// <summary>
    /// The "chrome" layer of the app that provides top-level navigation with
    /// proper keyboarding navigation.
    /// Originally taken from the UWP Samples from Microsoft on GitHub and adjusted for our needs.
    /// author: Raphael Zenhäusern
    /// </summary>
    public sealed partial class Shell : Page
    {
        private Menu menu = new Menu();
        private bool hasPhysicalBackButton;
        private Settings settings;
        private ApplicationTheme osTheme;

        // Background Song
        MediaElement pummeluff;
        private bool _backgroundSongPlaying;

        public static Shell Current = null;

        public Shell()
        {
            this.InitializeComponent();

            this.Loaded += (sender, args) =>
            {
                Current = this;

                this.TogglePaneButton.Focus(FocusState.Programmatic);
            };

            this.ShellSplitView.RegisterPropertyChangedCallback(
                SplitView.DisplayModeProperty,
                (s, a) =>
                {
                    // Ensure that we update the reported size of the TogglePaneButton when the SplitView's
                    // DisplayMode changes.
                    this.CheckTogglePaneButtonSizeChanged();
                });


            SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManager_BackRequested;

            // Check the availability of a physical back button
            hasPhysicalBackButton = ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");

            // Getting a pointer to the Settings
            settings = Settings.Instance;

            NavMenuList.ItemsSource = menu.MenuItems;

            this.InitializeVisualState();
        }


        /// <summary>
        /// Additional methods to tweak the layout to the state expected by the user.
        /// author: Raphael Zenhäusern
        /// </summary>
        public void InitializeVisualState()
        {
            // Subscribe the AppBar for further Changes in AccentColor
            settings.PropertyChanged += Settings_PropertyChanged;

            this.RequestedTheme = settings.AppTheme;

            // If we have no physical back buttons, we display one in the title bar
            if (!hasPhysicalBackButton)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppFrame.CanGoBack ?
                    AppViewBackButtonVisibility.Visible :
                    AppViewBackButtonVisibility.Collapsed;
            }

            // Eventhandler to select the proper menu entry when the page is finished loading
            this.Loaded += (sender, e) =>
            {
                var item = (from p in this.menu.MenuItems where p.DestPage == this.AppFrame.CurrentSourcePageType select p).SingleOrDefault();
                var container = (ListViewItem)NavMenuList.ContainerFromItem(item);

                // While updating the selection state of the item prevent it from taking keyboard focus.  If a
                // user is invoking the back button via the keyboard causing the selected nav menu item to change
                // then focus will remain on the back button.
                if (container != null) container.IsTabStop = false;
                NavMenuList.SetSelectedItem(container);
                if (container != null) container.IsTabStop = true;

            };
        }

        public Frame AppFrame { get { return this.frame; } }

        /// <summary>
        /// Default keyboard focus movement for any unhandled keyboarding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppShell_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            FocusNavigationDirection direction = FocusNavigationDirection.None;
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Left:
                case Windows.System.VirtualKey.GamepadDPadLeft:
                case Windows.System.VirtualKey.GamepadLeftThumbstickLeft:
                case Windows.System.VirtualKey.NavigationLeft:
                    direction = FocusNavigationDirection.Left;
                    break;
                case Windows.System.VirtualKey.Right:
                case Windows.System.VirtualKey.GamepadDPadRight:
                case Windows.System.VirtualKey.GamepadLeftThumbstickRight:
                case Windows.System.VirtualKey.NavigationRight:
                    direction = FocusNavigationDirection.Right;
                    break;

                case Windows.System.VirtualKey.Up:
                case Windows.System.VirtualKey.GamepadDPadUp:
                case Windows.System.VirtualKey.GamepadLeftThumbstickUp:
                case Windows.System.VirtualKey.NavigationUp:
                    direction = FocusNavigationDirection.Up;
                    break;

                case Windows.System.VirtualKey.Down:
                case Windows.System.VirtualKey.GamepadDPadDown:
                case Windows.System.VirtualKey.GamepadLeftThumbstickDown:
                case Windows.System.VirtualKey.NavigationDown:
                    direction = FocusNavigationDirection.Down;
                    break;
            }

            if (direction != FocusNavigationDirection.None)
            {
                var control = FocusManager.FindNextFocusableElement(direction) as Control;
                if (control != null)
                {
                    control.Focus(FocusState.Programmatic);
                    e.Handled = true;
                }
            }
        }

        public ApplicationTheme OSTheme {
            get
            {
                return osTheme;
            }

            set
            {
                osTheme = value;
                // Adjusting the TitleBar
                this.AdjustTitleBarColor();
            }
        }

        /// <summary>
        /// Callback method for when the settings have changed. We are especially interested for the theme event
        /// author: Raphael zenhäusern
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "AppTheme")
            {
                // AppTheme changed, first check if it is set to default and on desktop (herefore we assume the statusbar is missing)
                if(settings.AppTheme == ElementTheme.Default && !ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
                {
                    // Yes, now set to the OS theme
                    this.RequestedTheme = osTheme == ApplicationTheme.Dark ? ElementTheme.Dark : ElementTheme.Light;
                } else
                {
                    // No, we just directly assign the theme
                    this.RequestedTheme = settings.AppTheme;
                }

                // update titleBar Color
                this.AdjustTitleBarColor();

            }
        }

        /// <summary>
        /// Adjust the color of the TitleBar or SysytemTray color. Bartype depends on devicetype. Color depends on active theme.
        /// author: Raphael Zenhäusern
        /// </summary>
        private void AdjustTitleBarColor()
        {
            // Hardcode the themespecific colors becaus the brushes will return the wrong value if method is invoked while app is running
            // Light
            Color titleBarBackgroundColorLight = this.ColorFromARGBHex("#FFE6E6E6");
            Color titleBarButtonHoverColorLight = this.ColorFromARGBHex("#FFB3B3B3");
            Color titleBarButtonPressedColorLight = this.ColorFromARGBHex("#FF9F9F9F");

            Color titleBarForegroundColorLight = Colors.Black;

            // Dark
            Color titleBarBackgroundColorDark = this.ColorFromARGBHex("#FF1F1F1F");
            Color titleBarButtonHoverColorDark = this.ColorFromARGBHex("#FF404040");
            Color titleBarButtonPressedColorDark = this.ColorFromARGBHex("#FF555555");

            Color titleBarForegroundColorDark = Colors.White;

            // Set the colors according to theme
            Color titleBackgroundColor;
            Color titleForegroundColor;
            Color titleButtonHoverColor;
            Color titleButtonPressColor;

            // catch the case where requestedTheme is default on phone
            // This can't occur on Desktop as there it is catched earlyer in the chain
            ElementTheme displayingTheme;
            if (this.RequestedTheme == ElementTheme.Default)
            {
                displayingTheme = osTheme == ApplicationTheme.Dark ? ElementTheme.Dark : ElementTheme.Light;
            }
            else
            {
                displayingTheme = this.RequestedTheme;
            }


            if (displayingTheme == ElementTheme.Light)
            {
                titleBackgroundColor = titleBarBackgroundColorLight;
                titleForegroundColor = titleBarForegroundColorLight;
                titleButtonHoverColor = titleBarButtonHoverColorLight;
                titleButtonPressColor = titleBarButtonPressedColorLight;
            } else
            {
                titleBackgroundColor = titleBarBackgroundColorDark;
                titleForegroundColor = titleBarForegroundColorDark;
                titleButtonHoverColor = titleBarButtonHoverColorDark;
                titleButtonPressColor = titleBarButtonPressedColorDark;
            }



            // If App runs on a PC
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null)
                {

                    titleBar.BackgroundColor = titleBackgroundColor;
                    titleBar.InactiveBackgroundColor = titleBackgroundColor;
                    titleBar.ForegroundColor = titleForegroundColor;
                    titleBar.ButtonBackgroundColor = titleBackgroundColor;
                    titleBar.ButtonInactiveBackgroundColor = titleBackgroundColor;
                    titleBar.ButtonHoverBackgroundColor = titleButtonHoverColor;
                    titleBar.ButtonPressedBackgroundColor = titleButtonPressColor;
                    titleBar.ButtonForegroundColor = titleForegroundColor;
                    titleBar.ButtonHoverForegroundColor = titleForegroundColor;
                    titleBar.ButtonPressedForegroundColor = titleForegroundColor;
                }
            }

            // If App runs on a mobile Device
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    statusBar.BackgroundOpacity = 1;
                    statusBar.BackgroundColor = titleBackgroundColor;
                    statusBar.ForegroundColor = titleForegroundColor;
                }
            }
        }

        /// <summary>
        /// Helper method to convert ARGB Hex strings into color
        /// author: Raphael Zenhäusern
        /// </summary>
        /// <param name="argbhex"></param>
        /// <returns></returns>
        private Color ColorFromARGBHex(string argbhex)
        {
            if(argbhex.Length != 9)
            {
                // String submittes has wrong length
                throw new ArgumentException("Ivalid length for hex string submitted.");
            }

            var color = Windows.UI.Color.FromArgb(
                Convert.ToByte(argbhex.Substring(1, 2), 16),
                Convert.ToByte(argbhex.Substring(3, 2), 16),
                Convert.ToByte(argbhex.Substring(5, 2), 16),
                Convert.ToByte(argbhex.Substring(7, 2), 16));

            if(color == null)
            {
                // Color for some reason not valid, must be invalid string
                throw new ArgumentException("Ivalid hex string submitted.");
            }

            return color;
        }


        public void EnablePummeluffMode(bool enable)
        {
            if (enable)
            {
                // ITS A TRAP!!! Easteregg ;)
                Application.Current.Resources["SystemAccentColor"] = Color.FromArgb(255, 255, 20, 147);

                // Refresh the requested theme to refresh accent color
                ElementTheme tempTheme = Settings.Instance.AppTheme;
                if (tempTheme == ElementTheme.Default)
                {
                    Settings.Instance.AppTheme = osTheme == ApplicationTheme.Dark ? ElementTheme.Light : ElementTheme.Dark;
                } else
                {
                    Settings.Instance.AppTheme = tempTheme == ElementTheme.Dark ? ElementTheme.Light : ElementTheme.Dark;
                }
                Settings.Instance.AppTheme = tempTheme;
                PlayBackgroundSong(true);
            }
            else
            {
                Application.Current.Resources["SystemAccentColor"] = Color.FromArgb(255, 0, 153, 153);

                // Refresh the requested theme to refresh accent color
                ElementTheme tempTheme = Settings.Instance.AppTheme;
                if (tempTheme == ElementTheme.Default)
                {
                    Settings.Instance.AppTheme = osTheme == ApplicationTheme.Dark ? ElementTheme.Light : ElementTheme.Dark;
                }
                else
                {
                    Settings.Instance.AppTheme = tempTheme == ElementTheme.Dark ? ElementTheme.Light : ElementTheme.Dark;
                }
                Settings.Instance.AppTheme = tempTheme;
                PlayBackgroundSong(false);
            }
        }

        private async Task InitBackgroundSong()
        {
            if (pummeluff == null)
            {
                pummeluff = new MediaElement();

                Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
                Windows.Storage.StorageFile file = await folder.GetFileAsync("pummeluff.mp3");
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                pummeluff.SetSource(stream, file.ContentType);

                pummeluff.MediaEnded += pummeluff_SongEnded;
            }
        }

        private async void PlayBackgroundSong(bool play)
        {
            await InitBackgroundSong();
            if (!play && _backgroundSongPlaying)
            {
                pummeluff.Stop();
                _backgroundSongPlaying = false;
            }
            else if (play)
            {
                pummeluff.Play();
                _backgroundSongPlaying = true;
            }
        }

        private void pummeluff_SongEnded(object sender, RoutedEventArgs e)
        {
            _backgroundSongPlaying = false;
        }

        #region BackRequested Handlers

        private void SystemNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            bool handled = e.Handled;
            this.BackRequested(ref handled);
            e.Handled = handled;
        }

        private void BackRequested(ref bool handled)
        {
            // Get a hold of the current frame so that we can inspect the app back stack.

            if (this.AppFrame == null)
                return;

            // Check to see if this is the top-most page on the app back stack.
            if (this.AppFrame.CanGoBack && !handled)
            {
                // If not, set the event to handled and go back to the previous page in the app.
                handled = true;
                this.AppFrame.GoBack();
            }
        }

        #endregion

        #region Navigation

        /// <summary>
        /// Navigate to the Page for the selected <paramref name="listViewItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="listViewItem"></param>
        private void NavMenuList_ItemInvoked(object sender, ListViewItem listViewItem)
        {
            var item = (MenuItem)((NavMenuListView)sender).ItemFromContainer(listViewItem);
            if (item != null)
            {
                if (item.DestPage != null &&
                    item.DestPage != this.AppFrame.CurrentSourcePageType)
                {
                    this.AppFrame.Navigate(item.DestPage, item.Arguments);
                }
            }
        }

        /// <summary>
        /// Navigate to the settings page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            // Enable the Button under any circumstance
            SettingsButton.IsChecked = true;

            var settingsPage = typeof(SettingsPage);
            if (settingsPage != this.AppFrame.CurrentSourcePageType)
            {
                this.AppFrame.Navigate(settingsPage);
            }

            // Unselect any other menu point
            NavMenuList.SetSelectedItem(null);

            // Close the Menu if it is in narrow mode
            if (this.ShellSplitView.IsPaneOpen && (
                this.ShellSplitView.DisplayMode == SplitViewDisplayMode.CompactOverlay ||
                this.ShellSplitView.DisplayMode == SplitViewDisplayMode.Overlay))
            {
                this.ShellSplitView.IsPaneOpen = false;
            }
        }

        /// <summary>
        /// Ensures the nav menu reflects reality when navigation is triggered outside of
        /// the nav menu buttons.
        /// author: Raphael Zenhäusern
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {
            if (e.SourcePageType == typeof(SettingsPage))
            {
                // we are heading towards the settings page, highligt button in any case
                SettingsButton.IsChecked = true;

                // deselect potentially selected other menu entrys
                NavMenuList.SetSelectedItem(null);
            }
            else
            {
                // we are not heading to the settings page, disable the highligting of the button in any case
                SettingsButton.IsChecked = false;

                if (e.NavigationMode == NavigationMode.Back)
                {
                    var item = (from p in this.menu.MenuItems where p.DestPage == e.SourcePageType select p).SingleOrDefault();
                    if (item == null && this.AppFrame.BackStackDepth > 0)
                    {
                        // In cases where a page drills into sub-pages then we'll highlight the most recent
                        // navigation menu item that appears in the BackStack
                        foreach (var entry in this.AppFrame.BackStack.Reverse())
                        {
                            item = (from p in this.menu.MenuItems where p.DestPage == entry.SourcePageType select p).SingleOrDefault();
                            if (item != null)
                                break;
                        }
                    }

                    var container = (ListViewItem)NavMenuList.ContainerFromItem(item);

                    // While updating the selection state of the item prevent it from taking keyboard focus.  If a
                    // user is invoking the back button via the keyboard causing the selected nav menu item to change
                    // then focus will remain on the back button.
                    if (container != null) container.IsTabStop = false;
                    NavMenuList.SetSelectedItem(container);
                    if (container != null) container.IsTabStop = true;
                }

            }
        }


        private void OnNavigatedToPage(object sender, NavigationEventArgs e)
        {
            // After a successful navigation set keyboard focus to the loaded page
            if (e.Content is Page && e.Content != null)
            {
                var control = (Page)e.Content;
                control.Loaded += Page_Loaded;

                // After each navigation step we toggle the visibility of the back button
                if (!hasPhysicalBackButton)
                {
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                        AppFrame.CanGoBack ?
                        AppViewBackButtonVisibility.Visible :
                        AppViewBackButtonVisibility.Collapsed;
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ((Page)sender).Focus(FocusState.Programmatic);
            ((Page)sender).Loaded -= Page_Loaded;
            this.CheckTogglePaneButtonSizeChanged();
        }

        #endregion

        public Rect TogglePaneButtonRect
        {
            get;
            private set;
        }

        /// <summary>
        /// An event to notify listeners when the hamburger button may occlude other content in the app.
        /// The custom "PageHeader" user control is using this.
        /// </summary>
        public event TypedEventHandler<Shell, Rect> TogglePaneButtonRectChanged;

        /// <summary>
        /// Callback when the SplitView's Pane is toggled open or close.  When the Pane is not visible
        /// then the floating hamburger may be occluding other content in the app unless it is aware.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TogglePaneButton_Checked(object sender, RoutedEventArgs e)
        {
            this.CheckTogglePaneButtonSizeChanged();
        }

        /// <summary>
        /// Check for the conditions where the navigation pane does not occupy the space under the floating
        /// hamburger button and trigger the event.
        /// </summary>
        private void CheckTogglePaneButtonSizeChanged()
        {
            if (this.ShellSplitView.DisplayMode == SplitViewDisplayMode.Inline ||
                this.ShellSplitView.DisplayMode == SplitViewDisplayMode.Overlay)
            {
                var transform = this.TogglePaneButton.TransformToVisual(this);
                var rect = transform.TransformBounds(new Rect(0, 0, this.TogglePaneButton.ActualWidth, this.TogglePaneButton.ActualHeight));
                this.TogglePaneButtonRect = rect;
            }
            else
            {
                this.TogglePaneButtonRect = new Rect();
            }

            var handler = this.TogglePaneButtonRectChanged;
            if (handler != null)
            {
                // handler(this, this.TogglePaneButtonRect);
                handler.DynamicInvoke(this, this.TogglePaneButtonRect);
            }
        }

        /// <summary>
        /// Enable accessibility on each nav menu item by setting the AutomationProperties.Name on each container
        /// using the associated Label of each item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NavMenuItemContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (!args.InRecycleQueue && args.Item != null && args.Item is MenuItem)
            {
                args.ItemContainer.SetValue(AutomationProperties.NameProperty, ((MenuItem)args.Item).Title);
            }
            else
            {
                args.ItemContainer.ClearValue(AutomationProperties.NameProperty);
            }
        }
    }
}
