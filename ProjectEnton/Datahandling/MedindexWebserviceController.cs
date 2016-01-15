using ProjectEnton.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEnton.Datahandling
{
    static class MedindexWebserviceController
    {
        public static async Task<eMediat.PRODUCTPRD[]> GetDrugListAsync(DateTime fromDate)
        {
            eMediat.DownloadProductResponse resp;

            // Had to rewrite line no 19737 in reference.cs
            eMediat.downloadSoapClient downloadClient = new eMediat.downloadSoapClient();
            downloadClient.ClientCredentials.UserName.UserName = "EPN236342@hcisolutions.ch";
            downloadClient.ClientCredentials.UserName.Password = "UMPbDJu7!W";

            eMediat.DownloadProductInput input = new eMediat.DownloadProductInput();
            input.FROMDATE = fromDate;
            input.FILTER = eMediat.DownloadProductInputFILTER.ALL;
            input.INDEX = "medindex";
            resp = await downloadClient.DownloadProductAsync(input);
            
            return resp.PRODUCT.PRD;
        }

        public static async Task<eMediat.PRODUCTPRD[]> GetDrugListSinceLastCheckAsync()
        {
            return await MedindexWebserviceController.GetDrugListAsync(Settings.Instance.LastDBUpdate);
        }
    }
}
