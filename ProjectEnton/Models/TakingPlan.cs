using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEnton.Models
{

    /// <summary>
    /// Declares the enum DayTime to handle at which point of the day the drug needs to be taken. If the value is 1, the drug should be taken.
    /// author: Florian Schnyder (input by MasterChief Raphael Zenhäusern)
    /// </summary>
    [Flags]
    public enum DayTime { Morning = 0x01, Lunch = 0x02, Evening = 0x04, Night = 0x08 };
    //public enum DayTime { Morning = 1, Lunch = 2, Evening = 4, Night = 8 };

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
        /// </summary>
        public TakingPlan()
        {

        }
        /// <summary>
        /// Creates an object that contains a list with all planned takings for a drug, based on the start- and endtime and the day times where the drug needs to be taken
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
        }
    }
}
