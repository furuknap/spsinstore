using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.Reflection;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Administration;
using System.Collections.ObjectModel;

namespace SPSIN.Store
{
    public class AssemblyDescription
    {
        public AssemblyName Name;
        public string Class;
    }

    public static class SPSINStorePackageUtilities
    {
        public static readonly Dictionary<SolutionType, AssemblyDescription> pakcageInstallerAssemblies = new Dictionary<SolutionType, AssemblyDescription>()
        {
         
        {SolutionType.Farm,new AssemblyDescription() { Name=new AssemblyName("SPSIN.Store.FarmPackageInstaller, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ffc246be9b9dd5bf"), Class="SPSIN.Store.FarmPackageInstaller" }},
        {SolutionType.Sandbox,new AssemblyDescription() { Name=new AssemblyName("SPSIN.Store.SandboxPackageInstaller, Version=1.0.0.0, Culture=neutral, PublicKeyToken=fa3ee84a19141c8d"), Class="SPSIN.Store.SandboxPackageInstaller" }},
        {SolutionType.App,new AssemblyDescription() { Name=new AssemblyName("SPSIN.Store.AppPackageInstaller, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2c7f7c7149d00714"), Class="SPSIN.Store.AppPackageInstaller" }}

        };

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

        public static bool IsSolutionInstalled(StorePackage package, SPWeb targetWeb)
        {
            bool isInstalled = false;
            if (package.SolutionType == SolutionType.Farm)
            {
                try
                {
                    Assembly a = Assembly.Load(pakcageInstallerAssemblies[SolutionType.Farm].Name.FullName);
                    PackageInstaller installer = (PackageInstaller)a.CreateInstance(pakcageInstallerAssemblies[SolutionType.Farm].Class);
                    isInstalled = installer.IsSolutionInstalled(package, targetWeb);
                }
                catch
                {
                    throw;
                }
            }
            else if (package.SolutionType == SolutionType.Sandbox)
            {
                try
                {
                    Assembly a = Assembly.Load(pakcageInstallerAssemblies[SolutionType.Sandbox].Name.FullName);
                    PackageInstaller installer = (PackageInstaller)a.CreateInstance(pakcageInstallerAssemblies[SolutionType.Sandbox].Class);
                    isInstalled = installer.IsSolutionInstalled(package, targetWeb);
                }
                catch
                {
                    throw;
                }

            }
            return isInstalled;
        }


        public static bool CanAddFarmSolutions(SPContext context, SPWeb activationWeb)
        {
            bool result = false;
            if (SPFarm.Local.CurrentUserIsAdministrator())
            {
                result = true;
            }
            return result;
        }

        public static bool CanAddAppSolutions(SPContext context, SPWeb activationWeb)
        {
            // Yeah, yeah, need better check...
            return false;
        }

        public static bool CanAddSandboxSolutions(SPContext context, SPWeb activationWeb)
        {
            bool result = false;
            Version spVersion = SPFarm.Local.BuildVersion;

            if (spVersion > new Version("14.0.0.0"))
            {
                // Yeah, yeah, need better check...
                if (activationWeb.CurrentUser.IsSiteAdmin)
                {
                    result = true;
                }
            }

            return result;
        }
        public static void AddSandboxSolution(AddPackageContext context)
        {
            AddSandboxSolution(context, true);
        }
        public static void AddSandboxSolution(AddPackageContext context, Boolean autoActivate)
        {
            try
            {
                Assembly a = Assembly.Load(pakcageInstallerAssemblies[SolutionType.Sandbox].Name.FullName);
                PackageInstaller installer = (PackageInstaller)a.CreateInstance(pakcageInstallerAssemblies[SolutionType.Sandbox].Class);
                installer.AddPackage(context, autoActivate);
            }
            catch
            {
                throw;
            }
        }

        public static void AddFarmSolution(AddPackageContext context)
        {
            AddFarmSolution(context, true);
        }
        public static void AddFarmSolution(AddPackageContext context, Boolean autoActivate)
        {
            try
            {
                Assembly a = Assembly.Load(pakcageInstallerAssemblies[SolutionType.Farm].Name.FullName);
                PackageInstaller installer = (PackageInstaller)a.CreateInstance(pakcageInstallerAssemblies[SolutionType.Farm].Class);
                installer.AddPackage(context, autoActivate);
            }
            catch
            {
                throw;
            }
        }

    }
}
