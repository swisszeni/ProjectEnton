using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;

namespace ProjectEnton.Models
{
    /// <summary>
    /// This static class provides the default values for the taking day times (times are hardcoded at the moment), the theme color and if the Microsoft Band 2 is in use or not
    /// Pattern is singleton to maintain one set of settings. And the singleton is even multithread safe :)
    /// author: Florian Schnyder, Raphael Zenhäusern
    /// </summary>
    public class Settings : INotifyPropertyChanged
    {
        // Uility variables
        private static volatile Settings instance;
        private static object syncRoot = new Object();

        // local setting vars
        public TimeSpan defaultMorningTakingTime { get; set; }
        public TimeSpan defaultLunchTakingTime { get; set; }
        public TimeSpan defaultEveningTakingTime { get; set; }
        public TimeSpan defaultNightTakingTime { get; set; }
        private ElementTheme appTheme;
        public bool useMicrosoftBand { get; set; }

        /// <summary>
        /// Constructor marked private for singleton instance. Instanciates the setting vars from the file system.
        /// author: Raphael Zenhäusern
        /// </summary>
        private Settings()
        {
            this.appTheme = ApplicationData.Current.LocalSettings.Values.ContainsKey("AppTheme")
                            ? (ElementTheme)ApplicationData.Current.LocalSettings.Values["AppTheme"]
                            : ElementTheme.Default;

            this.defaultMorningTakingTime = ApplicationData.Current.LocalSettings.Values.ContainsKey("DefaultMorningTakingTime")
                ? (TimeSpan)ApplicationData.Current.LocalSettings.Values["DefaultMorningTakingTime"]
                : new TimeSpan(6, 0, 0);

            this.defaultLunchTakingTime = ApplicationData.Current.LocalSettings.Values.ContainsKey("DefaultLunchTakingTime")
                ? (TimeSpan)ApplicationData.Current.LocalSettings.Values["DefaultLunchTakingTime"]
                : new TimeSpan(12, 0, 0);

            this.defaultEveningTakingTime = ApplicationData.Current.LocalSettings.Values.ContainsKey("DefaultEveningTakingTime")
                ? (TimeSpan)ApplicationData.Current.LocalSettings.Values["DefaultEveningTakingTime"]
                : new TimeSpan(18, 0, 0);

            this.defaultNightTakingTime = ApplicationData.Current.LocalSettings.Values.ContainsKey("DefaultNightTakingTime")
                ? (TimeSpan)ApplicationData.Current.LocalSettings.Values["DefaultNightTakingTime"]
                : new TimeSpan(0, 0, 0);

            this.useMicrosoftBand = ApplicationData.Current.LocalSettings.Values.ContainsKey("UseMicrosoftBand")
                ? (bool)ApplicationData.Current.LocalSettings.Values["UseMicrosoftBand"]
                : false;
        }

        /// <summary>
        /// Accessor for the singleton instance, creates new if not yet constructed
        /// author: Raphae zenhäusern
        /// </summary>
        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Settings();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Accessors for the Apptheme
        /// </summary>
        public ElementTheme AppTheme
        {
            get
            {
                return this.appTheme;
            }

            set
            {
                ApplicationData.Current.LocalSettings.Values["AppTheme"] = (int)value;
                this.appTheme = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
