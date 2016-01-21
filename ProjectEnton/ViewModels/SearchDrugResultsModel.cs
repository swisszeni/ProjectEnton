using ProjectEnton.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEnton.ViewModels
{
    /// <summary>
    /// This class connects the SearchDrugResult page with the drugs found in the database. According to the MVVM pattern, page and model need to be treated seperately.
    /// author: Florian Schnyder
    /// </summary>
    class SearchDrugResultsModel : INotifyPropertyChanged
    {
        private List<Drug> currentSearch = new List<Drug>();

        /// <summary>
        /// The constructor saves the given list of drugs in a local variable
        /// author: Florian Schnyder
        /// </summary>
        /// <param name="drugs"></param>
        public SearchDrugResultsModel(List<Drug> drugs)
        {
            currentSearch = drugs;
        }

        /// <summary>
        /// this block is responsible for the connection between the view and the result list
        /// The interface informs the view that a new search was done and that the "ResultList" ListView needs to be updated
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
        /// author: Florian Schnyder
        /// </summary>
        public List<Drug> CurrentSearch
        {
            get { return currentSearch; }
            set
            {
                if (currentSearch == value)
                {
                    return;
                }
                else
                {
                    /*foreach (var drug in currentSearch)
                    {
                        //ResultList.Items.Add(drug.name);
                    }*/
                    currentSearch = value;
                    OnPropertyChanged("ResultList");
                }

            }
        }


    }
}
