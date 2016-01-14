using Microsoft.Band;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEnton.Models
{
    class MicrosoftBand
    {
        // Uility variables
        private static volatile MicrosoftBand instance;
        private static object syncRoot = new Object();

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

        private MicrosoftBand()
        {

        }

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

        public bool IsConnected
        {
            get {  return BandClient != null; }
        }

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

        public void Destruct()
        {
            _bandClient = null;
            _selectedBand = null;
        }

    }
}
