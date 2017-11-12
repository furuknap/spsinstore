<%@ Page Language="C#" MasterPageFile="~/_layouts/minimal.master" Inherits="SPSIN.Store.ApplicationPages.Install, SPSIN.Store, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d8aa0390f48c012" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
<!-- Must add these manually because SP SIN may not be installed in Central Admin -->
<link rel="stylesheet" type="text/css" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.7.2/themes/start/jquery-ui.css"/>
<script type="text/javascript" src="http://code.jquery.com/jquery-1.10.2.min.js"></script>
<script type="text/javascript" src="http://code.jquery.com/ui/1.10.3/jquery-ui.min.js"></script>



</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <script type="text/javascript">
        $(function() {
            var progressbar = $("#progressbar"), progressLabel = $(".progress-label");
            progressbar.progressbar(
           {
               value: false,
               change: function() {
                   progressLabel.text(progressbar.progressbar("value") + "%");
               },
               complete: function() {
                   progressLabel.text("Complete!");
                   $("#completecontrol").show();
               }
           });
            function progress() {
                var val = progressbar.progressbar("value") || 0;
                progressbar.progressbar("value", val + 1);
                if (val < 99) {
                    setTimeout(progress, 20);
                }
            }
            setTimeout(progress, 1000);
        });

    </script>

    Installing <%=packageTitle %>.
    <div id="progressbar">
        <div class="progress-label">
            Preparing...</div>
    </div>
    <div id="completecontrol" style="display:none;">
        <input type="button" onclick="parent.closebutton();" value="Close" id="closebutton" />
    </div>
</asp:Content>
