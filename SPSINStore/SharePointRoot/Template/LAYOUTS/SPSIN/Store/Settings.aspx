<%@ Page Language="C#" MasterPageFile="~/_layouts/application.master" 
    Inherits="SPSIN.Store.ApplicationPages.Settings, SPSIN.Store, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d8aa0390f48c012" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <h1>
        SP SIN App Store Settings</h1>
    <br />
    <asp:Label Font-Bold="true" ForeColor="Red" runat="server" ID="SPSIN_Message" />
    <div>
        <asp:Label runat="server" ID="lblRepositoryPackageURL" Text="Package URL" /><br />
        <asp:TextBox runat="server" ID="tbRepositoryPackageURL" Width="400" />(<asp:LinkButton
            ID="lbtnResetRepositoryPackageURL" OnClick="lbtnResetRepositoryPackageURL_Click"
            runat="server" Text="Reset to default" />)
    </div>
    <div>
        <asp:Button runat="server" Text="Save" ID="btnSave" OnClick="btnSaveClick" />&nbsp;<asp:Button
            runat="server" Text="Cancel" ID="btnCancel" />
    </div>
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