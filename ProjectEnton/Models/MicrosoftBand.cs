using Microsoft.Band;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEnton.Models
{
    /// <summary>
    /// Singleton Class representing the connection to the Microsoft Band. Capable of checking connection status and reconnect to the device.
    /// author: Raphael Zenhäusern
    /// </summary>
    class MicrosoftBand
    {
        // Uility variables
        private static volatile MicrosoftBand instance;
        private static object syncRoot = new Object();

        /// <summary>
        /// Instance accessor for Singleton
        /// author: Raphael Zenhäusern
        /// </summary>
        public static MicrosoftBand Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new MicrosoftBand();
                    }
                }
                return instance;
            }
        }

        // Private constructor
        private MicrosoftBand() { }

        private IBandInfo _selectedBand;
        public IBandInfo SelectedBand
        {
            get { return _selectedBand; }
            set { _selectedBand = value; }
        }

        private IBandClient _bandClient;
        public IBandClient BandClient
        {
            get { return _bandClient; }
            set { _bandClient = value; }
        }

        /// <summary>
        /// Checks if the connection is still existent.
        /// author: Raphael Zenhäusern
        /// </summary>
        public bool IsConnected
        {
            get {  return BandClient != null; }
        }

        /// <summary>
        /// Searches for devices and selects the first one found.
        /// author: Raphael Zenhäusern
        /// </summary>
        /// <returns></returns>
        public async Task<bool> FindDevicesAsync()
        {
            var bands = await BandClientManager.Instance.GetBandsAsync();
            if (bands != null && bands.Length > 0)
            {
                SelectedBand = bands[0]; // Take the first band
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the active connection element if available. Otherwise trys to reestablish connection.
        /// author: Raphael Zenhäusern
        /// </summary>
        /// <returns></returns>
        public async Task<bool> InitAsync()
        {
            if (IsConnected)
                return true;
            bool found = await FindDevicesAsync();
            if (SelectedBand != null)
            {
                BandClient =
                  await BandClientManager.Instance.ConnectAsync(SelectedBand);
                // Connected!
            }
            return found;
        }

        /// <summary>
        /// Destructor to remove the references to the connected Band to free it.
        /// author: Raphael Zenhäusern
        /// </summary>
        public void Destruct()
        {
            _bandClient = null;
            _selectedBand = null;
        }

    }
}
