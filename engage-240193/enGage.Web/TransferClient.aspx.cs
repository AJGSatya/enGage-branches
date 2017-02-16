using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices;
using enGage.DL;
using enGage.BL;

namespace enGage.Web
{
    public partial class TransferClient : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    ((Main)Master).HeaderTitle = "Transfer client";
                    PopulateClientDetails();
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
           
            if (exec != null)
            {
                this.lblAccountExecutive.Text = "<b>" + exec.DisplayName + "</b>" + ", " + exec.Branch + " (" + exec.Region + ")";
            }
            else
            {
                this.lblAccountExecutive.Text = "<b>" + c.AccountExecutiveID + "</b>" + ", Unknown Branch (Unknown Region)";

            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.ucMessanger1.ClearMessages();
                this.ucMessanger1.UnmarkControls();

                bizMessage bizM = new bizMessage();
                if (this.lstExecutives.SelectedValue == "")
                {
                    this.ucMessanger1.ProcessMessage("Account Executive: " + bizM.GetMessageText("ValueNotSelected"), Enums.enMsgType.Err, "", null, true);
                    return;
                }

                bizClient biz = new bizClient();

                if (biz.TransferClient(int.Parse(Request.QueryString["cid"]), this.lstExecutives.SelectedValue) == true)
                {
                    Response.Redirect("FindClient.aspx", false);
                }
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);

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
                Response.Redirect("ViewClient.aspx?cid=" + Request.QueryString["cid"], false);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnLookup_Click(object sender, EventArgs e)
        {
            try
            {
                bizMessage bizM = new bizMessage();
                this.lblFindMessage.Visible = false;
                this.lstExecutives.Visible = false;
                if (this.txtFind.Text == "")
                {
                    this.lblFindMessage.Visible = true;
                    return;
                }
                bizSetting biz = new bizSetting();
                NameValueCollection execs = bizActiveDirectory.FindAccountExecutive(this.txtFind.Text);
                if (execs.Count > 0)
                {
                    this.lstExecutives.Items.Clear();
                    foreach (String exec in execs)
                    {
                        this.lstExecutives.Items.Add(new ListItem(exec, execs[exec]));
                    }
                    this.lstExecutives.Visible = true;
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
