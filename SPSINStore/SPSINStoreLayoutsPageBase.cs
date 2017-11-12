using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.SharePoint.WebControls;

namespace SPSIN.Store.ApplicationPages
{
    public abstract class SPSINStoreLayoutsPageBase : LayoutsPageBase
    {
        public bool RequirejQuery
        {
            get
            {
                return true;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}
