<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="FollowUpsAll.aspx.cs" Inherits="enGage.Web.FollowUpsAll" %>

<%@ Register Src="Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
<%@ Import Namespace="enGage.Web.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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
                    5: {
                        sorter: 'SpecialShortDate'
                    },
                    7: {
                        sorter: 'SpecialShortDate'
                    },
                    8: {
                        sorter: 'SpecialShortDate'
                    }

                }

            });

            $(".header", $(gridClientId)).removeClass("header").addClass("summarygrid-customheader");
            $(gridClientId).removeClass("summarygrid-customheader");
            // $("th , #<%=grvSummary.ClientID%>").addClass("header");


        }

        function setupActivitiesGridSorting(gridClientId) {

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

            $(".header", $(gridClientId)).removeClass("header").addClass("summarygrid-customheader");
            $(gridClientId).removeClass("summarygrid-customheader");


            if ($(gridClientId)[0] == null) 
                return;
    

            if ($(gridClientId)[0].tHead != null) // check that there are rowas in the grid
            {
                $(gridClientId).closest("tr").next().find(".grid-footer").css("display", "");
                $(gridClientId).tablesorterPager({ container: $(gridClientId).closest("tr").next().find(".pager"), size: 25 });
            }
            else {
                // hide the pager td
                $(gridClientId).closest("tr").next().find(".grid-footer").css("display", "none");

            }
        }

        function convertToArray(parameterValue) {
            var sorting = parameterValue;
            sorting = sorting.replace(/\[/g, "");
            sorting = sorting.replace(/\]/g, "");
            sorting = sorting.split(',');
            for (var i = 0; i < sorting.length; i++)
                sorting[i] = parseInt(sorting[i], 10);

            var result = new Array();
            result[0] = sorting;
            return result;
        }

        $(document).ready(function() {

            // hide all pagers first
        $(".grid-footer").css("display", "none");
        $(".page").css("min-height", "10px");
       

            setupGridSorting("#<%=grvSummary.ClientID%>", "");
            setupGridSorting("#<%=grvSharedSummary.ClientID%>", "");
            setupActivitiesGridSorting("#<%=grvQualifiedIn.ClientID%>");
            setupActivitiesGridSorting("#<%=grvInterested.ClientID%>");
            setupActivitiesGridSorting("#<%=grvAccepted.ClientID%>");


            // set sorting as from url
            if ($.urlParam('t') != null && $.urlParam('t') != 0) {
                var result = convertToArray($.urlParam('t'));
                $("#<%=grvSummary.ClientID%>").trigger("sorton", [result]);
            }
            // set sorting as from url
            if ($.urlParam('ts') != null && $.urlParam('ts') != 0) {
                var result = convertToArray($.urlParam('ts'));
                $("#<%=grvSharedSummary.ClientID%>").trigger("sorton", [result]);
            }
            // set sorting as from url
            if ($.urlParam('q') != null && $.urlParam('q') != 0) {
                var result = convertToArray($.urlParam('q'));
                $("#<%=grvQualifiedIn.ClientID%>").trigger("sorton", [result]);
            }



        });

        
    </script>

    <div class="page">

        <table class="page-table">
            <tr>
                <td class="page-group">
                    <asp:Label ID="lblTitle" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="margin: 0; padding: 0px; margin: 0px">
                    <asp:GridView ID="grvSummary" runat="server" AutoGenerateColumns="False" CssClass="summarygrid"
                        OnRowDataBound="grvSummary_RowDataBound" ShowHeader="true" Width="100%" GridLines="Horizontal"
                        AllowPaging="false" AllowSorting="false" BorderColor="#B7B6AB" BorderStyle="Solid"
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
                            <asp:BoundField DataField="Added" DataFormatString="{0:dd-MMM-yy}" HeaderStyle-BorderStyle="None"
                                HeaderText="Added" SortExpression="Added">
                                <ItemStyle Width="140px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderStyle-BorderStyle="None">
                                <ItemTemplate>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="0px" />
                            </asp:TemplateField>
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
                </td>
            </tr>
            <tr>
                <td style="margin: 0; padding: 0px; margin: 0px">
                    <asp:GridView ID="grvSharedSummary" runat="server" AutoGenerateColumns="False" CssClass="summarygrid"
                        OnRowDataBound="grvSharedSummary_RowDataBound" ShowHeader="true" Width="100%"
                        GridLines="Horizontal" AllowPaging="false" AllowSorting="false" BorderColor="#B7B6AB"
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
                            <asp:BoundField DataField="Added" DataFormatString="{0:dd-MMM-yy}" HeaderStyle-BorderStyle="None"
                                HeaderText="Added" SortExpression="Added" Visible="false">
                                <ItemStyle Width="140px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderStyle-BorderStyle="None">
                                <ItemTemplate>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="0px" />
                            </asp:TemplateField>
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
                </td>
            </tr>
            <tr>
                <td style="margin: 0; padding: 0px; margin: 0px">
                    <asp:GridView ID="grvIdentified" runat="server" AutoGenerateColumns="False" CssClass="grid"
                        OnRowDataBound="grvIdentified_RowDataBound" ShowHeader="False" Width="100%" GridLines="Horizontal"
                        AllowPaging="True" OnPageIndexChanging="grvIdentified_PageIndexChanging" PageSize="25">
                        <PagerSettings FirstPageImageUrl="~/images/ArrowFirst.gif" LastPageImageUrl="~/images/ArrowLast.gif"
                            Mode="NumericFirstLast" Position="Top" FirstPageText="first" LastPageText="last" />
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
                            <asp:BoundField DataField="Added" DataFormatString="Added: {0:dd-MMM-yy}" SortExpression="Added">
                                <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FollowUpDate" DataFormatString="Follow-up: {0:dd-MMM-yy}"
                                SortExpression="FollowUpDate" NullDisplayText="No Follow-up Date">
                                <ItemStyle Width="150px" />
                            </asp:BoundField>
                        </Columns>
                        <PagerStyle CssClass="grid-pager" />
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoFollowUps" runat="server" Text="There are no followups" CssClass="darkgrey-italic"></asp:Label>
                        </EmptyDataTemplate>
                        <SelectedRowStyle CssClass="selrow" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td style="margin: 0; padding: 0px; margin: 0px">
                    <asp:GridView ID="grvQualifiedIn" runat="server" AutoGenerateColumns="False" CssClass="summarygrid"
                        ShowHeader="true" Width="100%" GridLines="Horizontal" AllowPaging="false" AllowSorting="false"
                        BorderColor="#B7B6AB" BorderStyle="Solid" BorderWidth="1px" HeaderStyle-CssClass="grid-clientSideHeaderSorting"
                        OnRowDataBound="grvQualifiedIn_RowDataBound" OnPageIndexChanging="grvQualifiedIn_PageIndexChanging">
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
                            <asp:BoundField DataField="StatusName" SortExpression="StatusName" HeaderText="Status"
                                Visible="false">
                                <ItemStyle Width="120px" />
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
                        <PagerStyle CssClass="grid-pager" />
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoFollowUps" runat="server" Text="There are no followups" CssClass="darkgrey-italic"></asp:Label>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
       <tr>
                <td class="grid-footer"  style="margin: 0; padding: 0px; margin: 0px">
                    <div id="pager"   class="pager">
                        <form>
                        <img src="images/pager/first.png" class="first" />
                        <img src="images/pager/prev.png" class="prev" />
                        <input type="text" class="pagedisplay" />
                        <img src="images/pager/next.png" class="next" />
                        <img src="images/pager/last.png" class="last" />
                        <select class="pagesize">
                            <option selected="selected" value="25">25</option>
                            <option value="40">40</option>
                            <option value="100">100</option>
                            <option value="200">200</option>
                        </select>
                        </form>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="margin: 0; padding: 0px; margin: 0px">
                    <asp:GridView ID="grvInterested" runat="server" AutoGenerateColumns="False" CssClass="summarygrid"
                        ShowHeader="true" Width="100%" GridLines="Horizontal" AllowPaging="false" AllowSorting="false"
                        BorderColor="#B7B6AB" BorderStyle="Solid" BorderWidth="2px" HeaderStyle-CssClass="grid-clientSideHeaderSorting"
                        OnRowDataBound="grvInterested_RowDataBound" OnPageIndexChanging="grvInterested_PageIndexChanging">
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
                            <asp:BoundField DataField="StatusName" SortExpression="StatusName" Visible="false">
                                <ItemStyle Width="120px" />
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
                        <PagerStyle CssClass="grid-pager" />
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoFollowUps" runat="server" Text="There are no followups" CssClass="darkgrey-italic"></asp:Label>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
          <tr>
                <td class="grid-footer" >
                    <div  id="pager2" class="pager">
                        <form>
                        <img src="images/pager/first.png" class="first" />
                        <img src="images/pager/prev.png" class="prev" />
                        <input type="text" class="pagedisplay" />
                        <img src="images/pager/next.png" class="next" />
                        <img src="images/pager/last.png" class="last" />
                        <select class="pagesize">
                            <option selected="selected" value="25">25</option>
                            <option value="40">40</option>
                            <option value="100">100</option>
                            <option value="200">200</option>
                        </select>
                        </form>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="margin: 0; padding: 0px; margin: 0px">
                    <asp:GridView ID="grvGoToMarket" runat="server" AutoGenerateColumns="False" CssClass="grid"
                        ShowHeader="False" Width="100%" GridLines="Horizontal" OnRowDataBound="grvGoToMarket_RowDataBound"
                        AllowPaging="True" OnPageIndexChanging="grvGoToMarket_PageIndexChanging" PageSize="25">
                        <PagerSettings FirstPageImageUrl="~/images/ArrowFirst.gif" FirstPageText="first"
                            LastPageImageUrl="~/images/ArrowLast.gif" LastPageText="last" Mode="NumericFirstLast"
                            Position="Top" />
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
                        <PagerStyle CssClass="grid-pager" />
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoFollowUps" runat="server" Text="There are no followups" CssClass="darkgrey-italic"></asp:Label>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td style="margin: 0; padding: 0px; margin: 0px">
                    <asp:GridView ID="grvQuoted" runat="server" AutoGenerateColumns="False" CssClass="grid"
                        ShowHeader="False" Width="100%" GridLines="Horizontal" OnRowDataBound="grvQuoted_RowDataBound"
                        AllowPaging="True" OnPageIndexChanging="grvQuoted_PageIndexChanging" PageSize="25">
                        <PagerSettings FirstPageImageUrl="~/images/ArrowFirst.gif" FirstPageText="first"
                            LastPageImageUrl="~/images/ArrowLast.gif" LastPageText="last" Mode="NumericFirstLast"
                            Position="Top" />
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
                        <PagerStyle CssClass="grid-pager" />
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoFollowUps" runat="server" Text="There are no followups" CssClass="darkgrey-italic"></asp:Label>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td style="margin: 0; padding: 0px; margin: 0px">
                    <asp:GridView ID="grvAccepted" runat="server" AutoGenerateColumns="False" CssClass="summarygrid"
                        ShowHeader="true" Width="100%" GridLines="Horizontal" AllowPaging="false" AllowSorting="false"
                        BorderColor="#B7B6AB" BorderStyle="Solid" BorderWidth="2px" HeaderStyle-CssClass="grid-clientSideHeaderSorting"
                        OnRowDataBound="grvAccepted_RowDataBound" OnPageIndexChanging="grvAccepted_PageIndexChanging">
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
                            <asp:BoundField DataField="StatusName" SortExpression="StatusName" Visible="false">
                                <ItemStyle Width="120px" />
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
                        <PagerStyle CssClass="grid-pager" />
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoFollowUps" runat="server" Text="There are no followups" CssClass="darkgrey-italic"></asp:Label>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td class="grid-footer" >
                    <div  id="pager3" class="pager">
                        <form>
                        <img src="images/pager/first.png" class="first" />
                        <img src="images/pager/prev.png" class="prev" />
                        <input type="text" class="pagedisplay" />
                        <img src="images/pager/next.png" class="next" />
                        <img src="images/pager/last.png" class="last" />
                        <select class="pagesize">
                            <option selected="selected" value="25">25</option>
                            <option value="40">40</option>
                            <option value="100">100</option>
                            <option value="200">200</option>
                        </select>
                        </form>
                    </div>
                </td>
            </tr>
           
            <tr>
                <td>
                    <uc1:ucMessanger ID="ucMessanger1" runat="server" />
                </td>
            </tr>
        </table>

                                    &nbsp;<asp:Button ID="btnBack2" runat="server" CssClass="button-link" PostBackUrl="~/FollowUps.aspx" OnClientClick="javascript:window.location='/FollowUps.aspx'"
                        Text="Summary" />

    </div>
</asp:Content>
