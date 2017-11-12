using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.SharePoint;

namespace SPSIN.Store
{
    public abstract class PackageInstaller
    {
        public virtual bool CanAddPackage(AddPackageContext context)
        {
            throw new SPException("Solution installation ability detection is not implemented for this type of solution");
        }

        public virtual void AddPackage(AddPackageContext context, Boolean autoActivate)
        {
            throw new SPException("Solution installation is not implemented for this type of solution");
        }

        public virtual bool IsSolutionInstalled(StorePackage package, SPWeb targetWeb)
        {
            throw new SPException("Solution installation detection is not implemented for this type of solution");
        }
    }
}
