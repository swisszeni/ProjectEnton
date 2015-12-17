﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEnton.Models
{
    /// <summary>
    /// The taking class handles the taking of a drug. It contains the needed drug, the planned taking time and the actual taking time.
    /// For each taking, the user is allowed to add an optional comment
    /// </summary>
    class Taking
    {
        public DateTime plannedTaking { get; set; }
        public DateTime actualTaking { get; set; }
        public string comment { get; set; }
        /// <summary>
        /// The state variable shows the state of the taking. There are 4 possible states: planned, postphoned, cancelled and taken
        /// </summary>                               
        public string state { get; set; }

        /// <summary>
        /// Creates an object with a planned taking time and sets the state to "planned"
        /// </summary>
        /// <param name="drug"></param>
        /// <param name="plannedTaking"></param>
        public Taking(DateTime plannedTaking)
        {
            this.plannedTaking = plannedTaking;
            this.state = "planned";
        }

    }
}
