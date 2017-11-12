using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;

namespace SPSIN.Store.ApplicationPages
{
    public partial class Settings : SPSINStoreLayoutsPageBase
    {
        protected override void CreateChildControls()
        {
            UpdateForm();
        }

        private void UpdateForm()
        {
            UpdateForm(null);
        }
        private void UpdateForm(string Message)
        {
            tbRepositoryPackageURL.Text = SPSINStoreRepository.GetResolvedRepositoryURL(SPContext.Current.Web);
            if (! string.IsNullOrEmpty(Message)) {
                SPSIN_Message.Text = Message;
            }
        }

        public void lbtnResetRepositoryPackageURL_Click(object sender, EventArgs e)
        {
            SPSINStoreRepository.ResetRepositoryPackageURL(SPContext.Current.Web);
            UpdateForm("Repository Pakcage URL Reset");
        }

        public void btnSaveClick(object sender, EventArgs e)
        {
            SPSINStoreRepository.SetRepositoryPackageURL(SPContext.Current.Web, tbRepositoryPackageURL.Text);
            UpdateForm("Settings saved!");
        }
    }
}
