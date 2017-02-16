<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
CodeBehind="Tallyboard.aspx.cs" Inherits="enGage.Web.Tallyboard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
<%@ Register Src="Controls/ucSearchOptions.ascx" TagName="ucSearchOptions" TagPrefix="uc2" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



<style type="text/css">
    
        table.total {
	        font-family: verdana,arial,sans-serif;
	        font-size:16px;
	        color:#333333;
	        border-width: 1px;
	        border-color: #00467F;
	        border-style:solid;
	        border-collapse: collapse;
	        margin-top:5px;
	        background:#D3D2C4;
        }
        
        table.total .other
        {
            text-align:right;
            width:100px;
            font-weight:bold;
        }
    
        table.gridtable {
	        font-family: verdana,arial,sans-serif;
	        font-size:11px;
	        color:#333333;
	        border-width: 1px;
	        border-color: #D3D2C4;
	        border-collapse: collapse;
	        margin-top:5px;
        }
        table.gridtable thead {
	        border-width: 1px;
	        border-style: solid;
	        border-color: #D3D2C4;
        }
        table.gridtable thead td
        {
            background: #5F6062;
            color:#FFFFFF;
	        text-align:centre;
        }
        table.gridtable thead td.first 
        {
            text-align:left;
        }
        table.gridtable thead td.other
        {
            width:100px;
        }
        table.gridtable td {
	        border-width: 1px;
	        border-style: solid;
	        border-color: #D3D2C4;
        }
        table .activities
        {
            color:#E78A19;
            text-align:right;
        }
        table .clients
        {
            color:#0069C1;
            text-align:right;
        }
        table .opportunities
        {
            color:#00467F;
            text-align:right;
        }
        div.resultHeader
        {
            width:983px;
            height:57px;
            background-color:#D3D2C4
        }
        div.resultHeader #enGageLogo 
        {
            float:left;
        }
        div.resultHeader #CompanyLogo 
        {
            float:right;
        }
        div.resultHeader #resultTitle
        {
            color:#00467F;
            font-size:18pt;
            font-family:Verdana;
            font-style:normal;
            font-weight:400;
            text-align:center;
            white-space:pre-wrap;
            padding-top:14px;
        }
        div.resultHeader #resultFooter
        {
            clear:both;
        }
    </style>
    <div class="page">
        <table class="page-table" width="100%">
            <tr>
                <td style="vertical-align: super">
                    <asp:Button ID="btnDashBoard" runat="server" CssClass="button-action" OnClick="btnDashBoard_Click"
                        Text="Show Dashboard" />
                    <asp:Button ID="btnDashboardTotals" runat="server" CssClass="button-action" Text="Show Dashboard Totals"
                        OnClick="btnDashboardTotals_Click" />
                    <asp:Button ID="btnShow" runat="server" CssClass="button-action" Text="Show Tallyboard"
                        OnClick="btnShow_Click" />
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
        <div class="resultHeader">
            <img src="/images/HeaderName.gif" id="enGageLogo" />
            <img src="/images/HeaderLogo.gif" id="CompanyLogo" />
            <div id="resultTitle">OAMPS Insurance Broker Ltd</div>
            <div id="resultFooter"></div>
        </div>
        <asp:Panel ID="pnlResults" runat="server">
            <table class="total" width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="ltrTotalColumn" runat="server"></asp:Literal></td>
                    <td class="other activities">
                        <asp:Literal ID="ltrTotalActivities" runat="server"></asp:Literal>
                    </td>
                    <td class="other clients">
                        <asp:Literal ID="ltrTotalClients" runat="server"></asp:Literal>
                    </td>
                    <td class="other opportunities">
                        <asp:Literal ID="ltrTotalOpportunities" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
        
            <table class="gridtable" width="100%">
            <thead>
                <tr>
                    <td class="first">
                        <asp:Literal ID="ltrTallyForHeader" runat="server"></asp:Literal>
                    </td>    
                    <td class="other">Activities</td>
                    <td class="other">Clients</td>
                    <td class="other">Opportunities</td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptTallyboard" runat="server" 
                    onitemdatabound="rptTallyboard_ItemDataBound">
                    <AlternatingItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrTallyFor" runat="server"></asp:Literal></td>
                            <td class="activities">
                                <asp:Literal ID="ltrActivities" runat="server"></asp:Literal></td>
                            <td class="clients">
                                <asp:Literal ID="ltrClients" runat="server"></asp:Literal></td>
                            <td class="opportunities">
                                <asp:Literal ID="ltrOpportunities" runat="server"></asp:Literal></td>
                        </tr>
                    </AlternatingItemTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrTallyFor" runat="server"></asp:Literal></td>
                            <td class="activities">
                                <asp:Literal ID="ltrActivities" runat="server"></asp:Literal></td>
                            <td class="clients">
                                <asp:Literal ID="ltrClients" runat="server"></asp:Literal></td>
                            <td class="opportunities">
                                <asp:Literal ID="ltrOpportunities" runat="server"></asp:Literal></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <div style="width:100%; height:10px;"></div>
        </asp:Panel>
        <cc1:CollapsiblePanelExtender ID="pnlResults_CollapsiblePanelExtender" runat="server"
            CollapseControlID="btnResults" Collapsed="True" CollapsedImage="~/images/expand.png"
            CollapsedText="Results" Enabled="True" ExpandControlID="btnResults" ExpandedImage="~/images/collapse.png"
            ExpandedText="Results" ImageControlID="btnResults" SuppressPostBack="True" TargetControlID="pnlResults">
        </cc1:CollapsiblePanelExtender>
    </div>
</asp:content>


        
    
