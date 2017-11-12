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
            throw new SPException("Not implemented");
        }

        public virtual void AddPackage(AddPackageContext context)
        {
            throw new SPException("Not implemented");
        }
    }
}
