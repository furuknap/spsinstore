using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using System.Net;
using System.IO;

namespace SPSIN.Store.ApplicationPages
{
    public partial class Install : SPSINStoreLayoutsPageBase
    {
        Guid appID = new Guid();
        string targetWebURL = "";
        public StorePackage package = default(StorePackage);
        public string packageTitle = "";
        protected override void OnInit(EventArgs e)
        {
            // Sanitycheck
            SanityCheck();

            package = VerifyAppIDViability(appID);
            packageTitle = package.Title;

            SPSINStoreUtilities.InstallPackageInWeb(package, targetWebURL);
        }


        private void SanityCheck()
        {
            if (Request["appid"] != null)
            {
                try
                {
                    appID = new Guid(Request["appid"]);
                }
                catch
                {
                    throw new SPException("Cannot convert app id to GUID");
                }
            }
            else
            {
                throw new SPException("Cannot find the app id in the query string.");
            }
            if (Request["targetweburl"] == null)
            {
                throw new SPException("Cannot find the web id in the query string.");
            }
            else
            {
                targetWebURL = HttpUtility.UrlDecode(Request["targetweburl"]);
            }

        }

        private StorePackage VerifyAppIDViability(Guid appID)
        {
            using (SPSite site = new SPSite(targetWebURL))
            {
                using (SPWeb activationWeb = site.OpenWeb())
                {

                    Dictionary<Guid, StorePackage> packages = SPSINStoreUtilities.GetPackages(SPContext.Current, activationWeb);

                    if (!packages.ContainsKey(appID))
                    {
                        throw new SPException(string.Format("Cannot find the app with ID {0}. Please contact the app store owner and let them know that the app ID is missing."));
                    }
                    else
                    {
                        return packages[appID];
                    }
                }
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
