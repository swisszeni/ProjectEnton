using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ProjectEnton.Models
{
    /// <summary>
    /// The Picture class containts an url to a picture and and its uui
    /// </summary>
    class Picture
    {
        public string uri { get; set; }
        public string uuid { get; set; }
        
        /// <summary>
        /// This constructor can add a new image to the project
        /// </summary>
        /// <param name="image"></param>
        public Picture(Image image)
        {
            // Code to generate an uri and uuid for a new added picture
        }

        /// <summary>
        /// This constructor creates an object with an already existing uri and uuid
        /// </summary>
        /// <param name="uri">contains the path where the picture is stored</param>
        /// <param name="uuid">contains an Universally Unique Identifier</param>
        public Picture(string uri, string uuid)
        {
            this.uri = uri;
            this.uuid = uuid;
        }
    }
}
