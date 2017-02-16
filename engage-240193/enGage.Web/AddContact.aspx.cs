using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using enGage.DL;
using enGage.BL;

namespace enGage.Web
{
    public partial class AddContact : System.Web.UI.Page
    {
        protected Enums.enEditMode EditMode
        {
            get { return ((Enums.enEditMode)Enum.Parse(typeof(Enums.enEditMode), this.hidEditMode.Value)); }
            set { this.hidEditMode.Value = value.ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    PopulateClientDetails();
                    if (Request.QueryString["coid"] == null)
                    {
                        this.EditMode = Enums.enEditMode.Insert;
                        ((Main)Master).HeaderTitle = "Add Contact";
                        bizSetting bizS = new bizSetting();
                        ((Main)Master).HeaderDetails = "Contact to be added by "
                                                        + bizActiveDirectory.GetUserFullName(bizUser.GetCurrentUserNameWithoutDomain())
                                                        + " (Now)";
                    }
                    else
                    {
                        this.EditMode = Enums.enEditMode.Update;
                        ((Main)Master).HeaderTitle = "Edit Contact";
                        PopulateContactDetails();
                    }
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
            if (c.Industry != null) this.lblAssociation.Text = c.Industry.IndustryName + "(" + c.Industry.AnzsicCode + ")";
            if (c.AssociationCode != null) this.lblAssociation.Text = c.Association.AssociationName;

            //executive
            var u = bizUser.GetSMIAccountExecutiveIdBOAMPSUserName(c.AccountExecutiveID);
            bizUser.enGageUser exec = bizUser.GetAccountExecutive(u);
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            this.lblAccountExecutive.Text = "<b>" + exec.DisplayName + "</b>" + ", " + exec.Branch + " (" + exec.Region + ")";
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
            this.txtContactName.Text = c.ContactName;
            this.ddlTitle.SelectedValue = c.Title;
            this.txtMobile.Text = c.Mobile;
            this.txtDirectLine.Text = c.DirectLine;
            this.txtEmail.Text = c.Email;

            //audit 
            ((Main)Master).HeaderDetails = "Client added by "
                                            + bizActiveDirectory.GetUserFullName(c.AddedBy)
                                            + " (" + string.Format("{0:dd-MMM-yy}", c.Added) + ")";

            if (c.Modified.HasValue == true) ((Main)Master).HeaderDetails += " / modified by "
                                            + bizActiveDirectory.GetUserFullName(c.ModifiedBy)
                                            + " (" + string.Format("{0:dd-MMM-yy}", c.Modified.Value) + ")";

            bizClient bizC = new bizClient();
            Client cl = bizC.GetClient(int.Parse(Request.QueryString["cid"]));
            var u = bizUser.GetSMIAccountExecutiveIdBOAMPSUserName(cl.AccountExecutiveID);
            bizUser.enGageUser exec = bizUser.GetAccountExecutive(u);
            this.ucMessanger1.ProcessMessages(bizC.MSGS, false);

            if (Session["USER"] == null)
            {
                this.ucMessanger1.ProcessMessage("Session: " + bizM.GetMessageText("SessionMissing"), Enums.enMsgType.Err, "", null, true);
                return;
            }
            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];

            switch (user.Role)
            {
                case (int)Enums.enUserRole.Executive:
                    if (user.Branch == exec.Branch)
                    {
                        if (user.DisplayName != exec.DisplayName)
                        {
                            this.btnSave.Visible = false;
                        }
                    }
                    else
                    {
                        Response.Redirect("~/FindClient.aspx", false);
                        return;
                    }
                    break;
                case (int)Enums.enUserRole.Branch:
                    if (user.Branch != exec.Branch)
                    {
                        Response.Redirect("~/FindClient.aspx", false);
                        return;
                    }
                    break;
                case (int)Enums.enUserRole.Region:
                    if (user.Region == exec.Region)
                    {
                        this.btnSave.Visible = false;
                    }
                    else
                    {
                        Response.Redirect("~/FindClient.aspx", false);
                        return;
                    }
                    break;
                case (int)Enums.enUserRole.Company:
                    this.btnSave.Visible = false;
                    break;
                case (int)Enums.enUserRole.Administrator:
                    // full access
                    break;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.ucMessanger1.ClearMessages();
                this.ucMessanger1.UnmarkControls();

                Contact c = new Contact();
                bizContact biz = new bizContact();

                switch (this.EditMode)
                {
                    case Enums.enEditMode.Insert:
                        //general
                        c.ClientID = int.Parse(Request.QueryString["cid"]);
                        c.ContactName = this.txtContactName.Text;
                        c.Title = this.ddlTitle.SelectedValue;
                        c.Mobile = this.txtMobile.Text;
                        c.DirectLine = this.txtDirectLine.Text;
                        c.Email = this.txtEmail.Text;
                        //audit
                        c.AddedBy = bizUser.GetCurrentUserName();
                        c.Added = DateTime.Now;
                        //action
                        if (biz.ValidateContact(c) == false)
                        {
                            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                            return;
                        }
                        int coid = biz.InsertContact(c);
                        if (coid != 0)
                        {
                            Response.Redirect("ViewContact.aspx?cid=" + Request.QueryString["cid"]
                                            + "&coid=" + coid.ToString(), false);
                        }
                        else
                        {
                            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                        }
                        break;
                    case Enums.enEditMode.Update:
                        //general
                        c.ClientID = int.Parse(Request.QueryString["cid"]);
                        c.ContactID = int.Parse(Request.QueryString["coid"]);
                        c.ContactName = this.txtContactName.Text;
                        c.Title = this.ddlTitle.SelectedValue;
                        c.Mobile = this.txtMobile.Text;
                        c.DirectLine = this.txtDirectLine.Text;
                        c.Email = this.txtEmail.Text;
                        //audit
                        c.ModifiedBy = bizUser.GetCurrentUserName();
                        c.Modified = DateTime.Now;
                        if (biz.ValidateContact(c) == false)
                        {
                            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                            return;
                        }
                        if (biz.UpdateContact(c) == true)
                        {
                            Response.Redirect("ViewContact.aspx?cid=" + Request.QueryString["cid"]
                                            + "&coid=" + Request.QueryString["coid"], false);
                        }
                        else
                        {
                            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["coid"] != null)
                    Response.Redirect("ViewContact.aspx?cid=" + Request.QueryString["cid"] + "&coid=" + Request.QueryString["coid"], false);
                else
                    Response.Redirect("ViewClient.aspx?cid=" + Request.QueryString["cid"], false);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }
    }
}
