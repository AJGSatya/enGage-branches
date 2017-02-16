<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ClientOpportunitiesAll.aspx.cs" Inherits="enGage.Web.ClientOpportunitiesAll" %>
<%@ Register src="Controls/ucMessanger.ascx" tagname="ucMessanger" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page">
        <table class="page-table">
            <tr>
                <td class="page-group" colspan="3">
                    Client details
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <asp:Image ID="imgArrow" runat="server" ImageUrl="~/images/ArrowHollowSmall.gif" />
                </td>
                <td width="570px">
                    <asp:Label ID="lblClientName" runat="server" CssClass="page-title"></asp:Label>
                </td>
                <td class="page-label">
                    Phone:
                    <asp:Label ID="lblOfficePhone" runat="server" CssClass="page-title"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="underline" colspan="1">
                    &nbsp;
                </td>
                <td class="underline">
                    <asp:Label ID="lblAddress" runat="server"></asp:Label>
                </td>
                <td  class="underline">
                    <span class="page-label">Fax:</span>
                    <asp:Label ID="lblOfficeFacsimilie" runat="server" CssClass="page-text"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="page-label">
                    &nbsp;
                </td>
                <td class="page-label" colspan="2">
                    Account Executive:
                    <asp:Label ID="lblAccountExecutive" runat="server" CssClass="page-text"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="page-group" colspan="3">
                    Open client opportunities (<asp:Label ID="lblOpenOpportunities" runat="server"></asp:Label>)
                </td>
            </tr>
            <tr>
                <td colspan="3" style="padding: 0px; margin: 0px">
                    <asp:GridView ID="grvOpenOpportunities" runat="server" AutoGenerateColumns="False" 
                        CssClass="grid" GridLines="Horizontal" ShowHeader="False" Width="100%" 
                        onrowdatabound="grvOpenOpportunities_RowDataBound">
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
                                SortExpression="Added" DataFormatString="Added: {0:dd-MMM-yy}" >
                            <ItemStyle HorizontalAlign="Right" Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FollowUpDate" DataFormatString="Follow-up: {0:dd-MMM-yy}" 
                                HeaderText="FollowUpDate" SortExpression="FollowUpDate" >
                            <ItemStyle HorizontalAlign="Right" Width="150px" />
                            </asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoOpenOpportunities" runat="server" 
                                CssClass="darkgrey-italic" Text="No open opportunities"></asp:Label>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="3" class="page-group">
                    Closed client opportunities (<asp:Label ID="lblClosedOpportunities" runat="server"></asp:Label>)
                </td>
            </tr>
            <tr>
                <td colspan="3" style="padding: 0px; margin: 0px">
                    <asp:GridView ID="grvClosedOpportunities" runat="server" AutoGenerateColumns="False" 
                        CssClass="grid" GridLines="Horizontal" ShowHeader="False" Width="100%" 
                        onrowdatabound="grvClosedOpportunities_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Arrow">
                                <ItemTemplate>
                                    <asp:Image ID="imgArrow2" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
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
                                SortExpression="Added" DataFormatString="Added: {0:dd-MMM-yy}" >
                            <ItemStyle HorizontalAlign="Right" Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FollowUpDate" DataFormatString="Follow-up: {0:dd-MMM-yy}" 
                                HeaderText="FollowUpDate" SortExpression="FollowUpDate" >
                            <ItemStyle HorizontalAlign="Right" Width="150px" />
                            </asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoClosedOpportunities" runat="server" 
                                CssClass="darkgrey-italic" Text="No closed opportunities"></asp:Label>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Button ID="btnBack" runat="server" CssClass="button-link" 
                        Text="Client" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <uc1:ucMessanger ID="ucMessanger1" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
