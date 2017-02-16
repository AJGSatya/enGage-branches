using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using enGage.DL;
using enGage.BL;

namespace enGage.Web
{
    public partial class AddActivity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    if (Request.QueryString["cid"] == null || Request.QueryString["oid"] == null) return;

                    ((Main)Master).HeaderTitle = "Add activity";
                    bizSetting bizS = new bizSetting();

                    ((Main)Master).HeaderDetails = "Activity to be added by "
                                                    + bizActiveDirectory.GetUserFullName(bizUser.GetCurrentUserNameWithoutDomain())
                                                    + " (Now)";

                    PopulateClassification();
                    PopulateContacts();
                    PopulateClientSummary();
                    PopulateOpportunitySummary();
                    PopulateActivityDetails();
                    SetAdditionalControls();
                    PopulateAdditionalControls();
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
            bizMessage bizM = new bizMessage();

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

        private void PopulateClientSummary()
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

        private void PopulateOpportunitySummary()
        {
            bizMessage bizM = new bizMessage();

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

            this.lblOpportunityName.Text = o.OpportunityName;
            this.lblOpportunityDue.Text = string.Format("{0:dd-MMM-yy}", o.OpportunityDue);

            List<sp_web_GetCurrentOpportunityStatusResult> s = biz.GetCurrentOpportunityStatus(o.OpportunityID);
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            if (s != null) this.lblStatus.Text = s.First().StatusName;

            if (o.ContactID.HasValue == true) this.ddlContact.SelectedValue = o.ContactID.ToString();
        }

        private void PopulateActivityDetails()
        {
            bizMessage bizM = new bizMessage();
            bizSetting bizS = new bizSetting();
            bizActivity biz = new bizActivity();

            //status
            this.ddlOpportunityStatus.Items.Add(new ListItem("-- Please Select --", ""));
            List<sp_web_ListNextOpportunityStatusesResult> ss = biz.ListNextOpportunityStatuses(int.Parse(Request.QueryString["oid"]));
            foreach (sp_web_ListNextOpportunityStatusesResult s in ss)
            {
                this.ddlOpportunityStatus.Items.Add(new ListItem(s.StatusName, s.StatusID.ToString()));
            }
            if (ss.Count == 0) // pending
            {
                List<sp_web_ListCurrentOpportunityStatusesResult> cs = biz.ListCurrentOpportunityStatuses(int.Parse(Request.QueryString["oid"]));
                foreach (sp_web_ListCurrentOpportunityStatusesResult s in cs)
                {
                    this.ddlOpportunityStatus.Items.Add(new ListItem(s.StatusName, s.StatusID.ToString()));
                }
            }

            //bizClient bizC = new bizClient();
            //Client cc = bizC.GetClient(int.Parse(Request.QueryString["cid"]));
            //sp_web_GetUserByIDResult exec = bizC.GetAccountExecutive(cc.AccountExecutiveID);
            //this.ucMessanger1.ProcessMessages(bizC.MSGS, false);
            //bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
            //switch (user.Role)
            //{
            //    case (int)Enums.enUserRole.Executive:
            //        if (user.Branch == exec.Branch)
            //        {
            //            if (user.DisplayName != exec.DisplayName)
            //            {
            //                this.btnSave.Enabled = false;
            //                this.btnInactive.Enabled = false;
            //            }
            //        }
            //        else
            //        {
            //            Response.Redirect("~/FindClient.aspx", false);
            //            return;
            //        }
            //        break;
            //    case (int)Enums.enUserRole.Branch:
            //        if (user.Branch != exec.Branch)
            //        {
            //            Response.Redirect("~/FindClient.aspx", false);
            //            return;
            //        }
            //        break;
            //    case (int)Enums.enUserRole.Region:
            //        if (user.Region == exec.Region)
            //        {
            //            this.btnSave.Enabled = false;
            //            this.btnInactive.Enabled = false;
            //        }
            //        else
            //        {
            //            Response.Redirect("~/FindClient.aspx", false);
            //            return;
            //        }
            //        break;
            //    case (int)Enums.enUserRole.Company:
            //        this.btnSave.Enabled = false;
            //        this.btnInactive.Enabled = false;
            //        break;
            //    case (int)Enums.enUserRole.Administrator:
            //        // full access
            //        break;
            //}

        }

        private void SetDefaultValues()
        {
            bizOpportunity bizO = new bizOpportunity();
            Classification c = bizO.GetClassification(int.Parse(this.ddlClassification.SelectedValue));
            this.ucMessanger1.ProcessMessages(bizO.MSGS, true);

            if (c == null || this.txtOpportunityDue.Text == "") return;

            this.txtFollowUpDate.Text = string.Format("{0:dd/MM/yyyy}", DateTime.Parse(this.txtOpportunityDue.Text).AddDays(-c.FollowUpDefault * 7));
        }

        private void SetAdditionalControls()
        {
            this.trAccepted1.Visible = false;
            this.trLost1.Visible = false;
            this.trQualifiedIn1.Visible = false;
            this.trQualifiedIn2.Visible = false;
            this.trQualifiedIn3.Visible = false;
            this.trQualifiedIn4.Visible = false;
            this.trSubmitted1.Visible = false;
            this.trProcessed1.Visible = false;
            this.trProcessed2.Visible = false;

            switch (this.ddlOpportunityStatus.SelectedItem.Text)
            {
                case "Identified":
                    break;
                case "Qualified-in":
                    break;
                case "Qualified-out":
                    this.trLost1.Visible = true;
                    break;
                case "Interested":
                    this.trQualifiedIn1.Visible = true;
                    this.trQualifiedIn2.Visible = true;
                    this.trQualifiedIn3.Visible = true;
                    this.trQualifiedIn4.Visible = true;
                    break;
                case "Not Interested":
                    this.trLost1.Visible = true;
                    break;
                case "Go-to-Market":
                    break;
                case "Revisit next year":
                    this.trLost1.Visible = true;
                    break;
                case "Quoted":
                    this.trSubmitted1.Visible = true;
                    break;
                case "Can't Place":
                    this.trLost1.Visible = true;
                    break;
                case "Accepted":
                    this.trAccepted1.Visible = true;
                    break;
                case "Lost":
                    this.trLost1.Visible = true;
                    break;
                case "Processed":
                    this.trLost1.Visible = true;
                    this.trProcessed1.Visible = true;
                    this.trProcessed2.Visible = true;
                    break;
                default: // all pending statuses
                    break;
            }
        }

        private void PopulateAdditionalControls()
        {
            bizMessage bizM = new bizMessage();

            bizOpportunity biz = new bizOpportunity();
            Opportunity o = biz.GetOpportunity(int.Parse(Request.QueryString["oid"]));

            if (o == null) return;

            this.txtIncumbentBroker.Text = o.IncumbentBroker;
            this.txtIncumbentInsurer.Text = o.IncumbentInsurer;
            this.txtOpportunityDue.Text = String.Format("{0:dd/MM/yyyy}", o.OpportunityDue);
            this.ddlClassification.SelectedValue = o.ClassificationID.ToString();
            this.txtNetBrokerageQuoted.Text = o.NetBrokerageQuoted.ToString();
            this.txtNetBrokerageActual.Text = o.NetBrokerageActual.ToString();
            this.txtDateCompleted.Text = String.Format("{0:dd/MM/yyyy}", o.DateCompleted);
            this.txtMemoNumber.Text = o.MemoNumber;
            this.txtClientCode.Text = o.Client.ClientCode;
        }

        private bool UIValidation()
        {
            bizMessage bizM = new bizMessage();
            if (this.ddlOpportunityStatus.SelectedValue == "")
            {
                this.ucMessanger1.ProcessMessage("Stage of Sales / Aspire Cycle: " + bizM.GetMessageText("ValueNotSelected"), Enums.enMsgType.Err, "OpportunityStatus", typeof(DropDownList), true);
                return false;
            }
            bizActivity biz = new bizActivity();
            Status s = biz.GetStatusByID(int.Parse(this.ddlOpportunityStatus.SelectedValue));
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            if (s.OutcomeType != "C")
            {
                if (this.txtFollowUpDate.Text == "")
                {
                    this.ucMessanger1.ProcessMessage("Follow Up Date: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, "FollowUpDate", typeof(TextBox), true);
                    return false;
                }
            }
            if (this.txtFollowUpDate.Text != "")
            {
                DateTime result;
                if (DateTime.TryParse(this.txtFollowUpDate.Text, out result) == false)
                {
                    this.ucMessanger1.ProcessMessage("Follow Up Date: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err, "FollowUpDate", typeof(TextBox), true);
                    return false;
                }
            }
            switch (this.ddlOpportunityStatus.SelectedItem.Text)
            {
                case "Identified":
                    break;
                case "Qualified-in":
                    break;
                case "Qualified-out":
                    if (this.txtDateCompleted.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, "DateCompleted", typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        DateTime res;
                        if (DateTime.TryParse(this.txtDateCompleted.Text, out res) == false)
                        {
                            this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err, "DateCompleted", typeof(TextBox), true);
                            return false;
                        }
                    }
                    break;
                case "Interested":
                    DateTime result;
                    if (this.txtOpportunityDue.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Renewal Date: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, "OpportunityDue", typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        if (DateTime.TryParse(this.txtOpportunityDue.Text, out result) == false)
                        {
                            this.ucMessanger1.ProcessMessage("Renewal Date: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err, "OpportunityDue", typeof(TextBox), true);
                            return false;
                        }
                    }
                    if (this.ddlClassification.SelectedValue == "")
                    {
                        this.ucMessanger1.ProcessMessage("CSS Segment: " + bizM.GetMessageText("ValueNotSelected"), Enums.enMsgType.Err, "Classification", typeof(DropDownList), true);
                        return false;
                    }
                    break;
                case "Not Interested":
                    if (this.txtDateCompleted.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, "DateCompleted", typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        DateTime res;
                        if (DateTime.TryParse(this.txtDateCompleted.Text, out res) == false)
                        {
                            this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err, "DateCompleted", typeof(TextBox), true);
                            return false;
                        }
                    }
                    break;
                case "Go-to-Market":
                    break;
                case "Revisit next year":
                    if (this.txtDateCompleted.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, "DateCompleted", typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        DateTime res;
                        if (DateTime.TryParse(this.txtDateCompleted.Text, out res) == false)
                        {
                            this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err, "DateCompleted", typeof(TextBox), true);
                            return false;
                        }
                    }
                    break;
                case "Quoted":
                    if (this.txtNetBrokerageQuoted.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Expected OAMPS income Quoted: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, "NetBrokerageQuoted", typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        decimal res;
                        if (Decimal.TryParse(this.txtNetBrokerageQuoted.Text, out res) == false)
                        {
                            this.ucMessanger1.ProcessMessage("Expected OAMPS income Quoted: " + bizM.GetMessageText("FieldNotDecimal"), Enums.enMsgType.Err, "NetBrokerageQuoted", typeof(TextBox), true);
                            return false;
                        }
                    }
                    break;
                case "Can't Place":
                    if (this.txtDateCompleted.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, "DateCompleted", typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        DateTime res;
                        if (DateTime.TryParse(this.txtDateCompleted.Text, out res) == false)
                        {
                            this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err, "DateCompleted", typeof(TextBox), true);
                            return false;
                        }
                    }
                    break;
                case "Accepted":
                    if (this.txtNetBrokerageActual.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Expected OAMPS income Actual: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, "NetBrokerageActual", typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        decimal res;
                        if (Decimal.TryParse(this.txtNetBrokerageActual.Text, out res) == false)
                        {
                            this.ucMessanger1.ProcessMessage("Expected OAMPS income Actual: " + bizM.GetMessageText("FieldNotDecimal"), Enums.enMsgType.Err, "NetBrokerageActual", typeof(TextBox), true);
                            return false;
                        }
                    }
                    break;
                case "Lost":
                    if (this.txtDateCompleted.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, "DateCompleted", typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        DateTime res;
                        if (DateTime.TryParse(this.txtDateCompleted.Text, out res) == false)
                        {
                            this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err, "DateCompleted", typeof(TextBox), true);
                            return false;
                        }
                    }
                    break;
                case "Processed":
                    if (this.txtDateCompleted.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, "DateCompleted", typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        DateTime res;
                        if (DateTime.TryParse(this.txtDateCompleted.Text, out res) == false)
                        {
                            this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err, "DateCompleted", typeof(TextBox), true);
                            return false;
                        }
                    }
                    if (this.txtMemoNumber.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Memo Number: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, "MemoNumber", typeof(TextBox), true);
                        return false;
                    }
                    if (this.txtClientCode.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Client Code: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, "ClientCode", typeof(TextBox), true);
                        return false;
                    } 
                    break;
                default: // all pending statuses
                    break;
            }
            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.ucMessanger1.ClearMessages();
                this.ucMessanger1.UnmarkControls();

                if (this.UIValidation() == false) return;

                bizActivity biz = new bizActivity();

                Activity na = new Activity();
                //general
                na.OpportunityID = int.Parse(Request.QueryString["oid"]);
                if (this.ddlOpportunityStatus.SelectedValue != "") na.OpportunityStatusID = int.Parse(this.ddlOpportunityStatus.SelectedValue);
                na.ActivityNote = this.txtActivityNote.Text;
                if (this.txtFollowUpDate.Text != "") na.FollowUpDate = DateTime.Parse(this.txtFollowUpDate.Text);
                na.Inactive = false;
                if (this.ddlContact.SelectedValue != "") na.ContactID = int.Parse(this.ddlContact.SelectedValue);
                //additional
                Opportunity no = new Opportunity();
                Client nc = new Client();
                switch (this.ddlOpportunityStatus.SelectedItem.Text)
                {
                    case "Identified":
                        break;
                    case "Qualified-in":
                        break;
                    case "Qualified-out":
                        no.DateCompleted = DateTime.Parse(this.txtDateCompleted.Text);
                        break;
                    case "Interested":
                        no.OpportunityDue = DateTime.Parse(this.txtOpportunityDue.Text);
                        no.IncumbentBroker = this.txtIncumbentBroker.Text;
                        no.IncumbentInsurer = this.txtIncumbentInsurer.Text;
                        no.ClassificationID = int.Parse(this.ddlClassification.SelectedValue);
                        break;
                    case "Not Interested":
                        no.DateCompleted = DateTime.Parse(this.txtDateCompleted.Text);
                        break;
                    case "Go-to-Market":
                        break;
                    case "Revisit next year":
                        no.DateCompleted = DateTime.Parse(this.txtDateCompleted.Text);
                        break;
                    case "Quoted":
                        no.NetBrokerageQuoted = decimal.Parse(this.txtNetBrokerageQuoted.Text);
                        break;
                    case "Can't Place":
                        no.DateCompleted = DateTime.Parse(this.txtDateCompleted.Text);
                        break;
                    case "Accepted":
                        no.NetBrokerageActual = decimal.Parse(this.txtNetBrokerageActual.Text);
                        break;
                    case "Lost":
                        no.DateCompleted = DateTime.Parse(this.txtDateCompleted.Text);
                        break;
                    case "Processed":
                        no.DateCompleted = DateTime.Parse(this.txtDateCompleted.Text);
                        no.MemoNumber = this.txtMemoNumber.Text;
                        nc.ClientCode = this.txtClientCode.Text;
                        break;
                    default: // all pending statuses
                        break;
                }
                //audit
                na.AddedBy = bizUser.GetCurrentUserName();
                na.Added = DateTime.Now;
                //action
                if (biz.ValidateActivity(na) == false)
                {
                    this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                    return;
                }
                int aid = biz.InsertActivity(na, no, nc,no);
                if (aid != 0)
                {
                    this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                    Response.Redirect("ViewActivity.aspx?cid=" + Request.QueryString["cid"]
                                                     + "&oid=" + Request.QueryString["oid"]
                                                     + "&aid=" + aid.ToString()
                                                     , false);
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
                Response.Redirect("ViewOpportunity.aspx?cid=" + Request.QueryString["cid"] + "&oid=" + Request.QueryString["oid"], false);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void ddlOpportunityStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ddlOpportunityStatus.SelectedValue != "") SetAdditionalControls();   
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void ddlClassification_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ddlClassification.SelectedValue != "")
                {
                    if (this.ddlOpportunityStatus.SelectedItem.Text == "Qualified In") 
                        SetDefaultValues();
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
