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

namespace ProjectEnton.Views
{
    /// <summary>
    /// This page shows all drugs that that the user has to take or had to take
    /// author: Florian Schnyder
    /// </summary>
    public sealed partial class MyDrugsListPage : Page
    {
        public MyDrugsListPage()
        {
            this.InitializeComponent();

            /// For tests: Add some drugs to the static list of the "User" class
            /// author: Florian Schnyder
            User.takenDrugs.Add(new Drug(2, "Dafalgan", 2, "Paracetamol ", "Tablette", null));
            User.takenDrugs.Add(new Drug(1, "Parazetamol", 1.5, "Kilo", "Tablette", new Picture("www.test.ch", "www.test.ch")));

            this.DataContext = new DrugOverviewModel(User.takenDrugs);
        }

        private void AddMedicamentButton_Click(object sender, RoutedEventArgs e)
        {
            Shell.Current.AppFrame.Navigate(typeof(SearchDrug));
        }
    }
}
