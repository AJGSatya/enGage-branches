<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewOpportunityOld.aspx.cs" Inherits="enGage.Web.ViewOpportunity" %>
<%@ Register src="Controls/ucMessanger.ascx" tagname="ucMessanger" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page">
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
                                <asp:Label ID="lblOfficePhoneLabel" runat="server" CssClass="page-label" 
                                    Text="Phone: "></asp:Label>
                                <asp:Label ID="lblOfficePhone" runat="server" CssClass="page-title"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1" width="18px" class="underline">
                                &nbsp;</td>
                            <td colspan="2" class="underline">
                                <asp:Label ID="lblAddress" runat="server"></asp:Label>
                            </td>
                            <td class="right-aligned-underlined" colspan="2">
                                <asp:Label ID="lblAssociationLabel" runat="server" CssClass="page-label" 
                                    Text="Association: "></asp:Label>
                                <asp:Label ID="lblAssociation" runat="server" CssClass="page-text"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1" class="page-label">
                                &nbsp;</td>
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
                    Opportunity details</td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="imgBusinessType" runat="server" />
                </td>
                <td colspan="2">
                    <asp:Image ID="imgFlagged" runat="server" 
                        ImageUrl="~/images/OpportunityFlagged.gif" />
                    <asp:Label ID="lblOpportunityName" runat="server" CssClass="page-title" 
                        Height="20px"></asp:Label>
                </td>
                <td class="page-label">
                    Opportunity Due:</td>
                <td class="right-aligned">
                    <asp:Label ID="lblOpportunityDue" runat="server" CssClass="page-title"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="1" width="18px">
                    &nbsp;</td>
                <td class="page-label" width="120px">
                    Client Contact:</td>
                <td class="page-label" width="260px">
                    <asp:Label ID="lblContact" runat="server" CssClass="page-text"></asp:Label>
                </td>
                <td class="page-label" width="120px">
                    Status:</td>
                <td class="right-aligned">
                    <asp:Label ID="lblStatus" runat="server" CssClass="page-title"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    &nbsp;</td>
                <td class="page-label">
                    Incumbent Broker:</td>
                <td class="page-label">
                    <asp:Label ID="lblIncumbentBroker" runat="server" CssClass="page-text"></asp:Label>
                </td>
                <td class="page-label">
                    <asp:Label ID="lblClassificationLabel" runat="server" Text="CSS Segment:"></asp:Label>
                </td>
                <td class="right-aligned">
                    <asp:Label ID="lblClassification" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    &nbsp;</td>
                <td class="page-label">
                    Incumbent Underwriter:</td>
                <td class="page-label">
                    <asp:Label ID="lblIncumbentUnderwriter" runat="server" CssClass="page-text"></asp:Label>
                </td>
                <td class="page-label">
                    Expected OAMPS income Quoted:</td>
                <td class="right-aligned">
                    <asp:Label ID="lblNetBrokerageQuoted" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="1" class="underline">
                    &nbsp;</td>
                <td class="underline">
                    <asp:Label ID="lblMemoNumberLabel" runat="server" CssClass="page-label" 
                        Text="Memo Number:"></asp:Label>
                </td>
                <td class="underline">
                    <asp:Label ID="lblMemoNumber" runat="server" CssClass="page-text"></asp:Label>
                </td>
                <td class="underline">
                    <asp:Label ID="lblNetBrokerageActualLabel" runat="server" CssClass="page-label" 
                        Text="Expected OAMPS income Actual:"></asp:Label>
                </td>
                <td class="right-aligned-underlined">
                    <asp:Label ID="lblNetBrokerageActual" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    &nbsp;</td>
                <td colspan="2" class="page-label">
                    Next Activity:
                    <asp:Label ID="lblNextActivity" runat="server" CssClass="page-text-bold"></asp:Label>
                </td>
                <td colspan="2" class="right-aligned">
                    <asp:Label ID="lblFollowUpCompletedLabel" runat="server" CssClass="page-label" 
                        Text="Follow-up: "></asp:Label>
                    <asp:Label ID="lblFollowUpCompleted" runat="server" CssClass="page-text-bold"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="5" class="page-group">
                    Opportunity activities (<asp:Label ID="lblActive" runat="server"></asp:Label> active /
                    <asp:Label ID="lblInactive" runat="server"></asp:Label> inactive)</td>
            </tr>
            <tr>
                <td style="padding: 0px; margin: 0px" colspan="5">
                    <asp:GridView ID="grvActivities" runat="server" CssClass="grid" class="grid"
                        AutoGenerateColumns="False" ShowHeader="False" Width="100%" 
                        GridLines="Horizontal" onrowdatabound="grvActivities_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Arrow">
                                <ItemTemplate>
                                    <asp:Image ID="imgArror" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                </ItemTemplate>
                                <ItemStyle Width="18px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ActivityID" HeaderText="ActivityID" 
                                SortExpression="ActivityID" ReadOnly="True" Visible="False" />
                            <asp:BoundField DataField="StatusDescription" HeaderText="StatusDescription" 
                                SortExpression="StatusDescription" ReadOnly="True" >
                            <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Added" HeaderText="Added" SortExpression="Added" 
                                DataFormatString="{0:dd-MMM-yy}" ReadOnly="True" >
                            <ItemStyle Width="120px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActivityNote" HeaderText="ActivityNote" 
                                SortExpression="ActivityNote" ReadOnly="True" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="5" 
                    class="grid-footer-underlined">
                <asp:Image ID="imgAll" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                <asp:LinkButton ID="lnkSeeAll" runat="server" CommandArgument="q" 
                    Height="18px" onclick="lnkSeeAllSuspects_Click">see all</asp:LinkButton>
                </td>
            </tr>
        </table>
        <table class="page-table" style="width: 100%;">
            <tr>
                <td>
                    <asp:Button ID="btnFindClient" runat="server" Text="Find Client" 
                    CssClass="button-link" PostBackUrl="~/FindClient.aspx" />
                    <asp:Button ID="btnFollowups" runat="server" PostBackUrl="~/FollowUps.aspx" 
                        Text="On-the-go" CssClass="button-link" />
                    <asp:Button ID="btnDashboard" runat="server" Text="Dashboard" 
                    PostBackUrl="~/Dashboard.aspx" CssClass="button-link" Visible="false" />
                    <asp:Button ID="btnBack" runat="server" Text="Client" OnClick="btnBack_Click"
                        CssClass="button-link" />
                </td>
                <td class="right-aligned" colspan="1">
                    <asp:Button ID="btnFlagUnflag" runat="server" CssClass="button-action" 
                        onclick="btnFlagUnflag_Click" />
                    <asp:Button ID="btnAddActivity" runat="server" CssClass="button-action" 
                        onclick="btnAddActivity_Click" Text="Next Activity" />
                    <asp:Button ID="btnEditOpportunity" runat="server" CssClass="button-action" 
                        onclick="btnEditOpportunity_Click" Text="Edit" />
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
