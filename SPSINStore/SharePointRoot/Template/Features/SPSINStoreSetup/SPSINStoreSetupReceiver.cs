using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Globalization;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace SPSIN.Store.EventHandlers.Features
{
    public class SPSINStoreSetupReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = (SPWeb)properties.Feature.Parent;

            // App Activate Sin cycle control
            if (web.Features[new Guid("aaca46ae-60b0-4f1b-b8b8-28f958eb34f5")] == null)
            {
                web.Features.Add(new Guid("aaca46ae-60b0-4f1b-b8b8-28f958eb34f5"));
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
