using System;
using System.Collections.Generic;
using System.Web;

namespace SPSIN.Store
{
    public class StorePackage
    {
        public string Title;
        public string ID;
        public string Description;
        public string ReadMeURL;
        public string AuthorName;
        public string AuthorURL;
        public string License;
        public string PackageURL;
        public Version MinimumRequiredSPSINVersion;
        public Version MaximumAllowedSPSINVersion;
        public string Category;
        public string Tags;
        public string SetupFeatureID;
        public SolutionType SolutionType;
        public Version Version;
        public List<Version> SupportedSharePointVersions = new List<Version>();
    }

    public enum SolutionType
    {
        Farm,
        Sandbox,
        App
    }
}
