using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEnton.Models
{
    public class MenuItem
    {
        public string Icon { get; set; }
        public string IconSet { get; set; }
        public string Title { get; set; }
        public Type View { get; set; }

        public MenuItem(string icon, string iconSet, string title, Type view)
        {
            Icon = icon;
            IconSet = IconSet;
            Title = title;
            View = view;
        }
    }
}
