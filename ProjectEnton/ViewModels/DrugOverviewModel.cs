using ProjectEnton.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ProjectEnton.ViewModels
{
    /// <summary>
    /// This class connects the DrugOverview page with the drugs stored in the User.cs class. According to the MVVM pattern, page and model need to be treated seperately.
    /// author: Florian Schnyder
    /// </summary>
    class DrugOverviewModel : INotifyPropertyChanged
    {
        private List<Drug> allTakenDrugs = new List<Drug>();

        /// <summary>
        /// The constructor saves the given list of drugs in a local variable
        /// </summary>
        /// <param name="drugs"></param>
        public DrugOverviewModel(List<Drug> drugs)
        {
            allTakenDrugs = drugs;
        }

        /// <summary>
        /// this block is responsible for the connection between the view and the result list
        /// The interface informs the view that a new search was done and that the "DrugOverview" ListView needs to be updated
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
        public List<Drug> AllTakenDrugs
        {
            get { return allTakenDrugs; }
            set
            {
                if (allTakenDrugs == value)
                {
                    return;
                }
                else
                {
                    /*foreach (var drug in currentSearch)
                    {
                        //ResultList.Items.Add(drug.name);
                    }*/
                    allTakenDrugs = value;
                    OnPropertyChanged("DrugOverview");
                }

            }

        }

    }
}

