<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ViewContact.aspx.cs" Inherits="enGage.Web.ViewContact" %>

<%@ Register Src="Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page">
        <table id="trDetails" class="page-table" style="width: 100%;" runat="server">
            <tr>
                <td class="page-group" colspan="3">
                    Client summary
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
                                <asp:Label ID="lblOfficePhoneLabel" runat="server" CssClass="page-label" Text="Phone: "></asp:Label>
                                <asp:Label ID="lblOfficePhone" runat="server" CssClass="page-title"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1" width="18px" class="underline">
                                &nbsp;
                            </td>
                            <td class="underline">
                                <asp:Label ID="lblAddress" runat="server"></asp:Label>
                            </td>
                            <td class="right-aligned-underlined">
                                <asp:Label ID="lblAssociationLabel" runat="server" CssClass="page-label" Text="Association: "></asp:Label>
                                <asp:Label ID="lblAssociation" runat="server" CssClass="page-text"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1" class="page-label">
                                &nbsp;
                            </td>
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
                    Contact details
                </td>
            </tr>
            <tr>
                <td width="18px">
                    &nbsp;
                </td>
                <td class="page-label">
                    Salutation:
                </td>
                <td width="650px" class="page-text">
                    <asp:Label ID="lblTitle" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td class="page-label">
                    Contact Name:
                </td>
                <td class="page-text">
                    <asp:Label ID="lblContactName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td class="page-label">
                    Email:
                </td>
                <td class="page-text">
                    <asp:Label ID="lblEmail" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td class="page-label">
                    Mobile:
                </td>
                <td class="page-text">
                    <asp:Label ID="lblMobile" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="underline">
                    &nbsp;
                </td>
                <td class="underline">
                    <asp:Label ID="lblDirectLineLabel" runat="server" CssClass="page-label" 
                        Text="Direct Line:"></asp:Label>
                </td>
                <td class="underline">
                    <asp:Label ID="lblDirectLine" runat="server"></asp:Label>
                </td>
            </tr>
            </table>
        <table class="page-table" style="width: 100%;">
            <tr>
                <td>
                    <asp:Button ID="btnFindClient" runat="server" Text="Find Client" CssClass="button-link"
                        PostBackUrl="~/FindClient.aspx" />
                    <asp:Button ID="btnFollowups" runat="server" PostBackUrl="~/FollowUps.aspx" Text="Home"
                        CssClass="button-link" />
                    <asp:Button ID="btnDashboard" runat="server" Text="Dashboard" PostBackUrl="~/Dashboard.aspx"
                        CssClass="button-link" Visible="false" />
                    <asp:Button ID="btnBack" runat="server" Text="Client" OnClick="btnBack_Click"
                        CssClass="button-link" />
                </td>
                <td colspan="1" class="right-aligned">
                    <asp:Button ID="btnSendEmail" runat="server" CssClass="button-action" 
                        Text="Send Email" />
                    <asp:Button ID="btnSetPrimary" runat="server" CssClass="button-action" 
                        Text="Set Primary" onclick="btnSetPrimary_Click" />
                    <asp:Button ID="btnActiveInactive" runat="server" CssClass="button-action" 
                        onclick="btnActiveInactive_Click" />
                    <asp:Button ID="btnEditContact" runat="server" CssClass="button-action" OnClick="btnEditContact_Click"
                        Text="Edit" />
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
