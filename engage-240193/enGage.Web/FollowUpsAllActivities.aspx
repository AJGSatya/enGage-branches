<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="FollowUps.aspx.cs" Inherits="enGage.Web.FollowUpsAllActivities" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
<%@ Import Namespace="enGage.Web.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Magnific Popup -->
    <link rel="stylesheet" href="css/JqueryPopup/magnific-popup.css" />

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(startRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);
        function startRequest(sender, e) {
            document.getElementById('<%=ddlRegion.ClientID%>').disabled = true;
            document.getElementById('<%=ddlBranch.ClientID%>').disabled = true;
            document.getElementById('<%=ddlExecutive.ClientID%>').disabled = true;
            document.getElementById('<%=imgLoader.ClientID%>').src = 'images/ajax-loader.gif';
        }
        function endRequest(sender, e) {
            document.getElementById('<%=ddlRegion.ClientID%>').disabled = false;
            document.getElementById('<%=ddlBranch.ClientID%>').disabled = false;
            document.getElementById('<%=ddlExecutive.ClientID%>').disabled = false;
            document.getElementById('<%=imgLoader.ClientID%>').src = 'images/empty.gif';
        }
    </script>

    <script>



        function setupGridSorting(gridClientId, sortingColClientId) {

            // attachAccordionClickEvent();
            $(gridClientId).addClass("tablesorter");
            $("th , " + gridClientId).removeClass("header");
            $(gridClientId).tablesorter({
                sortList: [[7, 1]],
                dateFormat: "dd-mmm-yy",
                headers: {
                    // assign the secound column (we start counting zero) 
                    0: {
                        // disable it by setting the property sorter to false 
                        sorter: false
                    },
                    // assign the third column (we start counting zero) 
                    2: {
                        // disable it by setting the property sorter to false 
                        sorter: false
                    },
                    3: {
                        // disable it by setting the property sorter to false 
                        sorter: false
                    },
                    7: {
                        sorter: 'SpecialShortDate'
                    },
                    8: {
                        sorter: 'SpecialShortDate'
                    }

                }

            });

            $(gridClientId).bind("sortEnd", function(e) {
                $(sortingColClientId)[0].value = JSON.stringify(e.target.config.sortList);
            });

            $(".header", $(gridClientId)).removeClass("header").addClass("summarygrid-customheader");
            $(gridClientId).removeClass("summarygrid-customheader");
            // $("th , #<%=grvSummary.ClientID%>").addClass("header");


        }

        function setupActivitiesGridSorting(gridClientId, sortingColClientId) {

            // attachAccordionClickEvent();
            $(gridClientId).addClass("tablesorter");
            $("th , " + gridClientId).removeClass("header");
            $(gridClientId).tablesorter({
                sortList: [[7, 1]],
                dateFormat: "dd-mmm-yy",
                headers: {
                    // assign the secound column (we start counting zero) 
                    0: {
                        // disable it by setting the property sorter to false 
                        sorter: false
                    },
                    // assign the third column (we start counting zero) 
                    2: {
                        // disable it by setting the property sorter to false 
                        sorter: false
                    },
                    3: {
                        // disable it by setting the property sorter to false 
                        sorter: false
                    },
                    6: {
                        // disable it by setting the property sorter to false 
                        sorter: 'SpecialCurrency'
                    },
                    7: {
                        sorter: 'SpecialShortDate'
                    },
                    8: {
                        sorter: 'SpecialShortDate'
                    }

                }

            });

            $(gridClientId).bind("sortEnd", function(e) {
                $(sortingColClientId)[0].value = JSON.stringify(e.target.config.sortList);
            });

            $(".header", $(gridClientId)).removeClass("header").addClass("summarygrid-customheader");
            $(gridClientId).removeClass("summarygrid-customheader");
            // $("th , #<%=grvSummary.ClientID%>").addClass("header");


        }

        $(document).ready(function() {
            $("#<%=chartSummary.ClientID%>").fadeTo(0, 0);
            setupGridSorting("#<%=grvSummary.ClientID%>", "#<%=summarySortCol.ClientID%>");
            setupGridSorting("#<%=grvSharedSummary.ClientID%>", "#<%=sharedSummarySortCol.ClientID%>");
            setupActivitiesGridSorting("#<%=grvQualifiedIn.ClientID%>", "#<%=QualifySortCol.ClientID%>");
            setupActivitiesGridSorting("#<%=grvInterested.ClientID%>", "#<%=RespondSortCol.ClientID%>");
            setupActivitiesGridSorting("#<%=grvAccepted.ClientID%>", "#<%=CompleteSortCol.ClientID%>");






            $("#divSummary").corner("round 8px").parent().css('padding', '8px').corner("round 14px").css('padding-top', '20px');
            $("#divChart").corner("round 8px").parent().css('padding', '8px').corner("round 14px").css('padding-top', '20px');
            $("#divSharedSummary").corner("round 8px").parent().css('padding', '8px').corner("round 14px").css('padding-top', '20px');

            $("#divActivities").corner("round 8px").parent().css('padding', '8px').corner("round 14px").css('padding-top', '20px');

            setTimeout(function() { $("#<%=chartSummary.ClientID%>").fadeTo(300, 1) }, 100);
        });

        
    </script>

    <asp:ScriptManager ID="scriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="page">
        <table class="page-table">
            <tr>
                <td class="right-aligned">
                    <asp:Button ID="btnFindClient" runat="server" Text="Find Client" CssClass="button-link"
                        PostBackUrl="~/FindClient.aspx" />
                    <asp:Button ID="btnDashboard" runat="server" Text="Dashboard" PostBackUrl="~/Dashboard.aspx"
                        CssClass="button-link" Visible="false" />
                    <asp:Button ID="btnassignQueue" runat="server" Text="Assign" CssClass="button-action"
                        PostBackUrl="~/AssignQueue.aspx" />
                    <asp:Button ID="btnShareQueue" runat="server" Text="Share" CssClass="button-action"
                        PostBackUrl="~/ShareQueue.aspx" OnClick="btnShareQueue_Click" />
                    <asp:Button ID="btnUploadList" runat="server" Text="Upload Bulk List" CssClass="button-action"
                        Visible="False" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <fieldset>
                        <legend class="panel-header" style="font-weight: bold">Filter</legend>
                        <br />
                        <asp:DropDownList ID="ddlOAMPSIncome" runat="server" CssClass="control">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlBusinessType" runat="server" CssClass="control">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlFlagged" runat="server" CssClass="control">
                            <asp:ListItem Value="-1">Flagged (All)</asp:ListItem>
                            <asp:ListItem Value="1">Flagged</asp:ListItem>
                            <asp:ListItem Value="0">Unflagged</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Button ID="btnShowAll" runat="server" Text="Show" OnClick="btnShowAll_Click"
                            CssClass="button-action" Visible="false" />
                        <asp:UpdatePanel ID="udpLevel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <br />
                                <fieldset>
                                    <legend style="font-weight: lighter">Choose levels : </legend>
                                    <asp:DropDownList ID="ddlRegion" runat="server" CssClass="control" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    &nbsp;<asp:DropDownList ID="ddlBranch" runat="server" CssClass="control" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    &nbsp;<asp:DropDownList ID="ddlExecutive" runat="server" CssClass="control">
                                    </asp:DropDownList>
                                    <asp:Image ID="imgLoader" runat="server" ImageUrl="~/images/empty.gif" />
                                    <br />
                                    <uc1:ucMessanger ID="ucMessanger2" runat="server" />
                                </fieldset>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlRegion" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlBranch" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlExecutive" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <br />
                        <asp:Button ID="btnFilterOpportunities" runat="server" Text="Filter" Style="float: right"
                            CssClass="button-link" OnClick="btnFilterOpportunities_Click" />
                    </fieldset>
                </td>
            </tr>
            <tr style="display:none">
                <td>
                    <div style="background-color: #B0AFA7">
                        <div style="margin-top: -12px; margin-bottom: 13px; font-weight: bold;">
                            Current Activities</div>
                        <div id="divSummary" style="margin: -5px; background-color: #ffffff">
                            <table width="100%">
                                <!-- <tr>
                <td class="page-group" colspan="2" style="font-weight: bold">
                    <asp:Label ID="lblSummary" runat="server">This Week's Scheduled Activities</asp:Label>
                </td>
            </tr>-->
                                <tr>
                                    <td style="padding: 0px; margin: 0px" colspan="2">
                                        <asp:GridView ID="grvSummary" runat="server" AutoGenerateColumns="False" CssClass="summarygrid"
                                            OnRowDataBound="grvSummary_RowDataBound" ShowHeader="true" Width="100%" GridLines="Horizontal"
                                            AllowPaging="true" AllowSorting="false" PageSize="5" BorderColor="#B7B6AB" BorderStyle="Solid"
                                            BorderWidth="2px" HeaderStyle-CssClass="grid-clientSideHeaderSorting">
                                            <PagerSettings Visible="False" />
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-BorderStyle="None">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgAvailable" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ClientID" SortExpression="ClientID" HeaderStyle-BorderStyle="None"
                                                    Visible="False" />
                                                <asp:BoundField DataField="ClientName" SortExpression="ClientName" HeaderStyle-BorderStyle="None"
                                                    HeaderText="Client">
                                                    <ItemStyle Width="230px" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderStyle-BorderStyle="None">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgFlagged" runat="server" ImageUrl="~/images/OpportunityFlagged.gif" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-BorderStyle="None">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgBusinessType" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="18px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="OpportunityID" SortExpression="OpportunityID" HeaderStyle-BorderStyle="None"
                                                    Visible="False" />
                                                <asp:BoundField DataField="OpportunityName" SortExpression="OpportunityName" HeaderStyle-BorderStyle="None"
                                                    HeaderText="Opportunity">
                                                    <ItemStyle Width="230px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# GridUtilities.GetActivityPhase(DataBinder.Eval(Container.DataItem, "Action").ToString()) %>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        Phase</HeaderTemplate>
                                                    <HeaderStyle BorderStyle="None" />
                                                    <ItemStyle Width="70px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label3" runat="server" Text='<%# GridUtilities.GetOpportunityDollarValue(DataBinder.Eval(Container.DataItem, "NetBrokerageEstimated"),
                                    DataBinder.Eval(Container.DataItem, "NetBrokerageQuoted"),
                                    DataBinder.Eval(Container.DataItem, "NetBrokerageActual"),
                                    DataBinder.Eval(Container.DataItem, "Action").ToString()
                                    ) %>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        $ Value</HeaderTemplate>
                                                    <HeaderStyle BorderStyle="None" />
                                                    <ItemStyle Width="70px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Added" DataFormatString="{0:dd-MMM-yy}" HeaderStyle-BorderStyle="None"
                                                    HeaderText="Added" SortExpression="Added" Visible="false">
                                                    <ItemStyle Width="140px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FollowUpDate" HeaderText="FollowUp" HeaderStyle-BorderStyle="None"
                                                    DataFormatString="{0:dd-MMM-yy}" SortExpression="FollowUpDate" NullDisplayText="No Follow-up Date">
                                                    <ItemStyle Width="140px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="OpportunityDue" HeaderText="Renewal Date" HeaderStyle-BorderStyle="None"
                                                    DataFormatString="{0:dd-MMM-yy}" SortExpression="OpportunityDue" NullDisplayText="No Due Date">
                                                    <ItemStyle Width="140px" />
                                                </asp:BoundField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblNoFollowUps" runat="server" Text="No followups" CssClass="darkgrey-italic"></asp:Label>
                                            </EmptyDataTemplate>
                                            <SelectedRowStyle CssClass="selrow" />
                                        </asp:GridView>
                                        <input type="hidden" id="summarySortCol" name="summarySortCol" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="margin: 0" class="grid-footer" colspan="2">
                                        <asp:Image ID="imgAll0" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                        <asp:LinkButton ID="lnkSeeAll0" runat="server" CommandArgument="0" Height="18px"
                                            OnClick="lnkSeeAllSuspects_Click">see all</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="background-color: #B0AFA7">
                        <div id="divChart" style="margin: -5px; background-color: white">
                            <table width="100%">
                                <tr>
                                    <td style="padding: 0px; margin: 0px" colspan="2">
                                        <asp:Panel ID="PnlClick2" runat="server" Height="20px" CssClass="panel-header">
                                            <div style="float: left; vertical-align: middle; height: 20px">
                                                <asp:Image ID="Imgclick2" runat="server" ImageUrl="~/Images/expand.png" /></div>
                                            <div style="float: left; font-weight: bold; vertical-align: middle; margin-left: 5px">
                                                My Pipeline
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlChart" runat="server">
                                            <div style="text-align: center">
                                                <asp:Chart ID="chartSummary" runat="server" Width="500px" Height="300px">
                                                    <Legends>
                                                        <asp:Legend Docking="Bottom" Alignment="Center">
                                                        </asp:Legend>
                                                    </Legends>
                                                    <Series>
                                                        <asp:Series Name="Opportunities" ChartType="Funnel" IsValueShownAsLabel="True">
                                                        </asp:Series>
                                                    </Series>
                                                    <ChartAreas>
                                                        <asp:ChartArea Name="ChartArea1" Area3DStyle-Enable3D="True">
                                                            <Area3DStyle Enable3D="True" />
                                                        </asp:ChartArea>
                                                    </ChartAreas>
                                                </asp:Chart>
                                            </div>
                                        </asp:Panel>
                                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" CollapseControlID="PnlClick2"
                                            Collapsed="false" CollapsedText="" ExpandControlID="PnlClick2" TextLabelID="lblMessage"
                                            ExpandedText="" ImageControlID="Imgclick2" CollapsedImage="~/Images/expand.png"
                                            ExpandedImage="~/Images/collapse.png" ExpandDirection="Vertical" TargetControlID="pnlChart"
                                            ScrollContents="false">
                                        </cc1:CollapsiblePanelExtender>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="background-color: #B0AFA7">
                        <div id="divSharedSummary" style="margin: -5px; background-color: white">
                            <table width="100%">
                                <tr>
                                    <td style="padding: 0px; margin: 0px" colspan="2">
                                        <asp:Panel ID="PanelSharedSummaryclick" runat="server" Height="20px" CssClass="panel-header">
                                            <div style="float: left; vertical-align: middle; height: 20px">
                                                <asp:Image ID="ImageSharedclick" runat="server" ImageUrl="~/Images/expand.png" /></div>
                                            <div style="float: left; font-weight: bold; vertical-align: middle; margin-left: 5px">
                                                Activities Shared With Me
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="PanelsharedSummary" runat="server">
                                            <asp:GridView ID="grvSharedSummary" runat="server" AutoGenerateColumns="False" CssClass="summarygrid"
                                                OnRowDataBound="grvSharedSummary_RowDataBound" ShowHeader="true" Width="100%"
                                                GridLines="Horizontal" AllowPaging="true" AllowSorting="false" PageSize="5" BorderColor="#B7B6AB"
                                                BorderStyle="Solid" BorderWidth="2px" HeaderStyle-CssClass="grid-clientSideHeaderSorting">
                                                <PagerSettings Visible="False" />
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-BorderStyle="None">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgAvailable" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ClientID" SortExpression="ClientID" HeaderStyle-BorderStyle="None"
                                                        Visible="False" />
                                                    <asp:BoundField DataField="ClientName" SortExpression="ClientName" HeaderStyle-BorderStyle="None"
                                                        HeaderText="Client">
                                                        <ItemStyle Width="230px" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderStyle-BorderStyle="None">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgFlagged" runat="server" ImageUrl="~/images/OpportunityFlagged.gif" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-BorderStyle="None">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgBusinessType" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="18px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="OpportunityID" SortExpression="OpportunityID" HeaderStyle-BorderStyle="None"
                                                        Visible="False" />
                                                    <asp:BoundField DataField="OpportunityName" SortExpression="OpportunityName" HeaderStyle-BorderStyle="None"
                                                        HeaderText="Opportunity">
                                                        <ItemStyle Width="230px" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label2" runat="server" Text='<%# GridUtilities.GetActivityPhase(DataBinder.Eval(Container.DataItem, "Action").ToString()) %>'> </asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            Phase</HeaderTemplate>
                                                        <HeaderStyle BorderStyle="None" />
                                                        <ItemStyle Width="70px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label3" runat="server" Text='<%# GridUtilities.GetOpportunityDollarValue(DataBinder.Eval(Container.DataItem, "NetBrokerageEstimated"),
                                    DataBinder.Eval(Container.DataItem, "NetBrokerageQuoted"),
                                    DataBinder.Eval(Container.DataItem, "NetBrokerageActual"),
                                    DataBinder.Eval(Container.DataItem, "Action").ToString()
                                    ) %>'> </asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            $ Value</HeaderTemplate>
                                                        <HeaderStyle BorderStyle="None" />
                                                        <ItemStyle Width="70px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Added" DataFormatString="{0:dd-MMM-yy}" HeaderStyle-BorderStyle="None"
                                                        HeaderText="Added" SortExpression="Added" Visible="false">
                                                        <ItemStyle Width="140px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FollowUpDate" HeaderText="FollowUp" HeaderStyle-BorderStyle="None"
                                                        DataFormatString="{0:dd-MMM-yy}" SortExpression="FollowUpDate" NullDisplayText="No Follow-up Date">
                                                        <ItemStyle Width="140px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OpportunityDue" HeaderText="Renewal Date" HeaderStyle-BorderStyle="None"
                                                        DataFormatString="{0:dd-MMM-yy}" SortExpression="OpportunityDue" NullDisplayText="No Due Date">
                                                        <ItemStyle Width="140px" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblNoFollowUps" runat="server" Text="No followups" CssClass="darkgrey-italic"></asp:Label>
                                                </EmptyDataTemplate>
                                                <SelectedRowStyle CssClass="selrow" />
                                            </asp:GridView>
                                            <br />
                                            <table width="100%">
                                                <tr>
                                                    <td class="grid-footer">
                                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="7" Height="18px"
                                                            OnClick="lnkSeeAllSuspects_Click">see all</asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <input type="hidden" id="sharedSummarySortCol" name="sharedSummarySortCol" runat="server" />
                                   
                                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" CollapseControlID="PanelSharedSummaryclick"
                                            Collapsed="true" CollapsedText="" ExpandControlID="PanelSharedSummaryclick" TextLabelID="lblMessage"
                                            ExpandedText="" ImageControlID="ImageSharedclick" CollapsedImage="~/Images/expand.png"
                                            ExpandedImage="~/Images/collapse.png" ExpandDirection="Vertical" TargetControlID="PanelsharedSummary"
                                            ScrollContents="false">
                                        </cc1:CollapsiblePanelExtender>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
            <tr style="height: 10px; padding: 0px !important">
                <td colspan="2" style="padding: 0px !important">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <div style="background-color: #B0AFA7">
                        <div style="margin-top: -12px; margin-bottom: 13px; font-weight: bold;">
                            All Activities
                        </div>
                        <div id="divActivities" style="margin: -5px; background-color: white">
                        <input type="hidden" id="QualifySortCol" name="QualifySortCol" runat="server" />
                        <input type="hidden" id="RespondSortCol" name="RespondSortCol" runat="server" />
                        <input type="hidden" id="CompleteSortCol" name="CompleteSortCol" runat="server" />
                            <table width="100%">
                                <!--<tr>
                <td class="page-group" colspan="2" style="font-weight: bold;">
                    <asp:Label ID="Label1" runat="server">All Activities</asp:Label>
                </td>
            </tr>-->
                                <tr>
                                    <td colspan="2" style="padding: 0px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td class="page-group" colspan="2">
                                        <asp:Label ID="lblIdentified" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td style="padding: 0px; margin: 0px" colspan="2">
                                        <asp:GridView ID="grvIdentified" runat="server" AutoGenerateColumns="False" CssClass="grid"
                                            OnRowDataBound="grvIdentified_RowDataBound" ShowHeader="False" Width="100%" GridLines="Horizontal"
                                            AllowPaging="True" PageSize="3">
                                            <PagerSettings Visible="False" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgAvailable" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ClientID" SortExpression="ClientID" Visible="False" />
                                                <asp:BoundField DataField="ClientName" SortExpression="ClientName">
                                                    <ItemStyle Width="230px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgFlagged" runat="server" ImageUrl="~/images/OpportunityFlagged.gif" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgBusinessType" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="18px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="OpportunityID" SortExpression="OpportunityID" Visible="False" />
                                                <asp:BoundField DataField="OpportunityName" SortExpression="OpportunityName">
                                                    <ItemStyle Width="230px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Added" DataFormatString="Added: {0:dd-MMM-yy}" SortExpression="Added">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FollowUpDate" DataFormatString="Follow-up: {0:dd-MMM-yy}"
                                                    SortExpression="FollowUpDate" NullDisplayText="No Follow-up Date">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblNoFollowUps" runat="server" Text="No followups" CssClass="darkgrey-italic"></asp:Label>
                                            </EmptyDataTemplate>
                                            <SelectedRowStyle CssClass="selrow" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td style="margin: 0" class="grid-footer" colspan="2">
                                        <asp:Image ID="imgAll1" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                        <asp:LinkButton ID="lnkSeeAll1" runat="server" CommandArgument="1" Height="18px"
                                            OnClick="lnkSeeAllSuspects_Click">see all</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="margin: 0" class="page-group" colspan="2">
                                        <asp:Label ID="lblQualifiedIn" runat="server"></asp:Label>
                                        <br />
                                        <asp:Label ID="lblQualifyMatrix" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 0px; margin: 0px" colspan="2">
                                        <asp:GridView ID="grvQualifiedIn" runat="server" OnRowDataBound="grvQualifiedIn_RowDataBound"
                                            AutoGenerateColumns="False" CssClass="summarygrid" ShowHeader="true" Width="100%" 
                                            GridLines="Horizontal" AllowPaging="true" AllowSorting="false" PageSize="5" BorderColor="#B7B6AB"
                                            BorderStyle="Solid" BorderWidth="1px" HeaderStyle-CssClass="grid-clientSideHeaderSorting"  >
                                            <PagerSettings Visible="False" />
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-BorderStyle="None">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgAvailable" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ClientID" SortExpression="ClientID" Visible="False" HeaderStyle-BorderStyle="None" />
                                                <asp:BoundField DataField="ClientName" SortExpression="ClientName" HeaderText="Client" HeaderStyle-BorderStyle="None">
                                                    <ItemStyle Width="230px" CssClass="grid-Clikcablelink" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgFlagged" runat="server" ImageUrl="~/images/OpportunityFlagged.gif" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgBusinessType" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="OpportunityID" SortExpression="OpportunityID" Visible="False" />
                                                <asp:BoundField DataField="OpportunityName" SortExpression="OpportunityName" HeaderText="Opportunity">
                                                    <ItemStyle Width="230px" />
                                                </asp:BoundField>
                                                                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label2" runat="server" Text='<%# GridUtilities.GetActivityPhase(DataBinder.Eval(Container.DataItem, "Action").ToString()) %>'> </asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            Phase</HeaderTemplate>
                                                        <HeaderStyle BorderStyle="None" />
                                                        <ItemStyle Width="70px" />
                                                    </asp:TemplateField>
                                                                                                        <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label3" runat="server" Text='<%# GridUtilities.GetOpportunityDollarValue(DataBinder.Eval(Container.DataItem, "NetBrokerageEstimated"),
                                    DataBinder.Eval(Container.DataItem, "NetBrokerageQuoted"),
                                    DataBinder.Eval(Container.DataItem, "NetBrokerageActual"),
                                    DataBinder.Eval(Container.DataItem, "Action").ToString()
                                    ) %>'> </asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            $ Value</HeaderTemplate>
                                                        <HeaderStyle BorderStyle="None" />
                                                        <ItemStyle Width="70px" />
                                                    </asp:TemplateField>
                                                <asp:BoundField DataField="FollowUpDate" HeaderText="FollowUp" HeaderStyle-BorderStyle="None"
                                                    DataFormatString="{0:dd-MMM-yy}" SortExpression="FollowUpDate" NullDisplayText="No Follow-up Date">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="OpportunityDue" HeaderText="Renewal Date" HeaderStyle-BorderStyle="None"
                                                    DataFormatString="{0:dd-MMM-yy}" SortExpression="OpportunityDue" NullDisplayText="No Due Date">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblNoFollowUps" runat="server" Text="No followups" CssClass="darkgrey-italic"></asp:Label>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="grid-footer" colspan="2">
                                        <asp:Image ID="imgAll2" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                        <asp:LinkButton ID="lnkSeeAll2" runat="server" CommandArgument="2" Height="18px"
                                            OnClick="lnkSeeAllSuspects_Click">see all</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="page-group" colspan="2">
                                        <asp:Label ID="lblInterested" runat="server"></asp:Label>
                                        <br />
                                        <asp:Label ID="lblRespondMatrix" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 0px; margin: 0px" colspan="2">
                                        <asp:GridView ID="grvInterested" runat="server" AutoGenerateColumns="False" CssClass="summarygrid" ShowHeader="true" Width="100%" 
GridLines="Horizontal" AllowPaging="true" AllowSorting="false" PageSize="5" BorderColor="#B7B6AB"
 BorderStyle="Solid" BorderWidth="1px" HeaderStyle-CssClass="grid-clientSideHeaderSorting"
                                            OnRowDataBound="grvInterested_RowDataBound">
                                            <PagerSettings Visible="False" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgAvailable" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ClientID" SortExpression="ClientID" Visible="False" />
                                                <asp:BoundField DataField="ClientName" SortExpression="ClientName" HeaderText="Client">
                                                    <ItemStyle Width="230px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgFlagged" runat="server" ImageUrl="~/images/OpportunityFlagged.gif" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgBusinessType" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="OpportunityID" SortExpression="OpportunityID" Visible="False" />
                                                <asp:BoundField DataField="OpportunityName" SortExpression="OpportunityName" HeaderText="Opportunity">
                                                    <ItemStyle Width="230px" />
                                                </asp:BoundField>
                                                                                                   <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label2" runat="server" Text='<%# GridUtilities.GetActivityPhase(DataBinder.Eval(Container.DataItem, "Action").ToString()) %>'> </asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            Phase</HeaderTemplate>
                                                        <HeaderStyle BorderStyle="None" />
                                                        <ItemStyle Width="70px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label3" runat="server" Text='<%# GridUtilities.GetOpportunityDollarValue(DataBinder.Eval(Container.DataItem, "NetBrokerageEstimated"),
                                    DataBinder.Eval(Container.DataItem, "NetBrokerageQuoted"),
                                    DataBinder.Eval(Container.DataItem, "NetBrokerageActual"),
                                    DataBinder.Eval(Container.DataItem, "Action").ToString()
                                    ) %>'> </asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            $ Value</HeaderTemplate>
                                                        <HeaderStyle BorderStyle="None" />
                                                        <ItemStyle Width="70px" />
                                                    </asp:TemplateField>
                                                <asp:BoundField DataField="FollowUpDate" HeaderText="FollowUp" HeaderStyle-BorderStyle="None"
                                                    DataFormatString="{0:dd-MMM-yy}" SortExpression="FollowUpDate" NullDisplayText="No Follow-up Date">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                   <asp:BoundField DataField="OpportunityDue" HeaderText="Renewal Date" HeaderStyle-BorderStyle="None"
                                                    DataFormatString="{0:dd-MMM-yy}" SortExpression="OpportunityDue" NullDisplayText="No Due Date">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblNoFollowUps" runat="server" Text="No followups" CssClass="darkgrey-italic"></asp:Label>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="grid-footer" colspan="2">
                                        <asp:Image ID="imgAll3" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                        <asp:LinkButton ID="lnkSeeAll3" runat="server" CommandArgument="3" Height="18px"
                                            OnClick="lnkSeeAllSuspects_Click">see all</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td class="page-group" colspan="2">
                                        <asp:Label ID="lblGoToMarket" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td style="padding: 0px; margin: 0px" colspan="2">
                                        <asp:GridView ID="grvGoToMarket" runat="server" AutoGenerateColumns="False" CssClass="grid"
                                            ShowHeader="False" Width="100%" GridLines="Horizontal" AllowPaging="True" PageSize="3"
                                            OnRowDataBound="grvGoToMarket_RowDataBound">
                                            <PagerSettings Visible="False" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgAvailable" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ClientID" SortExpression="ClientID" Visible="False" />
                                                <asp:BoundField DataField="ClientName" SortExpression="ClientName">
                                                    <ItemStyle Width="230px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgFlagged" runat="server" ImageUrl="~/images/OpportunityFlagged.gif" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgBusinessType" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="OpportunityID" SortExpression="OpportunityID" Visible="False" />
                                                <asp:BoundField DataField="OpportunityName" SortExpression="OpportunityName">
                                                    <ItemStyle Width="230px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FollowUpDate" DataFormatString="Follow-up: {0:dd-MMM-yy}"
                                                    SortExpression="FollowUpDate" NullDisplayText="No Follow-up Date">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="OpportunityDue" DataFormatString="Due: {0:dd-MMM-yy}"
                                                    SortExpression="OpportunityDue" HeaderText="Renewal Date" NullDisplayText="No Due Date">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblNoFollowUps" runat="server" Text="No followups" CssClass="darkgrey-italic"></asp:Label>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td colspan="2" class="grid-footer">
                                        <asp:Image ID="imgAll4" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                        <asp:LinkButton ID="lnkSeeAll4" runat="server" CommandArgument="4" Height="18px"
                                            OnClick="lnkSeeAllSuspects_Click">see all</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td colspan="2" class="page-group">
                                        <asp:Label ID="lblQuoted" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td style="padding: 0px; margin: 0px" colspan="2">
                                        <asp:GridView ID="grvQuoted" runat="server" AutoGenerateColumns="False" CssClass="grid"
                                            ShowHeader="False" Width="100%" GridLines="Horizontal" AllowPaging="True" PageSize="3"
                                            OnRowDataBound="grvQuoted_RowDataBound">
                                            <PagerSettings Visible="False" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgAvailable" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ClientID" SortExpression="ClientID" Visible="False" />
                                                <asp:BoundField DataField="ClientName" SortExpression="ClientName">
                                                    <ItemStyle Width="230px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgFlagged" runat="server" ImageUrl="~/images/OpportunityFlagged.gif" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgBusinessType" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="OpportunityID" SortExpression="OpportunityID" Visible="False" />
                                                <asp:BoundField DataField="OpportunityName" SortExpression="OpportunityName">
                                                    <ItemStyle Width="230px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FollowUpDate" DataFormatString="Follow-up: {0:dd-MMM-yy}"
                                                    SortExpression="FollowUpDate" NullDisplayText="No Follow-up Date">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="OpportunityDue" DataFormatString="Due: {0:dd-MMM-yy}"
                                                    SortExpression="OpportunityDue" HeaderText="Renewal Date" NullDisplayText="No Due Date">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblNoFollowUps" runat="server" Text="No followups" CssClass="darkgrey-italic"></asp:Label>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td colspan="2" class="grid-footer">
                                        <asp:Image ID="imgAll5" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                        <asp:LinkButton ID="lnkSeeAll5" runat="server" CommandArgument="5" Height="18px"
                                            OnClick="lnkSeeAllSuspects_Click">see all</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="page-group">
                                        <asp:Label ID="lblAccepted" runat="server"></asp:Label>
                                        <br />
                                        <asp:Label ID="lblCompleteMatrix" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 0px; margin: 0px" colspan="2">
                                        <asp:GridView ID="grvAccepted" runat="server" AutoGenerateColumns="False" CssClass="summarygrid" ShowHeader="true" Width="100%" 
GridLines="Horizontal" AllowPaging="true" AllowSorting="false" PageSize="5" BorderColor="#B7B6AB"
 BorderStyle="Solid" BorderWidth="1px" HeaderStyle-CssClass="grid-clientSideHeaderSorting"
                                            OnRowDataBound="grvAccepted_RowDataBound">
                                            <PagerSettings Visible="False" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgAvailable" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ClientID" SortExpression="ClientID" Visible="False" />
                                                <asp:BoundField DataField="ClientName" SortExpression="ClientName"  HeaderText="Client">
                                                    <ItemStyle Width="230px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgFlagged" runat="server" ImageUrl="~/images/OpportunityFlagged.gif" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgBusinessType" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="OpportunityID" SortExpression="OpportunityID" Visible="False" />
                                                <asp:BoundField DataField="OpportunityName" SortExpression="OpportunityName"  HeaderText="Opportunity">
                                                    <ItemStyle Width="230px" />
                                                </asp:BoundField>
                                                                                                   <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label2" runat="server" Text='<%# GridUtilities.GetActivityPhase(DataBinder.Eval(Container.DataItem, "Action").ToString()) %>'> </asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            Phase</HeaderTemplate>
                                                        <HeaderStyle BorderStyle="None" />
                                                        <ItemStyle Width="70px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label3" runat="server" Text='<%# GridUtilities.GetOpportunityDollarValue(DataBinder.Eval(Container.DataItem, "NetBrokerageEstimated"),
                                    DataBinder.Eval(Container.DataItem, "NetBrokerageQuoted"),
                                    DataBinder.Eval(Container.DataItem, "NetBrokerageActual"),
                                    DataBinder.Eval(Container.DataItem, "Action").ToString()
                                    ) %>'> </asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            $ Value</HeaderTemplate>
                                                        <HeaderStyle BorderStyle="None" />
                                                        <ItemStyle Width="70px" />
                                                    </asp:TemplateField>
  <asp:BoundField DataField="FollowUpDate" HeaderText="FollowUp" HeaderStyle-BorderStyle="None"
                                                    DataFormatString="{0:dd-MMM-yy}" SortExpression="FollowUpDate" NullDisplayText="No Follow-up Date">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                               <asp:BoundField DataField="OpportunityDue" HeaderText="Renewal Date" HeaderStyle-BorderStyle="None"
                                                    DataFormatString="{0:dd-MMM-yy}" SortExpression="OpportunityDue" NullDisplayText="No Due Date">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblNoFollowUps" runat="server" Text="No followups" CssClass="darkgrey-italic"></asp:Label>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="grid-footer">
                                        <asp:Image ID="imgAll6" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                                        <asp:LinkButton ID="lnkSeeAll6" runat="server" CommandArgument="6" Height="18px"
                                            OnClick="lnkSeeAllSuspects_Click">see all</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <uc1:ucMessanger ID="ucMessanger1" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <script src="scripts/JqueryPopup/jquery.magnific-popup.min.js"></script>

    <script>
        function initUploadDialog(button) {
            $(button).magnificPopup({
                items: {
                    src: '/bulklistfileupload.aspx'
                },
                type: 'iframe'

            });

        }

        $(document).ready(function() {
            initUploadDialog("#<%=btnUploadList.ClientID%>");
        });
    </script>

</asp:Content>
