using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Popups;

namespace ProjectEnton.Models
{

    /// <summary>
    /// Declares the enum DayTime to handle at which point of the day the drug needs to be taken. If the value is 1, the drug should be taken.
    /// author: Florian Schnyder (input by MasterChief Raphael Zenhäusern)
    /// </summary>
    [Flags]
    public enum DayTime { Morning = 0x01, Lunch = 0x02, Evening = 0x04, Night = 0x08 };

    /// <summary>
    /// The taking plan contains all planned takings for a drug. It also contanis the day times (morning, lunch, evening, night) for a taking. If a day time is active, it contains the time for the reminder
    /// It also contains the option to pause the taking for a certain period of time
    /// author: Florian Schnyder
    /// </summary>

    class TakingPlan
    {
        public List<Taking> taking { get; set; }
        public Drug drug { get; set; }
        public DateTime takingStart { get; set; }
        public DateTime takingEnd { get; set; }
        public DateTime pauseStart { get; set; }
        public DateTime pauseEnd { get; set; }
        public DayTime takingDayTime { get; set; }
        public Dictionary<DayTime, DateTime> takingTime;

        /// <summary>
        /// Empty constructor
        /// author: Florian Schnyder
        /// </summary>
        public TakingPlan()
        {

        }
        /// <summary>
        /// Creates an object that contains a list with all planned takings for a drug, based on the start- and endtime and the day times where the drug needs to be taken
        /// author: Florian Schnyder
        /// </summary>
        /// <param name="drug">Contanis the drug object for which the taking plan has been created</param>
        /// <param name="takingStart">Startpoint for drug reminder</param>
        /// <param name="takingEnd">Endpoint for a drug reminder, if the enddate is alredy known</param>
        /// <param name="takingDayTime">Contanis the information at which points of the day (morning, lunch, evening, night) the drug should be taken</param>
        /// <param name="takingTime">Contanis the information at what time i have to take the drug</param>
        public TakingPlan(Drug drug, DateTime takingStart, DateTime takingEnd, DayTime takingDayTime, Dictionary<DayTime, DateTime> takingTime)
        {
            this.drug = drug;
            this.takingStart = takingStart;
            this.takingEnd = takingEnd;
            this.takingDayTime = takingDayTime;
            this.takingTime = takingTime;
            
            //creates daily notifications for the drug if the flag in DayTime is set. At the moment only for one week and without checking the takingEndDate....
            if (drug != null && this.takingStart != null && this.takingStart >= DateTime.Now)
            {
                Settings settings = Settings.Instance;

                //checks if the flag at the specified position is set. If yes, the durg will be added to the realted list
                if (takingDayTime.HasFlag(DayTime.Morning))
                {
                    //create an XML file with the implemented method
                    XmlDocument xml = createXML();

                    //add temporary variable to count one week
                    int temp = 0;

                    //Get the current year, month and day, afterwards, get the needed data form the settings class (just for prototype, afterwards, it will check the drugs own daily taking times)
                    DateTime plannedTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, settings.DefaultMorningTakingTime.Hours, settings.DefaultMorningTakingTime.Minutes, 0);

                    while (temp < 7)
                    {
                        plannedTime = plannedTime.AddDays(1);
                        // add the notification to the operating system
                        ScheduledToastNotification toast = new ScheduledToastNotification(xml, plannedTime);
                        toast.Id = drug.ToString() + temp;
                        ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
                        temp++;
                    }

                }

                if (takingDayTime.HasFlag(DayTime.Lunch))
                {
                    //create an XML file with the implemented method
                    XmlDocument xml = createXML();

                    //add temporary variable to count one week
                    int temp = 0;

                    //Get the current year, month and day, afterwards, get the needed data form the settings class (just for prototype, afterwards, it will check the drugs own daily taking times)
                    DateTime plannedTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, settings.DefaultLunchTakingTime.Hours, settings.DefaultLunchTakingTime.Minutes, 0);

                    while (temp < 7)
                    {
                        plannedTime = plannedTime.AddDays(1);
                        // add the notification to the operating system
                        ScheduledToastNotification toast = new ScheduledToastNotification(xml, plannedTime);
                        toast.Id = drug.ToString() + temp;
                        ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
                        temp++;
                    }

                }

                if (takingDayTime.HasFlag(DayTime.Evening))
                {
                    //create an XML file with the implemented method
                    XmlDocument xml = createXML();

                    //add temporary variable to count one week
                    int temp = 0;

                    //Get the current year, month and day, afterwards, get the needed data form the settings class (just for prototype, afterwards, it will check the drugs own daily taking times)
                    DateTime plannedTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, settings.DefaultEveningTakingTime.Hours, settings.DefaultEveningTakingTime.Minutes, 0);

                    while (temp < 7)
                    {
                        plannedTime = plannedTime.AddDays(1);
                        // add the notification to the operating system
                        ScheduledToastNotification toast = new ScheduledToastNotification(xml, plannedTime);
                        toast.Id = drug.ToString() + temp;
                        ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
                        temp++;
                    }

                }

                if (takingDayTime.HasFlag(DayTime.Night))
                {
                   
                    XmlDocument xml = createXML();

                    //add temporary variable to count one week
                    int temp = 0;

                    //Get the current year, month and day, afterwards, get the needed data form the settings class (just for prototype, afterwards, it will check the drugs own daily taking times)
                    DateTime plannedTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, settings.DefaultNightTakingTime.Hours, settings.DefaultNightTakingTime.Minutes, 0);

                    while (temp < 7)
                    {
                        plannedTime = plannedTime.AddDays(1);
                        // add the notification to the operating system
                        ScheduledToastNotification toast = new ScheduledToastNotification(xml, plannedTime);
                        toast.Id = drug.ToString() + temp;
                        ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
                        temp++;
                    }
                }
            }
        }

        // create an XML file that contains the needed information for the notification
        private XmlDocument createXML()
        {
            //First, we are creating a template with the drugs name
            StringBuilder template = new StringBuilder();
            template.Append("<toast><visual version='2'><binding template='ToastText02'>");
            template.AppendFormat("<text id='2'>{0}</text>", "Einnahmeerinnerung");
            template.AppendFormat("<text id='1'>{0}</text>", drug.name);
            template.Append("</binding></visual></toast>");
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(template.ToString());
            return xml;

        }

                
    }
    /*
    /// <summary>
    /// sends a daily notification to the operation system (currently only for one week)
    /// </summary>
    private void addNotifications()
    {   
        //First, we are creating a template with the drugs name
        StringBuilder template = new StringBuilder();
        template.Append("<toast><visual version='2'><binding template='ToastText02'>");
        template.AppendFormat("<text id='2'>{0}</text>", "Einnahmeerinnerung");
        template.AppendFormat("<text id='1'>{0}</text>", drug.name);
        template.Append("</binding></visual></toast>");
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(template.ToString());
        DateTimeOffset plannedTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,)
        //F
        ScheduledToastNotification toast = new ScheduledToastNotification(xml, plannedTime, new TimeSpan(0, 5, 0), 2);
        toast.Id = drug.ToString();
        ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
    }
    */
}

