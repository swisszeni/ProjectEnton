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
    /// This class connects the drug detail page with the selected. According to the MVVM pattern, page and model need to be treated seperately.
    /// author: Florian Schnyder
    /// </summary>
    class DrugDetailsModel : INotifyPropertyChanged
    {
        private Drug drug;

        /// <summary>
        /// The constructor saves the given drug in a local variable
        /// </summary>
        /// <param name="drug"></param>
        public DrugDetailsModel(Drug drug)
        {
            this.drug = drug;

        }

        /// <summary>
        /// this block is responsible for the connection between the view and the result list
        /// The interface checks if the drug detail page needs to be updated.
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
        /// For all blocks below
        /// get: Return the selected value of a drug. 
        /// set: If the value is equal, no changes will be done. Otherwise the new value will be stored
        /// </summary>
        public string Name
        {
            get { return drug.name; }
            set
            {
                if (drug.name == value)
                {
                    return;
                }
                else
                {
                    drug.name = value;
                    OnPropertyChanged("Name");
                }

            }

        }

        public string DosageUnit
        {
            get { return drug.dosageUnit; }
            set
            {
                if (drug.dosageUnit == value)
                {
                    return;
                }
                else
                {
                    drug.dosageUnit = value;
                    OnPropertyChanged("DosageUnit");
                }

            }
        }

        public double Dosage
        {
            get { return drug.dosage; }
            set
            {
                if (drug.dosage == value)
                {
                    return;
                }
                else
                {
                    drug.dosage = value;
                    OnPropertyChanged("Dosage");
                }

            }
        }

        public string MedicationForm
        {
            get { return drug.medicationForm; }
            set
            {
                if (drug.medicationForm == value)
                {
                    return;
                }
                else
                {
                    drug.medicationForm = value;
                    OnPropertyChanged("MedicationForm");
                }

            }
        }

        //public Picture Picture
        //{
        //    get { return drug.picture; }
        //    set
        //    {
        //        if (drug.picture == value)
        //        {
        //            return;
        //        }
        //        else
        //        {
        //            drug.picture = value;
        //            OnPropertyChanged("Picture");
        //        }

        //    }
        //}
    }
}
