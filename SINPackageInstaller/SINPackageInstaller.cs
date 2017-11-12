using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSIN.Store;

namespace SINPackageInstaller
{
    public class SINPackageInstaller : PackageInstaller
    {
        public override void AddPackage(AddPackageContext context, Boolean autoActivation)
        {
            base.AddPackage(context, autoActivation);
        }
    }
}
