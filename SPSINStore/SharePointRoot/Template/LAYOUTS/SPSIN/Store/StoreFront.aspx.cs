using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.Net;
using System.Xml;
using System.IO;
using System.Security.Principal;
using System.Collections.ObjectModel;
using Microsoft.SharePoint.Utilities;

namespace SPSIN.Store.ApplicationPages
{
    public partial class StoreFront : SPSINStoreLayoutsPageBase
    {
        //        List<PackageRepository> stores = new List<PackageRepository>();
        public string installerURL = "";
        public string targetWebURL = "";
        protected override void OnInit(EventArgs e)
        {
            SPAdministrationWebApplication centralAdmin = SPAdministrationWebApplication.Local;
            SPWebApplication currentApp = SPContext.Current.Site.WebApplication;

            if (currentApp.Id != centralAdmin.Id)
            {
                // Wrong web app, redirect.
                string url = centralAdmin.Sites[0].Url;
                if (url == "/")
                {
                    url = "";
                }
                string source = HttpContext.Current.Request.Url.AbsoluteUri;
                targetWebURL = HttpUtility.UrlEncode(source);
                url += "/_layouts/SPSIN/Store/";
                installerURL = url;

                url += "StoreFront.aspx?SourceURL=";
                installerURL += "install.aspx?SourceURL=";

                Uri referrer = HttpContext.Current.Request.UrlReferrer;
                if (referrer != null)
                {
                    url += referrer.ToString();
                    installerURL += referrer.ToString();
                }
                else
                {
                    url += HttpUtility.UrlEncode(source);
                    installerURL += HttpUtility.UrlEncode(source);
                }
                //Response.Redirect(url, true);
            }


            if (Page.IsPostBack)
            {
                SPUtility.ValidateFormDigest();
            }
            //            stores = SPSINStoreUtilities.LoadPackageRepositories(SPContext.Current.Web);
            base.OnInit(e);
        }

        protected override void CreateChildControls()
        {
            SPSIN_StorePanel.Controls.Clear();

            Control allPackagesControls = new Control();


            //if (!string.IsNullOrEmpty(HttpContext.Current.Request["SourceURL"]))
            //{
            try
            {
                //using (SPSite site = new SPSite(HttpContext.Current.Request["SourceURL"]))
                SPSite site = SPContext.Current.Site;
                {
                    //using (SPWeb activationWeb = site.OpenWeb())                    
                    SPWeb activationWeb = SPContext.Current.Web;
                    {

                        Dictionary<Guid, StorePackage> packages = SPSINStoreUtilities.GetPackages(SPContext.Current, activationWeb);

                        foreach (StorePackage package in packages.Values)
                        {
                            if (package.SolutionType == SolutionType.Farm)
                            {
                                allPackagesControls.Controls.Add(GetControlForFarmPackage(package, activationWeb));
                            }
                            else if (package.SolutionType == SolutionType.Sandbox)
                            {
                                allPackagesControls.Controls.Add(GetControlForSandboxPackage(package, activationWeb));
                            }
                            else if (package.SolutionType == SolutionType.App)
                            {
                                allPackagesControls.Controls.Add(GetControlForAppPackage(package, activationWeb));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Cannot connect to web. Permissions issue? 
                throw new SPException("Whoops! We cannot read the list of packages from the repository", e);

            }
            //}


            SPSIN_StorePanel.Controls.Add(allPackagesControls);
        }

        private Control GetControlForAppPackage(StorePackage package, SPWeb targetWeb)
        {
            return GetControlForFarmPackage(package, targetWeb);
        }

        private Control GetControlForSandboxPackage(StorePackage package, SPWeb targetWeb)
        {
            bool canAdd = SPSINStorePackageUtilities.CanAddSandboxSolutions(SPContext.Current, targetWeb);
            bool isInstalled = SPSINStorePackageUtilities.IsSolutionInstalled(package, targetWeb);
            Panel p = new Panel();
            p.CssClass = "SPSINStorePackagePanel";
            p.Enabled = canAdd;

            string panelString = string.Format(@"
<h3 class=""packageHeader""><a class=""addSandboxSolution"" href=""{2}"">{0}</a></h3>
<p>{1}</p>
", package.Title, package.Description, package.ReadMeURL);

            p.Controls.Add(new LiteralControl(panelString));

            Panel detailsPanel = new Panel();

            HyperLink authorLink = new HyperLink();
            authorLink.NavigateUrl = package.AuthorURL;
            authorLink.Text = package.AuthorName;
            authorLink.CssClass = "authorInfo";
            detailsPanel.Controls.Add(authorLink);
            detailsPanel.Controls.Add(new LiteralControl("<br/>"));

            LiteralControl typeInformation = new LiteralControl();
            typeInformation.Text = "<span class=\"infoHeader\">Type: </span><span class=\"infoContent\">Sandbox Solution</span><br/>";
            detailsPanel.Controls.Add(typeInformation);

            LiteralControl autoInformation = new LiteralControl();
            autoInformation.Text = string.Format("<span class=\"infoHeader\">Automatic activation: </span><span class=\"infoContent\">{0}</span><br/>", (string.IsNullOrEmpty(package.SetupFeatureID) ? "No" : "Yes"));
            detailsPanel.Controls.Add(autoInformation);

            Button quickAddButton = new Button();
            quickAddButton.Text = "Quick Install";
            quickAddButton.CssClass = "btnQuickInstall";
            quickAddButton.Attributes.Add("data-appid", package.ID);

            Button configureAddButton = new Button();
            configureAddButton.Text = "Configure Install";
            configureAddButton.ID = "configure_" + package.ID;

            p.Controls.Add(detailsPanel);

            if (!isInstalled)
            {
                if (canAdd)
                {
                    quickAddButton.Enabled = true;
                    configureAddButton.Enabled = true;
                }
                else
                {
                    quickAddButton.Enabled = false;
                    configureAddButton.Enabled = false;
                    quickAddButton.ToolTip = "You are unable to add this type of solution. Only site collection administrators can add farm solutions.";
                    configureAddButton.ToolTip = "You are unable to add this type of solution. Only site collection administrators can add farm solutions.";
                }
                p.Controls.Add(quickAddButton);
                p.Controls.Add(configureAddButton);
            }
            else
            {
                Panel alreadyInstalled = new Panel();
                alreadyInstalled.Controls.Add(new LiteralControl("Solution already installed."));
                p.Controls.Add(alreadyInstalled);
            }


            return p;
        }

        private Control GetControlForFarmPackage(StorePackage package, SPWeb targetWeb)
        {
            bool canAdd = SPSINStorePackageUtilities.CanAddFarmSolutions(SPContext.Current, targetWeb);
            bool isInstalled = SPSINStorePackageUtilities.IsSolutionInstalled(package, targetWeb);
            Panel p = new Panel();
            p.CssClass = "SPSINStorePackagePanel";
            p.Enabled = canAdd;

            string panelString = string.Format(@"
<h3 class=""packageHeader""><a class=""addFarmSolution"" href=""{2}"">{0}</a></h3>
<p>{1}</p>
", package.Title, package.Description, package.ReadMeURL);

            p.Controls.Add(new LiteralControl(panelString));

            Panel detailsPanel = new Panel();

            HyperLink authorLink = new HyperLink();
            authorLink.NavigateUrl = package.AuthorURL;
            authorLink.Text = package.AuthorName;
            authorLink.CssClass = "authorInfo";
            detailsPanel.Controls.Add(authorLink);
            detailsPanel.Controls.Add(new LiteralControl("<br/>"));

            LiteralControl typeInformation = new LiteralControl();
            typeInformation.Text = "<span class=\"infoHeader\">Type: </span><span class=\"infoContent\">Farm Solution</span><br/>";
            detailsPanel.Controls.Add(typeInformation);

            LiteralControl autoInformation = new LiteralControl();
            autoInformation.Text = string.Format("<span class=\"infoHeader\">Automatic activation: </span><span class=\"infoContent\">{0}</span><br/>", (string.IsNullOrEmpty(package.SetupFeatureID) ? "No" : "Yes"));
            detailsPanel.Controls.Add(autoInformation);

            Button quickAddButton = new Button();
            quickAddButton.Text = "Quick Install";
            quickAddButton.CssClass = "btnQuickInstall";
            quickAddButton.Attributes.Add("data-appid", package.ID);

            Button configureAddButton = new Button();
            configureAddButton.Text = "Configure Install";
            configureAddButton.ID = "configure_" + package.ID;

            p.Controls.Add(detailsPanel);

            if (!isInstalled)
            {
                if (canAdd)
                {
                    quickAddButton.Enabled = true;
                    configureAddButton.Enabled = true;
                }
                else
                {
                    quickAddButton.Enabled = false;
                    configureAddButton.Enabled = false;
                    quickAddButton.ToolTip = "You are unable to add this type of solution. Only farm administrators can add farm solutions.";
                    configureAddButton.ToolTip = "You are unable to add this type of solution. Only farm administrators can add farm solutions.";
                }
                p.Controls.Add(quickAddButton);
                p.Controls.Add(configureAddButton);
            }
            else
            {
                Panel alreadyInstalled = new Panel();
                alreadyInstalled.Controls.Add(new LiteralControl("Solution already installed."));
                p.Controls.Add(alreadyInstalled);
            }



            return p;
        }

        public static bool IsCurrentUserMachineAdmin()
        {
            bool result;
            using (WindowsIdentity current = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
                result = windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            return result;
        }



    }
}
