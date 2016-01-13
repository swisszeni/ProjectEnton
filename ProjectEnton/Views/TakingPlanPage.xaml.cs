using ProjectEnton.Models;
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
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class TakingPlanPage : Page
    {
        public TakingPlanPage()
        {
            this.InitializeComponent();

            /// Add todays date (with the days name) to the view
            TodaysDate.Text = DateTime.Now.ToString("dddd, dd.MM.yyy");

            ///For tests: generates a TakingPlan object
            Drug d = new Drug(2, "Dafalgan", "Ethanol", 2, "Pulver", null);
            TakingPlan dafalgan = new TakingPlan(d, new DateTime(2016,1,1), new DateTime(2016, 12, 12), new DayTime(), null);

            // Add the drug to the page (morning, noon and evening)
            morningListView.Items.Add(dafalgan.drug.ToString());
            noonListView.Items.Add(dafalgan.drug.ToString());
            eveningListView.Items.Add(dafalgan.drug.ToString());

            // Check if a Listview ist empty. If a view is empty, it won't be displayed
            if(morningListView.Items.Count == 0) { MorningPanel.Visibility = Visibility.Collapsed; }
            if (noonListView.Items.Count == 0) { NoonPanel.Visibility = Visibility.Collapsed; }
            if (eveningListView.Items.Count == 0) { EveningPanel.Visibility = Visibility.Collapsed; }
            if (nightListView.Items.Count == 0) { NightPanel.Visibility = Visibility.Collapsed; }

            //test with enum, but could not get the digits out of it.... :(
            /*
            int morningAndNight = 1001;
            dafalgan.takingDayTime. = (DayTime)morningAndNight;
            
            //Check if the transformation 
            int test = (int)dafalgan.takingDayTime;
           
            byte[] result = BitConverter.GetBytes(test);

            morningListView.Items.Add(Enum.GetValues(typeof(DayTime)));
            foreach(var a in result)
            {
                morningListView.Items.Add(a);
            }
            if (result[0] == 1)
            {
                morningListView.Items.Add(dafalgan.drug.ToString());
            }

            if (result[1] == 1)
            {
                noonListView.Items.Add(dafalgan.drug.ToString());
            }

            if (result[2] == 1)
            {
                eveningListView.Items.Add(dafalgan.drug.ToString());
            }

            if (result[3] == 1)
            {
                nightListView.Items.Add(dafalgan.drug.ToString());
            }*/
        }
    }
}
