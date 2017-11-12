using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Net;
using System.Xml;
using Microsoft.SharePoint.Administration;
using System.Reflection;
using System.IO;

namespace SPSIN.Store
{
    public static class SPSINStoreUtilities
    {
        private static readonly string webproperty_prefix = "SPSIN_Store_PendingActivationFeature_";

        public static void AddPendingActivationFeature(SPWeb web, Guid featureID)
        {
            if (web.Properties[webproperty_prefix + featureID.ToString()] == null)
            {
                web.Properties.Add(webproperty_prefix + featureID.ToString(), DateTime.Now.ToString());

                bool oldState = web.AllowUnsafeUpdates;
                web.AllowUnsafeUpdates = true;
                web.Properties.Update();
                web.AllowUnsafeUpdates = oldState;
            }
        }

        internal static SortedList<DateTime, Guid> GetPendingActivationFeatures(SPWeb web)
        {
            SortedList<DateTime, Guid> activationFeatures = new SortedList<DateTime, Guid>();
            foreach (string propertyName in web.Properties.Keys)
            {
                if (propertyName.StartsWith(webproperty_prefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    string featureID = propertyName.Replace(webproperty_prefix.ToLowerInvariant(), "");

                    DateTime added = Convert.ToDateTime(web.Properties[propertyName].ToString());

                    activationFeatures.Add(added, new Guid(featureID));
                }
            }

            return activationFeatures;

        }

        internal static void RemovePendingActivationFeature(SPWeb web, Guid featureID)
        {
            if (web.Properties[webproperty_prefix + featureID.ToString()] != null)
            {
                //web.Properties.Remove(webproperty_prefix + featureID.ToString());
                bool oldState = web.AllowUnsafeUpdates;
                web.AllowUnsafeUpdates = true;
                web.Properties.Update();
                web.AllowUnsafeUpdates = oldState;
            }
        }

        public static Dictionary<Guid, StorePackage> GetPackages(SPContext context, SPWeb web)
        {
            if (SPContext.Current != null)
            {
                List<PackageRepository> stores = LoadPackageRepositories(web);
                Dictionary<Guid, StorePackage> packages = new Dictionary<Guid, StorePackage>();
                foreach (PackageRepository store in stores)
                {
                    StoreContext storeContext = new StoreContext();
                    storeContext.HttpContext = HttpContext.Current;
                    storeContext.SPContext = SPContext.Current;
                    storeContext.Web = web;

                    Dictionary<Guid, StorePackage> storePackages = store.GetPackages(storeContext);
                    foreach (Guid storePackageID in storePackages.Keys)
                    {
                        if (!packages.ContainsKey(storePackageID))
                        {
                            packages.Add(storePackageID, storePackages[storePackageID]);
                        }
                    }

                }
                return packages;
            }
            else
            {
                throw new SPException("Cannot get SP SIN Store packages because there is no current SPContext. SP SIN Store needs to run in SharePoint.");
            }
        }



        internal static List<PackageRepository> LoadPackageRepositories(SPWeb web)
        {
            SPSite site = web.Site;

            List<PackageRepository> stores = new List<PackageRepository>();

            SPFeatureDefinitionCollection features = SPFarm.Local.FeatureDefinitions;

            
                
            foreach (SPFeatureDefinition definition in features)
            {
                if (definition != null
                    &&
                    definition.Properties["SPSIN_SINStoreRepository_Assembly"] != null
                    &&
                    definition.Properties["SPSIN_SINStoreRepository_Class"] != null
                    )
                {
                    bool isActive = false;
                    switch (definition.Scope)
                    {
                        case SPFeatureScope.Farm:
                            if (SPWebService.ContentService.Features[definition.Id] != null)
                            {
                                isActive = true;
                            }
                            break;
                        case SPFeatureScope.Site:
                            if (site.Features[definition.Id] != null)
                            {
                                isActive = true;
                            }
                            break;
                        case SPFeatureScope.Web:
                            if (web.Features[definition.Id] != null)
                            {
                                isActive = true;
                            }
                            break;
                        case SPFeatureScope.WebApplication:
                            if (site.WebApplication.Features[definition.Id] != null)
                            {
                                isActive = true;
                            }
                            break;
                        default:
                            break;
                    }
                    if (isActive)
                    {
                        try
                        {
                            string receiverAssembly = definition.Properties["SPSIN_SINStoreRepository_Assembly"].Value;
                            string receiverClass = definition.Properties["SPSIN_SINStoreRepository_Class"].Value;
                            Assembly assembly = Assembly.Load(receiverAssembly);
                            PackageRepository repositoryItem = (PackageRepository)assembly.CreateInstance(receiverClass);

                            stores.Add(repositoryItem);
                        }
                        catch
                        {
                            // Cannot load store assembly
                        }
                    }
                }

            }

            return stores;
        }


        internal static void InstallPackageInWeb(StorePackage package, string targetWebURL)
        {
            SPContext context = SPContext.Current;
            using (SPSite site = new SPSite(targetWebURL))
            {
                using (SPWeb activationWeb = site.OpenWeb())
                {
                    WebClient wc = new WebClient();
                    string tempFileName = Path.GetTempFileName();
                    string solutionFileName = package.SolutionFileName;
                    tempFileName = tempFileName.Replace(Path.GetFileName(tempFileName), solutionFileName);
                    wc.UseDefaultCredentials = true;
                    wc.DownloadFile(package.PackageURL, tempFileName);

                    AddPackageContext packageContext = new AddPackageContext();
                    packageContext.HttpContext = HttpContext.Current;
                    packageContext.SolutionFilePath = tempFileName;
                    packageContext.SPContext = SPContext.Current;
                    packageContext.StorePackage = package;
                    packageContext.TargetWeb = activationWeb;


                    if (package.SolutionType == SolutionType.Farm)
                    {
                        if (SPSINStorePackageUtilities.CanAddFarmSolutions(context, activationWeb))
                        {
                            SPSINStorePackageUtilities.AddFarmSolution(packageContext);
                        }
                    }
                    else if (package.SolutionType == SolutionType.Sandbox)
                    {
                        if (SPSINStorePackageUtilities.CanAddSandboxSolutions(SPContext.Current, activationWeb))
                        {
                            SPSINStorePackageUtilities.AddSandboxSolution(packageContext);
                        }
                    }
                    else
                    {
                        // use App type loader
                    }
                }
            }

        }

    }
}
