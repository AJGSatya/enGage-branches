<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAUPSS.ascx.cs" Inherits="enGage.Web.Controls.ucAUPSS" %>
<%@ Implements Interface="System.Web.UI.ICallbackEventHandler" %>

<script type="text/javascript"> // ADDED ONLY BECAUSE OF MICROSOFT BUG
//<![CDATA[
var GlvDelayedNextPageNo;

function WebForm_CallbackComplete_SyncFixed() {
     // the var statement ensure the variable is not global
     for (var i = 0; i < __pendingCallbacks.length; i++) {
        callbackObject = __pendingCallbacks[i];
        if (callbackObject && callbackObject.xmlRequest && 
			(callbackObject.xmlRequest.readyState == 4)) {
            // SyncFixed: line move below // WebForm_ExecuteCallback(callbackObject);
            if (!__pendingCallbacks[i].async) { 
                __synchronousCallBackIndex = -1;
            }
            __pendingCallbacks[i] = null;
            var callbackFrameID = "__CALLBACKFRAME" + i;
            var xmlRequestFrame = document.getElementById(callbackFrameID);
            if (xmlRequestFrame) {
                xmlRequestFrame.parentNode.removeChild(xmlRequestFrame);
            }
            // SyncFixed: the following statement has been moved down from above;
            WebForm_ExecuteCallback(callbackObject);
        }
    }
}

var OnloadWithoutSyncFixed = window.onload;

window.onload = function Onload(){
    if (typeof (WebForm_CallbackComplete) == "function") {
        // Set the fixed version
        WebForm_CallbackComplete = WebForm_CallbackComplete_SyncFixed;
        // CallTheOriginal OnLoad
        if (OnloadWithoutSyncFixed!=null) OnloadWithoutSyncFixed();
    }
}
//]]>
</script> 

<script type="text/javascript" language="javascript">
              
    function ReceiveServerData(arg, context) 
    {
        var control = document.getElementById("<%=ddlSuburb.ClientID %>");
        control.options.length = 0;
        if (arg == "") return;
        var ss = arg.split(";");
        for (i = 0; i <= ss.length-2; i++)
        {
            var opt = document.createElement("option");
            opt.text = ss[i].replace("#", " (") + ")";
            opt.value = ss[i].split("#")[0];
            control.options.add(opt);
        }
    }
       
    function RememberSuburb()
    {
        document.getElementById("<%=hidSuburb.ClientID %>").value = 
        document.getElementById("<%=ddlSuburb.ClientID %>").selectedIndex;
    }
     
</script>

<asp:TextBox ID="txtPostCode" runat="server" MaxLength="4" Width="32px"></asp:TextBox>
<asp:DropDownList ID="ddlSuburb" runat="server" onchange="RememberSuburb();">
</asp:DropDownList>
<asp:HiddenField ID="hidSuburb" runat="server" />

