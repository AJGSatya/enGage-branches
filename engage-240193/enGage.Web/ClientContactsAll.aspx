<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ClientContactsAll.aspx.cs" Inherits="enGage.Web.ClientContactsAll" %>
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
                <td class="page-label" width="700px" colspan="2" rowspan="1">
                    Account Executive:
                    <asp:Label ID="lblAccountExecutive" runat="server" CssClass="page-text"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="page-group" colspan="3">
                    Active client contacts (<asp:Label ID="lblActiveContacts" runat="server"></asp:Label>)
                </td>
            </tr>
            <tr>
                <td colspan="3" style="padding: 0px; margin: 0px" rowspan="1">
                    <asp:GridView ID="grvActiveContacts" runat="server" class="grid" AutoGenerateColumns="False"
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
                <td colspan="3" class="page-group">
                    Inactive client contacts (<asp:Label ID="lblInactiveContacts" runat="server"></asp:Label>)
                </td>
            </tr>
            <tr>
                <td colspan="3" style="padding: 0px; margin: 0px">
                    <asp:GridView ID="grvInactiveContacts" runat="server" class="grid" AutoGenerateColumns="False"
                        ShowHeader="False" Width="100%" GridLines="Horizontal" OnRowDataBound="grvContacts_RowDataBound"
                        CssClass="grid">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Image ID="imgArrow2" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
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
                                Text="No inactive contacts"></asp:Label>
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
                <td colspan="3" rowspan="1">
                    <uc1:ucMessanger ID="ucMessanger1" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
