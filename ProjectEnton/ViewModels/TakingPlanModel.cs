using ProjectEnton.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEnton.ViewModels
{
    class TakingPlanModel : INotifyPropertyChanged
    {
        private List<Drug> morning = new List<Drug>();
        private List<Drug> noon = new List<Drug>();
        private List<Drug> evening = new List<Drug>();
        private List<Drug> night = new List<Drug>();

        /// <summary>
        /// The constructor saves a list for each daytimes takings into local variables. It gets the drug variable out of each taking plan list.
        /// </summary>
        /// <param name="morning"></param>
        /// <param name="noon"></param>
        /// <param name="evening"></param>
        /// <param name="night"></param>
        public TakingPlanModel(List<Drug> morning, List<Drug> noon, List<Drug> evening, List<Drug> night)
        {
            this.morning = morning;
            this.noon = noon;
            this.evening = evening;
            this.night = night;            
            
        }

        /// <summary>
        /// this block is responsible for the connection between the view and the lists
        /// The interface informs the view that there has been a change in the different views
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected internal void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }

        }
        /// <summary>
        /// get: Return the current list. 
        /// set: If the list is equal, no changes will be done. Otherwise the new list will be saved
        /// </summary>
        public List<Drug> Morning
        {
            get { return morning;}
            set
            {
                if (morning == value)
                {
                    return;
                }
                else
                {
                    morning = value;
                    OnPropertyChanged("Morning");
                }

            }
        }

        public List<Drug> Noon
        {
            get { return noon; }
            set
            {
                if (noon == value)
                {
                    return;
                }
                else
                {
                    noon = value;
                    OnPropertyChanged("Noon");
                }

            }
        }

        public List<Drug> Evening
        {
            get { return evening; }
            set
            {
                if (evening == value)
                {
                    return;
                }
                else
                {
                    evening = value;
                    OnPropertyChanged("Evening");
                }

            }
        }

        public List<Drug> Night
        {
            get { return night; }
            set
            {
                if (night == value)
                {
                    return;
                }
                else
                {
                    night = value;
                    OnPropertyChanged("Night");
                }

            }
        }
    }
}
