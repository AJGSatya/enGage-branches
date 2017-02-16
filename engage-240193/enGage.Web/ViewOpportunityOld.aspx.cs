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
    public partial class ViewOpportunity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    ((Main)Master).HeaderTitle = "Work with opportunity";
                    PopulateClientDetails();
                    PopulateOpportunityDetails();
                }
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

            bizClient biz = new bizClient();
            Client c;
            c = biz.GetClient(int.Parse(Request.QueryString["cid"]));
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);

            if (c == null) return;

            //client details
            this.lblClientName.Text = c.ClientName;
            this.lblOfficePhone.Text = c.OfficePhone;
            if (c.Address == null)
            {
                this.lblAddress.Text = "no address";
                this.lblAddress.CssClass = "page-text-nodata";
            }
            else
            {
                this.lblAddress.Text = c.Address + ", " + c.Location + " " + c.StateCode + " " + c.PostCode;
            }
            if (c.Industry != null) this.lblAssociation.Text = c.Industry.IndustryName + " (" + c.Industry.AnzsicCode + ")";
            if (c.AssociationCode != null) this.lblAssociation.Text = c.Association.AssociationName;

            //executive
            var u = bizUser.GetSMIAccountExecutiveIdBOAMPSUserName(c.AccountExecutiveID);
            bizUser.enGageUser exec = bizUser.GetAccountExecutive(u);
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            this.lblAccountExecutive.Text = "<b>" + exec.DisplayName + "</b>" + ", " + exec.Branch + " (" + exec.Region + ")";

            if (c.Inactive == true)
            {
                this.lblClientName.Enabled = false;
                this.lblOfficePhone.Enabled = false;
                this.lblAddress.Enabled = false;
                this.lblAssociation.Enabled = false;
                this.lblAccountExecutive.Enabled = false;
            }
            else
            {
                this.lblClientName.Enabled = true;
                this.lblOfficePhone.Enabled = true;
                this.lblAddress.Enabled = true;
                this.lblAssociation.Enabled = true;
                this.lblAccountExecutive.Enabled = true;
            }
        }

        private void PopulateOpportunityDetails()
        {
            bizMessage bizM = new bizMessage();
            bizSetting bizS = new bizSetting();
            bizOpportunity biz = new bizOpportunity();
            Opportunity o;
            o = biz.GetOpportunity(int.Parse(Request.QueryString["oid"]));
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);

            if (o == null) return;

            switch (o.BusinessType.BusinessTypeName)
            {
                case Enums.enBusinessType.NewBusiness:
                    this.imgBusinessType.ImageUrl = "~/images/OpportunityNewBusiness.gif";
                    break;
                case Enums.enBusinessType.ReclaimedBusiness:
                    this.imgBusinessType.ImageUrl = "~/images/OpportunityReclaimedBusiness.gif";
                    break;
                case Enums.enBusinessType.ExistingClients:
                    this.imgBusinessType.ImageUrl = "~/images/OpportunityExistingClients.gif";
                    break;
                case Enums.enBusinessType.QuickQuote:
                    this.imgBusinessType.ImageUrl = "~/images/OpportunityQuickQuote.gif";
                    break;
                case Enums.enBusinessType.QuickWin:
                    this.imgBusinessType.ImageUrl = "~/images/OpportunityQuickWin.gif";
                    break;
                case Enums.enBusinessType.QuickCall:
                    this.imgBusinessType.ImageUrl = "~/images/OpportunityQuickCall.gif";
                    break;
            }

            this.imgFlagged.Visible = o.Flagged;
            if (o.Flagged == false) this.btnFlagUnflag.Text = "Flag";
            else this.btnFlagUnflag.Text = "Un-Flag";

            this.lblOpportunityName.Text = o.OpportunityName;
            this.lblOpportunityDue.Text = string.Format("{0:dd-MMM-yy}", o.OpportunityDue);
            if (o.ContactID != null) this.lblContact.Text = o.Contact.ContactName;
            this.lblMemoNumber.Text = o.MemoNumber;
            if (o.ClassificationID != null) this.lblClassification.Text = o.Classification.ClassificationName;
            this.lblIncumbentBroker.Text = o.IncumbentBroker;
            this.lblIncumbentUnderwriter.Text = o.IncumbentInsurer;
            this.lblNetBrokerageQuoted.Text = string.Format("{0:C}", o.NetBrokerageQuoted);
            this.lblNetBrokerageActual.Text = string.Format("{0:C}", o.NetBrokerageActual);

            List<sp_web_GetCurrentOpportunityStatusResult> s = biz.GetCurrentOpportunityStatus(o.OpportunityID);
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            if (s != null) this.lblStatus.Text = s.First().StatusName;

            bizActivity bizA = new bizActivity();
            this.lblNextActivity.Text = "";
            IList<sp_web_ListNextOpportunityStatusesResult> res = bizA.ListNextOpportunityStatuses(o.OpportunityID);
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            foreach (sp_web_ListNextOpportunityStatusesResult r in res)
            {
                this.lblNextActivity.Text += r.StatusName + ",";
            }
            char[] charsToTrim = { ',' };
            this.lblNextActivity.Text = this.lblNextActivity.Text.TrimEnd(charsToTrim);
            if (res.Count == 0)
            {
                IList<sp_web_ListCurrentOpportunityStatusesResult> rs = bizA.ListCurrentOpportunityStatuses(o.OpportunityID);
                this.ucMessanger1.ProcessMessages(biz.MSGS, false);
                foreach (sp_web_ListCurrentOpportunityStatusesResult r in rs)
                {
                    this.lblNextActivity.Text += r.StatusName + ",";
                }
                char[] chrsToTrim = { ',' };
                this.lblNextActivity.Text = this.lblNextActivity.Text.TrimEnd(chrsToTrim);
            }

            this.lblFollowUpCompleted.Text = string.Format("{0:dd-MMM-yy}",biz.GetCurrentFollowUpDate(o.OpportunityID));

            if (biz.IsOpportunityCompleted(o.OpportunityID) == true)
            {
                this.lblNextActivity.Text = "None - Opportunity has been completed";
                this.lblFollowUpCompletedLabel.Text = "Completed:";
                this.lblFollowUpCompleted.Text = string.Format("{0:dd-MMM-yy}",o.DateCompleted);
                this.btnAddActivity.Visible = false;
            }

            this.lblActive.Text = o.Activities.Where(a => a.Inactive == false).Count().ToString();
            this.lblInactive.Text = o.Activities.Where(a => a.Inactive == true).Count().ToString();
            if (int.Parse(this.lblInactive.Text) == 0) this.lnkSeeAll.Enabled = false;
            else this.lnkSeeAll.Enabled = true;

            //activities
            this.grvActivities.DataSource = biz.ListOpportunityActiveActivites(o.OpportunityID);
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            this.grvActivities.DataBind();

            //audit
            ((Main)Master).HeaderDetails = "Opportunity added by "
                                            + bizActiveDirectory.GetUserFullName(o.AddedBy)
                                            + " (" + string.Format("{0:dd-MMM-yy}", o.Added) + ")";

            if (o.Modified.HasValue == true)
                ((Main)Master).HeaderDetails += " / modified by "
                    + bizActiveDirectory.GetUserFullName(o.ModifiedBy)
                    + " (" + string.Format("{0:dd-MMM-yy}", o.Modified.Value) + ")";
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ViewClient.aspx?cid=" + Request.QueryString["cid"], false);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnEditOpportunity_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("EditOpportunity.aspx?cid=" + Request.QueryString["cid"] + "&oid=" + Request.QueryString["oid"], false);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnAddActivity_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("AddActivity.aspx?cid=" + Request.QueryString["cid"] + "&oid=" + Request.QueryString["oid"], false);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void grvActivities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewActivity.aspx?cid=" + Request.QueryString["cid"] + "&oid=" + Request.QueryString["oid"] + "&aid=" + DataBinder.Eval(e.Row.DataItem, "ActivityID") + "'";
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void lnkSeeAllSuspects_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("OpportunityActivitiesAll.aspx?cid=" + Request.QueryString["cid"] + "&oid=" + Request.QueryString["oid"], false);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnFlagUnflag_Click(object sender, EventArgs e)
        {
            try
            {
                bizOpportunity biz = new bizOpportunity();
                if (biz.SetFlag(int.Parse(Request.QueryString["oid"]), !this.imgFlagged.Visible) == true)
                {
                    PopulateOpportunityDetails();
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
