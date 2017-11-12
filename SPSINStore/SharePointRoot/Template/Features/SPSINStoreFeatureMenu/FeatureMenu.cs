using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint.Administration;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint;
using System.Drawing;

namespace SPSIN.Store
{
    public class FeatureMenu : Control
    {
        protected override void CreateChildControls()
        {
            // Show only to farm administrators?

            SPContext currentContext = SPContext.Current;

            if (currentContext != null)
            {
                try
                {
                    bool storeActivated = false;

                    // If in central admin, or elsewhere outside web, check for target web SP SIN Store activation
                    HttpRequest Request = HttpContext.Current.Request;

                    if (Request["SourceURL"] != null && !string.IsNullOrEmpty(Request["SourceURL"]))
                    {
                        using (SPSite site = new SPSite(Request["SourceURL"]))
                        {
                            using (SPWeb targetWeb = site.OpenWeb())
                            {
                                if (targetWeb.Features[new Guid("7e39c867-dcb9-456e-8017-a117d845ed03")] != null)
                                {
                                    storeActivated = true;
                                }
                            }
                        }
                    }
                    else if (currentContext.Web.Features[new Guid("7e39c867-dcb9-456e-8017-a117d845ed03")] != null)
                    {
                        storeActivated = true;
                    }
                    // SP SIN Store activated, show link
                    SPAdministrationWebApplication centralAdmin = SPAdministrationWebApplication.Local;

                    string url = centralAdmin.Sites[0].Url;
                    if (url == "/")
                    {
                        url = "";
                    }

                    HyperLink link = new HyperLink();

                    link.Text = "SP SIN App Store";
                    link.NavigateUrl = url + "/_layouts/SPSIN/Store/StoreFront.aspx";
                    link.Enabled = false;

                    if (storeActivated)
                    {
                        string source = HttpContext.Current.Request.Url.AbsoluteUri;
                        link.NavigateUrl += "?SourceURL=" + HttpUtility.UrlEncode(source);
                        link.Enabled = true;
                    }
                    Controls.Add(link);

                    if (!storeActivated)
                    {
                        Controls.Add(new LiteralControl("<br />"));
                        Label inactivateLabel = new Label();
                        inactivateLabel.Text = "SP SIN Store feature not activated. Activate from Site Settings->Site Features to enable link";
                        inactivateLabel.ForeColor = Color.DarkGray;
                        inactivateLabel.Font.Italic = true;
                        Controls.Add(inactivateLabel);
                    }

                }
                catch
                {
                }

            }

            if (HttpContext.Current != null)
            {
                HttpRequest Request = HttpContext.Current.Request;
                if (Request["SourceURL"] != null && !string.IsNullOrEmpty(Request["SourceURL"]))
                {
                    string source = HttpUtility.UrlDecode(Request["SourceURL"]);
                    Controls.Add(new LiteralControl("<br />\n"));
                    HyperLink link = new HyperLink();
                    link.Text = "Back";
                    link.NavigateUrl = source;
                    Controls.Add(link);
                }
            }

        }
    }
}
