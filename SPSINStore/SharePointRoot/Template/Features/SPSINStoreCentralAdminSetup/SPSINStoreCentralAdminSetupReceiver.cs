using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Globalization;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace SPSIN.Store.EventHandlers.Features
{
    public class SPSINStoreCentralAdminSetupReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPAdministrationWebApplication centralWeb = SPAdministrationWebApplication.Local;
            SPSite caSite = centralWeb.Sites[0];
            SPWeb caWeb = caSite.RootWeb;
            

            Guid featureID2010 = new Guid("61b45a0f-5a98-4353-aec3-9ff6791468c3");
            Guid featureID2007 = new Guid("5c87f143-910f-4bde-a0c0-a474f8813880");

            if (SPFarm.Local.BuildVersion >= new Version("14.0.0.0"))
            {
                if (caWeb.Features[featureID2010] == null)
                {
                    caWeb.Features.Add(featureID2010);
                }
                // SP2010 mode
            }
            else
            {
                // SP2007 mode
                if (caWeb.Features[featureID2007] == null)
                {
                    caWeb.Features.Add(featureID2007);
                }
            }

        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
        
        }

        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
        
        }

        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {

        }

    }
}
