using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
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
    public partial class AddClient : System.Web.UI.Page
    {
        #region Protected Properties
        
        protected string AddressClientId
        {
            get { return this.txtAddress.ClientID; }
        }
        protected string SuburbClientId
        {
            get { return this.ucAUPSS1.SuburbControl.ClientID; }
        }
        //protected string StateCodeClientId
        //{
        //    get { return this.ucAUPSS1.StateCodeControl.ClientID; }
        //}
        protected string PostCodeClientId
        {
            get { return this.ucAUPSS1.PostCodeControl.ClientID; }
        }

        #endregion

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
                    PopulateClassification();
                    PopulateBusinessType();
                    if (Request.QueryString["cc"] != null) PopulateClientFromBA();
                    SetBusinessTypeControls();
                }
                SetAddressControls();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void SetControls()
        {
            ((Main)Master).HeaderTitle = "Add Client";
            bizSetting bizS = new bizSetting();

            ((Main)Master).HeaderDetails = "Client to be added by "
                                            + bizActiveDirectory.GetUserFullName(bizS.GetSetting(bizUser.GetCurrentUserNameWithoutDomain()))
                                            + " (Now)";
            //executive
            bizClient biz = new bizClient();
            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
            bizUser.enGageUser exec = bizUser.GetAccountExecutive(user.SMIUserName);
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            this.lblAccountExecutive.Text = "<b>" + exec.DisplayName + "</b>" + ", " + exec.Branch + " (" + exec.Region + ")";
            //defaults
            this.ddlFlagged.SelectedValue = "false";

            if (Request.QueryString["name"] != null)
            {
                txtClientName.Text = HttpUtility.UrlDecode(Request.QueryString["name"]);
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

        private void PopulateIndustries()
        {
            bizMessage bizM = new bizMessage();

            bizIndustry biz = new bizIndustry();
            IQueryable<Industry> inds = biz.ListIndustriesByKeyword(this.txtFindIndustry.Text);
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);

            this.lstIndustry.Items.Clear();
            if (this.txtFindIndustry.Text != "")
            {
                foreach (Industry ind in inds)
                {
                    this.lstIndustry.Items.Add(new ListItem(ind.IndustryName + " (" + ind.AnzsicCode + ")", ind.AnzsicCode));
                }
                this.lstIndustry.Visible = true;
                this.lblFoundIndustries.Text = this.lstIndustry.Items.Count.ToString() + " industries found";
            }
            else
            {
                this.lstIndustry.Visible = false;
                this.lblFoundIndustries.Text = "";
            }
        }

        private void PopulateAssociations()
        {
            bizMessage bizM = new bizMessage();

            bizClient biz = new bizClient();
            IQueryable<Association> asss = biz.ListAssociationsByIndustry(this.lstIndustry.SelectedValue);
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);

            this.ddlAssociation.Items.Clear();
            this.ddlAssociation.Items.Add(new ListItem("-- Please Select --", ""));
            foreach (Association ass in asss)
            {
                this.ddlAssociation.Items.Add(new ListItem(ass.AssociationName, ass.AssociationCode));
            }
        }

        private void PopulateBusinessType()
        {
           
            bizOpportunity biz = new bizOpportunity();
            List<BusinessType> bts = biz.ListBusinessTypes();
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            //this.ddlBusinessType.Items.Add(new ListItem("-- Please Select --", ""));

            // remove quick win and quick qoute

            bts = bts.Where(x => (x.BusinessTypeName != "Quick win" && x.BusinessTypeName != "Quick quote")).ToList();
            foreach (BusinessType bt in bts)
            {
                this.ddlBusinessType.Items.Add(new ListItem(bt.BusinessTypeName, bt.BusinessTypeID.ToString()));
            }
            //if (this.ddlBusinessType.Items.Count > 0) this.ddlBusinessType.SelectedIndex = 0;
        }

        private void PopulateClientFromBA()
        {
            bizMessage bizM = new bizMessage();
            bizClient biz = new bizClient();
            List<sp_engage_get_clientResult> client;
            client = biz.GetClientFromBA(Request.QueryString["cc"].ToString());
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);

            if (client.Count == 1)
            {
                this.txtClientCode.Text = client[0].ClientCode;
                this.txtClientName.Text = client[0].ClientName;
                this.txtAddress.Text = client[0].Address_Line_1 + "\n" + 
                                       client[0].Address_Line_2 + "\n" + 
                                       client[0].Address_Line_3;
                this.txtOfficePhone.Text = client[0].Phone;
                this.txtOfficeFacsimilie.Text = client[0].Fax;
                this.txtABNACN.Text = client[0].ABN;
                int result;
                if (int.TryParse(client[0].Postcode, out result) == true &&
                    client[0].Postcode.Length == 4)
                {
                    this.ucAUPSS1.PostCode = client[0].Postcode;
                }
                //if (this.ucAUPSS1.SuburbControl.Items.FindByValue(client[0].Suburb) != null)
                //{
                //    this.ucAUPSS1.Suburb = client[0].Suburb;
                //}
                this.txtSource.Text = "iBAIS";
                if (client[0].Anzsic_Code != "") this.txtFindIndustry.Text = client[0].Anzsic_Code;
                PopulateIndustries();
                if (this.lstIndustry.Items.Count == 1) this.lstIndustry.SelectedIndex = 0;
                if (client[0].ActiveClientInd.ToString() == "N")
                {
                    if (this.ddlBusinessType.Items.Count > 0) this.ddlBusinessType.SelectedValue = this.ddlBusinessType.Items.FindByText("Reclaimed business").Value;
                }
                else
                {
                    if (this.ddlBusinessType.Items.Count > 0) this.ddlBusinessType.SelectedValue = this.ddlBusinessType.Items.FindByText("New business (Existing clients)").Value;
                }
            }
        }

        private void SetBusinessTypeControls()
        {
            switch (this.ddlBusinessType.SelectedItem.Text)
            {
                case "Quick quote": // go-to-market
                    this.trActivityNote.Visible = true;
                    this.trQuickWin1.Visible = false;
                    this.trQuickWin2.Visible = false;
                    break;
                case "Quick win": // accepted
                    this.trActivityNote.Visible = true;
                    this.trQuickWin1.Visible = true;
                    this.trQuickWin2.Visible = true;
                    break;
                case "Quick call": // go-to-market
                    this.trActivityNote.Visible = true;
                    this.trQuickWin1.Visible = false;
                    this.trQuickWin2.Visible = false;
                    break;
                default:
                    this.trActivityNote.Visible = false;
                    this.trQuickWin1.Visible = false;
                    this.trQuickWin2.Visible = false;
                    break;
            }
        }

        private void SetAddressControls()
        {
            if (this.rblAddressTypes.SelectedIndex == 1)
            {
                this.trAUPSS.Style.Remove("display");
                this.trAUPSS.Style.Add("display", "none");
            }
            else
            {
                this.trAUPSS.Style.Remove("display");
                this.trAUPSS.Style.Add("display", "");
            }
        }

        private bool UIValidation()
        {
            bizMessage bizM = new bizMessage();

            if (this.txtSource.Text.Trim().Length ==0)
            {
                this.ucMessanger1.ProcessMessage("Source: " + bizM.GetMessageText("EmptySource"), Enums.enMsgType.Err, "Source", typeof(TextBox), true);
                return false;
            }

            //if (this.txtInsuredName.Text.Trim().Length == 0)
            //{
            //    this.ucMessanger1.ProcessMessage("Insured As: " + bizM.GetMessageText("EmptySource"), Enums.enMsgType.Err, "InsuredName", typeof(TextBox), true);
            //    return false;
            //}

            if (this.txtOfficePhone.Text.Trim().Length ==0)
            {
                this.ucMessanger1.ProcessMessage("Phone: " + bizM.GetMessageText("EmptySource"), Enums.enMsgType.Err, "OfficePhone", typeof(TextBox), true);
                return false;
            }

            if (this.txtAddress.Text.Length > this.txtAddress.MaxLength)
            {
                this.ucMessanger1.ProcessMessage("Address: " + bizM.GetMessageText("ValueGreaterThanMax"), Enums.enMsgType.Err, "", null, true);
                return false;
            }

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
                    //if (this.ddlClassification.SelectedValue == "")
                    //{
                    //    this.ucMessanger1.ProcessMessage("CSS Segment: " + bizM.GetMessageText("ValueNotSelected"), Enums.enMsgType.Err, "Classification", typeof(DropDownList), true);
                    //    return false;
                    //}
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
                    //if (this.ddlClassification.SelectedValue == "")
                    //{
                    //    this.ucMessanger1.ProcessMessage("CSS Segment: " + bizM.GetMessageText("ValueNotSelected"), Enums.enMsgType.Err, "Classification", typeof(DropDownList), true);
                    //    return false;
                    //}
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
                    //if (this.txtOpportunityDue.Text != "")
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
                    case "Quick call":
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
            Client c = new Client();
            bizClient biz = new bizClient();

            //CLIENT//
            //general
            c.Inactive = false;
            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
            c.AccountExecutiveID = user.UserName;
            c.ClientCode = this.txtClientCode.Text;
            c.ClientName = this.txtClientName.Text;
            c.RegisteredName = this.txtRegisteredName.Text;
            //c.InsuredName = this.txtInsuredName.Text;
            if (this.txtABNACN.Text != "") c.ABNACN = this.txtABNACN.Text;
            c.Source = this.txtSource.Text;
            c.OfficeFacsimilie = this.txtOfficeFacsimilie.Text;
            c.OfficePhone = this.txtOfficePhone.Text;
            //address
            if (this.txtAddress.Text != "") c.Address = this.txtAddress.Text;
            if (this.rblAddressTypes.SelectedIndex == 0)
            {
                if (this.ucAUPSS1.PostCode != "") c.PostCode = this.ucAUPSS1.PostCode;
                if (this.ucAUPSS1.StateCode != "") c.StateCode = this.ucAUPSS1.StateCode;
                if (this.ucAUPSS1.Suburb != "") c.Location = this.ucAUPSS1.Suburb;
            }
            //industry
            if (this.lstIndustry.SelectedValue != "")
                c.AnzsicCode = this.lstIndustry.SelectedValue;
            if (this.ddlAssociation.SelectedValue != "")
                c.AssociationCode = this.ddlAssociation.SelectedValue;
            c.AssociationMemberNumber = this.txtAssociationMemberNumber.Text;
            //audit
            c.AddedBy = bizUser.GetCurrentUserName();
            c.Added = DateTime.Now;

            //CONTACT//
            Contact cl = new Contact();
            bizContact bizC = new bizContact();
            if (this.ckbAddContact.Checked == true)
            {
                //general
                cl.ContactName = this.txtContactName.Text;
                cl.Title = this.ddlTitle.SelectedValue;
                cl.Mobile = this.txtMobile.Text;
                cl.DirectLine = this.txtDirectLine.Text;
                cl.Email = this.txtEmail.Text;
                //audit
                cl.AddedBy = bizUser.GetCurrentUserName();
                cl.Added = DateTime.Now;
            }

            //OPPORTUNITY//
            Opportunity o = new Opportunity();
            bizOpportunity bizO = new bizOpportunity();
            //general
            o.OpportunityName = this.txtOpportunityName.Text;
            if (this.txtOpportunityDue.Text != "") o.OpportunityDue = DateTime.Parse(this.txtOpportunityDue.Text);
            o.IncumbentBroker = this.txtIncumbentBroker.Text;
            o.IncumbentInsurer = this.txtIncumbentInsurer.Text;
            if (this.ddlClassification.SelectedValue != "") o.ClassificationID = int.Parse(this.ddlClassification.SelectedValue);
            if (this.ddlBusinessType.SelectedValue != "") o.BusinessTypeID = int.Parse(this.ddlBusinessType.SelectedValue);
            if (this.ddlFlagged.SelectedValue != "") o.Flagged = bool.Parse(this.ddlFlagged.SelectedValue);
            //audit
            o.AddedBy = bizUser.GetCurrentUserName();
            o.Added = DateTime.Now;

            //ACTIVITY//
            Activity na = new Activity();
            bizActivity bizA = new bizActivity();
            //general
            na.OpportunityStatusID = bizA.GetInitialStatus().StatusID;
            if (this.txtFollowUpDate.Text != "") na.FollowUpDate = DateTime.Parse(this.txtFollowUpDate.Text);
            na.Inactive = false;
            na.ActivityNote = "";
            //audit
            na.AddedBy = bizUser.GetCurrentUserName();
            na.Added = DateTime.Now;

            //action
            if (biz.ValidateClient(c) == false)
            {
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                return;
            }
            if (this.ckbAddContact.Checked == true)
            {
                if (bizC.ValidateContact(cl) == false)
                {
                    this.ucMessanger1.ProcessMessages(bizC.MSGS, true);
                    return;
                }
                c.Contacts.Add(cl);
            }
            if (bizO.ValidateOpportunity(o) == false)
            {
                this.ucMessanger1.ProcessMessages(bizO.MSGS, true);
                return;
            }
            c.Opportunities.Add(o);
            if (bizA.ValidateActivity(na,true) == false)
            {
                this.ucMessanger1.ProcessMessages(bizA.MSGS, true);
                return;
            }
            o.Activities.Add(na);
            int oid = biz.InsertClient(c);
            if (oid != 0)
            {
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                Response.Redirect("ViewClient.aspx?cid=" + oid.ToString(), false);
            }
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);     
        }

        private void InsertQuickQuote()
        {
            Client c = new Client();
            bizClient biz = new bizClient();

            //CLIENT//
            //general
            c.Inactive = false;
            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
            c.AccountExecutiveID = user.UserName;
            c.ClientCode = this.txtClientCode.Text;
            c.ClientName = this.txtClientName.Text;
            c.RegisteredName = this.txtRegisteredName.Text;
            //c.InsuredName = this.txtInsuredName.Text;
            if (this.txtABNACN.Text != "") c.ABNACN = this.txtABNACN.Text;
            c.Source = this.txtSource.Text;
            c.OfficeFacsimilie = this.txtOfficeFacsimilie.Text;
            c.OfficePhone = this.txtOfficePhone.Text;
            //address
            if (this.txtAddress.Text != "") c.Address = this.txtAddress.Text;
            if (this.rblAddressTypes.SelectedIndex == 0)
            {
                if (this.ucAUPSS1.PostCode != "") c.PostCode = this.ucAUPSS1.PostCode;
                if (this.ucAUPSS1.StateCode != "") c.StateCode = this.ucAUPSS1.StateCode;
                if (this.ucAUPSS1.Suburb != "") c.Location = this.ucAUPSS1.Suburb;
            }
            //industry
            if (this.lstIndustry.SelectedValue != "")
                c.AnzsicCode = this.lstIndustry.SelectedValue;
            if (this.ddlAssociation.SelectedValue != "")
                c.AssociationCode = this.ddlAssociation.SelectedValue;
            c.AssociationMemberNumber = this.txtAssociationMemberNumber.Text;
            //audit
            c.AddedBy = bizUser.GetCurrentUserName();
            c.Added = DateTime.Now;

            if (biz.ValidateClient(c) == false)
            {
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                return;
            }

            //CONTACT//
            Contact cl = new Contact();
            bizContact bizC = new bizContact();
            if (this.ckbAddContact.Checked == true)
            {
                //general
                cl.ContactName = this.txtContactName.Text;
                cl.Title = this.ddlTitle.SelectedValue;
                cl.Mobile = this.txtMobile.Text;
                cl.DirectLine = this.txtDirectLine.Text;
                cl.Email = this.txtEmail.Text;
                //audit
                cl.AddedBy = bizUser.GetCurrentUserName();
                cl.Added = DateTime.Now;
            }

            if (this.ckbAddContact.Checked == true)
            {
                if (bizC.ValidateContact(cl) == false)
                {
                    this.ucMessanger1.ProcessMessages(bizC.MSGS, true);
                    return;
                }
                c.Contacts.Add(cl);
            }

            //OPPORTUNITY//
            Opportunity o = new Opportunity();
            bizOpportunity bizO = new bizOpportunity();
            //general
            o.OpportunityName = this.txtOpportunityName.Text;
            if (this.txtOpportunityDue.Text != "") o.OpportunityDue = DateTime.Parse(this.txtOpportunityDue.Text);
            o.IncumbentBroker = this.txtIncumbentBroker.Text;
            o.IncumbentInsurer = this.txtIncumbentInsurer.Text;
            if (this.ddlClassification.SelectedValue != "") o.ClassificationID = int.Parse(this.ddlClassification.SelectedValue);
            if (this.ddlBusinessType.SelectedValue != "") o.BusinessTypeID = int.Parse(this.ddlBusinessType.SelectedValue);
            if (this.ddlFlagged.SelectedValue != "") o.Flagged = bool.Parse(this.ddlFlagged.SelectedValue);
            //audit
            o.AddedBy = bizUser.GetCurrentUserName();
            o.Added = DateTime.Now;

            if (bizO.ValidateQuickQuoteOpportunity(o) == false)
            {
                this.ucMessanger1.ProcessMessages(bizO.MSGS, true);
                return;
            }
            c.Opportunities.Add(o);

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
                else na.ActivityNote = "Quick quote";
                //audit
                na.AddedBy = bizUser.GetCurrentUserName();
                na.Added = DateTime.Now;
                o.Activities.Add(na);
                if (s.StatusName == "Go-to-Market") break;
            }

            foreach (Activity na in o.Activities)
            {
                if (bizA.ValidateActivity(na,true) == false)
                {
                    this.ucMessanger1.ProcessMessages(bizA.MSGS, true);
                    return;
                }
            }

            int oid = biz.InsertClient(c);
            if (oid != 0)
            {
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                Response.Redirect("ViewClient.aspx?cid=" + oid.ToString(), false);
            }
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);     
        }

        private void InsertQuickWin()
        {
            Client c = new Client();
            bizClient biz = new bizClient();

            //CLIENT//
            //general
            c.Inactive = false;
            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
            c.AccountExecutiveID = user.UserName;
            c.ClientCode = this.txtClientCode.Text;
            c.ClientName = this.txtClientName.Text;
            c.RegisteredName = this.txtRegisteredName.Text;
            //c.InsuredName = this.txtInsuredName.Text;
            if (this.txtABNACN.Text != "") c.ABNACN = this.txtABNACN.Text;
            c.Source = this.txtSource.Text;
            c.OfficeFacsimilie = this.txtOfficeFacsimilie.Text;
            c.OfficePhone = this.txtOfficePhone.Text;
            //address
            if (this.txtAddress.Text != "") c.Address = this.txtAddress.Text;
            if (this.rblAddressTypes.SelectedIndex == 0)
            {
                if (this.ucAUPSS1.PostCode != "") c.PostCode = this.ucAUPSS1.PostCode;
                if (this.ucAUPSS1.StateCode != "") c.StateCode = this.ucAUPSS1.StateCode;
                if (this.ucAUPSS1.Suburb != "") c.Location = this.ucAUPSS1.Suburb;
            }
            //industry
            if (this.lstIndustry.SelectedValue != "")
                c.AnzsicCode = this.lstIndustry.SelectedValue;
            if (this.ddlAssociation.SelectedValue != "")
                c.AssociationCode = this.ddlAssociation.SelectedValue;
            c.AssociationMemberNumber = this.txtAssociationMemberNumber.Text;
            //audit
            c.AddedBy = bizUser.GetCurrentUserName();
            c.Added = DateTime.Now;

            if (biz.ValidateClient(c) == false)
            {
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                return;
            }

            //CONTACT//
            Contact cl = new Contact();
            bizContact bizC = new bizContact();
            if (this.ckbAddContact.Checked == true)
            {
                //general
                cl.ContactName = this.txtContactName.Text;
                cl.Title = this.ddlTitle.SelectedValue;
                cl.Mobile = this.txtMobile.Text;
                cl.DirectLine = this.txtDirectLine.Text;
                cl.Email = this.txtEmail.Text;
                //audit
                cl.AddedBy = bizUser.GetCurrentUserName();
                cl.Added = DateTime.Now;
            }

            if (this.ckbAddContact.Checked == true)
            {
                if (bizC.ValidateContact(cl) == false)
                {
                    this.ucMessanger1.ProcessMessages(bizC.MSGS, true);
                    return;
                }
                c.Contacts.Add(cl);
            }

            //OPPORTUNITY//
            Opportunity o = new Opportunity();
            bizOpportunity bizO = new bizOpportunity();
            //general
            o.OpportunityName = this.txtOpportunityName.Text;
            if (this.txtOpportunityDue.Text != "") o.OpportunityDue = DateTime.Parse(this.txtOpportunityDue.Text);
            o.IncumbentBroker = this.txtIncumbentBroker.Text;
            o.IncumbentInsurer = this.txtIncumbentInsurer.Text;
            if (this.ddlClassification.SelectedValue != "") o.ClassificationID = int.Parse(this.ddlClassification.SelectedValue);
            if (this.ddlBusinessType.SelectedValue != "") o.BusinessTypeID = int.Parse(this.ddlBusinessType.SelectedValue);
            if (this.ddlFlagged.SelectedValue != "") o.Flagged = bool.Parse(this.ddlFlagged.SelectedValue);
            o.NetBrokerageQuoted = decimal.Parse(this.txtNetBrokerageQuoted.Text);
            o.NetBrokerageActual = decimal.Parse(this.txtNetBrokerageActual.Text);
            //audit
            o.AddedBy = bizUser.GetCurrentUserName();
            o.Added = DateTime.Now;

            if (bizO.ValidateQuickQuoteOpportunity(o) == false)
            {
                this.ucMessanger1.ProcessMessages(bizO.MSGS, true);
                return;
            }
            c.Opportunities.Add(o);

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
                else na.ActivityNote = "Quick win";
                //audit
                na.AddedBy = bizUser.GetCurrentUserName();
                na.Added = DateTime.Now;
                o.Activities.Add(na);
            }

            foreach (Activity na in o.Activities)
            {
                if (bizA.ValidateActivity(na,true) == false)
                {
                    this.ucMessanger1.ProcessMessages(bizA.MSGS, true);
                    return;
                }
            }

            int oid = biz.InsertClient(c);
            if (oid != 0)
            {
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                Response.Redirect("ViewClient.aspx?cid=" + oid.ToString(), false);
            }
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);     
        }

        private void InsertQuickCall()
        {
            Client c = new Client();
            bizClient biz = new bizClient();

            //CLIENT//
            //general
            c.Inactive = false;
            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
            c.AccountExecutiveID = user.UserName;
            c.ClientCode = this.txtClientCode.Text;
            c.ClientName = this.txtClientName.Text;
            c.RegisteredName = this.txtRegisteredName.Text;
            //c.InsuredName = this.txtInsuredName.Text;
            if (this.txtABNACN.Text != "") c.ABNACN = this.txtABNACN.Text;
            c.Source = this.txtSource.Text;
            c.OfficeFacsimilie = this.txtOfficeFacsimilie.Text;
            c.OfficePhone = this.txtOfficePhone.Text;
            //address
            if (this.txtAddress.Text != "") c.Address = this.txtAddress.Text;
            if (this.rblAddressTypes.SelectedIndex == 0)
            {
                if (this.ucAUPSS1.PostCode != "") c.PostCode = this.ucAUPSS1.PostCode;
                if (this.ucAUPSS1.StateCode != "") c.StateCode = this.ucAUPSS1.StateCode;
                if (this.ucAUPSS1.Suburb != "") c.Location = this.ucAUPSS1.Suburb;
            }
            //industry
            if (this.lstIndustry.SelectedValue != "")
                c.AnzsicCode = this.lstIndustry.SelectedValue;
            if (this.ddlAssociation.SelectedValue != "")
                c.AssociationCode = this.ddlAssociation.SelectedValue;
            c.AssociationMemberNumber = this.txtAssociationMemberNumber.Text;
            //audit
            c.AddedBy = bizUser.GetCurrentUserName();
            c.Added = DateTime.Now;

            if (biz.ValidateClient(c) == false)
            {
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                return;
            }

            //CONTACT//
            Contact cl = new Contact();
            bizContact bizC = new bizContact();
            if (this.ckbAddContact.Checked == true)
            {
                //general
                cl.ContactName = this.txtContactName.Text;
                cl.Title = this.ddlTitle.SelectedValue;
                cl.Mobile = this.txtMobile.Text;
                cl.DirectLine = this.txtDirectLine.Text;
                cl.Email = this.txtEmail.Text;
                //audit
                cl.AddedBy = bizUser.GetCurrentUserName();
                cl.Added = DateTime.Now;
            }

            if (this.ckbAddContact.Checked == true)
            {
                if (bizC.ValidateContact(cl) == false)
                {
                    this.ucMessanger1.ProcessMessages(bizC.MSGS, true);
                    return;
                }
                c.Contacts.Add(cl);
            }

            //OPPORTUNITY//
            Opportunity o = new Opportunity();
            bizOpportunity bizO = new bizOpportunity();
            //general
            o.OpportunityName = this.txtOpportunityName.Text;
            if (this.txtOpportunityDue.Text != "") o.OpportunityDue = DateTime.Parse(this.txtOpportunityDue.Text);
            o.IncumbentBroker = this.txtIncumbentBroker.Text;
            o.IncumbentInsurer = this.txtIncumbentInsurer.Text;
            if (this.ddlClassification.SelectedValue != "") o.ClassificationID = int.Parse(this.ddlClassification.SelectedValue);
            if (this.ddlBusinessType.SelectedValue != "") o.BusinessTypeID = int.Parse(this.ddlBusinessType.SelectedValue);
            if (this.ddlFlagged.SelectedValue != "") o.Flagged = bool.Parse(this.ddlFlagged.SelectedValue);
            //o.NetBrokerageQuoted = decimal.Parse(this.txtNetBrokerageQuoted.Text);
            //o.NetBrokerageActual = decimal.Parse(this.txtNetBrokerageActual.Text);
            //audit
            o.AddedBy = bizUser.GetCurrentUserName();
            o.Added = DateTime.Now;

            if (bizO.ValidateQuickQuoteOpportunity(o) == false)
            {
                this.ucMessanger1.ProcessMessages(bizO.MSGS, true);
                return;
            }
            c.Opportunities.Add(o);

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
                if (bizA.ValidateActivity(na,true) == false)
                {
                    this.ucMessanger1.ProcessMessages(bizA.MSGS, true);
                    return;
                }
            }

            int oid = biz.InsertClient(c);
            if (oid != 0)
            {
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                Response.Redirect("ViewClient.aspx?cid=" + oid.ToString(), false);
            }
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);  
        }

        protected void btnFindIndustry_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtFindIndustry.Text == "")
                {
                    this.lblFoundIndustries.Text = "You have to type a keyword";
                }
                else
                {
                    PopulateIndustries();
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void lstIndustry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PopulateAssociations();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void ckbAddContact_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.tblContact.Visible = this.ckbAddContact.Checked;
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
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
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

    }
}
