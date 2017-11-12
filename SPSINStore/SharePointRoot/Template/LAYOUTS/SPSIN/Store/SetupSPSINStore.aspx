<%@ Page Language="C#" MasterPageFile="~/_layouts/application.master" 
    Inherits="SPSIN.Store.ApplicationPages.SetupSPSINStore, SPSIN.Store, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d8aa0390f48c012" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderLeftActions" runat="server">
            <p>
                <asp:HyperLink ID="HyperLink1" NavigateUrl="http://spsin.com/" Target="_blank" runat="server">SP SIN on the web</asp:HyperLink><br />
            </p>
            <p>
                <asp:HyperLink NavigateUrl="http://spsin.com/" Target="_blank" runat="server" ID="SPSIN_UpdateLink"
                    Visible="false">Upgrade Available!</asp:HyperLink></p>
    <SharePoint:DelegateControl ID="DelegateControl1" ControlId="SPSINFeatureMenu" runat="server" AllowMultipleControls="true">
    </SharePoint:DelegateControl>
</asp:Content>