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
    /// This page is responsible for the drug search. The user can either enter a text or scan a barcode to get the needed drug.
    /// author: Florian Schnyder, Dominique Walter, Raphael Zenhäusern
    /// </summary>
    public sealed partial class SearchDrug : Page
    {
        public SearchDrug()
        {
            this.InitializeComponent();
            
        }


        /// <summary>
        /// TRemoves all entered characters from the input field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelDrugButton_Click(object sender, RoutedEventArgs e)
        {
            DrugsearchTextBox.Text = "";
        }

        /// <summary>
        /// Search all drugs with the given user input in the "DrugsearchTextBox" Field. Navigates the user to a new page. The new page shows all the drugs which could be found with the given input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchDrugButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SearchDrugResults));
        }
    }
}
