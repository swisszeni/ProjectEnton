using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace ProjectEnton.Models
{
    /// <summary>
    /// This static class provides the default values for the taking day times, the theme color and if the Microsoft Band 2 is in use or not
    /// </summary>
    static class Settings
    {
        public static DateTime defaultMorningTakingTime { get; set; }
        public static DateTime defaultLunchTakingTime { get; set; }
        public static DateTime defaultEveningTakingTime { get; set; }
        public static DateTime defaultNightTakingTime { get; set; }
        public static string themeColor { get; set; }
        public static bool microsoftBandIsUsed { get; set; } 
    }
}
