using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.Net;
using Microsoft.SharePoint.Administration;
using System.IO;
using System.Collections.ObjectModel;

namespace SPSIN.Store
{
    public class FarmPackageInstaller : PackageInstaller
    {
        public override void AddPackage(AddPackageContext context, Boolean autoActivate)
        {
            StorePackage package = context.StorePackage;
            try
            {
                Collection<SPWebApplication> webApps = new Collection<SPWebApplication>();
                webApps.Add(context.TargetWeb.Site.WebApplication);

                bool previousUnsafeUpdatesContextWeb = context.SPContext.Web.AllowUnsafeUpdates;
                context.SPContext.Web.AllowUnsafeUpdates = true;
                SPSolution solution = SPFarm.Local.Solutions.Add(context.SolutionFilePath);
                solution.Deploy(DateTime.Now, true, webApps, true);

                if (!string.IsNullOrEmpty(package.SetupFeatureID))
                {
                    if (autoActivate)
                    {
                        SPSINStorePackageUtilities.AddPendingActivationFeature(context.TargetWeb, new Guid(package.SetupFeatureID));
                    }
                }
                context.SPContext.Web.AllowUnsafeUpdates = previousUnsafeUpdatesContextWeb;
            }
            catch (Exception exc)
            {
                throw new SPException(exc.ToString());
            }
        }

        public override bool IsSolutionInstalled(StorePackage package, SPWeb targetWeb)
        {
            bool isInstalled = false;
            if (package.SolutionType == SolutionType.Farm)
            {
                SPSolutionCollection allSolutions = SPFarm.Local.Solutions;
                foreach (var solution in allSolutions)
                {
                    if (!isInstalled)
                    {
                        if (solution.SolutionFile.Name.IndexOf(package.SolutionFileName, StringComparison.InvariantCultureIgnoreCase) >= 0)
                        {
                            isInstalled = true;
                        }
                    }
                }
            }
            return isInstalled;

        }
    }
}
