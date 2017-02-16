<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewActivity.aspx.cs" Inherits="enGage.Web.ViewActivity" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register src="Controls/ucMessanger.ascx" tagname="ucMessanger" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page">
        <asp:ScriptManager runat="server" ID="scriptManagerId">
            <Scripts>
                <asp:ScriptReference Path="../scripts/CallWebServiceMethods.js" />
            </Scripts>
        </asp:ScriptManager>
        <table id="Table1" class="page-table" style="width: 100%;" runat="server">
            <tr>
                <td class="page-group">
                    Client summary
                </td>
            </tr>
            <tr>
                <td style="padding: 0px; margin: 0px">
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
                <td class="page-group">
                    Opportunity summary
                </td>
            </tr>
            <tr>
                <td style="padding: 0px; margin: 0px">
                    <table class="page-table" width="100%">
                        <tr>
                            <td width="18px">
                                
                    <asp:Image ID="imgBusinessType" runat="server" />
                                
                            </td>
                            <td width="18px">
                                
                    <asp:Image ID="imgFlagged" runat="server" 
                        ImageUrl="~/images/OpportunityFlagged.gif" />
                                
                            </td>
                            <td>
                                
                    <asp:Label ID="lblOpportunityName" runat="server" CssClass="page-title" 
                        Height="20px"></asp:Label>
                                
                            </td>
                            <td class="right-aligned">
                                
                    <asp:Label ID="lblOpportunityStatus" runat="server" CssClass="page-title"></asp:Label>
                                
                            </td>
                            <td class="page-label" width="100px">
                                
                                Opportunity Due:</td>
                            <td width="100px">
                                
                    <asp:Label ID="lblOpportunityDue" runat="server" CssClass="page-title"></asp:Label>
                                
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
             <tr>
                <td class="page-group">
                    Activity details</td>
            </tr>
       </table>
       <table id="tblActivity" runat="server" class="page-table" style="width: 100%;">
            <tr>
                <td width="18px">
                    &nbsp;</td>
                <td class="page-label">
                    Status:</td>
                <td>
                    <asp:Label ID="lblActivityStatus" runat="server" CssClass="page-title"></asp:Label>
                </td>
                <td class="right-aligned">
                    <asp:Label ID="lblFollowUpLabel" runat="server" CssClass="page-label" 
                        Text="Follow-up:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblFollowUpDate" runat="server" CssClass="page-title"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td class="page-label">
                    Contact:</td>
                <td>
                    <asp:Label ID="lblContact" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="underline">
                    &nbsp;</td>
                <td class="underline">
                    <asp:Label ID="lblActivityNoteLabel" runat="server" CssClass="page-label" 
                        Text="Activity Note:"></asp:Label>
                </td>
                <td class="underline" colspan="3" width="500px">
                    <asp:Label ID="lblActivityNote" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="trQualifiedIn4" visible="false">
                <td>
                    &nbsp;
                </td>
                <td class="page-label">
                    Opportunity Due:</td>
                <td colspan="3">
                    <asp:Label ID="lblOpportunityDue2" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="trQualifiedIn1" visible="false">
                <td>
                    &nbsp;
                </td>
                <td class="page-label">
                    Incumbent Broker:</td>
                <td colspan="3">
                    <asp:Label ID="lblIncumbentBroker" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="trQualifiedIn2" visible="false">
                <td>
                    &nbsp;
                </td>
                <td class="page-label">
                    Incumbent Insurer:</td>
                <td colspan="3">
                    <asp:Label ID="lblIncumbentInsurer" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="trQualifiedIn3" visible="false">
                <td>
                    &nbsp;
                </td>
                <td class="page-label">
                    Estimated Income:</td>
                <td colspan="3">
                    <asp:Label ID="lblClassification" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="trSubmitted1" visible="false">
                <td>
                    &nbsp;
                </td>
                <td class="page-label">
                    Net Brokerage Quoted:</td>
                <td colspan="3">
                    <asp:Label ID="lblNetBrokerageQuoted" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="trAccepted1" visible="false">
                <td>
                    &nbsp;
                </td>
                <td class="page-label">
                    Net Brokerage Actual:</td>
                <td colspan="3">
                    <asp:Label ID="lblNetBrokerageActual" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="trLost1" visible="false">
                <td>
                    &nbsp;
                </td>
                <td class="page-label">
                    Date Completed:</td>
                <td colspan="3">
                    <asp:Label ID="lblDateCompleted" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="trProcessed1" visible="false">
                <td>
                    &nbsp;
                </td>
                <td class="page-label">
                    Memo Number:</td>
                <td colspan="3">
                    <asp:Label ID="lblMemoNumber" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="trProcessed2" visible="false">
                <td>
                    &nbsp;
                </td>
                <td class="page-label">
                    Client Code:</td>
                <td colspan="3">
                    <asp:Label ID="lblClientCode" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="underline" colspan="5" height="8px">
                    </td>
            </tr>
            </table>
            <table class="page-table" style="width: 100%;">
            <tr>
                <td>
                    <asp:Button ID="btnFindClient" runat="server" Text="Find Client" 
                    CssClass="button-link" PostBackUrl="~/FindClient.aspx" />
                    <asp:Button ID="btnFollowups" runat="server" PostBackUrl="~/FollowUps.aspx" 
                        Text="Home" CssClass="button-link" />
                    <asp:Button ID="btnDashboard" runat="server" Text="Dashboard" 
                    PostBackUrl="~/Dashboard.aspx" CssClass="button-link" Visible="false" />
                    <asp:Button ID="btnClient" runat="server" Text="Client" OnClick="btnClient_Click"
                        CssClass="button-link" />
                    <asp:Button ID="btnOpportunity" runat="server" CssClass="button-link" 
                        onclick="btnOpportunity_Click" Text="Opportunity" />
                </td>
                <td class="right-aligned" colspan="1">
                    <asp:Button ID="btnSetReminder" runat="server" CssClass="button-action"
                     Text="Set Reminder" onclick="btnSetReminder_Click" Visible="true" />
                    <asp:Button ID="btnSendEmail" runat="server" CssClass="button-action" 
                        onclick="btnSendEmail_Click" Text="Send Email" />
                    <asp:Button ID="btnInactivate" runat="server" CssClass="button-action" 
                        onclick="btnInactivate_Click" Text="Inactivate" />
                    <asp:Button ID="btnAddActivity" runat="server" CssClass="button-action" 
                        onclick="btnAddActivity_Click" Text="Next Activity" />
                    <asp:Button ID="btnEditActivity" runat="server" CssClass="button-action" 
                        onclick="btnEditActivity_Click" Text="Edit" />
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
