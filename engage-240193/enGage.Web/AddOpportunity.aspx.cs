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
    public partial class AddOpportunity : System.Web.UI.Page
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
                    SetControls();
                    PopulateClientDetails();
                    PopulateClassification();
                    PopulateBusinessType();
                    PopulateContacts();
                    PopulateOpportunityStatuses();
                    SetBusinessTypeControls();
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void SetControls()
        {
            ((Main)Master).HeaderTitle = "Add new opportunity";
            bizSetting bizS = new bizSetting();
            ((Main)this.Master).HeaderDetails = "Opportunity added by " 
                                                    + bizActiveDirectory.GetUserFullName(bizUser.GetCurrentUserNameWithoutDomain())
                                                    + " (Now)";
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

        private void PopulateBusinessType()
        {
            bizOpportunity biz = new bizOpportunity();
            List<BusinessType> bts = biz.ListBusinessTypes();
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);

            bts = bts.Where(x => (x.BusinessTypeName != "Quick win" && x.BusinessTypeName != "Quick quote") && x.BusinessTypeName != "Quick call").ToList();

            //this.ddlBusinessType.Items.Add(new ListItem("-- Please Select --", ""));
            foreach (BusinessType bt in bts)
            {
                this.ddlBusinessType.Items.Add(new ListItem(bt.BusinessTypeName, bt.BusinessTypeID.ToString()));
            }
            if (this.ddlBusinessType.Items.Count > 0)
            {
                this.ddlBusinessType.SelectedIndex = this.ddlBusinessType.Items.IndexOf(this.ddlBusinessType.Items.FindByText(Enums.enBusinessType.NewBusiness));
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

        private void PopulateOpportunityStatuses()
        {
            //current status
            bizActivity bizA = new bizActivity();
            Status ins = bizA.GetInitialStatus();
            this.ucMessanger1.ProcessMessages(bizA.MSGS, true);
            this.lblOpportunityStatus.Text = ins.StatusName;

            //next status
            List<Status> ns = bizA.GetNextStatuses(ins.StatusID);
            this.lblNextActivity.Text = "";
            foreach (Status s in ns)
            {
                this.lblNextActivity.Text += s.StatusName + ",";
            }
            char[] charsToTrim = {','};
            this.lblNextActivity.Text = this.lblNextActivity.Text.TrimEnd(charsToTrim);           
        }

        private void PopulateOpportunityStatusesQuickQuote()
        {
            //current status
            bizActivity bizA = new bizActivity();
            Status cs = bizA.GetStatusByName("Go-to-Market");
            this.lblOpportunityStatus.Text = cs.StatusName;

            //next status
            List<Status> ns = bizA.GetNextStatuses(cs.StatusID);
            this.lblNextActivity.Text = "";
            foreach (Status s in ns)
            {
                this.lblNextActivity.Text += s.StatusName + ",";
            }
            char[] charsToTrim = { ',' };
            this.lblNextActivity.Text = this.lblNextActivity.Text.TrimEnd(charsToTrim);  
        }

        private void PopulateOpportunityStatusesQuickWin()
        {
            //current status
            bizActivity bizA = new bizActivity();
            Status cs = bizA.GetStatusByName("Accepted");
            this.lblOpportunityStatus.Text = cs.StatusName;

            //next status
            List<Status> ns = bizA.GetNextStatuses(cs.StatusID);
            this.lblNextActivity.Text = "";
            foreach (Status s in ns)
            {
                this.lblNextActivity.Text += s.StatusName + ",";
            }
            char[] charsToTrim = { ',' };
            this.lblNextActivity.Text = this.lblNextActivity.Text.TrimEnd(charsToTrim);
        }

        private void SetDefaultValues()
        {
            bizOpportunity bizO = new bizOpportunity();
            Classification c = bizO.GetClassification(int.Parse(this.ddlClassification.SelectedValue));
            this.ucMessanger1.ProcessMessages(bizO.MSGS, true);

            if (c == null || this.txtOpportunityDue.Text == "") return;
            if(this.txtFollowUpDate.Text=="")
                this.txtFollowUpDate.Text = string.Format("{0:dd/MM/yyyy}", DateTime.Parse(this.txtOpportunityDue.Text).AddDays(-c.FollowUpDefault * 7));
        }

        private void SetBusinessTypeControls()
        {
            switch (this.ddlBusinessType.SelectedItem.Text)
            {
                case "Quick quote": // go-to-market
                    PopulateOpportunityStatusesQuickQuote();
                    this.trActivityNote.Visible = true;
                    this.trQuickWin1.Visible = false;
                    this.trQuickWin2.Visible = false;
                    break;
                case "Quick win": // accepted
                    PopulateOpportunityStatusesQuickWin();
                    this.trActivityNote.Visible = true;
                    this.trQuickWin1.Visible = true;
                    this.trQuickWin2.Visible = true;
                    break;
                default:
                    PopulateOpportunityStatuses();
                    this.trActivityNote.Visible = false;
                    this.trQuickWin1.Visible = false;
                    this.trQuickWin2.Visible = false;
                    break;
            }
        }

        private bool UIValidation()
        {
            bizMessage bizM = new bizMessage();

            switch (this.ddlBusinessType.SelectedItem.Text)
            {
                case "Quick quote": // go-to-market
                    DateTime r1;
                    if (this.txtOpportunityDue.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Renewal Date: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, "OpportunityDue", typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        if (DateTime.TryParse(this.txtOpportunityDue.Text, out r1) == false)
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
                case "Quick win": // accepted
                    DateTime r2;
                    if (this.txtOpportunityDue.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Renewal Date: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, "OpportunityDue", typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        if (DateTime.TryParse(this.txtOpportunityDue.Text, out r2) == false)
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
                    if (this.txtNetBrokerageQuoted.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Broking Income Quoted: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, "NetBrokerageQuoted", typeof(TextBox), true);
                        return false;
                    }
                    if (this.txtNetBrokerageActual.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Actual Broking Income: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, "NetBrokerageActual", typeof(TextBox), true);
                        return false;
                    }
                    break;
                default:
                    DateTime result;
                    //if (this.txtOpportunityDue.Text == "")
                    //{
                    //    if (DateTime.TryParse(this.txtOpportunityDue.Text, out result) == false)
                    //    {
                    //        this.ucMessanger1.ProcessMessage("Renewal Date: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err, "OpportunityDue", typeof(TextBox), true);
                    //        return false;
                    //    }
                    //}
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

                bizMessage bizM = new bizMessage();

                if (Session["USER"] == null)
                {
                    this.ucMessanger1.ProcessMessage("Session: " + bizM.GetMessageText("SessionMissing"), Enums.enMsgType.Err, "", null, true);
                    return;
                }

                if (UIValidation() == false) return;

                switch (this.ddlBusinessType.SelectedItem.Text)
                {
                    case "Quick quote": // go-to-market
                        InsertQuickQuote();
                        break;
                    case "Quick win": // accepted
                        InsertQuickWin();
                        break;
                    case "Quick call": // quick call
                        InsertQuickCall();
                        break;
                    default:
                        Insert();
                        break;
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void Insert()
        {
            Opportunity o = new Opportunity();
            bizOpportunity biz = new bizOpportunity();

            //general opportunity
            o.ClientID = int.Parse(Request.QueryString["cid"]);
            o.Inactive = false;
            o.Flagged = false;
            o.OpportunityName = this.txtOpportunityName.Text;
            if (this.txtOpportunityDue.Text != "") o.OpportunityDue = DateTime.Parse(this.txtOpportunityDue.Text);
            o.IncumbentBroker = this.txtIncumbentBroker.Text;
            o.IncumbentInsurer = this.txtIncumbentInsurer.Text;
            if (this.ddlClassification.SelectedValue != "") o.ClassificationID = int.Parse(this.ddlClassification.SelectedValue);
            if (this.ddlBusinessType.SelectedValue != "") o.BusinessTypeID = int.Parse(this.ddlBusinessType.SelectedValue);
            if (this.ddlContact.SelectedValue != "") o.ContactID = int.Parse(this.ddlContact.SelectedValue);
            //audit
            o.AddedBy = bizUser.GetCurrentUserName();
            o.Added = DateTime.Now;

            //general activity
            Activity na = new Activity();
            bizActivity bizA = new bizActivity();
            na.OpportunityStatusID = bizA.GetInitialStatus().StatusID;
            if (this.txtFollowUpDate.Text != "") na.FollowUpDate = DateTime.Parse(this.txtFollowUpDate.Text);
            na.Inactive = false;
            na.ActivityNote = "";
            //audit
            na.AddedBy = bizUser.GetCurrentUserName();
            na.Added = DateTime.Now;

            //action
            if (biz.ValidateOpportunity(o) == false)
            {
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                return;
            }
            o.Activities.Add(na);
            int oid = biz.InsertOpportunity(o);
            if (oid != 0)
            {
                Response.Redirect("ViewClient.aspx?cid=" + Request.QueryString["cid"], false);
            }
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
        }

        private void InsertQuickQuote()
        {
            Opportunity o = new Opportunity();
            bizOpportunity biz = new bizOpportunity();

            //OPPORTUNITY//
            o.ClientID = int.Parse(Request.QueryString["cid"]);
            o.Inactive = false;
            o.Flagged = false;
            o.OpportunityName = this.txtOpportunityName.Text;
            if (this.txtOpportunityDue.Text != "") o.OpportunityDue = DateTime.Parse(this.txtOpportunityDue.Text);
            o.IncumbentBroker = this.txtIncumbentBroker.Text;
            o.IncumbentInsurer = this.txtIncumbentInsurer.Text;
            if (this.ddlClassification.SelectedValue != "") o.ClassificationID = int.Parse(this.ddlClassification.SelectedValue);
            if (this.ddlBusinessType.SelectedValue != "") o.BusinessTypeID = int.Parse(this.ddlBusinessType.SelectedValue);
            if (this.ddlContact.SelectedValue != "") o.ContactID = int.Parse(this.ddlContact.SelectedValue);
            //audit
            o.AddedBy = bizUser.GetCurrentUserName();
            o.Added = DateTime.Now;

            if (biz.ValidateQuickQuoteOpportunity(o) == false)
            {
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                return;
            }

            //ACTIVITY//
            bizActivity bizA = new bizActivity();
            char ot = 'S';
            List<sp_web_ListStatusesByOutcomeTypeResult> ss = bizA.ListStatusesByOutcomeType(ot);
            foreach (var s in ss)
            {
                Activity na = new Activity();
                //general
                na.OpportunityStatusID = s.StatusID;
                if (this.txtFollowUpDate.Text != "") na.FollowUpDate = DateTime.Parse(this.txtFollowUpDate.Text);
                na.Inactive = false;
                if (s.StatusName == "Go-to-Market") na.ActivityNote = this.txtActivityNote.Text;
                else na.ActivityNote = "";
                //audit
                na.AddedBy = bizUser.GetCurrentUserName();
                na.Added = DateTime.Now;
                o.Activities.Add(na);
                if (s.StatusName == "Go-to-Market") break;
            }

            foreach (Activity na in o.Activities)
            {
                if (bizA.ValidateActivity(na) == false)
                {
                    this.ucMessanger1.ProcessMessages(bizA.MSGS, true);
                    return;
                }
            }

            int oid = biz.InsertOpportunity(o);
            if (oid != 0)
            {
                Response.Redirect("ViewClient.aspx?cid=" + Request.QueryString["cid"], false);
            }
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
        }

        private void InsertQuickWin()
        {
            Opportunity o = new Opportunity();
            bizOpportunity biz = new bizOpportunity();

            //OPPORTUNITY//
            o.ClientID = int.Parse(Request.QueryString["cid"]);
            o.Inactive = false;
            o.Flagged = false;
            o.OpportunityName = this.txtOpportunityName.Text;
            if (this.txtOpportunityDue.Text != "") o.OpportunityDue = DateTime.Parse(this.txtOpportunityDue.Text);
            o.IncumbentBroker = this.txtIncumbentBroker.Text;
            o.IncumbentInsurer = this.txtIncumbentInsurer.Text;
            if (this.ddlClassification.SelectedValue != "") o.ClassificationID = int.Parse(this.ddlClassification.SelectedValue);
            if (this.ddlBusinessType.SelectedValue != "") o.BusinessTypeID = int.Parse(this.ddlBusinessType.SelectedValue);
            if (this.ddlContact.SelectedValue != "") o.ContactID = int.Parse(this.ddlContact.SelectedValue);
            if (this.txtNetBrokerageQuoted.Text != "") o.NetBrokerageQuoted = decimal.Parse(this.txtNetBrokerageQuoted.Text);
            if (this.txtNetBrokerageActual.Text != "") o.NetBrokerageActual = decimal.Parse(this.txtNetBrokerageActual.Text);
            //audit
            o.AddedBy = bizUser.GetCurrentUserName();
            o.Added = DateTime.Now;

            if (biz.ValidateOpportunity(o) == false)
            {
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                return;
            }

            //ACTIVITY//
            bizActivity bizA = new bizActivity();
            char ot = 'S';
            List<sp_web_ListStatusesByOutcomeTypeResult> ss = bizA.ListStatusesByOutcomeType(ot);
            foreach (var s in ss)
            {
                Activity na = new Activity();
                //general
                na.OpportunityStatusID = s.StatusID;
                if (this.txtFollowUpDate.Text != "") na.FollowUpDate = DateTime.Parse(this.txtFollowUpDate.Text);
                na.Inactive = false;
                if (s.StatusName == "Accepted") na.ActivityNote = this.txtActivityNote.Text;
                else na.ActivityNote = "";
                //audit
                na.AddedBy = bizUser.GetCurrentUserName();
                na.Added = DateTime.Now;
                o.Activities.Add(na);
            }

            foreach (Activity na in o.Activities)
            {
                if (bizA.ValidateActivity(na) == false)
                {
                    this.ucMessanger1.ProcessMessages(bizA.MSGS, true);
                    return;
                }
            }

            int oid = biz.InsertOpportunity(o);
            if (oid != 0)
            {
                Response.Redirect("ViewClient.aspx?cid=" + Request.QueryString["cid"], false);
            }
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
        }

        private void InsertQuickCall()
        {
            Opportunity o = new Opportunity();
            bizOpportunity biz = new bizOpportunity();

            //OPPORTUNITY//
            o.ClientID = int.Parse(Request.QueryString["cid"]);
            o.Inactive = false;
            o.Flagged = false;
            o.OpportunityName = this.txtOpportunityName.Text;
            if (this.txtOpportunityDue.Text != "") o.OpportunityDue = DateTime.Parse(this.txtOpportunityDue.Text);
            o.IncumbentBroker = this.txtIncumbentBroker.Text;
            o.IncumbentInsurer = this.txtIncumbentInsurer.Text;
            if (this.ddlClassification.SelectedValue != "") o.ClassificationID = int.Parse(this.ddlClassification.SelectedValue);
            if (this.ddlBusinessType.SelectedValue != "") o.BusinessTypeID = int.Parse(this.ddlBusinessType.SelectedValue);
            if (this.ddlContact.SelectedValue != "") o.ContactID = int.Parse(this.ddlContact.SelectedValue);
            //audit
            o.AddedBy = bizUser.GetCurrentUserName();
            o.Added = DateTime.Now;

            if (biz.ValidateQuickQuoteOpportunity(o) == false)
            {
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                return;
            }

            //ACTIVITY//
            bizActivity bizA = new bizActivity();
            char ot = 'S';
            List<sp_web_ListStatusesByOutcomeTypeResult> ss = bizA.ListStatusesByOutcomeType(ot);
            foreach (var s in ss)
            {
                Activity na = new Activity();
                //general
                na.OpportunityStatusID = s.StatusID;
                if (this.txtFollowUpDate.Text != "") na.FollowUpDate = DateTime.Parse(this.txtFollowUpDate.Text);
                na.Inactive = false;
                if (s.StatusName == "Go-to-Market") na.ActivityNote = this.txtActivityNote.Text;
                else na.ActivityNote = "Quick call";
                //audit
                na.AddedBy = bizUser.GetCurrentUserName();
                na.Added = DateTime.Now;
                o.Activities.Add(na);
                if (s.StatusName == "Go-to-Market") break;
            }

            foreach (Activity na in o.Activities)
            {
                if (bizA.ValidateActivity(na) == false)
                {
                    this.ucMessanger1.ProcessMessages(bizA.MSGS, true);
                    return;
                }
            }

            int oid = biz.InsertOpportunity(o);
            if (oid != 0)
            {
                Response.Redirect("ViewClient.aspx?cid=" + Request.QueryString["cid"], false);
            }
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
        }
        
        protected void btnCancel_Click(object sender, EventArgs e)
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

        protected void ddlClassification_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ddlClassification.SelectedValue != "")
                {
                    SetDefaultValues();
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", true);
            }
        }

        protected void ddlBusinessType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SetBusinessTypeControls();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", true);
            }
        }
    }
}
