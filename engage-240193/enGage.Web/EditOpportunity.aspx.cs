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
    public partial class EditOpportunity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    ((Main)Master).HeaderTitle = "Edit opportunity";
                    PopulateClassification();
                    PopulateBusinessType();
                    PopulateContacts();
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

        private void PopulateClassification()
        {
            bizOpportunity biz = new bizOpportunity();
            List<Classification> cls;
            cls = biz.ListClassifications();
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);

            if (cls == null) return;

            this.ddlClassification.Items.Clear();
            this.ddlClassification.Items.Add(new ListItem("-- Please Select --", ""));
            foreach (Classification cl in cls)
            {
                this.ddlClassification.Items.Add(new ListItem(cl.ClassificationName, cl.ClassificationID.ToString()));
            }
        }

        private void PopulateBusinessType()
        {
            bizOpportunity biz = new bizOpportunity();
            List<BusinessType> bts = biz.ListBusinessTypes();
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            this.ddlBusinessType.Items.Add(new ListItem("-- Please Select --", ""));
            foreach (BusinessType bt in bts)
            {
                this.ddlBusinessType.Items.Add(new ListItem(bt.BusinessTypeName, bt.BusinessTypeID.ToString()));
            }
        }

        private void PopulateContacts()
        {
            bizClient biz = new bizClient();
            List<sp_web_ListClientContactsResult> ccs = biz.ListClientContacts(int.Parse(Request.QueryString["cid"]));
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            this.ddlContact.Items.Add(new ListItem("-- Please Select --", ""));
            foreach (sp_web_ListClientContactsResult cc in ccs)
            {
                this.ddlContact.Items.Add(new ListItem(cc.ContactName, cc.ContactID.ToString()));
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

        private void PopulateOpportunityDetails()
        {
            bizMessage bizM = new bizMessage();
            bizSetting bizS = new bizSetting();

            bizOpportunity biz = new bizOpportunity();
            Opportunity o;
            o = biz.GetOpportunity(Convert.ToInt32(Request.QueryString["oid"]));
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);

            if (o == null) return;

            //general
            this.txtOpportunityName.Text = o.OpportunityName;
            if (o.OpportunityDue.HasValue == true) this.txtOpportunityDue.Text = String.Format("{0:dd/MM/yyyy}", o.OpportunityDue.Value);
            this.txtIncumbentBroker.Text = o.IncumbentBroker;
            this.txtIncumbentInsurer.Text = o.IncumbentInsurer;
            if (o.ClassificationID.HasValue == true) this.ddlClassification.SelectedValue = o.ClassificationID.Value.ToString();
            if (o.BusinessTypeID.HasValue == true) this.ddlBusinessType.SelectedValue = o.BusinessTypeID.Value.ToString();
            if (o.ContactID.HasValue == true) this.ddlContact.SelectedValue = o.ContactID.Value.ToString();
            this.ddlFlagged.SelectedValue = o.Flagged.ToString();
            if (o.NetBrokerageEstimated.HasValue == true) this.txtEstimatedBrokingIncome.Text = o.NetBrokerageEstimated.ToString();
            if (o.NetBrokerageQuoted.HasValue == true) this.txtNetBrokerageQuoted.Text = o.NetBrokerageQuoted.ToString();
            if (o.NetBrokerageActual.HasValue == true) this.txtNetBrokerageActual.Text = o.NetBrokerageActual.ToString();
            if (o.DateCompleted.HasValue == true) this.txtDateCompleted.Text = String.Format("{0:dd/MM/yyyy}", o.DateCompleted);
            this.txtMemoNumber.Text = o.MemoNumber;

            //status
            List<sp_web_GetCurrentOpportunityStatusResult> s = biz.GetCurrentOpportunityStatus(o.OpportunityID);
            if (s != null) this.lblOpportunityStatus.Text = s.First().StatusName;

            //next status
            bizActivity bizA = new bizActivity();
            IList<sp_web_ListNextOpportunityStatusesResult> res = bizA.ListNextOpportunityStatuses(o.OpportunityID);
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            this.lblNextActivity.Text = "";
            foreach (sp_web_ListNextOpportunityStatusesResult r in res)
            {
                this.lblNextActivity.Text += r.StatusName + ",";
            }
            char[] charsToTrim = {','};
            this.lblNextActivity.Text.TrimEnd(charsToTrim);
            if (biz.IsOpportunityCompleted(o.OpportunityID) == true) this.lblNextActivity.Text = "None - Opportunity has been completed";

            //audit
            ((Main)Master).HeaderDetails = "Opportunity added by "
                                            + bizActiveDirectory.GetUserFullName(o.AddedBy)
                                            + " (" + string.Format("{0:dd-MMM-yy}", o.Added) + ")";

            if (o.Modified.HasValue == true)
                ((Main)Master).HeaderDetails += " / modified by "
                    + bizActiveDirectory.GetUserFullName(o.ModifiedBy)
                    + " (" + string.Format("{0:dd-MMM-yy}", o.Modified.Value) + ")";

            ////buttons
            //this.btnSave.Enabled = !o.Inactive;
            ////controls
            //this.txtOpportunityName.Enabled = !o.Inactive;
            //this.txtOpportunityDue.Enabled = !o.Inactive;
            //this.btnOpportunityDue.Enabled = !o.Inactive;
            //this.txtIncumbentBroker.Enabled = !o.Inactive;
            //this.txtIncumbentInsurer.Enabled = !o.Inactive;
            //this.ddlClassification.Enabled = !o.Inactive;
            //this.txtNetBrokerageQuoted.Enabled = !o.Inactive;
            //this.txtNetBrokerageActual.Enabled = !o.Inactive;
            //this.txtDateCompleted.Enabled = !o.Inactive;
            //this.btnDateCompleted.Enabled = !o.Inactive;
            //this.txtMemoNumber.Enabled = !o.Inactive;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Opportunity o = new Opportunity();
                bizOpportunity biz = new bizOpportunity();

                bizMessage bizM = new bizMessage();
                DateTime result;
                if (this.txtOpportunityDue.Text != "")
                {
                    if (DateTime.TryParse(this.txtOpportunityDue.Text, out result) == false)
                    {
                        this.ucMessanger1.ProcessMessage("Renewal Date: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err, "OpportunityDue", typeof(TextBox), true);
                        return;
                    }
                }

                o.ClientID = int.Parse(Request.QueryString["cid"]);
                o.OpportunityID = int.Parse(Request.QueryString["oid"]);
                o.OpportunityName = this.txtOpportunityName.Text;
                if (this.txtOpportunityDue.Text != "") o.OpportunityDue = DateTime.Parse(this.txtOpportunityDue.Text);
                o.IncumbentBroker = this.txtIncumbentBroker.Text;
                o.IncumbentInsurer = this.txtIncumbentInsurer.Text;
                if (this.ddlClassification.SelectedValue != "") o.ClassificationID = int.Parse(this.ddlClassification.SelectedValue);
                if (this.ddlBusinessType.SelectedValue != "") o.BusinessTypeID = int.Parse(this.ddlBusinessType.SelectedValue);
                if (this.ddlContact.SelectedValue != "") o.ContactID = int.Parse(this.ddlContact.SelectedValue);
                o.Flagged = bool.Parse(this.ddlFlagged.SelectedValue);
                if (this.txtEstimatedBrokingIncome.Text != "") o.NetBrokerageEstimated = decimal.Parse(this.txtEstimatedBrokingIncome.Text);
                if (this.txtNetBrokerageQuoted.Text != "") o.NetBrokerageQuoted = decimal.Parse(this.txtNetBrokerageQuoted.Text);
                if (this.txtNetBrokerageActual.Text != "") o.NetBrokerageActual = decimal.Parse(this.txtNetBrokerageActual.Text);
                if (this.txtDateCompleted.Text != "") o.DateCompleted = DateTime.Parse(this.txtDateCompleted.Text);
                o.MemoNumber = this.txtMemoNumber.Text;
                //audit
                o.ModifiedBy = bizUser.GetCurrentUserName();
                o.Modified = DateTime.Now;
                //action
                if (biz.ValidateOpportunity(o) == false)
                {
                    this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                    return;
                }
                if (biz.UpdateOpportunity(o) == true)
                {
                    Response.Redirect("ViewOpportunity.aspx?cid=" + Request.QueryString["cid"] + "&oid=" + Request.QueryString["oid"],false);
                }
                else
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ViewOpportunity.aspx?cid=" + Request.QueryString["cid"] + "&oid=" + Request.QueryString["oid"], false);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

    }
}
