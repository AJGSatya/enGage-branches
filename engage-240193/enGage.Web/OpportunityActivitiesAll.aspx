<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="OpportunityActivitiesAll.aspx.cs" Inherits="enGage.Web.OpportunityActivitiesAll" %>
<%@ Register src="Controls/ucMessanger.ascx" tagname="ucMessanger" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page">
        <table class="page-table" style="width: 100%;" runat="server">
            <tr>
                <td class="page-group" colspan="6">
                    Client summary
                </td>
            </tr>
            <tr>
                <td colspan="1" width="18px">
                    <asp:Image ID="imgArrow" runat="server" ImageUrl="~/images/ArrowHollowSmall.gif" />
                </td>
                <td colspan="1">
                    <asp:Label ID="lblClientName" runat="server" CssClass="page-title"></asp:Label>
                </td>
                <td class="right-aligned" colspan="1">
                    <asp:Label ID="lblOfficePhoneLabel" runat="server" CssClass="page-label" 
                        Text="Phone: "></asp:Label>
                    <asp:Label ID="lblOfficePhone" runat="server" CssClass="page-title"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="1" width="18px" class="underline">
                    &nbsp;</td>
                <td colspan="1" class="underline">
                    <asp:Label ID="lblAddress" runat="server"></asp:Label>
                </td>
                <td class="right-aligned-underlined" colspan="1">
                    <asp:Label ID="lblAssociationLabel" runat="server" CssClass="page-label" 
                        Text="Association: "></asp:Label>
                    <asp:Label ID="lblAssociation" runat="server" CssClass="page-text"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="1" class="page-label">
                    &nbsp;</td>
                <td colspan="2" class="page-label">
                    Account Executive: 
                    <asp:Label ID="lblAccountExecutive" runat="server" CssClass="page-text"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="page-group" colspan="3">
                    Opportunity active activities (<asp:Label ID="lblActive" runat="server"></asp:Label>)</td>
            </tr>
            <tr>
                <td style="padding: 0px; margin: 0px" colspan="3">
                    <asp:GridView ID="grvActiveActivities" runat="server" CssClass="grid" class="grid"
                        AutoGenerateColumns="False" ShowHeader="False" Width="100%" 
                        GridLines="Horizontal" onrowdatabound="grvActivities_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Arrow">
                                <ItemTemplate>
                                    <asp:Image ID="imgArror" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                </ItemTemplate>
                                <ItemStyle Width="18px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ActivityID" HeaderText="ActivityID" 
                                SortExpression="ActivityID" ReadOnly="True" Visible="False" />
                            <asp:BoundField DataField="StatusDescription" HeaderText="StatusDescription" 
                                SortExpression="StatusDescription" ReadOnly="True" >
                            <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Added" HeaderText="Added" SortExpression="Added" 
                                DataFormatString="{0:dd-MMM-yy}" ReadOnly="True" >
                            <ItemStyle Width="120px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActivityNote" HeaderText="ActivityNote" 
                                SortExpression="ActivityNote" ReadOnly="True" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td 
                    class="page-group" colspan="3">
                    Opportunity inactive activities (<asp:Label ID="lblInactive" runat="server"></asp:Label>)</td>
            </tr>
            <tr>
                <td style="padding: 0px; margin: 0px" colspan="3">
                    <asp:GridView ID="grvInactiveActivities" runat="server" CssClass="grid" class="grid"
                        AutoGenerateColumns="False" ShowHeader="False" Width="100%" 
                        GridLines="Horizontal" onrowdatabound="grvActivities_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Arrow">
                                <ItemTemplate>
                                    <asp:Image ID="imgArror0" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                </ItemTemplate>
                                <ItemStyle Width="18px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ActivityID" HeaderText="ActivityID" 
                                SortExpression="ActivityID" ReadOnly="True" Visible="False" />
                            <asp:BoundField DataField="StatusDescription" HeaderText="StatusDescription" 
                                SortExpression="StatusDescription" ReadOnly="True" >
                            <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Added" HeaderText="Added" SortExpression="Added" 
                                DataFormatString="{0:dd-MMM-yy}" ReadOnly="True" >
                            <ItemStyle Width="120px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActivityNote" HeaderText="ActivityNote" 
                                SortExpression="ActivityNote" ReadOnly="True" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <table class="page-table" style="width: 100%;">
            <tr>
                <td colspan="3">
                    <asp:Button ID="btnBack" runat="server" CssClass="button-link" 
                        Text="Opportunity" />
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
