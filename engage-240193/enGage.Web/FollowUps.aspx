<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="FollowUps.aspx.cs" Inherits="enGage.Web.FollowUps" %>

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
            setupGridSorting("#<%=grvSummary.ClientID%>", "#<%=summarySortCol.ClientID%>");


            $("#divSummary").corner("round 8px").parent().css('padding', '8px').corner("round 14px").css('padding-top', '20px');
            $("#divChart").corner("round 8px").parent().css('padding', '8px').corner("round 14px").css('padding-top', '20px');
            $("#divSharedSummary").corner("round 8px").parent().css('padding', '8px').corner("round 14px").css('padding-top', '20px');

            $("#divActivities").corner("round 8px").parent().css('padding', '8px').corner("round 14px").css('padding-top', '20px');

        });

        
    </script>

    <asp:ScriptManager ID="scriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="page">
        <table class="page-table">
            <tr>
                <td class="left-align">
                    <a href="/FollowUpsAllActivities.aspx" target="_blank">All Activities</a>
                </td>
            </tr>
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
            <tr>
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
            <tr style="height: 10px; padding: 0px !important">
                <td colspan="2" style="padding: 0px !important">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <uc1:ucMessanger ID="ucMessanger1" runat="server" />
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
