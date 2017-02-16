<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="DashboardTotals.aspx.cs" Inherits="enGage.Web.DashboardTotals" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="Controls/ucSearchOptions.ascx" TagName="ucSearchOptions" TagPrefix="uc2" %>
<%@ Register Src="Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page">
    <table class="page-table">
        <tr>
            <td>
                <asp:Button ID="btnDashboard" runat="server" CssClass="button-action"
                    Text="Show Dashboard" onclick="btnDashboard_Click" />
                <asp:Button ID="btnShow" runat="server" CssClass="button-action" OnClick="btnShow_Click"
                    Text="Show Dashboard Totals" />
                <asp:Button ID="btnTallyBoard" runat="server" CssClass="button-action" Text="Show Tallyboard"
                        OnClick="btnTallyBoard_Click" />    
                    
            </td>
            <td class="right-aligned">
                <asp:Button ID="btnFindClient" runat="server" Text="Find Client" CssClass="button-link"
                    PostBackUrl="~/FindClient.aspx" />
                <asp:Button ID="btnFollowUps" runat="server" PostBackUrl="~/FollowUps.aspx" Text="Home"
                    CssClass="button-link" />
            </td>
        </tr>
        <tr>
            <td style="padding: 0px; margin: 0px" colspan="2">
                <uc2:ucSearchOptions ID="ucSearchOptions1" runat="server" DisableExecutives="True" OnControlLoaded="ucSearchOptions1_OnControlLoaded" />
            </td>
        </tr>
        <tr>
            <td class="page-group" colspan="2">
                <asp:ImageButton ID="btnResults" runat="server" ImageUrl="~/images/collapse.png" />
                &nbsp;Results
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <uc1:ucMessanger ID="ucMessanger1" runat="server" />
            </td>
        </tr>
    </table>
    <div><!-- style="overflow: hidden !important"> -->
        <asp:Panel ID="pnlResults" runat="server">
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
                Height="30px" Width="100%" ShowDocumentMapButton="False" ShowFindControls="False"
                ShowPromptAreaButton="False" ShowRefreshButton="False" Visible="False"
                SizeToReportContent="True" style="margin-bottom:30px" 
                ondrillthrough="ReportViewer1_Drillthrough">
                <LocalReport ReportPath="Reports\DashboardTotals.rdlc" EnableHyperlinks="True">
                    <DataSources>
                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                            Name="proc_rpt_DashboardTotalsResult" />
                    </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>
        </asp:Panel>
        <cc1:CollapsiblePanelExtender ID="pnlResults_CollapsiblePanelExtender" runat="server"
            CollapseControlID="btnResults" Collapsed="True" CollapsedImage="~/images/expand.png"
            CollapsedText="Results" Enabled="True" ExpandControlID="btnResults" ExpandedImage="~/images/collapse.png"
            ExpandedText="Results" ImageControlID="btnResults" SuppressPostBack="True" TargetControlID="pnlResults">
        </cc1:CollapsiblePanelExtender>
    </div>
  </div>
</asp:Content>
