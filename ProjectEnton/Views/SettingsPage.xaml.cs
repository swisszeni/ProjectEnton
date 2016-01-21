using ProjectEnton.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Band;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace ProjectEnton.Views
{
    /// <summary>
    /// This page deploys the code for handling all SettingPage intputs.
    /// author: Florian Schnyder
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private Settings settings;

        public SettingsPage()
        {
            settings = Settings.Instance;

            this.InitializeComponent();

            this.SetVisualsFromSettingVars();
        }

        /// <summary>
        /// Sets the values for the views with the settings values
        /// author: Raphael Zenhäusern
        /// </summary>
        private void SetVisualsFromSettingVars()
        {
            switch (settings.AppTheme)
            {
                case (ElementTheme.Light):
                    ThemeColorWhite.IsChecked = true;
                    break;
                case (ElementTheme.Dark):
                    ThemeColorBlack.IsChecked = true;
                    break;
                default:
                    ThemeColorSystem.IsChecked = true;
                    break;
            }

            //Gets the current time values stored within the static Settings.cs class 
            MorningTimePicker.Time = settings.DefaultMorningTakingTime;
            LunchTimePicker.Time = settings.DefaultLunchTakingTime;
            EveningTimePicker.Time = settings.DefaultEveningTakingTime;
            NightTimePicker.Time = settings.DefaultNightTakingTime;

            if (settings.UseMicrosoftBand)
            {
                ConnectionMicrosoftBand.IsEnabled = false;
                this.TryRestoringBandVisuals();
            } else
            {
                ConnectionMicrosoftBand.IsOn = false;
            }
        }

        /// <summary>
        /// Initializer to try to reestablish the connection to the Microsoft Band. If not available, ther won't be any errors displayed.
        /// author: Raphael Zenhäusern
        /// </summary>
        private async void TryRestoringBandVisuals()
        {
            // Microsoft Band is used, try getting the connection info
            // Get the Band singleton
            MicrosoftBand band = MicrosoftBand.Instance;

            // Try getting a Band
            try
            {
                bool bandFound = await band.InitAsync();
                ConnectionMicrosoftBand.IsOn = bandFound;
                if(bandFound)
                {
                    DisplayBandConnected(band.SelectedBand);
                }
            }
            catch
            {
                // Error while retrieving... fuck it
                ConnectionMicrosoftBand.IsOn = false;
            }
            finally
            {
                ConnectionMicrosoftBand.IsEnabled = true;
            }
        }


        /// <summary>
        /// This method checks if the new morning default time is within the range between 4 AM and 9.59 AM. 
        /// author: Florian Schnyder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MorningTimePicker_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            TimeSpan minTime = new TimeSpan(4, 0, 0);
            TimeSpan maxTime = new TimeSpan(9, 59, 59);

            /// <summary>
            /// If the new entered time is within the given range, it will be saved. If not, a popup informs the user that the time entered is not allowed and the previous time won't change.
            /// </summary>
            if (MorningTimePicker.Time < minTime || MorningTimePicker.Time > maxTime)
            {
                var dialog = new MessageDialog("Gewählte Zeit nicht gültig.\nErlaubte Zeitspanne: 04:00 - 09:59");
                await dialog.ShowAsync();

                MorningTimePicker.Time = settings.DefaultMorningTakingTime;
            }
            else
            {
                settings.DefaultMorningTakingTime = MorningTimePicker.Time;
            }
        }

        /// <summary>
        /// This method checks if the new lunch default time is within the range between 10 AM and 3.59 PM. 
        /// author: Florian Schnyder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LunchTimePicker_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            TimeSpan minTime = new TimeSpan(10, 0, 0);
            TimeSpan maxTime = new TimeSpan(15, 59, 59);

            /// <summary>
            /// If the new entered time is within the given range, it will be saved. If not, a popup informs the user that the time entered is not allowed and the previous time won't change.
            /// author: Florian Schnyder
            /// </summary>
            if (LunchTimePicker.Time < minTime || LunchTimePicker.Time > maxTime)
            {
                var dialog = new MessageDialog("Gewählte Zeit nicht gültig.\nErlaubte Zeitspanne: 10:00 - 15:59");
                await dialog.ShowAsync();

                LunchTimePicker.Time = settings.DefaultLunchTakingTime;
            }
            else
            {
                settings.DefaultLunchTakingTime = LunchTimePicker.Time;
            }
        }

        /// <summary>
        /// This method checks if the new evening default time is within the range between 16 PM and 21.59 PM. 
        /// author: Florian Schnyder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void EveningTimePicker_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            TimeSpan minTime = new TimeSpan(16, 0, 0);
            TimeSpan maxTime = new TimeSpan(21, 59, 59);

            /// <summary>
            /// If the new entered time is within the given range, it will be saved. If not, a popup informs the user that the time entered is not allowed and the previous time won't change.
            /// author: Florian Schnyder
            /// </summary>
            if (EveningTimePicker.Time < minTime || EveningTimePicker.Time > maxTime)
            {
                var dialog = new MessageDialog("Gewählte Zeit nicht gültig.\nErlaubte Zeitspanne: 16:00 - 21:59");
                await dialog.ShowAsync();

                EveningTimePicker.Time = settings.DefaultEveningTakingTime;
            }
            else
            {
                settings.DefaultEveningTakingTime = EveningTimePicker.Time;
            }
        }

        /// <summary>
        /// This method checks if the new night default time is within the range between 22 PM and 03.59 AM. 
        /// author: Florian Schnyder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void NightTimePicker_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            TimeSpan minTime = new TimeSpan(22, 0, 0);
            TimeSpan maxTime = new TimeSpan(3, 59, 59);

            /// <summary>
            /// If the new entered time is within the given range, it will be saved. If not, a popup informs the user that the time entered is not allowed and the previous time won't change.
            /// author: Florian Schnyder
            /// </summary>
            if ((NightTimePicker.Time < minTime && NightTimePicker.Time > maxTime) || (NightTimePicker.Time > maxTime && NightTimePicker.Time < minTime))
            {
                var dialog = new MessageDialog("Gewählte Zeit nicht gültig.\nErlaubte Zeitspanne: 22:00 - 3:59");
                await dialog.ShowAsync();

                NightTimePicker.Time = settings.DefaultNightTakingTime;
            }
            else
            {
                settings.DefaultNightTakingTime = NightTimePicker.Time;
            }
        }

        /// <summary>
        /// Actionhandler to set the according Theme in the settings.
        /// author: Raphael Zenhäusern
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThemeColor_Checked(object sender, RoutedEventArgs e)
        {
            var radio = sender as RadioButton;
            ElementTheme tempTheme = (ElementTheme)Convert.ToInt16(radio.Tag);
            if (settings.AppTheme != tempTheme)
            {
                settings.AppTheme = tempTheme;
            }
        }

        /// <summary>
        /// Actionhandler to trigger the use of the MS Band in the settings.
        /// author: Raphael Zenhäusern
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectionMicrosoftBand_Toggled(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleSwitch;

            if(toggle.IsOn)
            {
                // Ok user activated Use of Band, search for them
                SearchMicrosoftBandsConnectedAsync();
            } else
            {
                RemoveMicrosoftBand();
            }
        }

        #region MicrosoftBandLogic

        /// <summary>
        /// Method to actively search for Bands and connect to one if available
        /// author: Raphael Zenhäusern
        /// </summary>
        private async void SearchMicrosoftBandsConnectedAsync()
        {
            // Disable the switch
            ConnectionMicrosoftBand.IsEnabled = false;
            // Display the search indicator
            this.DisplayBandSearchingIndicator(true);

            // Get the Band singleton
            MicrosoftBand band = MicrosoftBand.Instance;

            // Try getting a Band
            try
            {
                bool bandFound = await band.InitAsync();
                if(!bandFound)
                {
                    // No bands available
                    DisplayNoBandsFoundAsync();
                } else
                {
                    // Connected to band!
                    // Set the resource as in use and required
                    using (IBandClient bandClient = band.BandClient)
                    {
                        DisplayBandConnected(band.SelectedBand);
                        settings.UseMicrosoftBand = true;
                    }
                }
            }
            catch (BandException ex)
            {
                DisplayBandConnectionFailureAsync(ex);
            } finally
            {
                // Enable the switch
                ConnectionMicrosoftBand.IsEnabled = true;
            }
        }

        /// <summary>
        /// Removes the active connection to the Microsoft Band and reflects this in the interface.
        /// author: Raphael Zenhäusern
        /// </summary>
        private void RemoveMicrosoftBand()
        {
            // Get the Band singleton
            MicrosoftBand band = MicrosoftBand.Instance;
            // Destruct the connection
            band.Destruct();
            settings.UseMicrosoftBand = false;
            BandConnectionInfotext.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Utility to display an error for the connection failure to the Band.
        /// author: Raphael Zenhäusern
        /// </summary>
        /// <param name="ex"></param>
        private async void DisplayBandConnectionFailureAsync(BandException ex)
        {
            var dialog = new MessageDialog("Bei der Verbindung mit dem Microsoft Band ist ein Fehler aufgetreten.", "Fehler bei Verbindung");
            await dialog.ShowAsync();
            DisplayBandSearchingIndicator(false);
            ConnectionMicrosoftBand.IsOn = false;
        }

        /// <summary>
        /// Utility to reflect the connected band in the interface.
        /// author: Raphael Zenhäusern
        /// </summary>
        /// <param name="bandInfo"></param>
        private void DisplayBandConnected(IBandInfo bandInfo)
        {
            BandConnectionInfotext.Text = "Verbunden mit " + bandInfo.Name;
            DisplayBandSearchingIndicator(false);
            BandConnectionInfotext.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Utility to display an error if there couldn't be any Microsoft Band found for various reasons.
        /// author: Raphael Zenhäusern
        /// </summary>
        private async void DisplayNoBandsFoundAsync()
        {
            var dialog = new MessageDialog("Es wurde kein Microsoft Band gefunden, stellen Sie sicher, dass Bluetooth aktiviert und das Band verbunden ist.", "Nicht gefunden");
            await dialog.ShowAsync();
            DisplayBandSearchingIndicator(false);
            ConnectionMicrosoftBand.IsOn = false;
        }

        /// <summary>
        /// Shows or hides the search indicator for Microsoft Band
        /// author: Raphael Zenhäusern
        /// </summary>
        /// <param name="display"></param>
        private void DisplayBandSearchingIndicator(bool display)
        {
            BandSearchInProgressGrid.Visibility = display ? Visibility.Visible : Visibility.Collapsed;
            BandSearchInProgressRing.IsActive = display;

            // If indicator is shown, Scrolling so Indicator is always visible
            if(display)
            {
                SettingsScrollView.UpdateLayout();
                SettingsScrollView.ChangeView(0.0f, SettingsScrollView.ScrollableHeight, 1);
            }
        }


        #endregion
    }
}

