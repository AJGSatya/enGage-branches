<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AddOpportunity.aspx.cs" Inherits="enGage.Web.AddOpportunity" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register src="Controls/ucMessanger.ascx" tagname="ucMessanger" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <script type="text/javascript" language="javascript">

       


        $(document).ready(function() {

        $("tr[hideclientside]").css("display", "none");
        $("[hideclientside]").css("display", "none");
        $("span[style]").closest("td").next("td").find("input[type=text]").addClass("validation-border");
        $("span[style]").closest("td").next("td").find("select").addClass("validation-border");
        // get all mandatory contorls and add the validation-err css  
            //$('#<%=btnSave.ClientID%>').click(function() { resetValidationStyle(); });
        });
    
        </script>
    <div class="page">
        <asp:ScriptManager runat="server" ID="scriptManagerId">                
            <Scripts>
                <asp:ScriptReference Path="../scripts/CallWebServiceMethods.js" />
            </Scripts>
        </asp:ScriptManager>
        <table class="page-table" style="width: 100%;" runat="server">
            <tr>
                <td class="page-group" colspan="6">
                    Client summary</td>
            </tr>
            <tr>
                <td style="padding: 0px; margin: 0px" colspan="6">
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
                <td class="page-group" colspan="6">
                    Opportunity details</td>
            </tr>
            <tr>
                <td width="18px" colspan="2">
                    &nbsp;</td>
                <td width="180px">
                    Opportunity<span style="color: #FF0000">*</span>:</td>
                <td width="340px">
                    <asp:TextBox ID="txtOpportunityName" runat="server" MaxLength="250" 
                        Width="300px" CssClass="control"></asp:TextBox>
                </td>
                <td width="130px">
                    Renewal Date:</td>
                <td width="120px">
                    <asp:TextBox ID="txtOpportunityDue" runat="server" Width="72px" MaxLength="10" 
                        CssClass="control"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtOpportunityDue_CalendarExtender" runat="server" 
                        Enabled="True" PopupButtonID="btnOpportunityDue" 
                        TargetControlID="txtOpportunityDue" Format="dd/MM/yyyy">
                    </cc1:CalendarExtender>
                    <asp:ImageButton ID="btnOpportunityDue" runat="server" 
                        ImageUrl="~/images/Calendar.gif" TabIndex="-1" CssClass="control" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;</td>
                <td>
                    Opportunity Type:</td>
                <td class="cell-value">
                    <asp:DropDownList ID="ddlBusinessType" runat="server" CssClass="control" 
                        AutoPostBack="True" 
                        onselectedindexchanged="ddlBusinessType_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="cell-value">
                    Follow-up:</td>
                <td class="cell-value">
                    <asp:TextBox ID="txtFollowUpDate" runat="server" Width="72px" MaxLength="10" 
                        CssClass="control"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtFollowupDate_CalendarExtender" runat="server" 
                        Enabled="True" Format="dd/MM/yyyy" PopupButtonID="btnFollowupDate" 
                        TargetControlID="txtFollowupDate">
                    </cc1:CalendarExtender>
                    <asp:ImageButton ID="btnFollowupDate" runat="server" 
                        ImageUrl="~/images/Calendar.gif" TabIndex="-1" CssClass="control" />
                </td>
            </tr>
            <tr hideclientside>
                <td  class="underline" colspan="2">
                    &nbsp;</td>
                <td  class="underline">
                    Flag Opportunity:</td>
                <td  class="underline">
                    <asp:DropDownList ID="ddlFlagged" runat="server" CssClass="control">
                        <asp:ListItem Value="False">Not Flagged</asp:ListItem>
                        <asp:ListItem Value="True">Flagged</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="underline">
                    Stage of Sales / Aspire Cycle:</td>
                <td class="underline">
                    <asp:Label ID="lblOpportunityStatus" runat="server" CssClass="page-title"></asp:Label>
                </td>
            </tr>
            <tr >
                <td colspan="2">
                    &nbsp;</td>
                <td>
                    Contact:</td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlContact" runat="server" CssClass="control">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr hideclientside >
                <td colspan="2">
                    &nbsp;</td>
                <td>
                    CSS Segment<span style="color: #FF0000">*</span>:</td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlClassification" runat="server" CssClass="control" 
                        AutoPostBack="True" 
                        onselectedindexchanged="ddlClassification_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr hideclientside >
                <td colspan="2">
                    &nbsp;</td>
                <td>
                    Incumbent Broker:                 
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtIncumbentBroker" runat="server" Width="300px" 
                        CssClass="control" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr hideclientside > 
                <td class="underline" colspan="2">
                    &nbsp;</td>
                <td class="underline">
                    Incumbent Insurer:</td>
                <td colspan="3" class="underline">
                    <asp:TextBox ID="txtIncumbentInsurer" runat="server" Width="300px" 
                        CssClass="control" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr hideclientside id="trQuickWin1">
                <td colspan="2">
                    &nbsp;</td>
                <td>
                    Broking Income Quoted ($)<span style="color: #FF0000">*</span>:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtNetBrokerageQuoted" runat="server" MaxLength="10" CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr hideclientside id="trQuickWin2">
                <td class="underline" colspan="2">
                    &nbsp;</td>
                <td class="underline">
                    Actual Broking Income ($)<span style="color: #FF0000">*</span>:
                </td>
                <td colspan="3" class="underline">
                    <asp:TextBox ID="txtNetBrokerageActual" runat="server" MaxLength="10" CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr hideclientside id="trActivityNote">
                <td class="underline" colspan="2">
                    &nbsp;</td>
                <td class="underline">
                    Activity Note:
                </td>
                <td colspan="3" class="underline">
                    <asp:TextBox ID="txtActivityNote" runat="server" CssClass="control" Height="72px"
                        TextMode="MultiLine" Width="460px"></asp:TextBox>
                </td>
            </tr>
            <tr hideclientside>
                <td colspan="2">
                    &nbsp;</td>
                <td>
                    Next Activity:</td>
                <td colspan="3">
                    <asp:Label ID="lblNextActivity" runat="server" CssClass="page-text-bold"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="6" class="right-aligned">
                    <asp:Button ID="btnCancel" runat="server" CssClass="button-action" 
                        Text="Cancel" OnClick="btnCancel_Click"
                        
                        
                        
                        OnClientClick="return confirm('Do you really want to cancel adding this opportunity?');" />
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" 
                        CssClass="button-action" />
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <uc1:ucMessanger ID="ucMessanger1" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
