<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="shareQueue.aspx.cs" Inherits="enGage.Web.ShareQueue" %>
<%@ Register src="Controls/ucMessanger.ascx" tagname="ucMessanger" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(startRequest);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);
    var submittedUpdatePanel;
     function startRequest(sender, e) {

         submittedUpdatePanel = e.get_postBackElement();
        if ($("#" + e.get_postBackElement().id).closest("div")[0].id.indexOf("udpFilterLevel") >= 0) {

            document.getElementById('<%=ddlFilterRegion.ClientID%>').disabled = true;
            document.getElementById('<%=ddlFilterBranch.ClientID%>').disabled = true;
            document.getElementById('<%=ddlFilterExcutives.ClientID%>').disabled = true;
            document.getElementById('<%=imgLoaderFilter.ClientID%>').src = 'images/ajax-loader.gif';
        }
        else {
            document.getElementById('<%=ddlRegion.ClientID%>').disabled = true;
            document.getElementById('<%=ddlBranch.ClientID%>').disabled = true;
            document.getElementById('<%=ddlExecutive.ClientID%>').disabled = true;
            document.getElementById('<%=imgLoader.ClientID%>').src = 'images/ajax-loader.gif';
        }

        $("#<%=ddlExecutive.ClientID%>").bind('change', function(e) {
            setButtonEnable();
        });
      
    }
    function endRequest(sender, e) {


        if ($("#" + submittedUpdatePanel.id).closest("div")[0].id.indexOf("udpFilterLevel") >= 0) {
            document.getElementById('<%=ddlFilterRegion.ClientID%>').disabled = false;
            document.getElementById('<%=ddlFilterBranch.ClientID%>').disabled = false;
            document.getElementById('<%=ddlFilterExcutives.ClientID%>').disabled = false;
            document.getElementById('<%=imgLoaderFilter.ClientID%>').src = 'images/empty.gif';
        }
        else {
            document.getElementById('<%=ddlRegion.ClientID%>').disabled = false;
            document.getElementById('<%=ddlBranch.ClientID%>').disabled = false;
            document.getElementById('<%=ddlExecutive.ClientID%>').disabled = false;
            document.getElementById('<%=imgLoader.ClientID%>').src = 'images/empty.gif';
        }


        setButtonEnable();
        $("#<%=ddlExecutive.ClientID%>").bind('change', function(e) {
            setButtonEnable();
        });
    }
</script>
    <script>


        function setUpGridSorting(gridID) {
            $(gridID).addClass("tablesorter");
            $(gridID).removeClass("header");
            $(gridID).tablesorter({

            sortList: [[9, 1]],
            dateFormat: "dd-mmm-yy",
                headers: {
                    // assign the secound column (we start counting zero) 
                    0: {
                        // disable it by setting the property sorter to false 
                        sorter: false
                    },
                    // assign the third column (we start counting zero) 
                    4: {
                        // disable it by setting the property sorter to false 
                        sorter: false
                    },
                    5: {
                        // disable it by setting the property sorter to false 
                        sorter: false
                    },
                    7: {
                        sorter: 'SpecialShortDate'
                    },
                    8: {
                        sorter: 'SpecialShortDate'
                    },
                    9: {
                        sorter: 'SpecialShortDate'
                    }


                }

            });
            if ($(gridID)[0].tHead != null) // check that there are rowas in the grid
            {
                $(gridID).tablesorterPager({ container: $(gridID).closest("tr").next().find(".pager"), size: 2 });
            }
            else{
                // hide the pager td
                $(gridID).closest("tr").next().find(".grid-footer").css("display", "none");
            
            }
            
            
            $(gridID).bind("sortEnd", function(e) {
            $("#<%=summarySortCol.ClientID%>")[0].value = JSON.stringify(e.target.config.sortList);
            });

            $(".header", $(gridID)).removeClass("header").addClass("summarygrid-customheader");
            $(gridID).removeClass("summarygrid-customheader");

        }
        function bindGridCheckBoxColumn(gridID) {

            // get the whole table not only the showenpage
            var table = $(gridID);
            if (table.length > 0 && table[0].config!="undefined" && table[0].config!=null) {


                $(gridID + " input[id*='chkbSelectAll']:checkbox").click(function(event) {
                    var tableData = $(table[0].config.rowsCopy);
                    if (tableData != "undefined" && tableData != null) {
                        var headerChk = $(gridID + " input[id*='chkbSelectAll']:checkbox");
                        tableData.each(function() {

                            var itemChk = this.find("input[id*='chkbOpprtunitySelect']:checkbox");

                            itemChk.each(function() {

                                this.checked = headerChk[0].checked;
                                var selectedOpprtunityID = $(this).closest("span").attr("oppoID");
                                var regex = new RegExp(selectedOpprtunityID, 'g');
                                if (headerChk[0].checked == true)
                                    $("#<%=selectedOpportunitiesIDs.ClientID%>")[0].value += '#' + selectedOpprtunityID;
                                else
                                    $("#<%=selectedOpportunitiesIDs.ClientID%>")[0].value = $("#<%=selectedOpportunitiesIDs.ClientID%>")[0].value.replace(regex, "");

                            })

                        });

                    }
                    // check if we should enable the button
                    setButtonEnable();
                    
                });
            
            
//            $(gridID + " input[id*='chkbSelectAll']:checkbox").click(function(event) {

//            var headerChk = $(gridID + " input[id*='chkbSelectAll']:checkbox");
//            var itemChk = $(gridID + " input[id*='chkbOpprtunitySelect']:checkbox");
//            
//                itemChk.each(function() { this.checked = headerChk[0].checked; })
//            });

            var tableData = $(table[0].config.rowsCopy);
            if (tableData != "undefined" && tableData != null) {
                var headerChk = $(gridID + " input[id*='chkbSelectAll']:checkbox");
                tableData.each(function() {

                    var itemChk = this.find("input[id*='chkbOpprtunitySelect']:checkbox");

                    itemChk.click(function(event) {


                        var headerChk = $(gridID + " input[id*='chkbSelectAll']:checkbox");
                        var selectedOpprtunityID = $(this).closest("span").attr("oppoID");
                        var regex = new RegExp(selectedOpprtunityID, 'g');
                        if ($(this)[0].checked == false) 
                        {
                            headerChk[0].checked = false;
                            $("#<%=selectedOpportunitiesIDs.ClientID%>")[0].value = $("#<%=selectedOpportunitiesIDs.ClientID%>")[0].value.replace(regex, "");
                        }
                        else
                            $("#<%=selectedOpportunitiesIDs.ClientID%>")[0].value += '#' + selectedOpprtunityID;

                        // check if we should enable the button
                        setButtonEnable();
                        
                        //then highlight the row

                        if ($(this).is(':checked')) {
                            $(this).parents("tr:first").addClass('highlightRow');
                        }
                        else {
                            $(this).parents("tr:first").removeClass('highlightRow');
                        }
                        event.stopPropagation();

                    });


                });

            }

        }
//            $(gridID + " input[id*='chkbOpprtunitySelect']:checkbox").click(function(event) {

//           
//                var headerChk = $(gridID + " input[id*='chkbSelectAll']:checkbox");

//                if ($(this)[0].checked == false) headerChk[0].checked = false;

//                //then highlight the row

//                if ($(this).is(':checked')) {
//                    $(this).parents("tr:first").addClass('highlightRow');
//                }
//                else {
//                    $(this).parents("tr:first").removeClass('highlightRow');
//                }
//                event.stopPropagation();
//                
//            });

        }

        function setUpPage() {

            setUpGridSorting("#<%=grvSummary.ClientID%>");
            setUpGridSorting("#<%=grvQualifiedIn.ClientID%>");
            setUpGridSorting("#<%=grvInterested.ClientID%>");
            setUpGridSorting("#<%=grvAccepted.ClientID%>");


            bindGridCheckBoxColumn("#<%=grvSummary.ClientID%>");
            bindGridCheckBoxColumn("#<%=grvIdentified.ClientID%>");
            bindGridCheckBoxColumn("#<%=grvInterested.ClientID%>");

            bindGridCheckBoxColumn("#<%=grvQualifiedIn.ClientID%>");
            bindGridCheckBoxColumn("#<%=grvAccepted.ClientID%>");

            // disable assign button
            $("#<%=btnShareSelectedOpportunitiesClients.ClientID%>").attr("disabled", true).css("cursor", "default").fadeTo(500, 0.5);
            $("#<%=ddlExecutive.ClientID%>").bind('change', function(e) {
                setButtonEnable();
            });

        }

        function setButtonEnable() {

            var regex = new RegExp('#', 'g');
            // check if we should enable the button
            if ($("#<%=selectedOpportunitiesIDs.ClientID%>")[0].value.replace(regex, "") != "" && $("#<%=ddlExecutive.ClientID%>")[0].selectedIndex !== 0)
                $("#<%=btnShareSelectedOpportunitiesClients.ClientID%>").removeAttr("disabled").css("cursor", "pointer").fadeTo(500, 1);
            else
                $("#<%=btnShareSelectedOpportunitiesClients.ClientID%>").attr("disabled", true).css("cursor", "default").fadeTo(500, 0.5);

        }
        
        $(document).ready(function() {
            // attachAccordionClickEvent();

            setUpPage();
//            window.setTimeout(function() { setUpPage() }, 0);
//            window.setTimeout(function() { setUpPage() }, 500);

            // $("th , #<%=grvSummary.ClientID%>").addClass("header");
        });

        
    </script>
     <style type="text/css">
       .highlightRow
        {
            background-color: #ffeb95 !important;
            cursor: pointer;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<input id="selectedOpportunitiesIDs" runat="server" type="hidden" />
    <div class="page-assign">
        <table class="page-table">
          <tr>
        <td class="right-aligned">
        <asp:Button ID="btnFollowUps" runat="server" PostBackUrl="~/FollowUps.aspx" Text="Home"
                        CssClass="button-link" />
                <asp:Button ID="btnFindClient" runat="server" Text="Find Client" 
                    CssClass="button-link" PostBackUrl="~/FindClient.aspx" />
                <asp:Button ID="btnDashboard" runat="server" Text="Dashboard" 
                    PostBackUrl="~/Dashboard.aspx" CssClass="button-link" Visible="false" />
            </td>
        </tr>
               <tr>
    <td colspan="2">
                        <fieldset >
                                <legend class="panel-header" style="font-weight:bold" >Filter</legend>
                             
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
                        CssClass="button-action"  Visible="false"/>
                                
                              
                        
                            <asp:UpdatePanel ID="udpFilterLevel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                <br />
                                <fieldset>
                                <legend style="font-weight:lighter" > Choose levels : </legend>

         <asp:DropDownList ID="ddlFilterRegion" runat="server" CssClass="control" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlFilterRegion_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    &nbsp;<asp:DropDownList ID="ddlFilterBranch" runat="server" CssClass="control" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlFilterBranch_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    &nbsp;<asp:DropDownList ID="ddlFilterExcutives" runat="server" CssClass="control">
                                    </asp:DropDownList>
                                    <asp:Image ID="imgLoaderFilter" runat="server" ImageUrl="~/images/empty.gif" />
                                     <br />
                                     <uc1:ucMessanger ID="ucMessanger3" runat="server" />
                               


</fieldset>
                                    
                           
                    
                                </ContentTemplate>
                                  <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="ddlFilterRegion" EventName="SelectedIndexChanged" />
                               <asp:AsyncPostBackTrigger ControlID="ddlFilterBranch" EventName="SelectedIndexChanged" />
                               <asp:AsyncPostBackTrigger ControlID="ddlFilterExcutives" EventName="SelectedIndexChanged" />
                               </Triggers>
                            </asp:UpdatePanel>
                            
                            <br />
                                 <asp:Button ID="btnFilterOpportunities" runat="server" 
                                Text="Filter" style="float:right"
                    CssClass="button-link" onclick="btnFilterOpportunities_Click"  />
                  </fieldset>             
                        </td>
            
        </tr>
        <tr>
            <td class="page-group" colspan="2" style="font-weight:bold">
                <asp:Label ID="Label1" runat="server">Share With</asp:Label>
            </td>
        </tr>
        
         <tr>
                        <td colspan="2">
                            <asp:UpdatePanel ID="udpLevel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    Level
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
                                     <br />
                                     <uc1:ucMessanger ID="ucMessanger2" runat="server" />
                                    <asp:Button ID="btnShareSelectedOpportunitiesClients" runat="server" 
                                Text="Share selected Opprtunities Clients" style="float:right"
                    CssClass="button-link" onclick="btnShareSelectedOpportunitiesClients_Click"  />
                    
                                </ContentTemplate>
                                                                <Triggers>
                                 <asp:AsyncPostBackTrigger ControlID="ddlRegion" EventName="SelectedIndexChanged" />
                               <asp:AsyncPostBackTrigger ControlID="ddlBranch" EventName="SelectedIndexChanged" />
                               <asp:AsyncPostBackTrigger ControlID="ddlExecutive" EventName="SelectedIndexChanged" />

                                <asp:PostBackTrigger ControlID="btnShareSelectedOpportunitiesClients" />
                                </Triggers>
                            </asp:UpdatePanel>
                            
                             
                        </td>
                    </tr>
        <tr>
            <td class="page-group" colspan="2" style="font-weight:bold">
                <asp:Label ID="lblSummary" runat="server">This Week's Scheduled Activities</asp:Label>
            </td>
        </tr>
         <tr>
            <td style="padding: 0px; margin: 0px" colspan="2">
  
                <asp:GridView ID="grvSummary" runat="server" AutoGenerateColumns="False" 
                    CssClass="summarygrid" 
                    datakeynames="OpportunityID"
                    onrowdatabound="grvSummary_RowDataBound" ShowHeader="true" Width="100%" 
                    GridLines="Horizontal" AllowPaging="False" AllowSorting="false" PageSize="3" BorderColor="#B7B6AB" BorderStyle="Solid" BorderWidth="2px"
                     HeaderStyle-CssClass="grid-clientSideHeaderSorting" >
                    <PagerSettings Visible="False" />
                    <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkbSelectAll" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox type="checkbox" name="chkbOpprtunitySelect" ID="chkbOpprtunitySelect" class="cbSelectRow" runat="server" oppoID='<%# DataBinder.Eval(Container.DataItem,"OpportunityID").ToString()%>' />
                        </ItemTemplate>
                        <HeaderStyle  BorderStyle="None"/>
                    </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-BorderStyle="None">
                            <ItemTemplate>
                                <asp:Image ID="imgAvailable" runat="server" 
                                    ImageUrl="~/images/ArrowSolidSmall.gif" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ClientID" SortExpression="ClientID"  HeaderStyle-BorderStyle="None"
                            Visible="False" />
                        <asp:BoundField DataField="ClientName" SortExpression="ClientName"  HeaderStyle-BorderStyle="None" HeaderText="Client">
                        <ItemStyle Width="210px" />
                        </asp:BoundField>
                         <asp:BoundField DataField="StatusName" SortExpression="StatusName"  HeaderStyle-BorderStyle="None" HeaderText="Status" >
                        <ItemStyle Width="80px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-BorderStyle="None">
                            <ItemTemplate>
                                <asp:Image ID="imgFlagged" runat="server" 
                                    ImageUrl="~/images/OpportunityFlagged.gif" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-BorderStyle="None">
                            <ItemTemplate>
                                <asp:Image ID="imgBusinessType" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="18px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="OpportunityID" SortExpression="OpportunityID"  HeaderStyle-BorderStyle="None"
                            Visible="False" />
                        <asp:BoundField DataField="OpportunityName" SortExpression="OpportunityName"  HeaderStyle-BorderStyle="None" HeaderText="Opportunity">
                        <ItemStyle Width="230px" />
                         </asp:BoundField>
                        <asp:BoundField DataField="SharedWithAccountExecutiveID" SortExpression="SharedWithAccountExecutiveID"  HeaderStyle-BorderStyle="None" HeaderText="Shared with">
                        <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Added" DataFormatString="{0:dd-MMM-yy}" HeaderStyle-BorderStyle="None" HeaderText="Added"
                            SortExpression="Added" >
                        <ItemStyle Width="130px" />
                        </asp:BoundField>
                      
                        <asp:BoundField DataField="FollowUpDate"  HeaderText="FollowUp" HeaderStyle-BorderStyle="None"
                            DataFormatString="{0:dd-MMM-yy}" SortExpression="FollowUpDate" 
                            NullDisplayText="No Follow-up Date" >
                        <ItemStyle Width="140px" />
                        </asp:BoundField>
                          <asp:BoundField DataField="OpportunityDue" HeaderText="Renewal Date"  HeaderStyle-BorderStyle="None" DataFormatString="{0:dd-MMM-yy}" 
                            SortExpression="OpportunityDue" NullDisplayText="No Due Date" >
                        <ItemStyle Width="150px" />
                        </asp:BoundField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="lblNoFollowUps" runat="server" Text="No followups" 
                            CssClass="darkgrey-italic"></asp:Label>
                    </EmptyDataTemplate>
                    <SelectedRowStyle CssClass="selrow" />
                      <PagerSettings Mode="NextPreviousFirstLast" Position="Bottom" />
                </asp:GridView>
                
                <input type="hidden" id="summarySortCol" name="summarySortCol" runat="server" />
            </td>
        </tr>
        <tr style="line-height:0.5">
            <td class="grid-footer" colspan="2" style="line-height:0.5">
           <div id="pager" class="pager">
	<form >
		<img src="images/pager/first.png" class="first"/>
		<img src="images/pager/prev.png" class="prev"/>
		<input type="text" class="pagedisplay"/>
		<img src="images/pager/next.png" class="next"/>
		<img src="images/pager/last.png" class="last"/>
		<select class="pagesize">
			<option selected="selected"  value="2">2</option>
			<option value="10">10</option>
			<option value="30">30</option>
			<option  value="40">40</option>
		</select>
	</form>
</div>                
                <asp:Image ID="imgAll0" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" 
                    Visible="false" />
                <asp:LinkButton ID="lnkSeeAll0" runat="server" CommandArgument="0" 
                    onclick="lnkSeeAllSuspects_Click" Visible="false">see all</asp:LinkButton>
            </td>
        </tr>
        <tr style="height:30px">
            <td  colspan="2" >
            
                &nbsp;</td>
        </tr>
        <tr style="display:none">
            <td class="page-group" colspan="2">
                <asp:Label ID="lblIdentified" runat="server"></asp:Label>
            </td>
        </tr>
        <tr style="display:none">
            <td style="padding: 0px; margin: 0px" colspan="2">
                <asp:GridView ID="grvIdentified" runat="server" AutoGenerateColumns="False" 
                    CssClass="grid" 
                    onrowdatabound="grvIdentified_RowDataBound" ShowHeader="False" Width="100%" 
                    GridLines="Horizontal" AllowPaging="True" PageSize="3">
                    <PagerSettings Visible="False" />
                    <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkbSelectAll" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox type="checkbox" name="chkbOpprtunitySelect" ID="chkbOpprtunitySelect" class="cbSelectRow" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle  BorderStyle="None"/>
                    </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-BorderStyle="None">
                            <ItemTemplate>
                                <asp:Image ID="imgAvailable" runat="server" 
                                    ImageUrl="~/images/ArrowSolidSmall.gif" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ClientID" SortExpression="ClientID"  HeaderStyle-BorderStyle="None"
                            Visible="False" />
                        <asp:BoundField DataField="ClientName" SortExpression="ClientName"  HeaderStyle-BorderStyle="None" HeaderText="Client">
                        <ItemStyle Width="210px" />
                        </asp:BoundField>
                         <asp:BoundField DataField="StatusName" SortExpression="StatusName"  HeaderStyle-BorderStyle="None" HeaderText="Status" >
                        <ItemStyle Width="80px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-BorderStyle="None">
                            <ItemTemplate>
                                <asp:Image ID="imgFlagged" runat="server" 
                                    ImageUrl="~/images/OpportunityFlagged.gif" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-BorderStyle="None">
                            <ItemTemplate>
                                <asp:Image ID="imgBusinessType" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="18px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="OpportunityID" SortExpression="OpportunityID"  HeaderStyle-BorderStyle="None"
                            Visible="False" />
                        <asp:BoundField DataField="OpportunityName" SortExpression="OpportunityName"  HeaderStyle-BorderStyle="None" HeaderText="Opportunity">
                        <ItemStyle Width="230px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Added" DataFormatString="{0:dd-MMM-yy}" HeaderStyle-BorderStyle="None" HeaderText="Added"
                            SortExpression="Added" >
                        <ItemStyle Width="130px" />
                        </asp:BoundField>
                   
                        <asp:BoundField DataField="FollowUpDate"  HeaderText="FollowUp" HeaderStyle-BorderStyle="None"
                            DataFormatString="{0:dd-MMM-yy}" SortExpression="FollowUpDate" 
                            NullDisplayText="No Follow-up Date" >
                        <ItemStyle Width="140px" />
                        </asp:BoundField>
                             <asp:BoundField DataField="OpportunityDue" HeaderText="Renewal Date"  HeaderStyle-BorderStyle="None" DataFormatString="{0:dd-MMM-yy}" 
                            SortExpression="OpportunityDue" NullDisplayText="No Due Date" >
                        <ItemStyle Width="150px" />
                        </asp:BoundField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="lblNoFollowUps" runat="server" Text="No followups" 
                            CssClass="darkgrey-italic"></asp:Label>
                    </EmptyDataTemplate>
                    <SelectedRowStyle CssClass="selrow" />
                </asp:GridView>
            </td>
        </tr>
        <tr style="display:none">
            <td style: "margin=0" class="grid-footer" colspan="2">
                <asp:Image ID="imgAll1" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" />
                <asp:LinkButton ID="lnkSeeAll1" runat="server" CommandArgument="1" 
                    Height="18px" onclick="lnkSeeAllSuspects_Click">see all</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td style: "margin=0" class="page-group" colspan="2">
                <asp:Label ID="lblQualifiedIn" runat="server"></asp:Label>
                <br />
                <asp:Label ID="lblQualifyMatrix" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="padding: 0px; margin: 0px" colspan="2">
                <asp:GridView ID="grvQualifiedIn" runat="server" AutoGenerateColumns="False" 
                    CssClass="summarygrid"  ShowHeader="True" Width="100%" 
                    GridLines="Horizontal" AllowPaging="False" 
                    PageSize="3" onrowdatabound="grvQualifiedIn_RowDataBound" HeaderStyle-CssClass="grid-clientSideHeaderSorting">
                    <PagerSettings Visible="false" />
                  <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkbSelectAll" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox type="checkbox" name="chkbOpprtunitySelect" ID="chkbOpprtunitySelect" class="cbSelectRow" runat="server" oppoID='<%# DataBinder.Eval(Container.DataItem,"OpportunityID").ToString()%>' />
                        </ItemTemplate>
                        <HeaderStyle  BorderStyle="None"/>
                    </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-BorderStyle="None">
                            <ItemTemplate>
                                <asp:Image ID="imgAvailable" runat="server" 
                                    ImageUrl="~/images/ArrowSolidSmall.gif" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ClientID" SortExpression="ClientID"  HeaderStyle-BorderStyle="None"
                            Visible="False" />
                        <asp:BoundField DataField="ClientName" SortExpression="ClientName"  HeaderStyle-BorderStyle="None" HeaderText="Client">
                        <ItemStyle Width="210px" />
                        </asp:BoundField>
                         <asp:BoundField DataField="StatusName" SortExpression="StatusName"  HeaderStyle-BorderStyle="None" HeaderText="Status" >
                        <ItemStyle Width="80px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-BorderStyle="None">
                            <ItemTemplate>
                                <asp:Image ID="imgFlagged" runat="server" 
                                    ImageUrl="~/images/OpportunityFlagged.gif" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-BorderStyle="None">
                            <ItemTemplate>
                                <asp:Image ID="imgBusinessType" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="18px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="OpportunityID" SortExpression="OpportunityID"  HeaderStyle-BorderStyle="None"
                            Visible="False" />
                        <asp:BoundField DataField="OpportunityName" SortExpression="OpportunityName"  HeaderStyle-BorderStyle="None" HeaderText="Opportunity">
                        <ItemStyle Width="230px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SharedWithAccountExecutiveID" SortExpression="SharedWithAccountExecutiveID"  HeaderStyle-BorderStyle="None" HeaderText="Shared with">
                        <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Added" DataFormatString="{0:dd-MMM-yy}" HeaderStyle-BorderStyle="None" HeaderText="Added"
                            SortExpression="Added" >
                        <ItemStyle Width="130px" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="FollowUpDate"  HeaderText="FollowUp" HeaderStyle-BorderStyle="None"
                            DataFormatString="{0:dd-MMM-yy}" SortExpression="FollowUpDate" 
                            NullDisplayText="No Follow-up Date" >
                        <ItemStyle Width="140px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="OpportunityDue" HeaderText="Renewal Date"  HeaderStyle-BorderStyle="None" DataFormatString="{0:dd-MMM-yy}" 
                            SortExpression="OpportunityDue" NullDisplayText="No Due Date" >
                        <ItemStyle Width="150px" />
                        </asp:BoundField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="lblNoFollowUps" runat="server" Text="No followups" 
                            CssClass="darkgrey-italic"></asp:Label>
                    </EmptyDataTemplate>
                    
                

                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td class="grid-footer" colspan="2">
            <div id="pager" class="pager">
	<form>
		<img src="images/pager/first.png" class="first"/>
		<img src="images/pager/prev.png" class="prev"/>
		<input type="text" class="pagedisplay"/>
		<img src="images/pager/next.png" class="next"/>
		<img src="images/pager/last.png" class="last"/>
		<select class="pagesize">
			<option selected="selected"  value="2">2</option>
			<option value="10">10</option>
			<option value="30">30</option>
			<option  value="40">40</option>
		</select>
	</form>
</div>
                <asp:Image ID="imgAll2" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" Visible="false" />
                <asp:LinkButton ID="lnkSeeAll2" runat="server" CommandArgument="2" 
                    Height="18px" onclick="lnkSeeAllSuspects_Click" Visible="false">see all</asp:LinkButton>
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
                <asp:GridView ID="grvInterested" runat="server" AutoGenerateColumns="False" 
                    CssClass="summarygrid" ShowHeader="True" Width="100%" 
                    GridLines="Horizontal" AllowPaging="True" 
                    PageSize="3" onrowdatabound="grvInterested_RowDataBound" HeaderStyle-CssClass="grid-clientSideHeaderSorting">
                    <PagerSettings Visible="False" />
                    <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkbSelectAll" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox type="checkbox" name="chkbOpprtunitySelect" ID="chkbOpprtunitySelect" class="cbSelectRow" runat="server" oppoID='<%# DataBinder.Eval(Container.DataItem,"OpportunityID").ToString()%>' />
                        </ItemTemplate>
                        <HeaderStyle  BorderStyle="None"/>
                    </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-BorderStyle="None">
                            <ItemTemplate>
                                <asp:Image ID="imgAvailable" runat="server" 
                                    ImageUrl="~/images/ArrowSolidSmall.gif" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ClientID" SortExpression="ClientID"  HeaderStyle-BorderStyle="None"
                            Visible="False" />
                        <asp:BoundField DataField="ClientName" SortExpression="ClientName"  HeaderStyle-BorderStyle="None" HeaderText="Client">
                        <ItemStyle Width="210px" />
                        </asp:BoundField>
                         <asp:BoundField DataField="StatusName" SortExpression="StatusName"  HeaderStyle-BorderStyle="None" HeaderText="Status" >
                        <ItemStyle Width="80px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-BorderStyle="None">
                            <ItemTemplate>
                                <asp:Image ID="imgFlagged" runat="server" 
                                    ImageUrl="~/images/OpportunityFlagged.gif" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-BorderStyle="None">
                            <ItemTemplate>
                                <asp:Image ID="imgBusinessType" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="18px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="OpportunityID" SortExpression="OpportunityID"  HeaderStyle-BorderStyle="None"
                            Visible="False" />
                        <asp:BoundField DataField="OpportunityName" SortExpression="OpportunityName"  HeaderStyle-BorderStyle="None" HeaderText="Opportunity">
                        <ItemStyle Width="230px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SharedWithAccountExecutiveID" SortExpression="SharedWithAccountExecutiveID"  HeaderStyle-BorderStyle="None" HeaderText="Shared with">
                        <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Added" DataFormatString="{0:dd-MMM-yy}" HeaderStyle-BorderStyle="None" HeaderText="Added"
                            SortExpression="Added" >
                        <ItemStyle Width="130px" />
                        </asp:BoundField>
                       
                        <asp:BoundField DataField="FollowUpDate"  HeaderText="FollowUp" HeaderStyle-BorderStyle="None"
                            DataFormatString="{0:dd-MMM-yy}" SortExpression="FollowUpDate" 
                            NullDisplayText="No Follow-up Date" >
                        <ItemStyle Width="140px" />
                        </asp:BoundField>
                         <asp:BoundField DataField="OpportunityDue" HeaderText="Renewal Date"  HeaderStyle-BorderStyle="None" DataFormatString="{0:dd-MMM-yy}" 
                            SortExpression="OpportunityDue" NullDisplayText="No Due Date" >
                        <ItemStyle Width="150px" />
                        </asp:BoundField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="lblNoFollowUps" runat="server" Text="No followups" 
                            CssClass="darkgrey-italic"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td class="grid-footer" colspan="2">
             <div id="Div2" class="pager">
	<form>
		<img src="images/pager/first.png" class="first"/>
		<img src="images/pager/prev.png" class="prev"/>
		<input type="text" class="pagedisplay"/>
		<img src="images/pager/next.png" class="next"/>
		<img src="images/pager/last.png" class="last"/>
		<select class="pagesize">
			<option selected="selected"  value="2">2</option>
			<option value="10">10</option>
			<option value="30">30</option>
			<option  value="40">40</option>
		</select>
	</form>
</div>
                <asp:Image ID="imgAll3" runat="server" ImageUrl="~/images/ArrowSolidSmall.gif" Visible="false" />
                <asp:LinkButton ID="lnkSeeAll3" runat="server" CommandArgument="3" 
                    Height="18px" onclick="lnkSeeAllSuspects_Click" Visible="false">see all</asp:LinkButton>
            </td>
        </tr>
        <tr style="display:none">
            <td class="page-group" colspan="2">
                <asp:Label ID="lblGoToMarket" runat="server"></asp:Label>
            </td>
        </tr>
        <tr style="display:none"> 
            <td style="padding: 0px; margin: 0px" colspan="2">
                <asp:GridView ID="grvGoToMarket" runat="server" AutoGenerateColumns="False" 
                    CssClass="grid" ShowHeader="False" Width="100%" 
                    GridLines="Horizontal" AllowPaging="True" 
                    PageSize="3" onrowdatabound="grvGoToMarket_RowDataBound">
                    <PagerSettings Visible="False" />
                    <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkbSelectAll" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox type="checkbox" name="chkbOpprtunitySelect" ID="chkbOpprtunitySelect" class="cbSelectRow" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle  BorderStyle="None"/>
                    </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Image ID="imgAvailable" runat="server" 
                                    ImageUrl="~/images/ArrowSolidSmall.gif" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ClientID" SortExpression="ClientID" 
                            Visible="False" />
                        <asp:BoundField DataField="ClientName" SortExpression="ClientName" >
                        <ItemStyle Width="230px" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Image ID="imgFlagged" runat="server" 
                                    ImageUrl="~/images/OpportunityFlagged.gif" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Image ID="imgBusinessType" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="OpportunityID" SortExpression="OpportunityID" 
                            Visible="False" />
                        <asp:BoundField DataField="OpportunityName" SortExpression="OpportunityName" >
                        <ItemStyle Width="230px" />
                        </asp:BoundField>
                      
                        <asp:BoundField DataField="FollowUpDate" 
                            DataFormatString="Follow-up: {0:dd-MMM-yy}" SortExpression="FollowUpDate" 
                            NullDisplayText="No Follow-up Date" >
                        <ItemStyle Width="150px" />
                        </asp:BoundField>
                          <asp:BoundField DataField="OpportunityDue" DataFormatString="Due: {0:dd-MMM-yy}" 
                            SortExpression="OpportunityDue" HeaderText="OpportunityDue" 
                            NullDisplayText="No Due Date" >
                        <ItemStyle Width="150px" />
                        </asp:BoundField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="lblNoFollowUps" runat="server" Text="No followups" 
                            CssClass="darkgrey-italic"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr style="display:none">
            <td colspan="2" class="grid-footer">
                <asp:Image ID="imgAll4" runat="server" 
                    ImageUrl="~/images/ArrowSolidSmall.gif" />
                <asp:LinkButton ID="lnkSeeAll4" runat="server" CommandArgument="4" 
                    Height="18px" onclick="lnkSeeAllSuspects_Click">see all</asp:LinkButton>
            </td>
        </tr>
        <tr style="display:none">
            <td colspan="2" class="page-group">
                <asp:Label ID="lblQuoted" runat="server"></asp:Label>
            </td>
        </tr>
        <tr style="display:none">
            <td style="padding: 0px; margin: 0px" colspan="2">
                <asp:GridView ID="grvQuoted" runat="server" AutoGenerateColumns="False" 
                    CssClass="grid" ShowHeader="False" Width="100%" 
                    GridLines="Horizontal" AllowPaging="True" 
                    PageSize="3" onrowdatabound="grvQuoted_RowDataBound">
                    <PagerSettings Visible="False" />
                    <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkbSelectAll" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox type="checkbox" name="chkbOpprtunitySelect" ID="chkbOpprtunitySelect" class="cbSelectRow" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle  BorderStyle="None"/>
                    </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Image ID="imgAvailable" runat="server" 
                                    ImageUrl="~/images/ArrowSolidSmall.gif" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ClientID" SortExpression="ClientID" 
                            Visible="False" />
                        <asp:BoundField DataField="ClientName" SortExpression="ClientName" >
                        <ItemStyle Width="230px" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Image ID="imgFlagged" runat="server" 
                                    ImageUrl="~/images/OpportunityFlagged.gif" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Image ID="imgBusinessType" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="OpportunityID" SortExpression="OpportunityID" 
                            Visible="False" />
                        <asp:BoundField DataField="OpportunityName" SortExpression="OpportunityName" >
                        <ItemStyle Width="230px" />
                        </asp:BoundField>
                 
                        <asp:BoundField DataField="FollowUpDate" 
                            DataFormatString="Follow-up: {0:dd-MMM-yy}" SortExpression="FollowUpDate" 
                            NullDisplayText="No Follow-up Date" >
                        <ItemStyle Width="150px" />
                        </asp:BoundField>
                               <asp:BoundField DataField="OpportunityDue" DataFormatString="Due: {0:dd-MMM-yy}" 
                            SortExpression="OpportunityDue" HeaderText="OpportunityDue" 
                            NullDisplayText="No Due Date" >
                        <ItemStyle Width="150px" />
                        </asp:BoundField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="lblNoFollowUps" runat="server" Text="No followups" 
                            CssClass="darkgrey-italic"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr style="display:none">
            <td colspan="2" class="grid-footer">
                <asp:Image ID="imgAll5" runat="server" 
                    ImageUrl="~/images/ArrowSolidSmall.gif" />
                <asp:LinkButton ID="lnkSeeAll5" runat="server" CommandArgument="5" 
                    Height="18px" onclick="lnkSeeAllSuspects_Click">see all</asp:LinkButton>
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
                <asp:GridView ID="grvAccepted" runat="server" AutoGenerateColumns="False" 
                    CssClass="summarygrid" ShowHeader="True" Width="100%" 
                    GridLines="Horizontal" AllowPaging="True" 
                    PageSize="3" onrowdatabound="grvAccepted_RowDataBound" HeaderStyle-CssClass="grid-clientSideHeaderSorting">
                    <PagerSettings Visible="False" />
                    <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkbSelectAll" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox type="checkbox" name="chkbOpprtunitySelect" ID="chkbOpprtunitySelect" class="cbSelectRow" runat="server" oppoID='<%# DataBinder.Eval(Container.DataItem,"OpportunityID").ToString()%>' />
                        </ItemTemplate>
                        <HeaderStyle  BorderStyle="None"/>
                    </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-BorderStyle="None">
                            <ItemTemplate>
                                <asp:Image ID="imgAvailable" runat="server" 
                                    ImageUrl="~/images/ArrowSolidSmall.gif" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ClientID" SortExpression="ClientID"  HeaderStyle-BorderStyle="None"
                            Visible="False" />
                        <asp:BoundField DataField="ClientName" SortExpression="ClientName"  HeaderStyle-BorderStyle="None" HeaderText="Client">
                        <ItemStyle Width="210px" />
                        </asp:BoundField>
                         <asp:BoundField DataField="StatusName" SortExpression="StatusName"  HeaderStyle-BorderStyle="None" HeaderText="Status" >
                        <ItemStyle Width="80px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-BorderStyle="None">
                            <ItemTemplate>
                                <asp:Image ID="imgFlagged" runat="server" 
                                    ImageUrl="~/images/OpportunityFlagged.gif" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-BorderStyle="None">
                            <ItemTemplate>
                                <asp:Image ID="imgBusinessType" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="18px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="OpportunityID" SortExpression="OpportunityID"  HeaderStyle-BorderStyle="None"
                            Visible="False" />
                        <asp:BoundField DataField="OpportunityName" SortExpression="OpportunityName"  HeaderStyle-BorderStyle="None" HeaderText="Opportunity">
                        <ItemStyle Width="230px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SharedWithAccountExecutiveID" SortExpression="SharedWithAccountExecutiveID"  HeaderStyle-BorderStyle="None" HeaderText="Shared with">
                        <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Added" DataFormatString="{0:dd-MMM-yy}" HeaderStyle-BorderStyle="None" HeaderText="Added"
                            SortExpression="Added" >
                        <ItemStyle Width="130px" />
                        </asp:BoundField>
     
                        <asp:BoundField DataField="FollowUpDate"  HeaderText="FollowUp" HeaderStyle-BorderStyle="None"
                            DataFormatString="{0:dd-MMM-yy}" SortExpression="FollowUpDate" 
                            NullDisplayText="No Follow-up Date" >
                        <ItemStyle Width="140px" />
                        </asp:BoundField>
                                           <asp:BoundField DataField="OpportunityDue" HeaderText="Renewal Date"  HeaderStyle-BorderStyle="None" DataFormatString="{0:dd-MMM-yy}" 
                            SortExpression="OpportunityDue" NullDisplayText="No Due Date" >
                        <ItemStyle Width="150px" />
                        </asp:BoundField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="lblNoFollowUps" runat="server" Text="No followups" 
                            CssClass="darkgrey-italic"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="grid-footer">
                        <div id="Div1" class="pager">
	<form>
		<img src="images/pager/first.png" class="first"/>
		<img src="images/pager/prev.png" class="prev"/>
		<input type="text" class="pagedisplay"/>
		<img src="images/pager/next.png" class="next"/>
		<img src="images/pager/last.png" class="last"/>
		<select class="pagesize">
			<option selected="selected"  value="2">2</option>
			<option value="10">10</option>
			<option value="30">30</option>
			<option  value="40">40</option>
		</select>
	</form>
</div>
                <asp:Image ID="imgAll6" runat="server" 
                    ImageUrl="~/images/ArrowSolidSmall.gif" Visible="false" />
                <asp:LinkButton ID="lnkSeeAll6" runat="server" CommandArgument="6" 
                    Height="18px" onclick="lnkSeeAllSuspects_Click" Visible="false">see all</asp:LinkButton>
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
