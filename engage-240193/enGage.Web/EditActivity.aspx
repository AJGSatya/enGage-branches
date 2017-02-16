<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="EditActivity.aspx.cs" Inherits="enGage.Web.EditActivity" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page">
        <asp:ScriptManager runat="server" ID="scriptManagerId">
            <Scripts>
                <asp:ScriptReference Path="../scripts/CallWebServiceMethods.js" />
            </Scripts>
        </asp:ScriptManager>
        <asp:HiddenField ID="hidEditMode" runat="server" />
        <table id="tblActivity" runat="server" class="page-table" style="width: 100%;">
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
                    Opportunity summary
                </td>
            </tr>
            <tr>
                <td style="padding: 0px; margin: 0px" colspan="3">
                    <table class="page-table" width="100%">
                        <tr>
                            <td width="18px">
                                <asp:Image ID="imgBusinessType" runat="server" />
                            </td>
                            <td width="18px">
                                <asp:Image ID="imgFlagged" runat="server" ImageUrl="~/images/OpportunityFlagged.gif" />
                            </td>
                            <td>
                                <asp:Label ID="lblOpportunityName" runat="server" CssClass="page-title" Height="20px"></asp:Label>
                            </td>
                            <td class="right-aligned">
                                <asp:Label ID="lblStatus" runat="server" CssClass="page-title"></asp:Label>
                            </td>
                            <td class="page-label" width="100px">
                                Renewal Date:
                            </td>
                            <td width="100px">
                                <asp:Label ID="lblOpportunityDue" runat="server" CssClass="page-title"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="page-group" colspan="3">
                    Activity details
                </td>
            </tr>
            <tr>
                <td width="18px">
                    &nbsp;
                </td>
                <td>
                    Status:
                </td>
                <td width="580px">
                    <asp:Label ID="lblActivityStatus" runat="server" CssClass="page-title"></asp:Label>
                    <asp:HiddenField ID="hidStatusID" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    Follow-up:
                </td>
                <td>
                    <asp:TextBox ID="txtFollowUpDate" runat="server" Width="72px" CssClass="control"
                        ToolTip="dd/mm/yyyy"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtFollowUpDate_CalendarExtender" runat="server" Enabled="True"
                        Format="dd/MM/yyyy" PopupButtonID="btnFollowUpDate" TargetControlID="txtFollowUpDate">
                    </cc1:CalendarExtender>
                    <asp:ImageButton ID="btnFollowUpDate" runat="server" ImageUrl="~/images/Calendar.gif"
                        TabIndex="-1" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    Contact:
                </td>
                <td>
                    <asp:DropDownList ID="ddlContact" runat="server" CssClass="control">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    Activity:
                </td>
                <td>
                    <asp:TextBox ID="txtActivityNote" runat="server" CssClass="control" Height="72px"
                        TextMode="MultiLine" Width="550px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="page-group" colspan="3">
                    Additional
                </td>
            </tr>
            <tr id="trQualifiedIn4" visible="false">
                <td>
                    &nbsp;
                </td>
                <td>
                    Renewal Date:
                </td>
                <td>
                    <asp:TextBox ID="txtOpportunityDue" runat="server" Width="72px" MaxLength="10" CssClass="control"
                        ToolTip="dd/mm/yyyy"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtOpportunityDue_CalendarExtender" runat="server" PopupButtonID="btnOpportunityDue"
                        TargetControlID="txtOpportunityDue" Format="dd/MM/yyyy">
                    </cc1:CalendarExtender>
                    <asp:ImageButton ID="btnOpportunityDue" runat="server" ImageUrl="~/images/Calendar.gif"
                        TabIndex="-1" />
                </td>
            </tr>
            <tr id="trQualifiedIn1" visible="false">
                <td>
                    &nbsp;
                </td>
                <td>
                    Incumbent Broker:
                </td>
                <td>
                    <asp:TextBox ID="txtIncumbentBroker" runat="server" Width="300px" 
                        CssClass="control" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr id="trQualifiedIn2" visible="false">
                <td>
                    &nbsp;
                </td>
                <td>
                    Incumbent Insurer:
                </td>
                <td>
                    <asp:TextBox ID="txtIncumbentInsurer" runat="server" Width="300px" 
                        CssClass="control" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr id="trQualifiedIn3" visible="false">
                <td>
                    &nbsp;
                </td>
                <td>
                    CSS Segment:
                </td>
                <td>
                    <asp:DropDownList ID="ddlClassification" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlClassification_SelectedIndexChanged"
                        CssClass="control">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trSubmitted1" visible="false">
                <td>
                    &nbsp;
                </td>
                <td>
                    Expected OAMPS income Quoted ($):
                </td>
                <td>
                    <asp:TextBox ID="txtNetBrokerageQuoted" runat="server" MaxLength="10" CssClass="control"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="txtNetBrokerageQuoted_FilteredTextBoxExtender" runat="server"
                        Enabled="True" TargetControlID="txtNetBrokerageQuoted" ValidChars=",.0123456789">
                    </cc1:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr id="trAccepted1" visible="false">
                <td>
                    &nbsp;
                </td>
                <td>
                    Expected OAMPS income Actual ($):
                </td>
                <td>
                    <asp:TextBox ID="txtNetBrokerageActual" runat="server" MaxLength="10" CssClass="control"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="txtNetBrokerageActual_FilteredTextBoxExtender" runat="server"
                        Enabled="True" TargetControlID="txtNetBrokerageActual" ValidChars=",.0123456789">
                    </cc1:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr id="trLost1" visible="false">
                <td>
                    &nbsp;
                </td>
                <td>
                    Date Completed:
                </td>
                <td>
                    <asp:TextBox ID="txtDateCompleted" runat="server" MaxLength="10" Width="72px" CssClass="control"
                        ToolTip="dd/mm/yyyy"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtDateCompleted_CalendarExtender" runat="server" Enabled="True"
                        Format="dd/MM/yyyy" PopupButtonID="btnDateCompleted" TargetControlID="txtDateCompleted">
                    </cc1:CalendarExtender>
                    <asp:ImageButton ID="btnDateCompleted" runat="server" ImageUrl="~/images/Calendar.gif"
                        TabIndex="-1" />
                </td>
            </tr>
            <tr id="trProcessed1" visible="false">
                <td>
                    &nbsp;
                </td>
                <td>
                    Memo Number:
                </td>
                <td>
                    <asp:TextBox ID="txtMemoNumber" runat="server" MaxLength="50" Width="150px" CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr id="trProcessed2" visible="false">
                <td>
                    &nbsp;
                </td>
                <td>
                    Client Code:
                </td>
                <td>
                    <asp:TextBox ID="txtClientCode" runat="server" MaxLength="50" Width="150px" CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="3" class="right-aligned">
                    <asp:Button ID="btnBack" runat="server" Text="Cancel" OnClientClick="return confirm('Do you really want to cancel editing this activity?');"
                        OnClick="btnBack_Click" CssClass="button-action" />
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="button-action" />
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
