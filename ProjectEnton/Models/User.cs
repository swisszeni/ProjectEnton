using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEnton.Models
{
    /// <summary>
    /// this static class contains a list with all durgs that the user has taken so far 
    /// author: Florian Schnyder
    /// </summary>
    static class User
    {
        public static List<Drug> takenDrugs { get; set; } = new List<Drug>();
        
        /*        
        /// <summary>
        /// Singleton pattern to make sure there is only one instance of this class
        /// </summary>
        private static User instance;

        private User()
        {
           
        }

        public static User Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new User();
                }
                return instance;
            }
        }
        */
    }
}
