using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace ProjectEnton.Models
{
    /// <summary>
    /// This static class provides the default values for the taking day times (times are hardcoded at the moment), the theme color and if the Microsoft Band 2 is in use or not
    /// author: Florian Schnyder
    /// </summary>
    public static class Settings
    {   
        //Replaced all DateTime variables with TimeSpan, makes more sense ;)
        public static TimeSpan defaultMorningTakingTime { get; set; } = new TimeSpan(6, 0, 0);
        public static TimeSpan defaultLunchTakingTime { get; set; } = new TimeSpan(12, 0, 0);
        public static TimeSpan defaultEveningTakingTime { get; set; } = new TimeSpan(18, 0, 0);
        public static TimeSpan defaultNightTakingTime { get; set; } = new TimeSpan(0, 0, 0);
        public static string themeColor { get; set; }
        public static bool microsoftBandIsUsed { get; set; }
    }
}
