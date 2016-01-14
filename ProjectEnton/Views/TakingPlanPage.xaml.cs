using ProjectEnton.Models;
using ProjectEnton.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace ProjectEnton.Views
{
    /// <summary>
    /// This class displays an overview for all drug takings for a specific day.
    /// At the moment, it only shows todays takings
    /// author: Florian Schnyder
    /// </summary>
    public sealed partial class TakingPlanPage : Page
    {
        public TakingPlanPage()
        {
            this.InitializeComponent();
            
            /// Add todays date (with the days name) to the view
            /// 
            TodaysDate.Text = DateTime.Now.ToString("dddd, dd.MM.yyy");

            ///For binding tests!!!! generates drug lists object and add an item
            List<Drug> morning = new List<Drug>();
            List<Drug> noon = new List<Drug>();
            List<Drug> evening = new List<Drug>();
            List<Drug> night = new List<Drug>();
            
            
            Drug morgen = new Drug(2, "Medi für Morgen", "Ethanol", 2, "Pulver", null);
            morning.Add(morgen);

            Drug mittag = new Drug(2, "Medi für Mittag", "Malz", 2, "Tablette", null);
            noon.Add(mittag);

            Drug abend = new Drug(2, "Medi für Abend", "Hopfen", 2, "Lösung", null);
            evening.Add(abend);

            Drug nacht = new Drug(2, "Medi für Nacht", "Gärste", 2, "Tablette", null);
            night.Add(nacht);

            /// Add the lists to the TakingPlanModel constructor and bind it with the DataContext
            this.DataContext = new TakingPlanModel(morning, noon, evening, night);

            ///Older tests, could be reactivated in the future...
            /* Add the drug to the page (morning, noon and evening)
            morningListView.Items.Add(dafalgan.drug.ToString());
            nightListView.Items.Add(dafalgan.drug.ToString());
            eveningListView.Items.Add(dafalgan.drug.ToString());
            

            //checks if the flag at the specified position is set. If yes, the durg will be added to the realted list
            if (dafalgan.takingDayTime.HasFlag(DayTime.Morning))
            {
                morningListView.Items.Add(dafalgan.drug.ToString());
            }

            if (dafalgan.takingDayTime.HasFlag(DayTime.Lunch))
            {
                noonListView.Items.Add(dafalgan.drug.ToString());
            }

            if (dafalgan.takingDayTime.HasFlag(DayTime.Evening))
            {
                eveningListView.Items.Add(dafalgan.drug.ToString());
            }

            if (dafalgan.takingDayTime.HasFlag(DayTime.Night))
            {
                nightListView.Items.Add(dafalgan.drug.ToString());
            }

            // Check if a Listview ist empty. If a view is empty, it won't be displayed
            if (morningListView.Items.Count == 0) { MorningPanel.Visibility = Visibility.Collapsed; }
            if (noonListView.Items.Count == 0) { NoonPanel.Visibility = Visibility.Collapsed; }
            if (eveningListView.Items.Count == 0) { EveningPanel.Visibility = Visibility.Collapsed; }
            if (nightListView.Items.Count == 0) { NightPanel.Visibility = Visibility.Collapsed; }
            */

        }
    }
}


