<%@ Page Language="C#" MasterPageFile="~/_layouts/application.master" Inherits="SPSIN.Store.ApplicationPages.StoreFront, SPSIN.Store, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d8aa0390f48c012" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<asp:Content ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
<link rel="Stylesheet" href="StoreFront.css" />
<script type="text/javascript">
    var installerURL = "<%=installerURL%>";
    var targtWebURL = "<%=targetWebURL %>";
</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderLeftActions" runat="server">
    <p>
        <asp:HyperLink ID="HyperLink1" NavigateUrl="http://spsin.com/" Target="_blank" runat="server">SP SIN on the web</asp:HyperLink><br />
    </p>
    <p>
        <asp:HyperLink NavigateUrl="http://spsin.com/" Target="_blank" runat="server" ID="SPSIN_UpdateLink"
            Visible="false">Upgrade Available!</asp:HyperLink></p>
    <SharePoint:DelegateControl ID="DelegateControl1" ControlId="SPSINFeatureMenu" runat="server"
        AllowMultipleControls="true">
    </SharePoint:DelegateControl>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div>
        <h1>
            SP SIN Store</h1>
        <br />
        <asp:Label Font-Bold="true" ForeColor="Red" runat="server" ID="SPSIN_Message" />
    </div>
    <asp:Panel runat="server" ID="SPSIN_StorePanel">
    </asp:Panel>
    <div class="InstallDialog" style="display: none;">
        <iframe id="InstallDialog" width="575" height="350" scrolling="no" frameborder="0"></iframe>
    </div>
</asp:Content>
