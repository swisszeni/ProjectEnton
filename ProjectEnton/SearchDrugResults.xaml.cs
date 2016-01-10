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

namespace ProjectEnton
{
    /// <summary>
    /// This page shows all drugs that matcht with the user input entered on page "SearchDrug"
    /// </summary>
    public sealed partial class SearchDrugResults : Page
    {
        /// <summary>
        /// The constructor binds the DataContext with the "SearchDrugResultsModel" class 
        /// </summary>
        public SearchDrugResults()
        {
            //Only for tests!!!
            List<Drug> testList = new List<Drug>();
            testList.Add(new Drug(1, "Aspirin", "Ethanol", 1.5, "Tablette", null));
            testList.Add(new Drug(1, "Dafalgan", "Ethanol", 2, "Pulver", null));

            this.InitializeComponent();
            this.DataContext = new SearchDrugResultsModel(testList);

        }

        /// <summary>
        /// Cancel the request for a new drug and navigate back to the "SearchDrug" page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SearchDrug));
        }

        private void AddDrugButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
