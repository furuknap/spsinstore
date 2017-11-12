using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.SharePoint;

namespace SPSIN.Store
{
    public class AppActivationSINCycle : SINCycleReceiver
    {
        public override void AfterContextLoad(SINCycleContext context)
        {
            SortedList<DateTime, Guid> pendingActivationFeatures = SPSINStoreUtilities.GetPendingActivationFeatures(context.SPContext.Web);

            if (pendingActivationFeatures.Count > 0)
            {
                SPWeb web = context.SPContext.Web;

                foreach (Guid featureID in pendingActivationFeatures.Values)
                {
                    if (web.Features[featureID] == null)
                    {
                        try
                        {
                            bool oldState = web.AllowUnsafeUpdates;
                            web.AllowUnsafeUpdates = true;
                            web.Features.Add(featureID);
                            web.AllowUnsafeUpdates = oldState;

                            // We've activated it, now remove it...
                            SPSINStoreUtilities.RemovePendingActivationFeature(context.SPContext.Web, featureID);
                        }
                        catch
                        {
                            // TODO: Add better handling for non-site owner features or problems...
                        }
                    }
                    else
                    {
                        // Already activated, remove
                        SPSINStoreUtilities.RemovePendingActivationFeature(context.SPContext.Web, featureID);

                    }

                }
                // Do sanity check!
                
            }
        }
    }
}
