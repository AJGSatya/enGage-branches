<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="FindClient.aspx.cs" Inherits="enGage.Web.FindClient" %>

<%@ Register Src="Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page">
        <asp:Panel DefaultButton="btnFind" runat="server">
            <table class="page-table" runat="server">
                <tr>
                    <td>
                        <asp:TextBox ID="txtSearchCriteria" runat="server" CssClass="control" MaxLength="250"
                            Width="250px"></asp:TextBox>
                        <asp:DropDownList ID="ddlClient" runat="server" CssClass="control">
                            <asp:ListItem Value="all">All Clients</asp:ListItem>
                            <asp:ListItem Value="A">Active Clients</asp:ListItem>
                            <asp:ListItem Value="I">Inactive Clients</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlMatch" runat="server" CssClass="control">
                            <asp:ListItem Value="C">Contains</asp:ListItem>
                            <asp:ListItem Value="E">Exact Match</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Button ID="btnFind" runat="server" CssClass="button-action" OnClick="btnFind_Click"
                            Text="Find" />
                        <asp:Button ID="btnAdd" runat="server" CssClass="button-action" PostBackUrl="~/AddClient.aspx"
                            Text="New" Visible="False"  />
                    </td>
                    <td class="right-aligned">
                        <asp:Button ID="btnFollowups" runat="server" CssClass="button-link" 
                            PostBackUrl="~/FollowUps.aspx" Text="Home" />
                        <asp:Button ID="btnDashboard" runat="server" CssClass="button-link" 
                            PostBackUrl="~/Dashboard.aspx" Text="Dashboard" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td class="page-group" id="tdHeaderCN" visible="false" colspan="2">
                        <b>
                            <asp:Label ID="lblResultCountCN" runat="server"></asp:Label>
                        </b>results with a <b>client</b> match for .. <b>&quot;
                            <asp:Label ID="lblSearchCN" runat="server"></asp:Label>
                            &quot;</b>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 0px; margin: 0px" colspan="2">
                        <asp:GridView ID="grvClientsClientName" runat="server" AutoGenerateColumns="False"
                            CssClass="grid" GridLines="Horizontal" OnRowDataBound="grvClients_RowDataBound"
                            ShowHeader="False" Width="100%" AllowPaging="True" PageSize="3">
                            <PagerSettings Visible="False" />
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
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td class="grid-footer" id="tdFooterCN" visible="false" colspan="2">
                        <asp:Image ID="imgAll1" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                        <asp:LinkButton ID="lnkCN" runat="server" CommandArgument="client" 
                            onclick="lnkCN_Click">see all</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td class="page-group" id="tdHeaderAD" visible="false" colspan="2">
                        <b>
                            <asp:Label ID="lblResultCountAD" runat="server"></asp:Label>
                        </b>results with an <b>address</b> match for .. <b>&quot; <b>
                            <asp:Label ID="lblSearchAD" runat="server"></asp:Label>
                        </b>&quot;</b>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 0px; margin: 0px" colspan="2">
                        <asp:GridView ID="grvClientsAddress" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            CssClass="grid" GridLines="Horizontal" OnRowDataBound="grvClients_RowDataBound"
                            PageSize="3" ShowHeader="False" Width="100%">
                            <PagerSettings Visible="False" />
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
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td class="grid-footer" id="tdFooterAD" visible="false" colspan="2">
                        <asp:Image ID="imgArrow5" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                        <asp:LinkButton ID="lnkAD" runat="server" CommandArgument="address" 
                            onclick="lnkCN_Click">see all</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td id="tdHeaderIND" class="page-group" visible="false" colspan="2">
                        <b>
                            <asp:Label ID="lblResultCountIND" runat="server"></asp:Label>
                        </b>results with an <b>industry</b> match for .. <b>&quot; <b>
                            <asp:Label ID="lblSearchIND" runat="server"></asp:Label>
                        </b>&quot;</b>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 0px; margin: 0px" colspan="2">
                        <asp:GridView ID="grvClientsIndustry" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" CssClass="grid" GridLines="Horizontal" 
                            OnRowDataBound="grvClients_RowDataBound" PageSize="3" ShowHeader="False" 
                            Width="100%">
                            <PagerSettings Visible="False" />
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
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td id="tdFooterIND" class="grid-footer" visible="false" colspan="2">
                        <asp:Image ID="imgArrow11" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                        <asp:LinkButton ID="lnkIND" runat="server" CommandArgument="industry" 
                            onclick="lnkCN_Click">see all</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td id="tdHeaderBA" class="page-group" colspan="2" visible="false">
                        <b>
                        <asp:Label ID="lblResultCountBA" runat="server"></asp:Label>
                        </b>results in iBAIS for ..<b>&quot;<asp:Label ID="lblSearchBA" runat="server"></asp:Label>
                        &quot;</b></td>
                </tr>
                <tr>
                    <td style="padding: 0px; margin: 0px" colspan="2">
                        <asp:GridView ID="grvClientsBA" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" CssClass="grid" GridLines="Horizontal" 
                            PageSize="3" ShowHeader="False" 
                            Width="100%" onrowdatabound="grvClientsBA_RowDataBound">
                            <PagerSettings Visible="False" />
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
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                            OldValuesParameterFormatString="original_{0}" SelectMethod="FindClientInBA" 
                            TypeName="enGage.BL.bizClient">
                            <SelectParameters>
                                <asp:Parameter Name="clientName" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
                <tr>
                    <td id="tdFooterBA" class="grid-footer" colspan="2" visible="false">
                        <asp:Image ID="imgArrow13" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                            
                        <asp:LinkButton ID="lnkBA" runat="server" CommandArgument="ba" 
                            onclick="lnkCN_Click">see all</asp:LinkButton>
                            
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <uc1:ucMessanger ID="ucMessanger1" runat="server" />        
                    </td>
                </tr>
            </table>
        </asp:Panel>            
    </div>
</asp:Content>
