/// <reference path="http://code.jquery.com/jquery-1.10.2.min.js" />
if (window.jQuery) {
    $(document).ready(function() {
        $(document).on("click", ".btnQuickInstall", function() {
            var installDestination = installerURL;

            installDestination = installDestination + "&IsDLG=1";
            installDestination = installDestination + "&appid=" + $(this).attr('data-appid');
            installDestination = installDestination + "&targetweburl=" + targtWebURL;
            // prompt(installDestination, installDestination);
            $("iframe#InstallDialog").attr('src', installDestination);
            $("div.InstallDialog").dialog(
            {
                dialogClass: "no-close",
                closeOnEscape: true,
                height: 425,
                width: 625,
                modal: true

            }
            ).show();
            return false;
        });
    });
}
else {
    alert("jQuery not loaded.");
}

function closebutton() {
    $('div.InstallDialog').dialog('close');
    location.reload();
}
