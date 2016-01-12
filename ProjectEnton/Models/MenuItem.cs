using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEnton.Models
{
    /// <summary>
    /// MenuItem represents one entry in the navigation menu and the page linked to it.
    /// author: Raphael Zenhäusern
    /// </summary>
    class MenuItem
    {
        public string IconString { get; set; }
        public string IconFontFamily { get; set; }
        public string Title { get; set; }
        public Type DestPage { get; set; }
        public object Arguments { get; set; }

        public MenuItem(string iconString, string iconFontFamily, string title, Type view)
        {
            IconString = iconString;
            IconFontFamily = iconFontFamily;
            Title = title;
            DestPage = view;
        }
    }
}
