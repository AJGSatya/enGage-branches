<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="FindClientAll.aspx.cs" Inherits="enGage.Web.FindClientAll" %>
<%@ Register src="Controls/ucMessanger.ascx" tagname="ucMessanger" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page">
        <table class="page-table">
            <tr>
                <td class="page-group" id="tdHeaderCN" visible="false">
                    <b>
                    <asp:Label ID="lblResultCount" runat="server"></asp:Label>
                    </b>results for .. <b>&quot;
                    <asp:Label ID="lblSearch" runat="server"></asp:Label>
                    &quot;</b>
                </td>
            </tr>
            <tr>
                <td style="padding: 0px; margin: 0px">
                    <asp:GridView ID="grvClients" runat="server" AutoGenerateColumns="False"
                        CssClass="grid" GridLines="Horizontal" OnRowDataBound="grvClients_RowDataBound"
                        ShowHeader="False" Width="100%" AllowPaging="True" 
                        onpageindexchanging="grvClients_PageIndexChanging" PageSize="25">
                        <PagerSettings FirstPageImageUrl="~/images/ArrowFirst.gif" 
                            FirstPageText="first" LastPageImageUrl="~/images/ArrowLast.gif" 
                            LastPageText="last" Mode="NumericFirstLast" Position="Top" />
                        <RowStyle CssClass="row" Wrap="True" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Image ID="imgArrow" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                </ItemTemplate>
                                <ItemStyle Width="18px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ClientID" HeaderText="ID" Visible="False" />
                            <asp:BoundField DataField="ClientName" HeaderText="Client Name">
                            </asp:BoundField>
                            <asp:BoundField DataField="Location" HeaderText="Location">
                            </asp:BoundField>
                            <asp:BoundField DataField="DisplayName" HeaderText="Account Executive">
                            </asp:BoundField>
                            <asp:BoundField DataField="Matched" HeaderText="Matched">
                            </asp:BoundField>
                        </Columns>
                        <PagerStyle CssClass="grid-pager" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td style="padding: 0px; margin: 0px">
                    <asp:GridView ID="grvClientsBA" runat="server" AllowPaging="True" 
                        AutoGenerateColumns="False" CssClass="grid" GridLines="Horizontal" 
                        PageSize="25" ShowHeader="False" 
                        Width="100%" onrowdatabound="grvClientsBA_RowDataBound" 
                        onpageindexchanging="grvClientsBA_PageIndexChanging">
                        <PagerSettings FirstPageImageUrl="~/images/ArrowFirst.gif" 
                            FirstPageText="first" LastPageImageUrl="~/images/ArrowLast.gif" 
                            LastPageText="last" Mode="NumericFirstLast" Position="Top" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                </ItemTemplate>
                                <ItemStyle Width="18px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ClientName" HeaderText="ClientName" 
                                SortExpression="ClientName">
                            </asp:BoundField>                                
                            <asp:BoundField DataField="ClientCode" HeaderText="ClientCode" 
                                SortExpression="ClientCode">
                            </asp:BoundField>
                            <asp:BoundField DataField="BranchCode" HeaderText="BranchCode" 
                                SortExpression="BranchCode">
                            </asp:BoundField>
                            <asp:BoundField DataField="Executive_Name" HeaderText="Executive_Name" 
                                SortExpression="Executive_Name">
                            </asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Image ID="imgArrow" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                    <asp:HyperLink ID="lnkCopy" Text="import" runat="server"></asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle Width="60px" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="grid-pager" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnBack" runat="server" CssClass="button-link" 
                        PostBackUrl="~/FindClient.aspx" Text="Summary" />
                <asp:Button ID="btnFindClient" runat="server" Text="Find Client" 
                    CssClass="button-link" PostBackUrl="~/FindClient.aspx" />
                </td>
            </tr>
            <tr>
                <td style="padding: 0px; margin: 0px">
                    <uc1:ucMessanger ID="ucMessanger1" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
