using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.SharePoint;

namespace SPSIN.Store
{
    public abstract class PackageRepository
    {
        public virtual string RepositoryHomePageURL { get; private set; }
        public virtual bool RequireAuthentication { get; set; }

        public virtual Dictionary<Guid, StorePackage> GetPackages(StoreContext context) 
        {
            return new Dictionary<Guid, StorePackage>();
        }
    }

    public class StoreContext
    {
        public SPWeb Web;
        public SPContext SPContext;
        public HttpContext HttpContext;

    }
}
