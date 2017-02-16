using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using enGage.DL;
using enGage.BL;

namespace enGage.Web
{
    public partial class ViewClient : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    if (Session["USER"] == null)
                    {
                        bizMessage bizM = new bizMessage();
                        this.ucMessanger1.ProcessMessage("Session: " + bizM.GetMessageText("SessionMissing"), Enums.enMsgType.Err, "", null, true);
                        return;
                    }
                }
                ((Main)Master).HeaderTitle = "Work with client";
                PopulateClientDetails();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void PopulateClientDetails()
        {
            bizMessage bizM = new bizMessage();
            bizSetting bizS = new bizSetting();

            bizClient biz = new bizClient();
            Client c;
            c = biz.GetClient(Convert.ToInt32(Request.QueryString["cid"]));
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);

            if (c == null) return;

            //general
            this.lblClientCode.Text = c.ClientCode;
            this.lblClientName.Text = c.ClientName;
            this.lblRegisteredName.Text = c.RegisteredName;
            this.lblInsuredName.Text = c.InsuredName;
            if (c.ABNACN != null) this.lblABNACN.Text = c.ABNACN;
            this.lblSource.Text = c.Source;
            this.lblOfficeFacsimilie.Text = c.OfficeFacsimilie;
            this.lblOfficePhone.Text = c.OfficePhone;

            //address
            if (c.Address == null)
            {
                this.lblAddress.Text = "no address";
                this.lblAddress.CssClass = "page-text-nodata";
            }
            else
            {
                this.lblAddress.Text = c.Address + ", " + c.Location + " " + c.StateCode + " " + c.PostCode;
            }
            
            //industry
            if (c.AnzsicCode != null)
            {
                this.lblIndustry.Text = c.Industry.IndustryName + " (" + c.Industry.AnzsicCode + ")";
            }
            if (c.AssociationCode != null)
            {
                this.lblAssociationName.Text = c.Association.AssociationName;
                this.lblAssociationMemberNumber.Text = c.AssociationMemberNumber;
            }

            //executive
            var u = bizUser.GetSMIAccountExecutiveIdBOAMPSUserName(c.AccountExecutiveID);
            bizUser.enGageUser exec = bizUser.GetAccountExecutive(u);
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            if (exec != null)
            {
                this.lblAccountExecutive.Text = "<b>" + exec.DisplayName + "</b>" + ", " + exec.Branch + " (" + exec.Region + ")";
            }
            else
            {
                this.lblAccountExecutive.Text = "<b>" + c.AccountExecutiveID + "</b>" + ", Unknown Branch (Unknown Region)";
            }
            //contacts
            bizContact bizC = new bizContact();
            List<Contact> cons = c.Contacts.Where(co => co.Inactive == false).ToList();
            this.grvContacts.DataSource = cons;
            this.ucMessanger1.ProcessMessages(bizC.MSGS, false);
            this.grvContacts.DataBind();
            this.lblActiveContacts.Text = cons.Count.ToString();
            this.lblInactiveContacts.Text = (c.Contacts.Count - cons.Count).ToString();

            //opportunities
            List<sp_web_ListClientOpenOpportunitiesResult> oppos = biz.ListClientOpenOpportunities(c.ClientID);
            this.grvOpportunities.DataSource = oppos;
            this.ucMessanger1.ProcessMessages(bizC.MSGS, false);
            this.grvOpportunities.DataBind();
            this.lblOpenOpportunities.Text = oppos.Count.ToString();
            List<sp_web_ListClientClosedOpportunitiesResult> coppos = biz.ListClientClosedOpportunities(c.ClientID);
            this.lblClosedOpportunities.Text = (coppos.Count).ToString();

            //audit
            ((Main)Master).HeaderDetails = "Client added by "
                                                + bizActiveDirectory.GetUserFullName(c.AddedBy)
                                                + " (" + string.Format("{0:dd-MMM-yy}", c.Added) + ")";

            if (c.Modified.HasValue == true)
                ((Main)Master).HeaderDetails += " / modified by " 
                    + bizActiveDirectory.GetUserFullName(c.ModifiedBy)
                    + " (" + string.Format("{0:dd-MMM-yy}", c.Modified.Value) + ")";

            //buttons
            this.hplContactsSeeAll.NavigateUrl = "ClientContactsAll.aspx?cid=" + c.ClientID.ToString();
            if (c.Contacts.Count - cons.Count == 0) this.hplContactsSeeAll.Enabled = false;
            this.hplOpportunitiesSeeAll.NavigateUrl = "ClientOpportunitiesAll.aspx?cid=" + c.ClientID.ToString();
            if (coppos.Count == 0) this.hplOpportunitiesSeeAll.Enabled = false;
            this.btnAddContact.PostBackUrl = "AddContact.aspx?cid=" + c.ClientID.ToString();
            this.btnAddOpportunity.PostBackUrl = "AddOpportunity.aspx?cid=" + c.ClientID.ToString();
            if (c.Inactive == true)
            {
                this.lblClientName.Enabled = false;
                this.lblOfficePhone.Enabled = false;
                this.lblAddress.Enabled = false;
                this.lblOfficeFacsimilie.Enabled = false;
                this.btnAddContact.Enabled = false;
                this.btnAddOpportunity.Enabled = false;
                this.btnActiveInactive.Text = "Activate";
                this.btnEditClient.Enabled = false;
            }
            else
            {
                this.lblClientName.Enabled = true;
                this.lblOfficePhone.Enabled = true;
                this.lblAddress.Enabled = true;
                this.lblOfficeFacsimilie.Enabled = true;
                this.btnAddContact.Enabled = true;
                this.btnAddOpportunity.Enabled = true;
                this.btnActiveInactive.Text = "Inactivate";
                this.btnEditClient.Enabled = true;
            }
            this.btnEditClient.PostBackUrl = "EditClient.aspx?cid=" + c.ClientID.ToString();
            this.btnTransfer.PostBackUrl = "TransferClient.aspx?cid=" + c.ClientID.ToString();

            // SECURITY
            //sp_web_GetUserByIDResult exec = biz.GetAccountExecutive(c.AccountExecutiveID);
            //this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
            switch (user.Role)
            {
                case (int)Enums.enUserRole.Executive:
                    if (user.Branch == exec.Branch)
                    {
                        if (exec == null)
                        {

                        }else if (user.DisplayName != exec.DisplayName)
                        {
                            DisableButtons();
                            DisableGrids();
                        }
                    }
                    else
                    {
                        Response.Redirect("~/FindClient.aspx", false);
                        return;
                    }
                    break;
                case (int)Enums.enUserRole.Branch:
                    if (exec == null)
                    {

                    }
                    else if (user.Branch == exec.Branch)
                    {
                        if (user.DisplayName != exec.DisplayName)
                        {
                            //DisableButtons();
                            //DisableGrids();
                        }
                    }
                    else
                    {
                        Response.Redirect("~/FindClient.aspx", false);
                        return;
                    }
                    break;
                case (int)Enums.enUserRole.Region:
                    if (exec == null)
                    {
                        DisableButtons();
                        DisableGrids();
                    }
                    else if (user.Region == exec.Region)
                    {
                        if (user.DisplayName != exec.DisplayName)
                        {
                            DisableButtons();
                            DisableGrids();
                        }
                    }
                    else
                    {
                        Response.Redirect("~/FindClient.aspx", false);
                        return;
                    }
                    break;
                case (int)Enums.enUserRole.Company:
                    if (exec == null)
                    {
                        DisableButtons();
                        DisableGrids();
                    }
                    else if (user.DisplayName != exec.DisplayName)
                    {
                        DisableButtons();
                        DisableGrids();
                    }
                    break;
                case (int)Enums.enUserRole.Administrator:
                    // full access
                    break;
            }
        }

        private void DisableButtons()
        {
            this.btnAddContact.Enabled = false;
            this.btnAddOpportunity.Enabled = false;
            this.btnTransfer.Enabled = false;
            this.btnActiveInactive.Enabled = false;
            this.btnEditClient.Enabled = false;
        }

        private void DisableGrids()
        {
            foreach (GridViewRow r in this.grvContacts.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow || r.RowType == DataControlRowType.EmptyDataRow)
                {
                    r.Attributes["OnMouseOver"] = null;
                    r.Attributes["OnMouseOut"] = null;
                    r.Attributes["onClick"] = null;
                }
            }
            this.hplContactsSeeAll.Enabled = false;
            foreach (GridViewRow r in this.grvOpportunities.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow || r.RowType == DataControlRowType.EmptyDataRow)
                {
                    r.Attributes["OnMouseOver"] = null;
                    r.Attributes["OnMouseOut"] = null;
                    r.Attributes["onClick"] = null;
                }
            }
            this.hplOpportunitiesSeeAll.Enabled = false;
        }

        protected void grvContacts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Contact c = (Contact)e.Row.DataItem;
                    
                    if (c.Mobile == "")
                    {
                        e.Row.Cells[4].Text = "no mobile";
                        e.Row.Cells[4].CssClass = "lightgrey-italic";
                    }
                    if (c.DirectLine == "")
                    {
                        e.Row.Cells[5].Text = "no direct line";
                        e.Row.Cells[5].CssClass = "lightgrey-italic";
                    }
                    if (c.Email == "")
                    {
                        e.Row.Cells[6].Text = "no email address";
                        e.Row.Cells[6].CssClass = "lightgrey-italic";
                    }

                    bizClient biz = new bizClient();
                    Client client = biz.GetClient(c.ClientID);
                    if (client.PrimaryContactID == c.ContactID) e.Row.Cells[3].CssClass = "blue-bold";

                    e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewContact.aspx?cid=" + DataBinder.Eval(e.Row.DataItem, "ClientID") + "&coid=" + DataBinder.Eval(e.Row.DataItem, "ContactID") + "'";
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void grvOpportunities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    sp_web_ListClientOpenOpportunitiesResult o = (sp_web_ListClientOpenOpportunitiesResult)e.Row.DataItem;

                    Image imgF = (Image)e.Row.FindControl("imgFlagged");
                    imgF.Visible = o.Flagged;

                    Image type = (Image)e.Row.FindControl("imgBusinessType");
                    switch (o.BusinessTypeName)
                    {
                        case Enums.enBusinessType.NewBusiness:
                            type.ImageUrl = "~/images/OpportunityNewBusiness.gif";
                            break;
                        case Enums.enBusinessType.ReclaimedBusiness:
                            type.ImageUrl = "~/images/OpportunityReclaimedBusiness.gif";
                            break;
                        case Enums.enBusinessType.ExistingClients:
                            type.ImageUrl = "~/images/OpportunityExistingClients.gif";
                            break;
                        case Enums.enBusinessType.QuickQuote:
                            type.ImageUrl = "~/images/OpportunityQuickQuote.gif";
                            break;
                        case Enums.enBusinessType.QuickWin:
                            type.ImageUrl = "~/images/OpportunityQuickWin.gif";
                            break;
                        case Enums.enBusinessType.QuickCall:
                            type.ImageUrl = "~/images/OpportunityQuickCall.gif";
                            break;
                    }

                    if (o.ClassificationName == "OAMPS Income above $20,000")
                    {
                        e.Row.Font.Bold = true;
                    }

                    if (o.FollowUpDate < DateTime.Today)
                    {
                        e.Row.Cells[8].CssClass = "ochre-bold";
                    }

                    e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewOpportunity.aspx?cid=" + DataBinder.Eval(e.Row.DataItem, "ClientID") + "&oid=" + DataBinder.Eval(e.Row.DataItem, "OpportunityID") + "'";
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnActiveInactive_Click(object sender, EventArgs e)
        {
            try
            {
                bizClient biz = new bizClient();
                bool flag = false;
                if (this.btnActiveInactive.Text == "Inactivate") flag = true;
                if (biz.SetInactiveField(int.Parse(Request.QueryString["cid"]), flag) == true)
                {
                    PopulateClientDetails();
                }
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

    }
}
