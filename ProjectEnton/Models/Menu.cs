using ProjectEnton.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEnton.Models
{
    /// <summary>
    /// Menu represents a collection of menu items for display in the navigation menu. Keep in mind, that the settings entry is generated separately. Containerclass
    /// author: Raphael Zenhäusern
    /// </summary>
    class Menu
    {
        public List<MenuItem> MenuItems { get; set; }
        public Menu()
        {
            MenuItems = new List<MenuItem>();
            MenuItems.Add(new MenuItem("\U0001F48A", "Segoe UI Symbol", "Meine Medikamente", typeof(MyDrugsListPage)));
            MenuItems.Add(new MenuItem("\uE823", "Segoe MDL2 Assets", "Medikationsplan", typeof(TakingPlanPage))); 
        }
    }
}