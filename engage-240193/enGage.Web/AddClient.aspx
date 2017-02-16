<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AddClient.aspx.cs"
    EnableEventValidation="false" Inherits="enGage.Web.AddClient" Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ucAUPSS.ascx" TagName="ucAUPSS" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">

        function getAddressClientId() {
            return '<%= this.AddressClientId %>';
        }
        function getSuburbClientId() {
            return '<%= this.SuburbClientId %>';
        }
        function getPostCodeClientId() {
            return '<%= this.PostCodeClientId %>';
        }

        function set_address_controls() {
            var radio = document.getElementById("<%=rblAddressTypes.ClientID %>" + "_0");
            if (radio.checked == true) {
                document.getElementById("<%=trAUPSS.ClientID %>").style.display = '';
            }
            else {
                document.getElementById("<%=trAUPSS.ClientID %>").style.display = 'none';
            }
        }

        function cancelation() {

            var r = confirm('Do you really want to cancel editing this client?')
            if (r == true) {
                history.back();
            }
        }


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
        <table style="width: 100%;" id="tblDetails" class="page-table" runat="server">
            <tr>
                <td colspan="5">
                    <asp:ScriptManager runat="server" ID="scriptManagerId">
                        <Scripts>
                            <asp:ScriptReference Path="../scripts/CallWebServiceMethods.js" />
                        </Scripts>
                    </asp:ScriptManager>
                </td>
            </tr>
            <tr>
                <td class="page-group" colspan="5">
                    Client details
                </td>
            </tr>
            <tr>
                <td width="170">
                    Client Name<span style="color: #FF0000">*</span>:
                </td>
                <td colspan="4" width="610px">
                    <asp:TextBox ID="txtClientName" runat="server" MaxLength="250" Width="400px" CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr hideclientside>
                <td>
                    Registered Name:&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtRegisteredName" runat="server" MaxLength="250" Width="400px"
                        CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr hideclientside>
                <td>
                    ABN/ACN:
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtABNACN" runat="server" MaxLength="20" Width="200px" CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr hideclientside>
                <td class="underline">
                    Insured as<span style="color: #FF0000">*</span>:
                </td>
                <td colspan="4" class="underline">
                    <asp:TextBox ID="txtInsuredName" runat="server" MaxLength="250" Width="400px" CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="underline">
                    Source<span style="color: #FF0000">*</span>:
                </td>
                <td colspan="4" class="underline">
                    <asp:TextBox ID="txtSource" runat="server" MaxLength="150" Width="400px" CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr hideclientside>
                <td>
                    Address Type:
                </td>
                <td colspan="4">
                    <asp:RadioButtonList ID="rblAddressTypes" runat="server" RepeatDirection="Horizontal"
                        onclick="set_address_controls()" CssClass="control">
                        <asp:ListItem Selected="True">Australia</asp:ListItem>
                        <asp:ListItem>Overseas</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    Address:
                    <br />
                    <br />
                    <br />
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="control" Height="55px" TextMode="MultiLine"
                        MaxLength="250" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trAUPSS" runat="server">
                <td>
                    Postcode:
                </td>
                <td colspan="4">
                    <uc3:ucAUPSS ID="ucAUPSS1" runat="server" CssClass="control" />
                </td>
            </tr>
            <tr>
                <td>
                    Phone<span style="color: #FF0000">*</span>:
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtOfficePhone" runat="server" MaxLength="20" Width="200px" CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="underline">
                    Fax:
                </td>
                <td colspan="4" class="underline">
                    <asp:TextBox ID="txtOfficeFacsimilie" runat="server" MaxLength="20" Width="200px"
                        CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="padding: 0px; margin: 0px" colspan="5">
                    <asp:UpdatePanel ID="uplIndustry" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="PnlClick" runat="server" Height="20px" CssClass="panel-header" >
                                <div style="float: left; vertical-align: middle; height: 20px">
                                    <asp:Image ID="Imgclick" runat="server" ImageUrl="~/Images/expand.png" /></div>
                                <div style="float: left; font-weight: bold ;vertical-align: middle;">
                                    
                                        Industry
                                </div>
                               
                            </asp:Panel>
                            <asp:Panel ID="pBody" runat="server" CssClass="cpBody">
                                <table style="width: 100%;">
                                    <tr>
                                        <td>
                                            Industry:
                                        </td>
                                        <td width="604px">
                                            <asp:TextBox ID="txtFindIndustry" runat="server" Width="400px" CssClass="control"></asp:TextBox>
                                            <asp:Button ID="btnFindIndustry" runat="server" OnClick="btnFindIndustry_Click" Text="Look-up"
                                                CssClass="button-action" />
                                            <asp:Label ID="lblFoundIndustries" runat="server"></asp:Label>
                                            <br />
                                            <asp:ListBox ID="lstIndustry" runat="server" Visible="False" AutoPostBack="True"
                                                OnSelectedIndexChanged="lstIndustry_SelectedIndexChanged" CssClass="control">
                                            </asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Association:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAssociation" runat="server" CssClass="control">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Membership:
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtAssociationMemberNumber" runat="server" MaxLength="50" Width="200px"
                                                CssClass="control"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" CollapseControlID="pnlClick"
                                Collapsed="true" CollapsedText="Industry" ExpandControlID="pnlClick" TextLabelID="lblMessage"
                                ExpandedText="Industry" ImageControlID="Imgclick" CollapsedImage="~/Images/expand.png"
                                ExpandedImage="~/Images/collapse.png" ExpandDirection="Vertical" TargetControlID="pBody"
                                ScrollContents="false">
                            </cc1:CollapsiblePanelExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="underline">
                    OAMPS Code:
                </td>
                <td colspan="4" class="underline">
                    <asp:TextBox ID="txtClientCode" runat="server" MaxLength="50" Width="200px" CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="padding: 0px; margin: 0px" class="underline" colspan="5">
                    <asp:UpdatePanel ID="uplContact" runat="server">
                        <ContentTemplate>
                            <asp:CheckBox ID="ckbAddContact" runat="server" Text="Add Contact" AutoPostBack="True"
                                CssClass="control" OnCheckedChanged="ckbAddContact_CheckedChanged" />
                            <table id="tblContact" class="page-table" width="100%" runat="server" visible="False">
                                <tr>
                                    <td>
                                        Salutation:
                                    </td>
                                    <td width="606px">
                                        <asp:DropDownList ID="ddlTitle" runat="server" CssClass="control">
                                            <asp:ListItem Selected="True" Value="">--Please Select--</asp:ListItem>
                                            <asp:ListItem>Mr.</asp:ListItem>
                                            <asp:ListItem>Mrs.</asp:ListItem>
                                            <asp:ListItem>Ms.</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Contact Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtContactName" runat="server" MaxLength="50" Width="250px" CssClass="control"
                                            ToolTip="name"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Email:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Width="250px" CssClass="control"
                                            ToolTip="name@companyname.com.au"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Mobile:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMobile" runat="server" Width="150px" CssClass="control" ToolTip="0n nnnn nnnn"
                                            MaxLength="25"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Direct Line:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDirectLine" runat="server" MaxLength="25" Width="150px" CssClass="control"
                                            ToolTip="04nn nnn nnn"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                            
                            <td style="padding: 0px; margin: 0px; " colspan="5">          
                               
                                <div  class="panel-header" style="height:20px;font-weight: bold ;vertical-align: middle;">
                                    
                                        Opportunity
                                </div>

                            </td> 
            </tr>
            <tr>
                <td>
                    Opportunity<span style="color: #FF0000">*</span>:
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtOpportunityName" runat="server" MaxLength="250" Width="300px"
                        CssClass="control"></asp:TextBox>
                </td>
                <td width="100px">
                    Renewal Date:
                </td>
                <td width="120px">
                    <asp:TextBox ID="txtOpportunityDue" runat="server" Width="72px" MaxLength="10" CssClass="control"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtOpportunityDue_CalendarExtender" runat="server" PopupButtonID="btnOpportunityDue"
                        TargetControlID="txtOpportunityDue" Format="dd/MM/yyyy" FirstDayOfWeek="Monday">
                    </cc1:CalendarExtender>
                    <asp:ImageButton ID="btnOpportunityDue" runat="server" ImageUrl="~/images/Calendar.gif"
                        TabIndex="-1" style="vertical-align:middle"  />
                </td>
            </tr>
            <tr>
                <td>
                    Opportunity Type:
                </td>
                <td width="147px">
                    <asp:DropDownList ID="ddlBusinessType" runat="server" CssClass="control" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlBusinessType_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td  width="200px">
                    <asp:DropDownList hideclientside ID="ddlFlagged" runat="server" CssClass="control">
                        <asp:ListItem Value="true">Flagged</asp:ListItem>
                        <asp:ListItem Value="false">Not flagged</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td >
                    Follow up:
                </td>
                <td>
                    <asp:TextBox ID="txtFollowUpDate" runat="server" Width="72px" CssClass="control"
                        MaxLength="10"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtFollowUpDate_CalendarExtender" runat="server" Enabled="True"
                        Format="dd/MM/yyyy" PopupButtonID="btnFollowUpDate" TargetControlID="txtFollowUpDate"
                        FirstDayOfWeek="Monday">
                    </cc1:CalendarExtender>
                    <asp:ImageButton ID="btnFollowUpDate" runat="server" ImageUrl="~/images/Calendar.gif"
                        TabIndex="-1" style="vertical-align:middle" />
                </td>
            </tr>
            <tr hideclientside>
                <td>
                    CSS Segment:
                </td>
                <td colspan="4">
                    <asp:DropDownList ID="ddlClassification" runat="server" CssClass="control">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr hideclientside>
                <td>
                    Incumbent Broker:
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtIncumbentBroker" runat="server" Width="200px" CssClass="control"
                        MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr hideclientside>
                <td class="underline">
                    Incumbent Insurer:
                </td>
                <td colspan="4" class="underline">
                    <asp:TextBox ID="txtIncumbentInsurer" runat="server" Width="200px" CssClass="control"
                        MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            
            <tr id="trQuickWin1" hideclientside>
                <td>
                    Broking Income Quoted ($):
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtNetBrokerageQuoted" runat="server" MaxLength="10" CssClass="control"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="txtNetBrokerageQuoted_FilteredTextBoxExtender" runat="server"
                        Enabled="True" TargetControlID="txtNetBrokerageQuoted" ValidChars=",.0123456789">
                    </cc1:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr id="trQuickWin2" hideclientside>
                <td class="underline">
                      Actual Broking Income ($):
                </td>
                <td colspan="4" class="underline">
                    <asp:TextBox ID="txtNetBrokerageActual" runat="server" MaxLength="10" CssClass="control"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="txtNetBrokerageActual_FilteredTextBoxExtender" runat="server"
                        Enabled="True" TargetControlID="txtNetBrokerageActual" ValidChars=",.0123456789">
                    </cc1:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr id="trActivityNote">
                <td class="underline">
                    Activity Note:
                </td>
                <td colspan="4" class="underline">
                    <asp:TextBox ID="txtActivityNote" runat="server" CssClass="control" Height="72px"
                        TextMode="MultiLine" Width="580px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="page-label">
                    Account Executive:
                </td>
                <td class="page-text" colspan="4">
                    <asp:Label ID="lblAccountExecutive" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="5" class="right-aligned">
                    <input id="btnBack" type="button" value="Cancel" onclick="cancelation();" class="button-action" />
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="button-action" />
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <uc1:ucMessanger ID="ucMessanger1" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
