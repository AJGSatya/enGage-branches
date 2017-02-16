<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="NorthSydneyReport.aspx.cs" Inherits="enGage.Web.NorthSydneyReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="Controls/ucSearchOptions.ascx" TagName="ucSearchOptions" TagPrefix="uc2" %>
<%@ Register Src="Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div class="page">
    <table class="page-table">
        <tr>
            <td colspan="2">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
        </tr>
        <tr>
            <td>
                From
                <asp:TextBox ID="txtFrom" runat="server" Width="72px" CssClass="control" ToolTip="dd/mm/yyyy"></asp:TextBox>
                <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="True"
                    Format="dd/MM/yyyy" PopupButtonID="btnFrom" TargetControlID="txtFrom">
                </cc1:CalendarExtender>
                <asp:ImageButton ID="btnFrom" runat="server" ImageUrl="~/images/Calendar.gif" TabIndex="-1" />
                &nbsp;To
                <asp:TextBox ID="txtTo" runat="server" Width="72px" CssClass="control" ToolTip="dd/mm/yyyy"></asp:TextBox>
                <cc1:CalendarExtender ID="txtTo_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy"
                    PopupButtonID="btnTo" TargetControlID="txtTo">
                </cc1:CalendarExtender>
                <asp:ImageButton ID="btnTo" runat="server" ImageUrl="~/images/Calendar.gif" TabIndex="-1" />
                &nbsp;Team
                <asp:DropDownList ID="ddlTeam" runat="server" CssClass="control">
                </asp:DropDownList>
            &nbsp;<asp:Button ID="btnShow" runat="server" CssClass="button-action" OnClick="btnShow_Click"
                    Text="Show" />
            </td>
            <td class="right-aligned">
                        <asp:Button ID="btnFollowups" runat="server" CssClass="button-link" 
                            PostBackUrl="~/FollowUps.aspx" Text="Home" />
                        <asp:Button ID="btnDashboard" runat="server" CssClass="button-link" 
                            PostBackUrl="~/Dashboard.aspx" Text="Dashboard" />
            </td>
        </tr>
        </table>
    <div>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
            Height="600px" Width="983px" ShowDocumentMapButton="False" 
            ShowFindControls="False" ShowPromptAreaButton="False" 
            ShowRefreshButton="False" Visible="False">
            <LocalReport ReportPath="Reports\NorthSydney.rdlc">
            </LocalReport>
        </rsweb:ReportViewer>
    </div>
    <div>
        <uc1:ucMessanger ID="ucMessanger1" runat="server" />
    </div>
  </div>
</asp:Content>
