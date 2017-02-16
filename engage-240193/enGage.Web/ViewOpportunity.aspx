<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewOpportunity.aspx.cs" Inherits="enGage.Web.EnhancedOpportunity" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="Controls/ucMessanger.ascx" tagname="ucMessanger" tagprefix="uc1" %>
<%@ Import Namespace="enGage.Web.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <style type="text/css">
        .accordionHeader
        {
            color: white;
            /*background-color: #719DDB;*/
            font: bold 11px auto "Trebuchet MS", Verdana;
            font-size: 12px;
            cursor: pointer;
            /*padding: 4px;*/
            /*margin-top: 3px;*/
        }
        .accordionContent
        {
           /* background-color: #DCE4F9;
            font: normal 10px auto Verdana, Arial;*/
            border: 1px gray;                
            padding: 4px;
            padding-top: 7px;
            overflow:hidden;
        }
</style>
 <script type="text/javascript">
     var collapsiblePanelExtendersArray = new Array();
        function attachAccordionClickEvent()
        {
            var accCtrl = $find('<%=AccordionCtrl.ClientID%>_AccordionExtender');
            accCtrl.add_selectedIndexChanged(onAccordionPaneChanged);
        }

        function setupCollapsiblePanelExtenderHandlers() {

            collapsiblePanelExtendersArray[0] = $find('<%=CollapsiblePanelExtender0.ClientID %>');
            collapsiblePanelExtendersArray[1] = $find('<%=CollapsiblePanelExtender1.ClientID %>');
            collapsiblePanelExtendersArray[2] = $find('<%=CollapsiblePanelExtender2.ClientID %>');

            for (i = 0; i < collapsiblePanelExtendersArray.length; i++) {
                attachCollapsiblePanelExtenderHandlers(collapsiblePanelExtendersArray[i]);
            }
        }

        function attachCollapsiblePanelExtenderHandlers(CollapsiblePanelExtender) {

            CollapsiblePanelExtender.add_expanding(dummyPanleExpandHandler);
            CollapsiblePanelExtender.add_collapsing(dummyPanlecollapseHandler);
        }
 
                
        function onAccordionPaneChanged(sender, eventArgs)
        {
            var selPane = sender.get_SelectedIndex() + 1;

            //set other panels as collapsed
            for (i = 0; i < collapsiblePanelExtendersArray.length; i++) {
                collapsiblePanelExtendersArray[i].set_Collapsed(true);

            }
           
        }

        function dummyPanleExpandHandler(sender, args) {
            var accCtrl = $find('<%=AccordionCtrl.ClientID%>_AccordionExtender');
            if (sender._id.indexOf("CollapsiblePanelExtender" + accCtrl.get_SelectedIndex()) >= 0
            && sender._collapsed
            )
                args.set_cancel(true);//="true";
        }

        function dummyPanlecollapseHandler(sender, args) {
            var accCtrl = $find('<%=AccordionCtrl.ClientID%>_AccordionExtender');
            if (sender._id.indexOf("CollapsiblePanelExtender" + accCtrl.get_SelectedIndex()) >= 0
            && !sender._collapsed
            )
                args.set_cancel(true); //="true";
        }

        $(document).ready(function() {
            // attachAccordionClickEvent();
            //$("tr[hideclientside]").css("display", "none");
        //roundPanels("divQualifying");
        //roundPanels("divResponding");
        //roundPanels("divCompleting");
        
        });


        function roundPanels(paneldivbaseName)
        {
            $("#"+paneldivbaseName).corner("round top 8px").parent().css('padding', '8px').corner("round top 14px").css('padding-top', '20px').css('padding-bottom', '1px');
            $("#"+paneldivbaseName+"Bottom").corner("round bottom 8px").parent().css('padding', '8px').corner("round bottom 14px").css('padding-top', '0px');
            $("#"+paneldivbaseName+"Bottom").closest(".accordionContent").css("padding", '0px').css("padding-top", '5px');
        }
        function pageLoad() {
            attachAccordionClickEvent();
            setupCollapsiblePanelExtenderHandlers();
        }
    
    </script>

    <div class="page">
     <asp:ScriptManager ID="scriptManagerId" runat="server">
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
                    Renewal Date:</td>
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
                    Broking Income Quoted:</td>
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
                        Text="Actual Broking Income:"></asp:Label>
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
                    Opportunity new activities (<asp:Label ID="Label1" runat="server"></asp:Label> active /
                    <asp:Label ID="Label2" runat="server"></asp:Label> inactive)</td>
            </tr>
            <tr>
            <td colspan="5" >
             <cc1:Accordion ID="AccordionCtrl" runat="server" 
        SelectedIndex="0" HeaderCssClass="accordionHeader"
        ContentCssClass="accordionContent" AutoSize="None" 
        FadeTransitions="true" RequireOpenedPane="true">
            <Panes>
           
                <cc1:AccordionPane ID="AccordionPane0" runat="server">
                    <Header>

                     <asp:Panel ID="PnlClick" runat="server" Height="20px" CssClass="panel-header" >
                                <div style="float: left; vertical-align: middle; height: 20px">
                                    <asp:Image ID="Imgclick" runat="server" ImageUrl="~/Images/expand.png" name="imgPanelHeader" /></div>
                                <div style="float: left; font-weight: bold ;vertical-align: middle; margin-left:5px">
                                        Qualifying
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="dummyPanel" runat="server"></asp:Panel>

                            
                             <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender0" runat="server" CollapseControlID="PnlClick"
                                Collapsed="false" CollapsedText="Industry" ExpandControlID="PnlClick" TextLabelID="lblMessage"
                                ExpandedText="Industry" ImageControlID="Imgclick" CollapsedImage="~/Images/expand.png"
                                ExpandedImage="~/Images/collapse.png" ExpandDirection="Vertical" TargetControlID="dummyPanel"
                                ScrollContents="false">
                            </cc1:CollapsiblePanelExtender>
                
                    </Header>
                    <Content>
                    
                   
                        <table style="height:100%">
                          <tr>
                <td width="18px">
                    &nbsp;
                </td>
                <td width="200px">
                    Status:<span style="color: #FF0000">*</span>
                    <asp:DropDownList ID="ddlQualifyOpportunityStatus" runat="server" OnSelectedIndexChanged="ddlOpportunityStatus_SelectedIndexChanged"
                        AutoPostBack="True" CssClass="control">
                    </asp:DropDownList>
                </td>
                <td colspan="1" width="380px">
                    Contact:
                    <asp:DropDownList ID="ddlQualifyContact" runat="server" CssClass="control">
                    </asp:DropDownList>
                </td>
                <td width="200px">
                    Follow-up:
                    <asp:Literal ID="imgQualifyManadatory" runat="server"><span style="color: #FF0000">*</span></asp:Literal>
                    <asp:TextBox ID="txtQualifyFollowUpDate" runat="server" Width="72px" 
                        CssClass="control" ToolTip="dd/mm/yyyy"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtFollowUpDate_CalendarExtender" runat="server" Enabled="True"
                        Format="dd/MM/yyyy" PopupButtonID="btnFollowUpDate" TargetControlID="txtQualifyFollowUpDate">
                    </cc1:CalendarExtender>
                    <asp:ImageButton ID="btnFollowUpDate" runat="server" ImageUrl="~/images/Calendar.gif"
                        TabIndex="-1" />
                </td>
            </tr>
                        <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="3" valign="top">
                    <span style="height:100%; vertical-align:top">Activity:</span>
                    <asp:TextBox ID="txtQualifyActivityNote" runat="server" CssClass="control" Height="72px"
                        TextMode="MultiLine" Width="680px"></asp:TextBox>
                </td>
            </tr>
                <tr>
                <td class="page-group" colspan="4">
                    Additional
                </td>
            </tr>
              <tr id="trQualifyLost1" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Date Completed:<span style="color: #FF0000">*</span>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtQualifyDateCompleted" runat="server" MaxLength="10" Width="72px" 
                        CssClass="control" ToolTip="dd/mm/yyyy"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender5" runat="server" Enabled="True"
                        Format="dd/MM/yyyy" PopupButtonID="btnQualifyDateCompleted" TargetControlID="txtQualifyDateCompleted">
                    </cc1:CalendarExtender>
                    <asp:ImageButton ID="btnQualifyDateCompleted" runat="server" ImageUrl="~/images/Calendar.gif"
                        TabIndex="-1" />
                </td>
            </tr>
            <tr id="trQualifyEstimated" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Estimated Broking Income ($):<span style="color: #FF0000">*</span>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtQualifyEstimatedBrokingIncome" runat="server" MaxLength="10" CssClass="control"></asp:TextBox>
                     <asp:CheckBox ID="chkQualifiyingPersonalLineOnly" runat="server" ToolTip="Override CSS Segment to 'Personal Lines Only'" TextAlign="Right" Text="Personal Lines Only" />
                    <cc1:FilteredTextBoxExtender ID="txtEstimatedBrokingIncome_FilteredTextBoxExtender" runat="server"
                        Enabled="True" TargetControlID="txtQualifyEstimatedBrokingIncome" ValidChars=",.0123456789">
                    </cc1:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td colspan="4" class="right-aligned">
                    <asp:Button ID="btnQualifyCancel" runat="server" Text="Cancel" OnClientClick="return confirm('Do you really want to cancel Adding/Editing this activity?');"
                        OnClick="btnBack_Click" CssClass="button-action" />
                    <asp:Button ID="btnQualifySave" runat="server" Text="Save" OnClick="btnSave_Click" 
                        CssClass="button-action" />
                        <asp:Button ID="btnQualifySaveAndReminder" runat="server" Text="Save & Set Reminder" OnClick="btnSaveAndReminder_Click" 
                        CssClass="button-action" />
                </td>
            </tr>
                        
                        </table>
                        
                 
                    </Content>
                    
                </cc1:AccordionPane>
                <cc1:AccordionPane ID="AccordionPane1" runat="server">
                    <Header> 
                           
                    
                    <asp:Panel ID="PnlClick1" runat="server" Height="20px" CssClass="panel-header" >
                                <div style="float: left; vertical-align: middle; height: 20px">
                                    <asp:Image ID="Imgclick1" runat="server" ImageUrl="~/Images/expand.png" /></div>
                                <div style="float: left; font-weight: bold ;vertical-align: middle; margin-left:5px">
                                        Responding
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="dummyPanel1" runat="server"></asp:Panel>
                            <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" CollapseControlID="PnlClick1"
                                Collapsed="true" CollapsedText="" ExpandControlID="PnlClick1" TextLabelID="lblMessage"
                                ExpandedText="" ImageControlID="Imgclick1" CollapsedImage="~/Images/expand.png"
                                ExpandedImage="~/Images/collapse.png" ExpandDirection="Vertical" TargetControlID="dummyPanel1"
                                ScrollContents="false">
                            </cc1:CollapsiblePanelExtender>
                        
                            </Header>
                    <Content>
                  
                                               <table style="height:100%">
                          <tr>
                <td width="18px">
                    &nbsp;
                </td>
                <td width="200px">
                    Status:<span style="color: #FF0000">*</span>
                    <asp:DropDownList ID="ddlRespondStatus" runat="server" OnSelectedIndexChanged="ddlOpportunityStatus_SelectedIndexChanged"
                        AutoPostBack="True" CssClass="control">
                    </asp:DropDownList>
                </td>
                <td colspan="1" width="380px">
                    Contact:
                    <asp:DropDownList ID="ddlRespondContacts" runat="server" CssClass="control">
                    </asp:DropDownList>
                </td>
                <td width="200px">
                    Follow-up:<asp:Literal ID="imgRespondMandatroy" runat="server"><span style="color: #FF0000">*</span></asp:Literal>
                    <asp:TextBox ID="txtRespondFollowUpDate" runat="server" Width="72px" 
                        CssClass="control" ToolTip="dd/mm/yyyy"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                        Format="dd/MM/yyyy" PopupButtonID="btnRespondFollowUpDate" TargetControlID="txtRespondFollowUpDate">
                    </cc1:CalendarExtender>
                    <asp:ImageButton ID="btnRespondFollowUpDate" runat="server" ImageUrl="~/images/Calendar.gif"
                        TabIndex="-1" />
                </td>
            </tr>
                        <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="3" valign="top">
                    <span style="height:100%; vertical-align:top">Activity:</span>
                    <asp:TextBox ID="txtRespondActivityNote" runat="server" CssClass="control" Height="72px"
                        TextMode="MultiLine" Width="680px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="page-group" colspan="4">
                    Additional
                </td>
            </tr>
            <tr id="trQualifiedIn4" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Client Renewal Date:<span style="color: #FF0000">*</span>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtRespondOpportunityDue" runat="server" Width="72px" MaxLength="10" 
                        CssClass="control" ToolTip="dd/mm/yyyy"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtRespondOpportunityDue_CalendarExtender" runat="server" PopupButtonID="btnRespondOpportunityDue"
                        TargetControlID="txtRespondOpportunityDue" Format="dd/MM/yyyy">
                    </cc1:CalendarExtender>
                    <asp:ImageButton ID="btnRespondOpportunityDue" runat="server" ImageUrl="~/images/Calendar.gif"
                        TabIndex="-1" />
                </td>
            </tr>
            <tr id="trQualifiedIn1" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Incumbent Broker:
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtIncumbentBroker" runat="server" Width="300px" 
                        CssClass="control" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr id="trQualifiedIn2" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Incumbent Insurer:
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtIncumbentInsurer" runat="server" Width="300px" 
                        CssClass="control" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr id="trQualifiedIn3" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    CSS Segment:<span style="color: #FF0000"></span>
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddlRespondClassification" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlClassification_SelectedIndexChanged"
                        CssClass="control">
                    </asp:DropDownList>
                    <asp:CheckBox ID="chkRespondPersonalLinesOverride" runat="server" ToolTip="Override CSS Segment to 'Personal Lines Only'" TextAlign="Right" Text="Personal Lines Only" />
                </td>
            </tr>
             <tr id="trInsuredAs" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Insured As :<span style="color: #FF0000">*</span>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtExtraInsuredAS" runat="server" MaxLength="100" CssClass="control"></asp:TextBox>
                   
                  
                </td>
            </tr>
             <tr id="trRespondExtraEstimated" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Estimated Broking Income ($):<span style="color: #FF0000">*</span>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtRespondExtraEstimatedBrokingIncome" runat="server" MaxLength="10" CssClass="control"></asp:TextBox>
                     <asp:CheckBox ID="chkRespondExtraEstimatedPersonalLinesOverride" runat="server" ToolTip="Override CSS Segment to 'Personal Lines Only'" TextAlign="Right" Text="Personal Lines Only" />
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                        Enabled="True" TargetControlID="txtRespondExtraEstimatedBrokingIncome" ValidChars=",.0123456789">
                    </cc1:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr id="trSubmitted1" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Broking Income Quoted ($):<span style="color: #FF0000">*</span>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtRespondNetBrokerageQuoted" runat="server" MaxLength="10" CssClass="control"></asp:TextBox>
                     <asp:CheckBox ID="chkRespondQutedPersonalLinesOverride" runat="server" ToolTip="Override CSS Segment to 'Personal Lines Only'" TextAlign="Right" Text="Personal Lines Only" />
                    <cc1:FilteredTextBoxExtender ID="txtNetBrokerageQuoted_FilteredTextBoxExtender" runat="server"
                        Enabled="True" TargetControlID="txtRespondNetBrokerageQuoted" ValidChars=",.0123456789">
                    </cc1:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr id="trAccepted1" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Actual Broking Income ($):<span style="color: #FF0000">*</span>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtRespondNetBrokerageActual" runat="server" MaxLength="10" CssClass="control"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="txtNetBrokerageActual_FilteredTextBoxExtender" runat="server"
                        Enabled="True" TargetControlID="txtRespondNetBrokerageActual" ValidChars=",.0123456789">
                    </cc1:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr id="trLost1" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Date Completed:<span style="color: #FF0000">*</span>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtRespondDateCompleted" runat="server" MaxLength="10" Width="72px" 
                        CssClass="control" ToolTip="dd/mm/yyyy"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtDateCompleted_CalendarExtender" runat="server" Enabled="True"
                        Format="dd/MM/yyyy" PopupButtonID="btnRespondDateCompleted" TargetControlID="txtRespondDateCompleted">
                    </cc1:CalendarExtender>
                    <asp:ImageButton ID="btnRespondDateCompleted" runat="server" ImageUrl="~/images/Calendar.gif"
                        TabIndex="-1" />
                </td>
            </tr>
            <tr id="trProcessed1" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Memo Number:
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtRespondMemoNumber" runat="server" MaxLength="50" Width="150px" CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr id="trProcessed2" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Client Code:
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtRespondClientCode" runat="server" MaxLength="50" Width="150px" CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" class="right-aligned">
                    <asp:Button ID="Button1" runat="server" Text="Cancel" OnClientClick="return confirm('Do you really want to cancel editing this activity?');"
                        OnClick="btnBack_Click" CssClass="button-action" />
                    <asp:Button ID="Button2" runat="server" Text="Save" OnClick="btnSave_Click" 
                        CssClass="button-action" />
                         <asp:Button ID="btnRespondSaveAndReminder" runat="server" Text="Save & Set Reminder" OnClick="btnSaveAndReminder_Click" 
                        CssClass="button-action" />
                </td>
            </tr>
                        
                        </table>
                       
                    </Content>
                </cc1:AccordionPane>
                <cc1:AccordionPane ID="AccordionPane2" runat="server">
                    <Header>
                   
                     <asp:Panel ID="PnlClick2" runat="server" Height="20px" CssClass="panel-header" >
                                <div style="float: left; vertical-align: middle; height: 20px">
                                    <asp:Image ID="Imgclick2" runat="server" ImageUrl="~/Images/expand.png" /></div>
                                <div style="float: left; font-weight: bold ;vertical-align: middle; margin-left:5px">
                                        Completing
                                </div>
                            </asp:Panel>
                                                        <asp:Panel ID="dummyPanel2" runat="server"></asp:Panel>
                            <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" CollapseControlID="PnlClick2"
                                Collapsed="true" CollapsedText="" ExpandControlID="PnlClick2" TextLabelID="lblMessage"
                                ExpandedText="" ImageControlID="Imgclick2" CollapsedImage="~/Images/expand.png"
                                ExpandedImage="~/Images/collapse.png" ExpandDirection="Vertical" TargetControlID="dummyPanel2"
                                ScrollContents="false">
                            </cc1:CollapsiblePanelExtender>
                 
                    </Header>
                    <Content>
                 
                        <table style="height:100%">
                          <tr>
                <td width="18px">
                    &nbsp;
                </td>
                <td width="200px">
                    Status:<span style="color: #FF0000">*</span>
                    <asp:DropDownList ID="ddlCompleteStatus" runat="server" OnSelectedIndexChanged="ddlOpportunityStatus_SelectedIndexChanged"
                        AutoPostBack="True" CssClass="control">
                    </asp:DropDownList>
                </td>
                <td colspan="1" width="380px">
                    Contact:
                    <asp:DropDownList ID="ddlCompleteContacts" runat="server" CssClass="control">
                    </asp:DropDownList>
                </td>
                <td width="200px">
                    Follow-up:<asp:Literal ID="imgCompleteMandatory" runat="server"><span style="color: #FF0000">*</span></asp:Literal>
                    <asp:TextBox ID="txtCompleteFollowUpDate" runat="server" Width="72px" 
                        CssClass="control" ToolTip="dd/mm/yyyy"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True"
                        Format="dd/MM/yyyy" PopupButtonID="btncompleteFollowUpDate" TargetControlID="txtCompleteFollowUpDate">
                    </cc1:CalendarExtender>
                    <asp:ImageButton ID="btncompleteFollowUpDate" runat="server" ImageUrl="~/images/Calendar.gif"
                        TabIndex="-1" />
                </td>
            </tr>
                        <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="3" valign="top">
                 <span style="height:100%; vertical-align:top">Activity:</span>
                    <asp:TextBox ID="txtCompleteActivityNote" runat="server" CssClass="control" Height="72px"
                        TextMode="MultiLine" Width="680px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="page-group" colspan="4">
                    Additional
                </td>
            </tr>
            <tr id="tr1" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Renewal Date:<span style="color: #FF0000">*</span>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtCompleteOpportunityDue" runat="server" Width="72px" MaxLength="10" 
                        CssClass="control" ToolTip="dd/mm/yyyy"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="btnCompleteOpportunityDue"
                        TargetControlID="txtCompleteOpportunityDue" Format="dd/MM/yyyy">
                    </cc1:CalendarExtender>
                    <asp:ImageButton ID="btnCompleteOpportunityDue" runat="server" ImageUrl="~/images/Calendar.gif"
                        TabIndex="-1" />
                </td>
            </tr>
            <tr id="tr2" visible="false" runat="server" >
                <td>
                    &nbsp;
                </td>
                <td>
                    Incumbent Broker:
                </td>
                <td colspan="2">
                    <asp:TextBox ID="TextBox6" runat="server" Width="300px" 
                        CssClass="control" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr id="tr3" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Incumbent Insurer:
                </td>
                <td colspan="2">
                    <asp:TextBox ID="TextBox7" runat="server" Width="300px" 
                        CssClass="control" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr id="tr4" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    CSS Segment:<span style="color: #FF0000">*</span>
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddlCompleteClassification" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlClassification_SelectedIndexChanged"
                        CssClass="control">
                    </asp:DropDownList>
                </td>
            </tr>
               <tr id="trCompleteInsuredAs" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Insured As :<span style="color: #FF0000">*</span>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtCompleteExtraInsuredAS" runat="server" MaxLength="100" CssClass="control"></asp:TextBox>
                   
                  
                </td>
            </tr>
             <tr id="trCompleteExtraEstimated" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Estimated Broking Income ($):<span style="color: #FF0000">*</span>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtCompleteExtraEstimatedBrokingIncome" runat="server" MaxLength="10" CssClass="control"></asp:TextBox>
                     <asp:CheckBox ID="chkCompleteExtraEstimatedPersonalLinesOverride" runat="server" ToolTip="Override CSS Segment to 'Personal Lines Only'" TextAlign="Right" Text="Personal Lines Only" />
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server"
                        Enabled="True" TargetControlID="txtCompleteExtraEstimatedBrokingIncome" ValidChars=",.0123456789">
                    </cc1:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr id="trCompleteExtraQuoted" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Broking Income Quoted ($):<span style="color: #FF0000">*</span>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtCompleteExtraNetBrokerageQuoted" runat="server" MaxLength="10" CssClass="control"></asp:TextBox>
                    <asp:CheckBox ID="chkExtraCompleteQutedPersonalLinesOverride" runat="server" ToolTip="Override CSS Segment to 'Personal Lines Only'" TextAlign="Right" Text="Personal Lines Only" />
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                        Enabled="True" TargetControlID="txtCompleteExtraNetBrokerageQuoted" ValidChars=",.0123456789">
                    </cc1:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr id="trCompleteAccepted1" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Actual Broking Income ($):<span style="color: #FF0000">*</span>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtCompleteNetBrokerageActual" runat="server" MaxLength="10" CssClass="control"></asp:TextBox>
                    <asp:CheckBox ID="chkCompletePersonalLinesOverride" runat="server" ToolTip="Override CSS Segment to 'Personal Lines Only'" TextAlign="Right" Text="Personal Lines Only" />
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                        Enabled="True" TargetControlID="txtCompleteNetBrokerageActual" ValidChars=",.0123456789">
                    </cc1:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr id="trCompleteLost1" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Date Completed:<span style="color: #FF0000">*</span>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtCompleteDateCompleted" runat="server" MaxLength="10" Width="72px" 
                        CssClass="control" ToolTip="dd/mm/yyyy"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True"
                        Format="dd/MM/yyyy" PopupButtonID="btnCompleteDateCompleted" TargetControlID="txtCompleteDateCompleted">
                    </cc1:CalendarExtender>
                    <asp:ImageButton ID="btnCompleteDateCompleted" runat="server" ImageUrl="~/images/Calendar.gif"
                        TabIndex="-1" />
                </td>
            </tr>
            <tr id="trCompleteProcessed1" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Memo Number:<span style="color: #FF0000">*</span>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtCompleteMemoNumber" runat="server" MaxLength="50" Width="150px" CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr id="trCompleteProcessed2" visible="false" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Client Code:<span style="color: #FF0000">*</span>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtCompleteClientCode" runat="server" MaxLength="50" Width="150px" CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" class="right-aligned">
                    <asp:Button ID="Button3" runat="server" Text="Cancel" OnClientClick="return confirm('Do you really want to cancel editing this activity?');"
                        OnClick="btnBack_Click" CssClass="button-action" />
                    <asp:Button ID="Button4" runat="server" Text="Save" OnClick="btnSave_Click" 
                        CssClass="button-action" />
                         <asp:Button ID="btnCompleteSaveAndReminder" runat="server" Text="Save & Set Reminder" OnClick="btnSaveAndReminder_Click" 
                        CssClass="button-action" />
                </td>
            </tr>
                        
                        </table>
                       
                    </Content>               
                </cc1:AccordionPane>
               
            </Panes>
        </cc1:Accordion>        

             
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
                        AutoGenerateColumns="False" ShowHeader="True" Width="100%" 
                        GridLines="Horizontal" onrowdatabound="grvActivities_RowDataBound" OnRowCommand="grvActivities_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:Image ID="imgArror" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                </ItemTemplate>
                                <ItemStyle Width="18px" />
                                 <HeaderStyle  BorderStyle="None" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ActivityID" HeaderText=""  HeaderStyle-BorderStyle="None"
                                SortExpression="ActivityID" ReadOnly="True" Visible="False" />
                            <asp:BoundField DataField="StatusDescription" HeaderText="Status"  HeaderStyle-BorderStyle="None"
                                SortExpression="StatusDescription" ReadOnly="True" >
                            <ItemStyle Width="100px" />
                            </asp:BoundField>
                             <asp:TemplateField >
                            <ItemTemplate  >
                            <asp:Label ID="Label2" runat="server" Text='<%# GridUtilities.GetActivityPhase(DataBinder.Eval(Container.DataItem, "Action").ToString()) %>'> </asp:Label>
                                
                            </ItemTemplate>
                            <HeaderTemplate>Phase</HeaderTemplate>
                            <HeaderStyle  BorderStyle="None" />
                            <ItemStyle Width="70px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Added" HeaderText="Added" SortExpression="Added" HeaderStyle-BorderStyle="None"
                                DataFormatString="{0:dd-MMM-yy hh:mm tt}" ReadOnly="True" >
                            <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActivityNote" HeaderText="Note" HeaderStyle-BorderStyle="None"
                                SortExpression="ActivityNote" ReadOnly="True" ItemStyle-Width="380"  />
                                
                                <asp:TemplateField >
                            <ItemTemplate  >
                            <asp:Label ID="Label3" runat="server" Text='<%# GridUtilities.GetAddedOrModifiedUser(DataBinder.Eval(Container.DataItem, "AddedBy"),DataBinder.Eval(Container.DataItem, "ModifiedBy")) %>'> </asp:Label>
                                
                            </ItemTemplate>
                            <HeaderTemplate>Add/Modified By</HeaderTemplate>
                            <HeaderStyle  BorderStyle="None" />
                            <ItemStyle Width="100px" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField >
                            <ItemTemplate>
                                 <asp:Button ID="btnSetReminder" runat="server" CssClass="button-action"
                     Text="Set Reminder" Visible="true"  Width="95"
                     CommandName="setreminder"
                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ActivityID") %>' 
                     />
                            </ItemTemplate>
                             <HeaderStyle  BorderStyle="None" />
                            </asp:TemplateField>
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
                        Text="Home" CssClass="button-link" />
                    <asp:Button ID="btnDashboard" runat="server" Text="Dashboard" 
                    PostBackUrl="~/Dashboard.aspx" CssClass="button-link" Visible="false" />
                     <asp:Button ID="btnClient" runat="server" Text="Client" OnClick="btnClient_Click"
                        CssClass="button-link" />
                </td>
                <td class="right-aligned" colspan="1">
                    <asp:Button ID="btnFlagUnflag" runat="server" CssClass="button-action" 
                        onclick="btnFlagUnflag_Click" Visible="false" />
                    <asp:Button ID="btnAddActivity" runat="server" CssClass="button-action" 
                        onclick="btnAddActivity_Click" Text="Next Activity" Visible="false" />
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
