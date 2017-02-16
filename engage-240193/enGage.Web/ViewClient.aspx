<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ViewClient.aspx.cs" Inherits="enGage.Web.ViewClient" %>

<%@ Register Src="Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page">
        <table class="page-table" width="100%">     
            <tr>
                <td class="page-group" colspan="5">
                    Client details
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <asp:Image ID="imgArrow" runat="server" ImageUrl="~/images/ArrowHollowSmall.gif" />
                </td>
                <td colspan="2">
                    <asp:Label ID="lblClientName" runat="server" CssClass="page-title"></asp:Label>
                </td>
                <td class="page-label">
                    Phone:
                    </td>
                <td class="page-label" width="175px">
                    <asp:Label ID="lblOfficePhone" runat="server" CssClass="page-title"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="1" class="underline">
                    &nbsp;
                </td>
                <td colspan="2" class="underline">
                    <asp:Label ID="lblAddress" runat="server"></asp:Label>
                </td>
                <td class="underline">
                    <span class="page-label">Fax:</span>
                    </td>
                <td class="underline">
                    <asp:Label ID="lblOfficeFacsimilie" runat="server" CssClass="page-text"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="page-label">
                    &nbsp;
                </td>
                <td class="page-label" colspan="1" width="120px">
                    Registered name:
                </td>
                <td class="page-text-bold" width="400px">
                    <asp:Label ID="lblRegisteredName" runat="server"></asp:Label>
                </td>
                <td class="page-label">
                    ABN/ACN:
                    </td>
                <td class="page-label">
                    <asp:Label ID="lblABNACN" runat="server" CssClass="page-text"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="page-label">
                    &nbsp;
                </td>
                <td class="page-label">
                    Insured as:
                </td>
                <td class="page-text" colspan="3">
                    <asp:Label ID="lblInsuredName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="page-label">
                    &nbsp;
                </td>
                <td class="page-label">
                    Source:
                </td>
                <td class="page-text" colspan="3">
                    <asp:Label ID="lblSource" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="page-label">
                    &nbsp;
                </td>
                <td class="page-label">
                    Industry:
                </td>
                <td class="page-text" colspan="3">
                    <asp:Label ID="lblIndustry" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="page-label">
                    &nbsp;
                </td>
                <td class="page-label">
                    Association:
                </td>
                <td class="page-text">
                    <asp:Label ID="lblAssociationName" runat="server"></asp:Label>
                </td>
                <td class="page-label">
                    Membership:
                    </td>
                <td class="page-label">
                    <asp:Label ID="lblAssociationMemberNumber" runat="server" CssClass="page-text"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="underline">
                    &nbsp;
                </td>
                <td class="underline">
                    <span class="page-label">OAMPS code:</span>
                </td>
                <td class="underline">
                    <asp:Label ID="lblClientCode" class="page-text" runat="server"></asp:Label>
                </td>
                <td class="underline" colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="page-label">
                    &nbsp;
                </td>
                <td class="page-label">
                    Account Executive:
                </td>
                <td class="page-text" colspan="3">
                    <asp:Label ID="lblAccountExecutive" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="page-group" colspan="5">
                    Client contacts
                    (<asp:Label ID="lblActiveContacts" runat="server"></asp:Label> active /
                    <asp:Label ID="lblInactiveContacts" runat="server"></asp:Label> inactive)</td>
            </tr>
            <tr>
                <td colspan="5" style="padding: 0px; margin: 0px">
                    <asp:GridView ID="grvContacts" runat="server" class="grid" AutoGenerateColumns="False"
                        ShowHeader="False" Width="100%" GridLines="Horizontal" OnRowDataBound="grvContacts_RowDataBound"
                        CssClass="grid">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Image ID="imgArrow1" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                </ItemTemplate>
                                <ItemStyle Width="18px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ClientID" HeaderText="ClientID" Visible="False" />
                            <asp:BoundField DataField="ContactID" HeaderText="ContactID" SortExpression="ContactID"
                                Visible="False" />
                            <asp:BoundField DataField="ContactName" HeaderText="ContactName" 
                                SortExpression="ContactName" />
                            <asp:BoundField DataField="Mobile" HeaderText="Mobile" SortExpression="Mobile" />
                            <asp:BoundField DataField="DirectLine" HeaderText="DirectLine" SortExpression="DirectLine" />
                            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoContacts" runat="server" CssClass="darkgrey-italic" 
                                Text="No active contacts"></asp:Label>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td class="grid-footer" colspan="5">
                <asp:Image ID="imgAll1" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                <asp:HyperLink ID="hplContactsSeeAll" runat="server" Height="18px">see all</asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td class="page-group" colspan="5">
                    Client opportunities (<asp:Label ID="lblOpenOpportunities" runat="server"></asp:Label>
                    open /
                    <asp:Label ID="lblClosedOpportunities" runat="server"></asp:Label>
                    closed)
                </td>
            </tr>
            <tr>
                <td colspan="5" style="padding: 0px; margin: 0px">
                    <asp:GridView ID="grvOpportunities" runat="server" AutoGenerateColumns="False" 
                        CssClass="grid" GridLines="Horizontal" ShowHeader="False" Width="100%" 
                        onrowdatabound="grvOpportunities_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Arrow">
                                <ItemTemplate>
                                    <asp:Image ID="imgArrow1" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                </ItemTemplate>
                                <ItemStyle Width="18px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Flagged">
                                <ItemTemplate>
                                    <asp:Image ID="imgFlagged" runat="server" 
                                        ImageUrl="~/images/OpportunityFlagged.gif" />
                                </ItemTemplate>
                                <ItemStyle Width="18px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BusinessType">
                                <ItemTemplate>
                                    <asp:Image ID="imgBusinessType" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="18px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ClientID" HeaderText="ClientID" 
                                SortExpression="ClientID" Visible="False" />
                            <asp:BoundField DataField="OpportunityID" Visible="False" HeaderText="OpportunityID" 
                                SortExpression="OpportunityID" />
                            <asp:BoundField DataField="OpportunityName" HeaderText="OpportunityName" 
                                SortExpression="OpportunityName" />
                            <asp:BoundField DataField="StatusName" HeaderText="StatusName" 
                                SortExpression="StatusName" />
                            <asp:BoundField DataField="Added" HeaderText="Added" 
                                SortExpression="Added" DataFormatString="Added: {0:dd-MMM-yy}" />
                            <asp:BoundField DataField="FollowUpDate" DataFormatString="Follow-up: {0:dd-MMM-yy}" 
                                HeaderText="FollowUpDate" SortExpression="FollowUpDate" />
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoOpportunities" runat="server" CssClass="darkgrey-italic" 
                                Text="No open opportunities"></asp:Label>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="5" class="grid-footer-underlined">
                <asp:Image ID="imgAll2" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                <asp:HyperLink ID="hplOpportunitiesSeeAll" runat="server" Height="18px">see all</asp:HyperLink>
                </td>
            </tr>
        </table>
        <table id="Table1" class="page-table" width="100%" runat="server">
            <tr>
                <td colspan="1">
                <asp:Button ID="btnFindClient" runat="server" Text="Find Client" 
                    CssClass="button-link" PostBackUrl="~/FindClient.aspx" />
                    <asp:Button ID="btnFollowups" runat="server" PostBackUrl="~/FollowUps.aspx" 
                        Text="Home" CssClass="button-link" />
                <asp:Button ID="btnDashboard" runat="server" Text="Dashboard" 
                    PostBackUrl="~/Dashboard.aspx" CssClass="button-link" Visible="false" />
                </td>
                <td colspan="1" id="tdEdit" class="right-aligned">
                    <asp:Button ID="btnAddContact" runat="server" Text="Add Contact" 
                        CssClass="button-action" />
                    <asp:Button ID="btnAddOpportunity" runat="server" Text="Add Opportunity" 
                        CssClass="button-action" />
                    <asp:Button ID="btnTransfer" runat="server" CssClass="button-action" 
                        Text="Transfer" />
                    <asp:Button ID="btnActiveInactive" runat="server" CssClass="button-action" 
                        onclick="btnActiveInactive_Click" />
                    <asp:Button ID="btnEditClient" runat="server" Text="Edit" 
                        CssClass="button-action" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <uc1:ucMessanger ID="ucMessanger1" runat="server" />
                </td>
            </tr>  
        </table>
    </div>
</asp:Content>
