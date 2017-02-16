<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="EditOpportunity.aspx.cs" Inherits="enGage.Web.EditOpportunity" %>

<%@ Register Src="Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page">
        <asp:ScriptManager runat="server" ID="scriptManagerId">
            <Scripts>
                <asp:ScriptReference Path="../scripts/CallWebServiceMethods.js" />
            </Scripts>
        </asp:ScriptManager>
        <table class="page-table" style="width: 100%;" runat="server">
            <tr>
                <td class="page-group" colspan="5">
                    Client summary
                </td>
            </tr>
            <tr>
                <td style="padding: 0px; margin: 0px" colspan="5">
                    <table class="page-table" width="100%">
                        <tr>
                            <td colspan="1" width="18px">
                                <asp:Image ID="imgArrow" runat="server" ImageUrl="~/images/ArrowHollowSmall.gif" />
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblClientName" runat="server" CssClass="page-title"></asp:Label>
                            </td>
                            <td class="right-aligned" colspan="2">
                                <asp:Label ID="lblOfficePhoneLabel" runat="server" CssClass="page-label" Text="Phone: "></asp:Label>
                                <asp:Label ID="lblOfficePhone" runat="server" CssClass="page-title"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1" width="18px" class="underline">
                                &nbsp;
                            </td>
                            <td colspan="2" class="underline">
                                <asp:Label ID="lblAddress" runat="server"></asp:Label>
                            </td>
                            <td class="right-aligned-underlined" colspan="2">
                                <asp:Label ID="lblAssociationLabel" runat="server" CssClass="page-label" Text="Association: "></asp:Label>
                                <asp:Label ID="lblAssociation" runat="server" CssClass="page-text"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1" class="page-label">
                                &nbsp;
                            </td>
                            <td colspan="4" class="page-label">
                                Account Executive:
                                <asp:Label ID="lblAccountExecutive" runat="server" CssClass="page-text"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="page-group" colspan="5">
                    Opportunity details
                </td>
            </tr>
            <tr>
                <td colspan="1" width="18px">
                    &nbsp;
                </td>
                <td width="120px">
                    Opportunity:
                </td>
                <td>
                    <asp:TextBox ID="txtOpportunityName" runat="server" MaxLength="250" Width="300px"
                        CssClass="control"></asp:TextBox>
                </td>
                <td width="120px">
                    Renewal Date:
                </td>
                <td>
                    <asp:TextBox ID="txtOpportunityDue" runat="server" Width="72px" MaxLength="10" CssClass="control"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtOpportunityDue_CalendarExtender" runat="server" PopupButtonID="btnOpportunityDue"
                        TargetControlID="txtOpportunityDue" Format="dd/MM/yyyy">
                    </cc1:CalendarExtender>
                    <asp:ImageButton ID="btnOpportunityDue" runat="server" ImageUrl="~/images/Calendar.gif"
                        TabIndex="-1" CssClass="control" />
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    &nbsp;
                </td>
                <td>
                    Opportunity Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddlBusinessType" runat="server" CssClass="control">
                    </asp:DropDownList>
                </td>
                <td>
                    Date Completed:
                </td>
                <td>
                    <asp:TextBox ID="txtDateCompleted" runat="server" Width="72px" MaxLength="10" CssClass="control"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtDateCompleted_CalendarExtender" runat="server" PopupButtonID="btnDateCompleted"
                        TargetControlID="txtDateCompleted" Format="dd/MM/yyyy">
                    </cc1:CalendarExtender>
                    <asp:ImageButton ID="btnDateCompleted" runat="server" ImageUrl="~/images/Calendar.gif"
                        TabIndex="-1" CssClass="control" />
                </td>
            </tr>
            <tr>
                <td colspan="1" class="underline">
                    &nbsp;
                </td>
                <td class="underline">
                    Flag Opportunity:
                </td>
                <td class="underline">
                    <asp:DropDownList ID="ddlFlagged" runat="server" CssClass="control">
                        <asp:ListItem Value="False">Not Flagged</asp:ListItem>
                        <asp:ListItem Value="True">Flagged</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="underline">
                    Oppotunity Status:
                </td>
                <td class="underline">
                    <asp:Label ID="lblOpportunityStatus" runat="server" CssClass="page-title"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    &nbsp;
                </td>
                <td>
                    Contact:
                </td>
                <td>
                    <asp:DropDownList ID="ddlContact" runat="server" CssClass="control">
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    &nbsp;
                </td>
                <td>
                    CSS Segment:
                </td>
                <td>
                    <asp:DropDownList ID="ddlClassification" runat="server" CssClass="control">
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    &nbsp;
                </td>
                <td>
                    Incumbent Broker:
                </td>
                <td>
                    <asp:TextBox ID="txtIncumbentBroker" runat="server" Width="300px" 
                        CssClass="control" MaxLength="100"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    &nbsp;
                </td>
                <td>
                    Incumbent Underwriter:
                </td>
                <td>
                    <asp:TextBox ID="txtIncumbentInsurer" runat="server" Width="300px" 
                        CssClass="control" MaxLength="100"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    &nbsp;
                </td>
                <td>
                    Estimated Broking Income ($):
                </td>
                <td>
                    <asp:TextBox ID="txtEstimatedBrokingIncome" runat="server" MaxLength="10" CssClass="control"
                        Width="150px"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="txtEstimatedBrokingIncome_FilteredTextBoxExtender" runat="server"
                        Enabled="True" TargetControlID="txtEstimatedBrokingIncome" ValidChars=",.0123456789">
                    </cc1:FilteredTextBoxExtender>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    &nbsp;
                </td>
                <td>
                    Broking Income Quoted ($):
                </td>
                <td>
                    <asp:TextBox ID="txtNetBrokerageQuoted" runat="server" MaxLength="10" CssClass="control"
                        Width="150px"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="txtNetBrokerageQuoted_FilteredTextBoxExtender" runat="server"
                        Enabled="True" TargetControlID="txtNetBrokerageQuoted" ValidChars=",.0123456789">
                    </cc1:FilteredTextBoxExtender>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    &nbsp;
                </td>
                <td>
                    Actual Broking Income ($):
                </td>
                <td>
                    <asp:TextBox ID="txtNetBrokerageActual" runat="server" MaxLength="10" CssClass="control"
                        Width="150px"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="txtNetBrokerageActual_FilteredTextBoxExtender" runat="server"
                        Enabled="True" TargetControlID="txtNetBrokerageActual" ValidChars=",.0123456789">
                    </cc1:FilteredTextBoxExtender>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="1" class="underline">
                    &nbsp;
                </td>
                <td class="underline">
                    Memo Number:
                </td>
                <td class="underline">
                    <asp:TextBox ID="txtMemoNumber" runat="server" MaxLength="50" Width="200px" CssClass="control"></asp:TextBox>
                </td>
                <td class="underline">
                    &nbsp;
                </td>
                <td class="underline">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    &nbsp;
                </td>
                <td>
                    Next Activity:
                </td>
                <td>
                    <asp:Label ID="lblNextActivity" runat="server" CssClass="page-text-bold"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
        <table class="page-table" style="width: 100%;">
            <tr>
                <td class="right-aligned">
                    <asp:Button ID="btnCancel" runat="server" CssClass="button-action" Text="Cancel"
                        OnClick="btnCancel_Click" OnClientClick="return confirm('Do you really want to cancel editing this opportunity?');" />
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
