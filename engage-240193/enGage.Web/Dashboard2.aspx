<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Dashboard2.aspx.cs" Inherits="enGage.Web.Dashboard2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
<%@ Register Src="Controls/ucSearchOptions.ascx" TagName="ucSearchOptions" TagPrefix="uc2" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        table.gridtable {
	        font-family: verdana,arial,sans-serif;
	        font-size:11px;
	        color:#333333;
	        border-width: 1px;
	        border-color: #666666;
	        border-collapse: collapse;
        }
        table.gridtable th {
	        border-width: 1px;
	        padding: 8px;
	        border-style: solid;
	        border-color: #666666;
	        background-color: #dedede;
        }
        table.gridtable td {
	        border-width: 1px;
	        padding: 8px;
	        border-style: solid;
	        border-color: #666666;
	        background-color: #ffffff;
        }
    </style>
    <div class="page">
        <table class="page-table" width="100%">
            <tr>
                <td style="vertical-align: super">
                    <asp:Button ID="btnShow" runat="server" CssClass="button-action" OnClick="btnShow_Click"
                        Text="Show Dashboard" />
                    <asp:Button ID="btnDashboardTotals" runat="server" CssClass="button-action" Text="Show Dashboard Totals"
                        OnClick="btnDashboardTotals_Click" />
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
                    <uc2:ucSearchOptions ID="ucSearchOptions1" runat="server" DisableExecutives="False"
                        OnControlLoaded="ucSearchOptions1_OnControlLoaded" />
                </td>
            </tr>
            <tr>
                <td colspan="2" class="page-group">
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
        <div>
            <asp:Panel ID="pnlResults" runat="server">
                <table class="gridtable">
                    <thead>
                        <tr>
                            <td colspan="3" style="text-align:center">Activities</td>
                            <td colspan="5" style="text-align:center">Opportunity Pipeline</td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>&nbsp;</td>
                            <td style="text-align:center">Activities</td>
                            <td style="text-align:center">Follow-ups</td>
                            <td>&nbsp;</td>
                            <td style="text-align:center">Future</td>
                            <td style="text-align:center" colspan="2">Current Period</td>
                            <td style="text-align:center">Past</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td style="text-align:center">Completed</td>
                            <td style="text-align:center">Overdue</td>
                            <td>&nbsp;</td>
                            <td style="text-align:center">On-the-go</td>
                            <td style="text-align:center">On-the-go</td>
                            <td style="text-align:center">Complete</td>
                            <td style="text-align:center">incomplete</td>
                        </tr>
                        
                            <asp:Repeater ID="rptDashboard" runat="server" OnItemDataBound="rptDashboard_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td><asp:Literal runat="server" ID="ltrAction"></asp:Literal></td>
                                        <td><asp:Literal runat="server" ID="ltrActivities"></asp:Literal></td>
                                        <td><asp:Literal runat="server" ID="ltrFollowUps"></asp:Literal></td>
                                        <td><asp:Literal runat="server" ID="ltrAction2"></asp:Literal></td>
                                        <td><asp:Literal runat="server" ID="ltrPipelineOutcomes"></asp:Literal></td>
                                        <td><asp:Literal runat="server" ID="ltrSuccessOutcomes"></asp:Literal></td>
                                        <td><asp:Literal runat="server" ID="ltrCompleteOutcomes"></asp:Literal></td>
                                        <td><asp:Literal runat="server" ID="ltrDuesOutcomes"></asp:Literal></td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr>
                                        <td><asp:Literal runat="server" ID="ltrAction"></asp:Literal></td>
                                        <td><asp:Literal runat="server" ID="ltrActivities"></asp:Literal></td>
                                        <td><asp:Literal runat="server" ID="ltrFollowUps"></asp:Literal></td>
                                        <td><asp:Literal runat="server" ID="ltrAction2"></asp:Literal></td>
                                        <td><asp:Literal runat="server" ID="ltrPipelineOutcomes"></asp:Literal></td>
                                        <td><asp:Literal runat="server" ID="ltrSuccessOutcomes"></asp:Literal></td>
                                        <td><asp:Literal runat="server" ID="ltrCompleteOutcomes"></asp:Literal></td>
                                        <td><asp:Literal runat="server" ID="ltrDuesOutcomes"></asp:Literal></td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:Repeater>
                        <tr>
                            <td>Expected OAMPS income</td>
                            <td>win</td>
                            <td>win</td>
                            <td>Expected OAMPS income</td>
                            <td>win</td>
                            <td>to win</td>
                            <td>lost</td>
                            <td>win</td>
                        </tr>
                        <tr>
                            <td>Quoted</td>
                            <td><asp:Literal runat="server" ID="ltrSumActivitiesQuoted"></asp:Literal></td>
                            <td>Sum FollowUps Quoted</td>
                            <td>Quoted</td>
                            <td>Sum PipelineQuoted</td>
                            <td>Sum ToWinQuoted</td>
                            <td>Sum LostQuoted</td>
                            <td>Sum DueQuoted</td>
                        </tr>
                        <tr>
                            <td>Actual (Won)</td>
                            <td>Sum ActivitiesActual</td>
                            <td>Sum FollowUpsActual</td>
                            <td>Actual (Won)</td>
                            <td>Sum PipelineActual</td>
                            <td colspan="2">Sum WonActual</td>
                            <td>Sum DueActual</td>    
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="8">TO DO: Write the date here.</td>
                        </tr>
                    </tfoot>
                </table>
                <asp:Literal runat="server" ID="ltrText"></asp:Literal>
            </asp:Panel>
            <cc1:CollapsiblePanelExtender ID="pnlResults_CollapsiblePanelExtender" runat="server"
                CollapseControlID="btnResults" Collapsed="True" CollapsedImage="~/images/expand.png"
                CollapsedText="Results" Enabled="True" ExpandControlID="btnResults" ExpandedImage="~/images/collapse.png"
                ExpandedText="Results" ImageControlID="btnResults" SuppressPostBack="True" TargetControlID="pnlResults">
            </cc1:CollapsiblePanelExtender>
        </div>
    </div>
</asp:Content>
