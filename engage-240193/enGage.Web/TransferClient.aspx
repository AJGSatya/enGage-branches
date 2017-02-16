<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="TransferClient.aspx.cs" Inherits="enGage.Web.TransferClient" %>
<%@ Register src="Controls/ucMessanger.ascx" tagname="ucMessanger" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page">
        <table class="page-table" style="width: 100%;" runat="server">
            <tr>
                <td colspan="3">
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                </td>
            </tr>
            <tr>
                <td class="page-group" colspan="3">
                    Client details
                </td>
            </tr>
            <tr>
                <td style="padding: 0px; margin: 0px" colspan="3">
                    <table class="page-table" width="100%">
                        <tr>
                            <td colspan="1" width="18px">
                                <asp:Image ID="imgArrow" runat="server" ImageUrl="~/images/ArrowHollowSmall.gif" />
                            </td>
                            <td>
                                <asp:Label ID="lblClientName" runat="server" CssClass="page-title"></asp:Label>
                            </td>
                            <td class="right-aligned">
                                <asp:Label ID="lblOfficePhoneLabel" runat="server" CssClass="page-label" 
                                    Text="Phone: "></asp:Label>
                                <asp:Label ID="lblOfficePhone" runat="server" CssClass="page-title"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1" width="18px" class="underline">
                                &nbsp;</td>
                            <td class="underline">
                                <asp:Label ID="lblAddress" runat="server"></asp:Label>
                            </td>
                            <td class="right-aligned-underlined">
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
                    </table>
                </td>
            </tr>
            <tr>
                <td class="page-group" colspan="3">
                    Opportunity details</td>
            </tr>
            <tr>
                <td colspan="1" width="18px">
                    &nbsp;</td>
                <td class="page-label" width="110px" valign="top">
                    Account Executive:
                </td>
                <td class="page-label" valign="top">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFind" runat="server" CssClass="control" 
                                Width="400px"></asp:TextBox>
                            <asp:Button ID="btnLookup" runat="server" CssClass="button-action" 
                                Text="Look-up" onclick="btnLookup_Click" />
                            <asp:Label ID="lblFindMessage" runat="server" CssClass="page-text" 
                                Visible="False">Name can't be empty</asp:Label>
                            <br />    
                            <asp:ListBox ID="lstExecutives" runat="server" CssClass="control" Rows="10" 
                                Visible="False"></asp:ListBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="3" class="underline">
                    &nbsp;</td>
            </tr>
        </table>
        <table class="page-table" style="width: 100%;">
            <tr>
                <td class="right-aligned">
                    <asp:Button ID="btnBack" runat="server" Text="Cancel" OnClientClick="return confirm('Do you really want to cancel transfering this client?');"
                        OnClick="btnBack_Click" CssClass="button-action" />
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="button-action" />
                </td>
            </tr>
            <tr>
                <td>
                    <uc1:ucMessanger ID="ucMessanger1" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
