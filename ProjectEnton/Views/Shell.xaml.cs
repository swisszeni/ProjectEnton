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

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

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

            // If we have no physical back buttons, we display one in the title bar
            if (!hasPhysicalBackButton)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppFrame.CanGoBack ?
                    AppViewBackButtonVisibility.Visible :
                    AppViewBackButtonVisibility.Collapsed;
            }

            // Adjusting the TitleBar
            this.AdjustTitleBarColor();

            NavMenuList.ItemsSource = menu.MenuItems;
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

        /// <summary>
        /// Adjust the color of the TitleBar or SysytemTray color. Bartype depends on devicetype. Color depends on active theme.
        /// author: Raphael Zenhäusern
        /// </summary>
        private void AdjustTitleBarColor()
        {

            // If App runs on a PC
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null)
                {

                    /*(Color)Application.Current.Resources["Blue"];
                    titleBar.ButtonBackgroundColor = Colors.DarkBlue;
                    titleBar.ButtonForegroundColor = Colors.White;
                    titleBar.BackgroundColor = Colors.Blue;
                    titleBar.ForegroundColor = Colors.White;*/
                }
            }

            // If App runs on a mobile Device
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    /*statusBar.BackgroundOpacity = 1;
                    statusBar.BackgroundColor = Colors.DarkBlue;
                    statusBar.ForegroundColor = Colors.White;*/
                }
            }
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
