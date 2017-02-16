<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="AddContact.aspx.cs" Inherits="enGage.Web.AddContact" %>

<%@ Register Src="~/Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
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
                <td width="650px">
                    <asp:DropDownList ID="ddlTitle" runat="server" CssClass="control">
                        <asp:ListItem Selected="True" Value="">--Please Select--</asp:ListItem>
                        <asp:ListItem>Mr.</asp:ListItem>
                        <asp:ListItem>Mrs.</asp:ListItem>
                        <asp:ListItem>Ms.</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td class="page-label">
                    Contact Name:
                </td>
                <td>
                    <asp:TextBox ID="txtContactName" runat="server" MaxLength="50" Width="250px" CssClass="control"
                        ToolTip="name"></asp:TextBox>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="1" class="page-label">
                    Email:
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Width="250px" CssClass="control"
                        ToolTip="name@companyname.com.au"></asp:TextBox>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="1" class="page-label">
                    Mobile:
                </td>
                <td>
                    <asp:TextBox ID="txtMobile" runat="server" Width="150px" CssClass="control" 
                        ToolTip="0n nnnn nnnn" MaxLength="25"></asp:TextBox>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="underline">
                    &nbsp;
                </td>
                <td colspan="1" class="underline">
                    <asp:Label ID="lblDirectLineLabel" runat="server" CssClass="page-label" 
                        Text="Direct Line:"></asp:Label>
                </td>
                <td class="underline">
                    <asp:TextBox ID="txtDirectLine" runat="server" MaxLength="25" Width="150px" CssClass="control"
                        ToolTip="04nn nnn nnn"></asp:TextBox>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3" class="right-aligned">
                    <asp:Button ID="btnCancel" runat="server" CssClass="button-action" Text="Cancel"
                        OnClientClick="return confirm('Do you really want to cancel editing this contact?');"
                        OnClick="btnCancel_Click" />
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="button-action" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <uc1:ucMessanger ID="ucMessanger1" runat="server" />
                    <asp:HiddenField ID="hidEditMode" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
