using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEnton.Models
{
    /// <summary>
    ///  The Drug class represents a drug with all needed attributes for this app Test
    /// </summary>
    class Drug  
    {
        public int id { get; set; }
        public string name { get; set; }
        public string activeComponent { get; set; }
        public float dosage { get; set; }
        public string medicationForm { get; set; }
        public Picture picture { get; set; }

        /// <summary>
        /// This constructor creates an object with the drugs name, its active component, the medication form and a picture
        /// </summary>
        /// <param name="id">Contanis the drugs id. Uniqe identifier</param>
        /// <param name="name">Contains the drugs name</param>
        /// <param name="activeComponent">Contains the active component of the drug</param>
        /// <param name="dosage">Shows the user the dosage of the needed drug</param>
        /// <param name="medicationForm">Contains the infomation of the drugs form (powder....)</param>
        /// <param name="picture"></param>
        public Drug(int id, string name, string activeComponent, float dosage, string medicationForm, Picture picture)
        {
            this.id = id;
            this.name = name;
            this.activeComponent = activeComponent;
            this.dosage = dosage;
            this.medicationForm = medicationForm;
            this.picture = picture;
        }

    }
}
