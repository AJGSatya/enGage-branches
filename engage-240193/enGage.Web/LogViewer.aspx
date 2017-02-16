<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="LogViewer.aspx.cs"
    Inherits="enGage.Web.LogViewer" Title="" %>
<%@ Register Src="~/Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page">
        <div>
            <uc1:ucMessanger ID="ucMessanger1" runat="server" />
        </div>
        <div class="page-text-bold">
            <span><b>Filter options: </b></span>
            <asp:RadioButtonList ID="rblFilterOption" runat="server" AutoPostBack="True" 
                RepeatColumns="4" CssClass="blue-bold">
                <asp:ListItem Selected="True" Value="0">Today</asp:ListItem>
                <asp:ListItem Value="1">Yesterday</asp:ListItem>
                <asp:ListItem Value="2">This Week</asp:ListItem>
                <asp:ListItem Value="3">Last Week</asp:ListItem>
                <asp:ListItem Value="4">This Month</asp:ListItem>
                <asp:ListItem Value="5">Last Month</asp:ListItem>
                <asp:ListItem Value="6">All</asp:ListItem>
            </asp:RadioButtonList>            
        </div>
        <div>         
            <asp:DataList ID="dalLogs" runat="server" DataKeyField="ExceptionLogId" DataSourceID="ObjectDataSource1">
                <ItemTemplate>
                    <table style="width: 100%; border-color: #6e8d82; border-width: 1px; border-style: solid">
                        <tr>
                            <td valign="top" width="120px">
                                Short Description:
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblShortDescription" runat="server" Text='<%# Eval("ShortDescription") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                Long Description:
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblLongDescription" runat="server" Text='<%# Eval("LongDescription") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Created Date:
                            </td>
                            <td width="300px">
                                <asp:Label ID="lblCreatedDate" runat="server" Text='<%# Eval("CreatedDate") %>'></asp:Label>
                            </td>
                            <td width="120px">
                                Created By:
                            </td>
                            <td width="300px">
                                <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
                SelectMethod="ListLogs" TypeName="enGage.BL.bizLog">
                <SelectParameters>
                    <asp:ControlParameter ControlID="rblFilterOption" Name="filter" PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>
