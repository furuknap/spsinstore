using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.IO;
using Microsoft.SharePoint.Administration;

namespace SPSIN.Store
{
    public class SandboxPackageInstaller : PackageInstaller
    {
        public override bool CanAddPackage(AddPackageContext context)
        {
            return base.CanAddPackage(context);
        }

        public override bool IsSolutionInstalled(StorePackage package, SPWeb targetWeb)
        {
            bool isInstalled = false;
            Version spVersion = SPFarm.Local.BuildVersion;

            if (spVersion > new Version("14.0.0.0") && package.SolutionType == SolutionType.Sandbox)
            {
                SPSite targetSite = targetWeb.Site;

                SPUserSolutionCollection allSolutions = targetSite.Solutions;

                foreach (SPUserSolution solution in allSolutions)
                {
                    if (!isInstalled)
                    {
                        if (solution.Name.IndexOf(package.SolutionFileName, StringComparison.InvariantCultureIgnoreCase) >= 0)
                        {
                            isInstalled = true;
                        }
                    }
                }
            }
            return isInstalled;

        }

        public override void AddPackage(AddPackageContext context, Boolean autoActivate)
        {
            SPWeb targetWeb = context.TargetWeb;
            SPSite targetSite = targetWeb.Site;
            StorePackage package = context.StorePackage;

            SPList solutionsList = targetSite.GetCatalog(SPListTemplateType.SolutionCatalog);

            bool currentWebUnsafeSettings = context.SPContext.Web.AllowUnsafeUpdates;
            bool currentSettings = targetWeb.AllowUnsafeUpdates;
            targetWeb.AllowUnsafeUpdates = true;
            context.SPContext.Web.AllowUnsafeUpdates = true;
            SPFile file = solutionsList.RootFolder.Files.Add(context.StorePackage.SolutionFileName, File.ReadAllBytes(context.SolutionFilePath));

            SPUserSolution solution = targetSite.Solutions.Add(file.Item.ID);
            targetWeb.AllowUnsafeUpdates = currentSettings;
            context.SPContext.Web.AllowUnsafeUpdates = currentWebUnsafeSettings;

            if (!string.IsNullOrEmpty(package.SetupFeatureID))
            {
                if (autoActivate)
                {
                    try
                    {
                        SPSINStorePackageUtilities.AddPendingActivationFeature(context.TargetWeb, new Guid(package.SetupFeatureID));
                    }
                    catch
                    {
                        // Cannot add automatic activation...
                    }
                }
            }



        }
    }
}
