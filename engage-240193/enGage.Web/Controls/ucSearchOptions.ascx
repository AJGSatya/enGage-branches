<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSearchOptions.ascx.cs"
    Inherits="enGage.Web.Controls.ucSearchOptions" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register src="ucMessanger.ascx" tagname="ucMessanger" tagprefix="uc1" %>

<script type="text/javascript">
Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(startRequest);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);
function startRequest(sender, e) 
{
    document.getElementById('<%=ddlRegion.ClientID%>').disabled = true;
    document.getElementById('<%=ddlBranch.ClientID%>').disabled = true;
    document.getElementById('<%=ddlExecutive.ClientID%>').disabled = true;
    document.getElementById('<%=imgLoader.ClientID%>').src = 'images/ajax-loader.gif';
}
function endRequest(sender, e) 
{
    document.getElementById('<%=ddlRegion.ClientID%>').disabled = false;
    document.getElementById('<%=ddlBranch.ClientID%>').disabled = false;
    document.getElementById('<%=ddlExecutive.ClientID%>').disabled = false;
    document.getElementById('<%=imgLoader.ClientID%>').src = 'images/empty.gif';
}
</script>

<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<table style="padding: 0px; margin: 0px; width: 100%;">
    <tr>
        <td class="page-group">
            <asp:ImageButton ID="btnCollapseExpand" runat="server" ImageUrl="~/images/collapse.png" />
            &nbsp;Searched for:
            <asp:Label ID="lblCriteriaDesc" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="report">
        <td>
            <asp:Panel ID="pnlCriteria" runat="server">
                <table style="padding: 0px; margin: 0px; width: 100%;">
                    <tr>
                        <td>
                            From
                            <asp:TextBox ID="txtFrom" runat="server" Width="72px" CssClass="control" ToolTip="dd/mm/yyyy"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="True"
                                Format="dd/MM/yyyy" PopupButtonID="btnFrom" TargetControlID="txtFrom">
                            </cc1:CalendarExtender>
                            <asp:ImageButton ID="btnFrom" runat="server" ImageUrl="~/images/Calendar.gif" TabIndex="-1" />
                            To
                            <asp:TextBox ID="txtTo" runat="server" Width="72px" CssClass="control" ToolTip="dd/mm/yyyy"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtTo_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                PopupButtonID="btnTo" TargetControlID="txtTo">
                            </cc1:CalendarExtender>
                            <asp:ImageButton ID="btnTo" runat="server" ImageUrl="~/images/Calendar.gif" TabIndex="-1" />
                        </td>
                        <td class="right-aligned">
                            <asp:Button ID="btnClearCriteria" runat="server" CssClass="button-action" OnClick="btnClearCriteria_Click"
                                Text="Reset Criteria" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:UpdatePanel ID="udpLevel" runat="server">
                                <ContentTemplate>
                                    Level
                                    <asp:DropDownList ID="ddlRegion" runat="server" CssClass="control" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    &nbsp;<asp:DropDownList ID="ddlBranch" runat="server" CssClass="control" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    &nbsp;<asp:DropDownList ID="ddlExecutive" runat="server" CssClass="control">
                                    </asp:DropDownList>
                                    <asp:Image ID="imgLoader" runat="server" ImageUrl="~/images/empty.gif" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:CollapsiblePanelExtender ID="pnlCriteria_CollapsiblePanelExtender" runat="server"
                CollapseControlID="btnCollapseExpand" CollapsedImage="~/images/expand.png" CollapsedText="Searched for: "
                Enabled="True" ExpandControlID="btnCollapseExpand" ExpandedImage="~/images/collapse.png"
                ExpandedText="Searched for: " SuppressPostBack="True" TargetControlID="pnlCriteria"
                ImageControlID="btnCollapseExpand">
            </cc1:CollapsiblePanelExtender>
        </td>
    </tr>
    <tr>
        <td class="page-group">
            <asp:ImageButton ID="btnExpandCollapse" runat="server" ImageUrl="~/images/expand.png" />
            &nbsp;Filtered by:
            <asp:Label ID="lblFiltersDesc" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Panel ID="pnlFilters" runat="server">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            Classification:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlClassification" runat="server" CssClass="control">
                            </asp:DropDownList>
                        </td>
                        <td class="right-aligned">
                            <asp:Button ID="btnClearFilters" runat="server" CssClass="button-action" OnClick="btnClearFilters_Click"
                                Text="Clear Filters" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Opportunity Type:
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlBusinessType" runat="server" CssClass="control">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120px">
                            Industry:
                        </td>
                        <td colspan="2">
                            <asp:UpdatePanel ID="udpIndustry" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtFindIndustry" runat="server" CssClass="control" Width="400px"></asp:TextBox>
                                    <asp:Button ID="btnFindIndustry" runat="server" CssClass="button-action" OnClick="btnFindIndustry_Click"
                                        Text="Look-up" />
                                    <asp:Label ID="lblFoundIndustries" runat="server"></asp:Label>
                                    <br />
                                    <div id="divIndustry" runat="server" class="result" visible="false">
                                        <asp:CheckBoxList ID="lstIndustry" runat="server" RepeatLayout="Flow">
                                        </asp:CheckBoxList>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Source:
                        </td>
                        <td colspan="2">
                            <asp:UpdatePanel ID="udpSource" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtFindSource" runat="server" CssClass="control" Width="400px"></asp:TextBox>
                                    <asp:Button ID="btnFindSource" runat="server" CssClass="button-action" OnClick="btnFindSource_Click"
                                        Text="Look-up" />
                                    <asp:Label ID="lblFoundSources" runat="server"></asp:Label>
                                    <br />
                                    <div id="divSource" runat="server" class="result" visible="false">
                                        <asp:CheckBoxList ID="lstSource" runat="server" RepeatLayout="Flow">
                                        </asp:CheckBoxList>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Opportunity:
                        </td>
                        <td colspan="2">
                            <asp:UpdatePanel ID="udpOpportunity" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtFindOpportunity" runat="server" CssClass="control" Width="400px"></asp:TextBox>
                                    <asp:Button ID="btnFindOpportunity" runat="server" CssClass="button-action"
                                        Text="Look-up" onclick="btnFindOpportunity_Click" />
                                    <asp:Label ID="lblFoundOpportunities" runat="server"></asp:Label>
                                    <br />
                                    <div id="divOpportunities" runat="server" class="result" visible="false">
                                        <asp:CheckBoxList ID="lstOpportunity" runat="server" RepeatLayout="Flow">
                                        </asp:CheckBoxList>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:CollapsiblePanelExtender ID="pnlFilters_CollapsiblePanelExtender" runat="server"
                CollapseControlID="btnExpandCollapse" CollapsedImage="~/images/expand.png" CollapsedText="Filtered by: "
                Enabled="True" Collapsed="True" ExpandControlID="btnExpandCollapse" ExpandedImage="~/images/collapse.png"
                ExpandedText="Filtered by: " ImageControlID="btnExpandCollapse" SuppressPostBack="True"
                TargetControlID="pnlFilters">
            </cc1:CollapsiblePanelExtender>
        </td>
    </tr>
    <tr>
        <td>
            
                    <uc1:ucMessanger ID="ucMessanger1" runat="server" />
            
        </td>
    </tr>
</table>
