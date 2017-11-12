using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSIN.Store
{
    public class AppPackageInstaller : PackageInstaller
    {
        public override bool CanAddPackage(AddPackageContext context)
        {
            return false;
        }

    }
}
