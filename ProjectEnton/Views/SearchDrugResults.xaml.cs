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
    /// This page shows all drugs that matcht with the user input entered on page "SearchDrug"
    /// </summary>
    public sealed partial class SearchDrugResults : Page
    {
        /// <summary>
        /// The constructor binds the DataContext with the "SearchDrugResultsModel" class 
        /// </summary>
        public SearchDrugResults()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Override the OnNaviagtedTo and checks if a drug object has been sent. If yes, enable data binding between the view and the drug model.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            List<Drug> resultList = e.Parameter as List<Drug>;
            if (resultList != null)
            {
                this.DataContext = new SearchDrugResultsModel(resultList);
            }
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
