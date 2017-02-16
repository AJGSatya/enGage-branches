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
    public partial class ViewContact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    ((Main)Master).HeaderTitle = "Work with contact";
                    PopulateClientDetails();
                    PopulateContactDetails();
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
            if (c.Industry != null) this.lblAssociation.Text = c.Industry.IndustryName + "(" + c.Industry.AnzsicCode.ToString() + ")";
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

        private void PopulateContactDetails()
        {
            bizMessage bizM = new bizMessage();
            bizSetting bizS = new bizSetting();

            bizContact biz = new bizContact();
            Contact c;
            c = biz.GetContact(Convert.ToInt32(Request.QueryString["coid"]));
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);

            if (c == null) return;

            //general
            this.lblContactName.Text = c.ContactName;
            this.lblTitle.Text = c.Title;
            this.lblMobile.Text = c.Mobile;
            this.lblDirectLine.Text = c.DirectLine;
            this.lblEmail.Text = c.Email;

            //buttons
            this.btnActiveInactive.Text = c.Inactive == true ? "Activate" : "Inactivate";
            if (c.Email != "") this.btnSendEmail.OnClientClick = "javascript:location.href='mailto:" + c.Email + "'";
            else this.btnSendEmail.Visible = false;

            //audit
            ((Main)Master).HeaderDetails = "Client added by "
                                + bizActiveDirectory.GetUserFullName(c.AddedBy)
                                + " (" + string.Format("{0:dd-MMM-yy}", c.Added) + ")";

            if (c.Modified.HasValue == true)
                ((Main)Master).HeaderDetails += " / modified by "
                    + bizActiveDirectory.GetUserFullName(c.ModifiedBy)
                    + " (" + string.Format("{0:dd-MMM-yy}", c.Modified.Value) + ")";
        }

        protected void btnEditContact_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("AddContact.aspx?cid=" + Request.QueryString["cid"] + "&coid=" + Request.QueryString["coid"], false);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "";
                query += "ViewClient.aspx";
                if (Request.QueryString["cid"] != null)
                {
                    query += "?cid=" + Request.QueryString["cid"];
                }
                Response.Redirect(query, false);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnSetPrimary_Click(object sender, EventArgs e)
        {
            try
            {
                bizClient biz = new bizClient();
                if (biz.SetPrimaryContact(int.Parse(Request.QueryString["cid"]), int.Parse(Request.QueryString["coid"])) == true)
                {
                    this.ucMessanger1.ProcessMessages(biz.MSGS, true);
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
                bizContact biz = new bizContact();
                bool inactive = this.btnActiveInactive.Text == "Activate" ? false : true;
                if (biz.SetToActiveInactive(int.Parse(Request.QueryString["coid"]), inactive) == true)
                {
                    PopulateContactDetails();
                    this.ucMessanger1.ProcessMessages(biz.MSGS, false);
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }
    }
}
