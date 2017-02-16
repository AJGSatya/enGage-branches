<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EditClient.aspx.cs"
    EnableEventValidation="false" Inherits="enGage.Web.EditClient" Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="Controls/ucAUPSS.ascx" TagName="ucAUPSS" TagPrefix="uc3" %>
<%@ Register Src="Controls/ucMessanger.ascx" TagName="ucMessanger" TagPrefix="uc1" %>
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
    
    </script>

    <div class="page">
        <table class="page-table" style="width: 100%;" id="tblDetails" runat="server">
            <tr>
                <td colspan="5">
                    <asp:ScriptManager ID="scriptManagerId" runat="server">
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
                <td width="18px">
                    <asp:Image ID="imgArrow" runat="server" ImageUrl="~/images/ArrowHollowSmall.gif" />
                </td>
                <td colspan="2" width="800px">
                    <asp:Label ID="lblClientName" runat="server" CssClass="page-title"></asp:Label>
                </td>
                <td class="page-label" width="50px">
                    Phone:
                    </td>
                <td class="page-label" width="200px">
                    <asp:Label ID="lblOfficePhone" runat="server" CssClass="page-title"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="underline">
                    &nbsp;
                </td>
                <td colspan="2" class="underline">
                    <asp:Label ID="lblAddress" runat="server"></asp:Label>
                </td>
                <td class="underline">
                    <asp:Label ID="lblFaxLabel" runat="server" CssClass="page-label" Text="Fax:"></asp:Label>
                    </td>
                <td class="underline">
                    <asp:Label ID="lblOfficeFacsimilie" runat="server" CssClass="page-text"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td width="110px">
                    Client Name:
                </td>
                <td>
                    <asp:TextBox ID="txtClientName" runat="server" MaxLength="250" Width="400px" CssClass="control"></asp:TextBox>
                </td>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    Registered Name:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtRegisteredName" runat="server" MaxLength="250" Width="400px"
                        CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    ABN/ACN:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtABNACN" runat="server" MaxLength="20" Width="200px" CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="underline">
                    &nbsp;
                </td>
                <td class="underline">
                    Insured as:
                </td>
                <td class="underline" colspan="3">
                    <asp:TextBox ID="txtInsuredName" runat="server" MaxLength="250" Width="400px" CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="underline">
                    &nbsp;
                </td>
                <td class="underline">
                    Source:
                </td>
                <td class="underline" colspan="3">
                    <asp:TextBox ID="txtSource" runat="server" MaxLength="150" Width="400px" CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    Address Type:
                </td>
                <td colspan="3">
                    <asp:RadioButtonList ID="rblAddressTypes" runat="server" RepeatDirection="Horizontal"
                        onclick="set_address_controls()">
                        <asp:ListItem Selected="True">Australia</asp:ListItem>
                        <asp:ListItem>Overseas</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    Address:
                    <br />
                    <br />
                    <br />
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="control" Height="55px" TextMode="MultiLine"
                        MaxLength="250" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trAUPSS" runat="server">
                <td>
                    &nbsp;
                </td>
                <td>
                    Location:
                </td>
                <td colspan="3">
                    <uc3:ucAUPSS ID="ucAUPSS1" CssClass="control" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    Phone:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtOfficePhone" runat="server" MaxLength="20" Width="200px" CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="underline">
                    &nbsp;
                </td>
                <td class="underline">
                    Fax:
                </td>
                <td class="underline" colspan="3">
                    <asp:TextBox ID="txtOfficeFacsimilie" runat="server" MaxLength="20" Width="200px"
                        CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="padding: 0px; margin: 0px; width:100%;" colspan="5">
                    <asp:UpdatePanel ID="uplIndustry" runat="server">
                        <ContentTemplate>
                            <table class="page-table" style="width: 100%;">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        Industry:
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                    </td>
                                    <td colspan="1" width="635px">
                                        <asp:TextBox ID="txtFindIndustry" runat="server" CssClass="control" Width="400px"></asp:TextBox>
                                        <asp:Button ID="btnFindIndustry" runat="server" OnClick="btnFindIndustry_Click" Text="Find"
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
                                        &nbsp;
                                    </td>
                                    <td>
                                        Association:
                                    </td>
                                    <td colspan="1">
                                        <asp:DropDownList ID="ddlAssociation" runat="server" CssClass="control">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    Membership:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtAssociationMemberNumber" runat="server" MaxLength="50" Width="200px"
                        CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="underline">
                    &nbsp;
                </td>
                <td class="underline">
                    OAMPS Code:
                </td>
                <td class="underline" colspan="3">
                    <asp:TextBox ID="txtClientCode" runat="server" MaxLength="50" Width="200px" CssClass="control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="page-label">
                    &nbsp;
                </td>
                <td class="page-label">
                    Account Executive:
                </td>
                <td class="page-text" colspan="3">
                    <asp:Label ID="lblAccountExecutive" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="5" class="right-aligned">
                    <asp:Button ID="btnCancel" runat="server" CssClass="button-action" 
                        Text="Cancel" OnClick="btnCancel_Click"
                        
                        OnClientClick="return confirm('Do you really want to cancel editing this client?');" />
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" 
                        CssClass="button-action" />
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
