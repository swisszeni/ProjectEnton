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
    /// 
    /// author: Florian Schnyder
    /// </summary>
    public sealed partial class MyDrugsListPage : Page
    {
        public MyDrugsListPage()
        {
            this.InitializeComponent();

            // For tests: Add some drugs to the static list of the "User" class
            User.takenDrugs.Add(new Drug(2, "Perskindol", "Ethanol", 2, "Pulver", null));
            User.takenDrugs.Add(new Drug(1, "Parazetamol", "Ethanol", 1.5, "Tablette", new Picture("www.test.ch", "www.test.ch")));

            this.DataContext = new DrugOverviewModel(User.takenDrugs);
        }

        private void AddMedicamentButton_Click(object sender, RoutedEventArgs e)
        {
            Shell.Current.AppFrame.Navigate(typeof(SearchDrug));
        }
    }
}
