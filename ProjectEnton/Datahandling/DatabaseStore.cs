using ProjectEnton.eMediat;
using ProjectEnton.Models;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEnton.Datahandling
{
    static class DatabaseStore
    {
        static string dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");

        private static SQLiteConnection DatabaseConnection()
        {
            return new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), dbPath);
        }

        private static void CreateMedDatabaseIfNotExists(SQLiteConnection conn)
        {
            conn.CreateTable<Drug>();
        }

        public async static Task IfNeededUpdateMedDatabaseAsync()
        {
            // Get the time since last update
            TimeSpan sinceLastCheck = Settings.Instance.LastDBUpdate - new DateTime();
            if(sinceLastCheck.Days > 0)
            {
                PRODUCTPRD[] newmeds = new PRODUCTPRD[0];
                try
                {
                    newmeds = await MedindexWebserviceController.GetDrugListSinceLastCheckAsync();
                }
                catch
                {
                    // YOLO Fuck errorhandling...
                }

                List<Drug> drugs = new List<Drug>();
                foreach (PRODUCTPRD prod in newmeds)
                {
                    if(prod.SMNR != null && prod.BNAMD != null)
                    {
                        drugs.Add(new Drug(Int32.Parse(prod.SMNR), prod.BNAMD, prod.DOSE, prod.DOSEU, prod.FORMD, null));
                    }
                }

                using (SQLiteConnection conn = DatabaseStore.DatabaseConnection())
                {
                    DatabaseStore.CreateMedDatabaseIfNotExists(conn);
                    conn.InsertAll(drugs);
                }
                Settings.Instance.LastDBUpdate = DateTime.Today;
            }
        }
    }
}
