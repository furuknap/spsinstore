using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.SharePoint;

namespace SPSIN.Store
{
    public class AddPackageContext
    {
        public SPContext SPContext;
        public HttpContext HttpContext;
        public StorePackage StorePackage;
        public SPWeb TargetWeb;
        public string SolutionFilePath;
    }
}
