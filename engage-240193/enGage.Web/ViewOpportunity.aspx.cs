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
    public partial class EnhancedOpportunity : System.Web.UI.Page
    {
        // controls to validate
        DropDownList ddlOpportunityStatus, ddlContact, ddlClassification;
        TextBox txtActivityNote, txtFollowUpDate, txtDateCompleted, txtOpportunityDue, txtNetBrokerageEstimated,txtNetBrokerageQuoted, txtNetBrokerageActual, txtMemoNumber, txtClientCode,txtInsuredAs;
        CheckBox chkPersonalLines;


        public Activity LoadedActivity
        {
            get;
            set;
        }

        public int CurrentCalssificationID
        {
            get { return (int)ViewState["cssId"]; }
            set { ViewState["cssId"] = value; }
        }

        [Serializable]
        public class OpportunityProxy
        {
            public decimal? NetBrokerageEstimated;
            public decimal? NetBrokerageQuoted;
            public decimal? NetBrokerageActual;
            public string InsuredAs;
        }
        public OpportunityProxy CurrentOpportunity
        {
            get { return (OpportunityProxy)ViewState["Oppo"]; }
            set { ViewState["Oppo"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }


        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    ((Main)Master).HeaderTitle = "Work with opportunity";
                    // set the controls to save,populate and validate
                    SetSelectedExpandedPanelControls();

                    PopulateClientDetails();
                    PopulateOpportunityDetails();
                    PopulateStatusDetails();
                    PopulateContacts();

                    PopulateActivityDetails();
                    PopulateClassification();
                    SetAdditionalControls();
                    PopulateAdditionalControls();
                }

                SetSelectedExpandedPanelControls();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
            
            base.OnPreRender(e);
        }
        private void ClearControl( Control control )
        {
            try
            {
                var textbox = control as TextBox;
                if (textbox != null)
                    textbox.Text = string.Empty;

                var dropDownList = control as DropDownList;
                if (dropDownList != null && dropDownList.Items.Count!=0)
                    dropDownList.SelectedIndex = 0;

                foreach (Control childControl in control.Controls)
                {
                    ClearControl(childControl);
                }

                // hide the aditional detail section
                SetAdditionalControls();
            }
            catch(Exception ex)
            {
                // hide all errors captured here
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

            // set the current opportunity
            CurrentOpportunity = new OpportunityProxy() { 
                NetBrokerageEstimated = o.NetBrokerageEstimated,
                NetBrokerageQuoted= o.NetBrokerageQuoted,
                NetBrokerageActual = o.NetBrokerageActual,
                    InsuredAs = (o.Client != null) ? o.Client.InsuredName : null
            };

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
            if (o.ClassificationID != null) this.lblClassification.Text = (o.Classification==null)?"" :o.Classification.ClassificationName;
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
                    + bizActiveDirectory.GetUserFullName(o.ModifiedBy) + " (" + string.Format("{0:dd-MMM-yy}", o.Modified.Value) + ")";

            // set current viewstate for classififcation id
            CurrentCalssificationID = -1;
            if(o.ClassificationID.HasValue)
                CurrentCalssificationID = o.ClassificationID.Value;
        }

        private void PopulateStatusDetails()
        {
            var textProperty = "StatusName";
            var valueProperty = "StatusID";

            ddlQualifyOpportunityStatus.DataSource = new bizActivity().GetOppprtunityStepStatuses(Enums.OpportunitySteps.Qualifying);
            ddlQualifyOpportunityStatus.DataTextField = textProperty;
            ddlQualifyOpportunityStatus.DataValueField = valueProperty;
            ddlQualifyOpportunityStatus.DataBind();

            ddlRespondStatus.DataSource = new bizActivity().GetOppprtunityStepStatuses(Enums.OpportunitySteps.Responding);
            ddlRespondStatus.DataTextField = textProperty;
            ddlRespondStatus.DataValueField = valueProperty;
            ddlRespondStatus.DataBind();

            ddlCompleteStatus.DataSource = new bizActivity().GetOppprtunityStepStatuses(Enums.OpportunitySteps.Completing);
            ddlCompleteStatus.DataTextField = textProperty;
            ddlCompleteStatus.DataValueField = valueProperty;
            ddlCompleteStatus.DataBind();

            ddlQualifyOpportunityStatus.Items.Insert(0, new ListItem("-- Please Select --", ""));
            ddlRespondStatus.Items.Insert(0, new ListItem("-- Please Select --", ""));
            ddlCompleteStatus.Items.Insert(0,new ListItem("-- Please Select --", ""));

            AccordionCtrl.DataBind();

        }

        private void PopulateContacts()
        {
            bizClient biz = new bizClient();
            List<sp_web_ListClientContactsResult> ccs = biz.ListClientContacts(int.Parse(Request.QueryString["cid"]));
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);

            PopulateContactsDropDownLists(ddlQualifyContact,ccs);
            PopulateContactsDropDownLists(ddlRespondContacts, ccs);
            PopulateContactsDropDownLists(ddlCompleteContacts, ccs);


        }
        private void PopulateContactsDropDownLists(DropDownList passedDropDownList,List<sp_web_ListClientContactsResult> contactsList)
        {

            passedDropDownList.Items.Add(new ListItem("-- Please Select --", ""));
            foreach (sp_web_ListClientContactsResult cc in contactsList)
            {
                passedDropDownList.Items.Add(new ListItem(cc.ContactName, cc.ContactID.ToString()));
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            RefreshPage();
        }

        protected void btnSaveAndReminder_Click(object sender, EventArgs e)
        {
            int? activityId=SaveActivity(false);
            if (activityId.HasValue)
            {
                if (activityId.Value != 0)


                   
                //ScriptManager.RegisterStartupScript(this, typeof(string), "REFREH_WINDOW", "location.reload();", true);   
                   
                
                //SetReminder(activityId.Value);

                OpenReminderPage(int.Parse(Request.QueryString["oid"]), int.Parse(Request.QueryString["cid"]), activityId.Value);
                // if (!ClientScript.IsClientScriptBlockRegistered("REFRESH_WINDOW")) 
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "REFRESH_WINDOW", " function refreshPageAfterSaving(){setTimeout(window.location.href='" + Request.Url.AbsoluteUri + "', 0);} refreshPageAfterSaving();", true); 
                RefreshPage();
            }
        }

        private void OpenReminderPage(int opportunityID, int clientID, int activityID)
        {
            List<string> queryStringList = new List<string>();

            queryStringList.Add(String.Format("cid={0}", clientID));


            queryStringList.Add(String.Format("oid={0}", opportunityID));


            queryStringList.Add(String.Format("aid={0}", activityID));

            string url = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path).Replace(Request.Url.Segments[Request.Url.Segments.Length-1],"") + "OpenReminder.aspx?" + string.Join("&", queryStringList.ToArray());
            string fullURL = "window.open('" + url + "', '_blank', 'height=1,width=1,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );";
            if (!ClientScript.IsClientScriptBlockRegistered("OPEN_REMINDER")) 
            ClientScript.RegisterClientScriptBlock(this.GetType(), "OPEN_REMINDER", fullURL, true);
        }

        protected void btnClient_Click(object sender, EventArgs e)
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

        protected void RefreshPage()
        {
            try
            {
                //Response.Redirect(Request.Url.LocalPath + "?cid=" + Request.QueryString["cid"] + "&oid=" + Request.QueryString["oid"], false);
                //SetSelectedExpandedPanelControls();
                PopulateClientDetails();
                PopulateOpportunityDetails();
                    Page.DataBind();

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
                    //e.Row.Attributes["onClick"] = "location.href='ViewActivity.aspx?cid=" + Request.QueryString["cid"] + "&oid=" + Request.QueryString["oid"] + "&aid=" + DataBinder.Eval(e.Row.DataItem, "ActivityID") + "'";
                    e.Row.Attributes["onClick"] = "location.href='" + Request.Url.LocalPath +"?cid="+ Request.QueryString["cid"] + "&oid=" + Request.QueryString["oid"] + "&aid=" + DataBinder.Eval(e.Row.DataItem, "ActivityID") + "'";
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void grvActivities_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            // If multiple buttons are used in a GridView control, use the
            // CommandName property to determine which button was clicked.
            if (e.CommandName == "setreminder")
            {
                // Convert the row index stored in the CommandArgument
                 int clickedActivityID = Convert.ToInt32(e.CommandArgument);
                // get the activityId and generate reminder
                SetReminder(clickedActivityID);
            }
        }

        protected void SetReminder(int activityID)
        {

            bizClient biz = new bizClient();
            Client c;
            c = biz.GetClient(int.Parse(Request.QueryString["cid"]));

            bizOpportunity biz2 = new bizOpportunity();
            Opportunity o;
            o = biz2.GetOpportunity(int.Parse(Request.QueryString["oid"]));

            bizActivity biz3 = new bizActivity();
            Activity a;
            a = biz3.GetActivity(activityID);

            string subject = string.Format("Client Follow-up: {0}, {1} - {2}", c.ClientName, o.OpportunityName, a.Status.StatusName);

            //To do - set this to the name of the activity.

            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;

            if (a.FollowUpDate != null)
            {
                startDate = DateTime.Parse(string.Format("{0} 08:00 AM", string.Format("{0:dd/MM/yyyy}", a.FollowUpDate)));
                endDate = DateTime.Parse(string.Format("{0} 08:05 AM", string.Format("{0:dd/MM/yyyy}", a.FollowUpDate)));
            }

            string urlPort = (HttpContext.Current.Request.Url.IsDefaultPort) ? "" : ":" + HttpContext.Current.Request.Url.Port.ToString();
            string activityURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path)+"?";

            List<string> queryStringList = new List<string>();

            if (!string.IsNullOrEmpty(Request.QueryString["cid"]))
            {
                queryStringList.Add(String.Format("cid={0}", Request.QueryString["cid"]));
            }

            if (!string.IsNullOrEmpty(Request.QueryString["oid"]))
            {
                queryStringList.Add(String.Format("oid={0}", Request.QueryString["oid"]));
            }
          
           queryStringList.Add(String.Format("aid={0}", activityID));
            


            activityURL += string.Join("&", queryStringList.ToArray());
            var htmltab = @"&nbsp\;";
            var opportunityDue = ((a.Opportunity.OpportunityDue.HasValue) ? @"<B>Renewal Date :</B> " + htmltab+a.Opportunity.OpportunityDue.Value.ToString("dd/MM/yyyy") + "<br>" : "<br>");
           
            string t = "BEGIN:VCALENDAR\n" +
"PRODID:-//Microsoft Corporation//Outlook MIMEDIR//EN\n" +
"VERSION:2.0\n" +
"BEGIN:VEVENT\n" +
"DTSTART:" + startDate.ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + "\n" +
"DTEND:" + endDate.ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + "\n" +
                //"LOCATION:My office\n" +
                //"CATEGORIES:Business\n" +
"DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + Helper.CalenderUtilities.EncodeQuotedPrintable("Currently at:" + a.Status.StatusName +"\n"+
((a.Opportunity.OpportunityDue.HasValue)? "Renewal Date : "+a.Opportunity.OpportunityDue.Value.ToString("dd/MM/yyyy")+"\n":"")+
                                                                                           "Activity Note: " + a.ActivityNote + "\n\n" + activityURL) + "=0D=0A\n" +
"SUMMARY:" + subject + "\n" +
"PRIORITY:0\n" +
"X-ALT-DESC;FMTTYPE=text/html:"+@"<!DOCTYPE HTML PUBLIC \""-//W3C//DTD HTML 3.2//EN\"">\n<HTML>\n<HEAD>\n<TITLE></TITLE>\n</HEAD>\n<BODY>\n"+
"<b>Currently at:</b> " + htmltab + a.Status.StatusName +"<br>"+
opportunityDue +
@"<br><b>Activity Note:</b> " + htmltab+ a.ActivityNote + "<br><br>" + "<A HREF=\"" + HttpUtility.UrlEncode(activityURL) + "\">"+ activityURL+"</A>" + "<br>" + "</BODY>\n</HTML> " +
"X-MICROSOFT-CDO-BUSYSTATUS:FREE" +
"TRIGGER:-PT15M" +
"END:VEVENT" +
"END:VCALENDAR";

           
            Response.Clear();
            Response.ContentType = "application/VCS";
            Response.AddHeader("content-disposition", "attachment; filename=\"calendar.vcs\"");
            Response.Write(t.ToString());
            Response.End();

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

        private void SetPersonalLineOnlyCheckBox()
        {
            if (CurrentCalssificationID == Classifications.MappedClassificationsRanges.Where(x => x.PersonalLinesOnly == true).FirstOrDefault().ID)
                chkPersonalLines.Checked = true;  
        }

        private void SetAdditionalControls()
        {
            imgQualifyManadatory.Visible = imgRespondMandatroy.Visible = imgCompleteMandatory.Visible = true;
            // for Qalify Pane
            this.trQualifyLost1.Visible = false;
            this.trQualifyEstimated.Visible = false;
            // for Respond Pane
            this.trAccepted1.Visible = false;
            this.trLost1.Visible = false;
            this.trQualifiedIn1.Visible = false;
            this.trQualifiedIn2.Visible = false;
            this.trQualifiedIn3.Visible = false;
            this.trQualifiedIn4.Visible = false;
            this.trSubmitted1.Visible = false;
            this.trProcessed1.Visible = false;
            this.trProcessed2.Visible = false;

            // for Complete Pane
            this.trCompleteAccepted1.Visible = false;
            this.trCompleteLost1.Visible = false;
            this.trCompleteProcessed1.Visible = false;
            this.trCompleteProcessed2.Visible = false;

            if (ddlClassification != null && ddlClassification.Items.Count == 0)
                PopulateClassification();

            switch (this.ddlOpportunityStatus.SelectedItem.Text)
            {
                case "Identified":
                    break;
                case "Qualified-in":
                    trQualifyEstimated.Visible = true;
                    
                    break;
                case "Qualified-out":
                    this.trQualifyLost1.Visible = true;
                    imgQualifyManadatory.Visible = false;
                    break;
                case "Interested":
                    this.trQualifiedIn1.Visible = true;
                    this.trQualifiedIn2.Visible = true;
                    this.trQualifiedIn3.Visible = true;
                    this.trQualifiedIn4.Visible = true;
                    chkPersonalLines = chkRespondPersonalLinesOverride;
                    if (CurrentCalssificationID != -1)
                        ddlRespondClassification.SelectedValue = CurrentCalssificationID.ToString();
                    ddlRespondClassification.Enabled = false;
                    SetPersonalLineOnlyCheckBox();
                    break;
                case "Not Interested":
                    this.trLost1.Visible = true;
                    break;
                case "Go-to-Market":
                    break;
                case "Revisit next year":
                    this.trLost1.Visible = true;
                    imgRespondMandatroy.Visible = false;
                    break;
                case "Quoted":
                    this.trSubmitted1.Visible = true;
                    chkPersonalLines = chkRespondQutedPersonalLinesOverride;
                    SetPersonalLineOnlyCheckBox();
                    break;
                case "Can't Place":
                    this.trLost1.Visible = true;
                    imgRespondMandatroy.Visible = false;
                    break;
                case "Accepted":
                    this.trCompleteAccepted1.Visible = true;
                    break;
                case "Lost":
                case "Can't process":
                    this.trCompleteLost1.Visible = true;
                    imgCompleteMandatory.Visible = false;
                    break;
                case "Processed":
                    this.trCompleteLost1.Visible = true;
                    
                    this.trCompleteProcessed1.Visible = true;
                    this.trCompleteProcessed2.Visible = true;
                    imgCompleteMandatory.Visible = false;
                    break;
                default: // all pending statuses
                    break;
            }

         // check for extra fields to show depending on missing data
            SetExtraMissingControls();

        }

        private void SetExtraMissingControls()
        {
            if (this.ddlOpportunityStatus.SelectedItem == null)
                return;

            
            // for Qalify Pane
            
            // for Respond Pane
            this.trRespondExtraEstimated.Visible = false;
            this.trInsuredAs.Visible = false;
            // for Complete Pane
            this.trCompleteExtraEstimated.Visible = false;
            this.trCompleteExtraQuoted.Visible = false;
            this.trCompleteInsuredAs.Visible = false;

            if (ddlClassification != null && ddlClassification.Items.Count == 0)
                PopulateClassification();

          

            switch (this.ddlOpportunityStatus.SelectedItem.Text)
            {
                case "Identified":
                    break;
                case "Qualified-in":
                    break;
                case "Qualified-out":
                    break;
                case "Interested":
                case "Not Interested": 
                case "Go-to-Market":
                case "Revisit next year":
                case "Quoted": 
                case "Can't Place":
                    if (string.IsNullOrEmpty(CurrentOpportunity.InsuredAs) && this.ddlOpportunityStatus.SelectedItem.Text=="Quoted")
                    {
                        trInsuredAs.Visible = true;
                        txtInsuredAs = txtExtraInsuredAS;
                    }
                    if (!CurrentOpportunity.NetBrokerageEstimated.HasValue)
                    {
                        this.trRespondExtraEstimated.Visible = true;
                        txtNetBrokerageEstimated = txtRespondExtraEstimatedBrokingIncome;

                        if (trQualifiedIn3.Visible == true)
                        {
                            chkRespondPersonalLinesOverride.Visible = false;
                            chkPersonalLines = chkRespondExtraEstimatedPersonalLinesOverride;
                        }
                        else if (trSubmitted1.Visible == true)
                        {
                            chkRespondExtraEstimatedPersonalLinesOverride.Visible = false;
                            chkPersonalLines = chkRespondQutedPersonalLinesOverride;
                        }
                        else
                        {
                            chkRespondExtraEstimatedPersonalLinesOverride.Visible = true;
                            chkPersonalLines = chkRespondExtraEstimatedPersonalLinesOverride;
                        }
                    }
                    else
                    {
                        if(chkRespondQutedPersonalLinesOverride.Visible)
                            chkPersonalLines = chkRespondQutedPersonalLinesOverride;
                        if (chkRespondPersonalLinesOverride.Visible)
                            chkPersonalLines = chkRespondPersonalLinesOverride;
                    }
                    break;
                case "Accepted":
                case "Lost":
                case "Can't process":
                case "Processed":
                    if (string.IsNullOrEmpty(CurrentOpportunity.InsuredAs))
                    {
                        trCompleteInsuredAs.Visible = true;
                        txtInsuredAs = txtCompleteExtraInsuredAS;
                    }
                    if (!CurrentOpportunity.NetBrokerageActual.HasValue)
                    {
                        this.trCompleteAccepted1.Visible = true;
                        txtNetBrokerageActual = txtCompleteNetBrokerageActual;
                        chkPersonalLines = chkCompletePersonalLinesOverride;
                    }

                    if (!CurrentOpportunity.NetBrokerageEstimated.HasValue)
                    {
                        this.trCompleteExtraEstimated.Visible = true;
                        txtNetBrokerageEstimated = txtCompleteExtraEstimatedBrokingIncome;

                        if (trCompleteAccepted1.Visible == true)
                        {
                            chkCompleteExtraEstimatedPersonalLinesOverride.Visible = false;
                            chkPersonalLines = chkCompletePersonalLinesOverride;
                        }
                        else
                        {
                            chkCompleteExtraEstimatedPersonalLinesOverride.Visible = true;
                            chkPersonalLines = chkCompleteExtraEstimatedPersonalLinesOverride;
                        }
                    }
                    if (!CurrentOpportunity.NetBrokerageQuoted.HasValue)
                    {
                        this.trCompleteExtraQuoted.Visible = true;
                        txtNetBrokerageQuoted = txtCompleteExtraNetBrokerageQuoted;
                        chkCompleteExtraEstimatedPersonalLinesOverride.Visible = false;

                        if (trCompleteAccepted1.Visible == true)
                        {
                            chkExtraCompleteQutedPersonalLinesOverride.Visible = false;
                            chkPersonalLines = chkCompletePersonalLinesOverride;
                        }
                        
                        else
                        {
                            chkExtraCompleteQutedPersonalLinesOverride.Visible = true;
                            chkPersonalLines = chkExtraCompleteQutedPersonalLinesOverride;
                        }
                    }

                    
                  
                    break;
                default: // all pending statuses
                    break;
            }

            // check for extra fields to show depending on missing data


        }

        private void SaveExtraMissingControls(Opportunity currentOpportunityToSave, Client currentClientToSave)
        {
            if (this.ddlOpportunityStatus.SelectedItem == null)
                return;


             switch (this.ddlOpportunityStatus.SelectedItem.Text)
            {
                case "Identified":
                    break;
                case "Qualified-in":
                    break;
                case "Qualified-out":
                    break;
                case "Interested":
                case "Not Interested":
                case "Go-to-Market":
                case "Revisit next year":
                case "Quoted":
                case "Can't Place":
                    if (string.IsNullOrEmpty(CurrentOpportunity.InsuredAs) && trInsuredAs.Visible)
                    {
                        if (currentClientToSave != null)
                            currentClientToSave.InsuredName = txtExtraInsuredAS.Text;
                        currentOpportunityToSave.Client = currentClientToSave;

                    }

                    if (!CurrentOpportunity.NetBrokerageEstimated.HasValue && this.trRespondExtraEstimated.Visible==true)
                    {
                        // save the estimated Income
                        currentOpportunityToSave.NetBrokerageEstimated = decimal.Parse(txtNetBrokerageEstimated.Text);
                        
                    }
                    break;
                case "Accepted":
                case "Lost":
                case "Processed":
                    if (string.IsNullOrEmpty(CurrentOpportunity.InsuredAs) && trCompleteInsuredAs.Visible)
                    {
                        if (currentClientToSave != null)
                            currentClientToSave.InsuredName = txtCompleteExtraInsuredAS.Text;
                    }

                    if (!CurrentOpportunity.NetBrokerageEstimated.HasValue && this.trCompleteExtraEstimated.Visible == true)
                    {
                        // save the estimated Income
                        currentOpportunityToSave.NetBrokerageEstimated = decimal.Parse(txtNetBrokerageEstimated.Text);

                    }
                    if (!CurrentOpportunity.NetBrokerageQuoted.HasValue && this.trCompleteExtraQuoted.Visible == true)
                    {
                        // save the quoted Income
                        currentOpportunityToSave.NetBrokerageQuoted = decimal.Parse(txtNetBrokerageQuoted.Text);

                    }

                    if (!CurrentOpportunity.NetBrokerageActual.HasValue && this.trCompleteAccepted1.Visible == true)
                    {
                        // save the estimated Income
                        currentOpportunityToSave.NetBrokerageActual = decimal.Parse(txtNetBrokerageActual.Text);

                    }
                    break;
                default: // all pending statuses
                    break;
            }

            // save classification
             SaveClassificationID(currentOpportunityToSave);


        }

        private void SaveClassificationID(Opportunity currentOpportunityToSave)
        {
            if (this.ddlOpportunityStatus.SelectedItem == null)
                return;


            switch (this.ddlOpportunityStatus.SelectedItem.Text)
            {
                case "Identified":
                    break;
                case "Qualified-in":
                    break;
                case "Qualified-out":
                    break;
                case "Interested":
                case "Not Interested":
                case "Go-to-Market":
                case "Revisit next year":
                case "Quoted":
                case "Can't Place":
                    if (!CurrentOpportunity.NetBrokerageEstimated.HasValue && this.trRespondExtraEstimated.Visible == true)
                    {
                        // save the estimated Income
                        currentOpportunityToSave.NetBrokerageEstimated = decimal.Parse(txtNetBrokerageEstimated.Text);
                        if(!currentOpportunityToSave.NetBrokerageQuoted.HasValue)
                            currentOpportunityToSave.ClassificationID = GetClassificationBasedonIncome(currentOpportunityToSave.NetBrokerageEstimated.Value);

                    }
                    break;
                case "Accepted":
                case "Lost":
                case "Processed":
                    if (!CurrentOpportunity.NetBrokerageEstimated.HasValue && this.trCompleteExtraEstimated.Visible == true)
                    {
                        // save the estimated Income
                        currentOpportunityToSave.NetBrokerageEstimated = decimal.Parse(txtNetBrokerageEstimated.Text);
                        if (!currentOpportunityToSave.NetBrokerageActual.HasValue &&  !currentOpportunityToSave.NetBrokerageQuoted.HasValue)
                                 currentOpportunityToSave.ClassificationID = GetClassificationBasedonIncome(currentOpportunityToSave.NetBrokerageEstimated.Value);

                    }
                    if (!CurrentOpportunity.NetBrokerageQuoted.HasValue && this.trCompleteExtraQuoted.Visible == true)
                    {
                        // save the quoted Income
                        currentOpportunityToSave.NetBrokerageQuoted = decimal.Parse(txtNetBrokerageQuoted.Text);
                         if (!currentOpportunityToSave.NetBrokerageActual.HasValue)
                             currentOpportunityToSave.ClassificationID = GetClassificationBasedonIncome(currentOpportunityToSave.NetBrokerageQuoted.Value);

                    }
                    break;
                default: // all pending statuses
                    break;
            }

            // check for extra fields to show depending on missing data


        }


        private void SetSelectedExpandedPanelControls()
        {
          
            switch (AccordionCtrl.SelectedIndex)
            {
                case 0: // qualify
                    {
                        ddlOpportunityStatus = ddlQualifyOpportunityStatus;
                        txtFollowUpDate = txtQualifyFollowUpDate;
                        ddlContact = ddlQualifyContact;
                        txtDateCompleted = txtQualifyDateCompleted;
                        txtActivityNote = txtQualifyActivityNote;
                        txtNetBrokerageEstimated = txtQualifyEstimatedBrokingIncome;
                        chkPersonalLines = chkQualifiyingPersonalLineOnly;
                    } break;
                case 1: // respond
                    {
                        ddlOpportunityStatus = ddlRespondStatus;
                        txtFollowUpDate = txtRespondFollowUpDate;
                        ddlContact = ddlRespondContacts;
                        txtActivityNote = txtRespondActivityNote;
                        txtDateCompleted = txtRespondDateCompleted;
                        txtOpportunityDue = txtRespondOpportunityDue;
                        txtNetBrokerageQuoted = txtRespondNetBrokerageQuoted;
                        txtNetBrokerageActual = txtRespondNetBrokerageActual;
                        txtMemoNumber = txtRespondMemoNumber;
                        txtClientCode = txtRespondClientCode;
                        ddlClassification = ddlRespondClassification;
                        
                        

                    } break;
                case 2: // complete
                    {
                        ddlOpportunityStatus = ddlCompleteStatus;
                        txtFollowUpDate = txtCompleteFollowUpDate;
                        ddlContact = ddlCompleteContacts;
                        txtActivityNote = txtCompleteActivityNote;
                        txtDateCompleted = txtCompleteDateCompleted;
                        txtOpportunityDue = txtCompleteOpportunityDue;
                        //txtNetBrokerageQuoted = txtCompleteExtraNetBrokerageQuoted;
                        txtNetBrokerageActual = txtCompleteNetBrokerageActual;
                        txtMemoNumber = txtCompleteMemoNumber;
                        txtClientCode = txtCompleteClientCode;
                        ddlClassification = ddlCompleteClassification;
                        chkPersonalLines = chkCompletePersonalLinesOverride;
                    } break;
            }

            SetExtraMissingControls();
            
        }
        protected void ddlOpportunityStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlOpportunityStatus == null) SetSelectedExpandedPanelControls();

                // clear all panes
                foreach (var pane in AccordionCtrl.Panes)
                {
                    if (AccordionCtrl.Panes[AccordionCtrl.SelectedIndex]!=pane)
                    ClearControl(pane);
                }

                if (ddlOpportunityStatus.SelectedValue != "") SetAdditionalControls();
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
                if (ddlClassification == null) SetSelectedExpandedPanelControls();
                if (ddlClassification.SelectedValue != "")
                {
                    if (ddlQualifyOpportunityStatus.SelectedItem.Text == "Qualified In")
                        SetDefaultValues();
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void SetDefaultValues()
        {
            bizOpportunity bizO = new bizOpportunity();
            Classification c = bizO.GetClassification(int.Parse(ddlClassification.SelectedValue));
            this.ucMessanger1.ProcessMessages(bizO.MSGS, true);

            if (c == null || this.txtOpportunityDue.Text == "") return;

            txtFollowUpDate.Text = string.Format("{0:dd/MM/yyyy}", DateTime.Parse(txtOpportunityDue.Text).AddDays(-c.FollowUpDefault * 7));
        }

        private void PopulateClassification()
        {
            bizMessage bizM = new bizMessage();

            bizOpportunity biz = new bizOpportunity();
            List<Classification> cls;
            cls = biz.ListClassifications();
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);

            if (cls == null || ddlClassification==null) return;

            ddlClassification.Items.Clear();
            ddlClassification.Items.Add(new ListItem("-- Please Select --", ""));
            foreach (Classification cl in cls)
            {
                this.ddlClassification.Items.Add(new ListItem(cl.ClassificationName, cl.ClassificationID.ToString()));
            }
        }

        private void UpdateClassification()
        {
            // based on dollar values
            UpdateClassification(txtRespondNetBrokerageActual.Text, ddlRespondClassification);
            UpdateClassification(txtCompleteNetBrokerageActual.Text, ddlCompleteClassification);

            UpdateClassification(txtRespondExtraEstimatedBrokingIncome.Text, ddlRespondClassification);
            UpdateClassification(txtCompleteExtraEstimatedBrokingIncome.Text, ddlCompleteClassification);
            UpdateClassification(txtCompleteExtraNetBrokerageQuoted.Text, ddlCompleteClassification);


        }

        private void UpdateClassificationID()
        {
            // based on dollar values
            UpdateClassification(txtRespondNetBrokerageActual.Text, ddlRespondClassification);
            UpdateClassification(txtCompleteNetBrokerageActual.Text, ddlCompleteClassification);

        }

       

        private void UpdateClassification(string value,DropDownList currentDDLClassfication)
        {
            // based on dollar values
            if (string.IsNullOrEmpty(value.Trim()) || currentDDLClassfication==null)
                return;

            var decValue = decimal.Parse(value);
            var filtertedCSS=GetClassificationBasedonIncome(decValue);
            if (filtertedCSS == -1)
                currentDDLClassfication.SelectedIndex = 0;
            else
                currentDDLClassfication.SelectedValue = filtertedCSS.ToString();
        }
        private int? GetClassificationBasedonIncome(decimal incomeValue)
        {
             
            incomeValue /=(decimal)1000.00; // comparison
    
            var filteretdCSS=Classifications.MappedClassificationsRanges.Where(x => incomeValue>=x.MinRange && incomeValue<x.MaxRange && !x.PersonalLinesOnly);
            if (filteretdCSS == null)
                return -1;
            if (filteretdCSS.Count() == 0)
                return -1;

            return filteretdCSS.FirstOrDefault().ID;
    
        }

        protected void UpdateActivity()
        {
            try
            {
                this.ucMessanger1.ClearMessages();
                this.ucMessanger1.UnmarkControls();


                // set the control to save and validate
                SetSelectedExpandedPanelControls();

                // special condition
                UpdateClassification();


                if (this.UIValidation() == false) return;

                bizActivity biz = new bizActivity();

                Activity a = biz.GetActivity(int.Parse(Request.QueryString["aid"]));
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                //general
                if (this.txtFollowUpDate.Text != "") a.FollowUpDate = DateTime.Parse(this.txtFollowUpDate.Text);
                else a.FollowUpDate = null;
                if (this.ddlContact.SelectedValue != "") a.ContactID = int.Parse(this.ddlContact.SelectedValue);
                else a.ContactID = null;
                a.ActivityNote = this.txtActivityNote.Text;
                a.Inactive = false;
                //additional
                switch (ddlOpportunityStatus.SelectedItem.Text)
                {
                    case "Identified":
                        break;
                    case "Qualified-in":
                        a.Opportunity.NetBrokerageEstimated = decimal.Parse(this.txtQualifyEstimatedBrokingIncome.Text);
                        if (chkQualifiyingPersonalLineOnly.Checked)
                            a.Opportunity.ClassificationID = Classifications.MappedClassificationsRanges.Where(x => x.PersonalLinesOnly == true).FirstOrDefault().ID;
                        else
                            a.Opportunity.ClassificationID = GetClassificationBasedonIncome(a.Opportunity.NetBrokerageEstimated.Value);
                        break;
                    case "Qualified-out":
                        a.Opportunity.DateCompleted = DateTime.Parse(this.txtDateCompleted.Text);
                        break;
                    case "Interested":
                        a.Opportunity.OpportunityDue = DateTime.Parse(this.txtOpportunityDue.Text);
                        a.Opportunity.IncumbentBroker = this.txtIncumbentBroker.Text;
                        a.Opportunity.IncumbentInsurer = this.txtIncumbentInsurer.Text;
                        if (chkPersonalLines.Checked)
                            a.Opportunity.ClassificationID = Classifications.MappedClassificationsRanges.Where(x => x.PersonalLinesOnly == true).FirstOrDefault().ID;
                        else
                        {
                            if (!string.IsNullOrEmpty(this.ddlClassification.SelectedValue))
                            {
                                a.Opportunity.ClassificationID = int.Parse(this.ddlClassification.SelectedValue);
                            }
                        }
                        break;
                    case "Not Interested":
                        a.Opportunity.DateCompleted = DateTime.Parse(this.txtDateCompleted.Text);
                        break;
                    case "Go-to-Market":
                        break;
                    case "Revisit next year":
                        a.Opportunity.DateCompleted = DateTime.Parse(this.txtDateCompleted.Text);
                        break;
                    case "Quoted":
                        a.Opportunity.NetBrokerageQuoted = decimal.Parse(this.txtNetBrokerageQuoted.Text);
                        if (chkPersonalLines.Checked)
                            a.Opportunity.ClassificationID = Classifications.MappedClassificationsRanges.Where(x => x.PersonalLinesOnly == true).FirstOrDefault().ID;
                        else
                            // as a special condition, the classification will always change depending on the dollar value
                            a.Opportunity.ClassificationID = GetClassificationBasedonIncome(a.Opportunity.NetBrokerageQuoted.Value);
                        break;
                    case "Can't Place":
                        a.Opportunity.DateCompleted = DateTime.Parse(this.txtDateCompleted.Text);
                        break;
                    case "Accepted":
                        a.Opportunity.NetBrokerageActual = decimal.Parse(this.txtNetBrokerageActual.Text);
                        if (chkPersonalLines.Checked)
                            a.Opportunity.ClassificationID = Classifications.MappedClassificationsRanges.Where(x => x.PersonalLinesOnly == true).FirstOrDefault().ID;
                        else
                        // as a special condition, the classification will always change depending on the dollar value
                        a.Opportunity.ClassificationID = GetClassificationBasedonIncome(a.Opportunity.NetBrokerageActual.Value);
                        break;
                    case "Lost":
                    case "Can't process":
                        a.Opportunity.DateCompleted = DateTime.Parse(this.txtDateCompleted.Text);
                        break;
                    case "Processed":
                        if (trCompleteAccepted1.Visible)
                        {
                            a.Opportunity.NetBrokerageActual = decimal.Parse(this.txtNetBrokerageActual.Text);
                            if (chkPersonalLines.Checked)
                                a.Opportunity.ClassificationID = Classifications.MappedClassificationsRanges.Where(x => x.PersonalLinesOnly == true).FirstOrDefault().ID;
                            else
                                // as a special condition, the classification will always change depending on the dollar value
                                a.Opportunity.ClassificationID = GetClassificationBasedonIncome(a.Opportunity.NetBrokerageActual.Value);
                        }

                        a.Opportunity.DateCompleted = DateTime.Parse(this.txtDateCompleted.Text);
                        a.Opportunity.MemoNumber = this.txtMemoNumber.Text;
                        a.Opportunity.Client.ClientCode = this.txtClientCode.Text;
                        break;
                    default: // all pending statuses
                        break;
                }


                // save extra controls, as a special condition
                SaveExtraMissingControls(a.Opportunity,a.Opportunity.Client);

                // update the opportunity Object in viewstate
                CurrentOpportunity = new OpportunityProxy() { 
                    
                    NetBrokerageEstimated = a.Opportunity.NetBrokerageEstimated,
                    NetBrokerageQuoted= a.Opportunity.NetBrokerageQuoted,
                    NetBrokerageActual= a.Opportunity.NetBrokerageActual,
                    InsuredAs = (a.Opportunity.Client != null) ? a.Opportunity.Client.InsuredName : null
                
                };
                
                //audit
                a.ModifiedBy    = bizUser.GetCurrentUserName();
                a.Modified      = DateTime.Now;
                //action
                if (biz.ValidateActivity(a) == false)
                {
                    this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                    return;
                }
                if (biz.UpdateActivity(a,a.Opportunity) == true)
                {
                    this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                    Response.Redirect(Request.Url.LocalPath+"?cid=" + Request.QueryString["cid"]
                                                     + "&oid=" + Request.QueryString["oid"]
                                                     + "&aid=" + Request.QueryString["aid"]
                                                     , false);
                }
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                // clear current expanded panel controls
                // ClearControl(AccordionCtrl.Panes[AccordionCtrl.SelectedIndex]);
                // Refresh the opportunity details
                //RefreshPage();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }


        private int? SaveActivity(bool refreshPage)
        {
            try
            {
                // check if the page is in activity edit mode.
                if (Request.QueryString["aid"] != null)
                {
                    // update the currect activity
                    UpdateActivity();
                    return null;
                }

                this.ucMessanger1.ClearMessages();
                this.ucMessanger1.UnmarkControls();

              

                // set the control to save and validate
                SetSelectedExpandedPanelControls();

                // special condition
                UpdateClassification();

                if (this.UIValidation() == false) return null;


                bizActivity biz = new bizActivity();

                Activity na = new Activity();
                //general

                na.OpportunityID = int.Parse(Request.QueryString["oid"]);
                if (ddlOpportunityStatus.SelectedValue != "") na.OpportunityStatusID = int.Parse(ddlOpportunityStatus.SelectedValue);
                na.ActivityNote = txtActivityNote.Text;
                if (txtFollowUpDate.Text != "") na.FollowUpDate = DateTime.Parse(txtFollowUpDate.Text);
                na.Inactive = false;
                if (ddlContact.SelectedValue != "") na.ContactID = int.Parse(ddlContact.SelectedValue);
                //additional
                Opportunity no = new Opportunity();
                Client nc = new Client();
                switch (ddlOpportunityStatus.SelectedItem.Text)
                {
                    case "Identified":
                        break;
                    case "Qualified-in":
                        no.NetBrokerageEstimated = decimal.Parse(txtNetBrokerageEstimated.Text);
                        // as a special condition, the classification will always change depending on the dollar value
                        if(chkQualifiyingPersonalLineOnly.Checked)
                            no.ClassificationID = Classifications.MappedClassificationsRanges.Where(x => x.PersonalLinesOnly == true).FirstOrDefault().ID;
                        else
                            no.ClassificationID = GetClassificationBasedonIncome(no.NetBrokerageEstimated.Value);
                        break;
                    case "Qualified-out":
                        no.DateCompleted = DateTime.Parse(txtDateCompleted.Text);
                        break;
                    case "Interested":
                        no.OpportunityDue = DateTime.Parse(txtOpportunityDue.Text);
                        no.IncumbentBroker = this.txtIncumbentBroker.Text;
                        no.IncumbentInsurer = this.txtIncumbentInsurer.Text;
                        if (chkPersonalLines != null)
                        {
                            if (chkPersonalLines.Checked)
                            {
                                no.ClassificationID = Classifications.MappedClassificationsRanges.Where(x => x.PersonalLinesOnly == true).FirstOrDefault().ID;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(ddlClassification.SelectedValue))
                                {
                                    no.ClassificationID = int.Parse(ddlClassification.SelectedValue);
                                }
                            }
                        }
                        break;
                    case "Not Interested":
                        no.DateCompleted = DateTime.Parse(txtDateCompleted.Text);
                        break;
                    case "Go-to-Market":
                        break;
                    case "Revisit next year":
                        no.DateCompleted = DateTime.Parse(txtDateCompleted.Text);
                        break;
                    case "Quoted":
                        no.NetBrokerageQuoted = decimal.Parse(txtNetBrokerageQuoted.Text);
                        // as a special condition, the classification will always change depending on the dollar value
                        if (chkPersonalLines.Checked)
                            no.ClassificationID = Classifications.MappedClassificationsRanges.Where(x => x.PersonalLinesOnly == true).FirstOrDefault().ID;
                        else
                            no.ClassificationID = GetClassificationBasedonIncome(no.NetBrokerageQuoted.Value);
                        break;
                    case "Can't Place":
                        no.DateCompleted = DateTime.Parse(txtDateCompleted.Text);
                        break;
                    case "Accepted":
                        no.NetBrokerageActual = decimal.Parse(txtNetBrokerageActual.Text);
                        if (chkPersonalLines.Checked)
                            no.ClassificationID = Classifications.MappedClassificationsRanges.Where(x => x.PersonalLinesOnly == true).FirstOrDefault().ID;
                        else
                        // as a special condition, the classification will always change depending on the dollar value
                        no.ClassificationID = GetClassificationBasedonIncome(no.NetBrokerageActual.Value);
                        break;
                    case "Lost":
                    case "Can't process":
                        no.DateCompleted = DateTime.Parse(txtDateCompleted.Text);
                        break;
                    case "Processed":
                        if (trCompleteAccepted1.Visible)
                        {
                            no.NetBrokerageActual = decimal.Parse(txtNetBrokerageActual.Text);
                            if (chkPersonalLines.Checked)
                                no.ClassificationID = Classifications.MappedClassificationsRanges.Where(x => x.PersonalLinesOnly == true).FirstOrDefault().ID;
                            else
                                // as a special condition, the classification will always change depending on the dollar value
                                no.ClassificationID = GetClassificationBasedonIncome(no.NetBrokerageActual.Value);
                        }

                        no.DateCompleted = DateTime.Parse(txtDateCompleted.Text);
                        no.MemoNumber = this.txtMemoNumber.Text;
                        nc.ClientCode = this.txtClientCode.Text;
                        break;
                    default: // all pending statuses
                        break;
                }

                // save extra controls, as a special condition
                SaveExtraMissingControls(no,nc);

                // update the opportunity Object in viewstate
                CurrentOpportunity = new OpportunityProxy() { 
                    NetBrokerageEstimated = no.NetBrokerageEstimated ,
                    NetBrokerageQuoted= no.NetBrokerageQuoted,
                    NetBrokerageActual=no.NetBrokerageActual,
                    InsuredAs = (no.Client!=null)?no.Client.InsuredName:null
                };

                //audit
                na.AddedBy = bizUser.GetCurrentUserName();
                na.Added = DateTime.Now;
                //action
                if (biz.ValidateActivity(na) == false)
                {
                    this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                    return null ;
                }
                int aid = biz.InsertActivity(na, no, nc,no);
                if (aid != 0)
                {
                    this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                    /* Response.Redirect("ViewActivity.aspx?cid=" + Request.QueryString["cid"]
                                                      + "&oid=" + Request.QueryString["oid"]
                                                      + "&aid=" + aid.ToString()
                                                      , false);*/
                }
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);

                // clear current expanded panel controls
                ClearControl(AccordionCtrl.Panes[AccordionCtrl.SelectedIndex]);
                // Refresh the opportunity details

                if(refreshPage)
                    RefreshPage();

                return aid;

               
                
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }

            return null;

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveActivity(true);            
        }


        private bool UIValidation()
        {
            // get current expanded panel
            var expandedPanel=AccordionCtrl.Panes[AccordionCtrl.SelectedIndex];

            bizMessage bizM = new bizMessage();
            if (ddlOpportunityStatus.SelectedValue == "")
            {
                this.ucMessanger1.ProcessMessage("Stage of Sales / Aspire Cycle: " + bizM.GetMessageText("ValueNotSelected"), Enums.enMsgType.Err, ddlOpportunityStatus.ID, typeof(DropDownList), true);
                return false;
            }
            bizActivity biz = new bizActivity();
            Status s = biz.GetStatusByID(int.Parse(ddlOpportunityStatus.SelectedValue));
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            if (s.OutcomeType != "C")
            {
                if (txtFollowUpDate.Text == "")
                {
                    this.ucMessanger1.ProcessMessage("Follow Up Date: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtFollowUpDate.ID, typeof(TextBox), true);
                    return false;
                }
            }
            if (txtFollowUpDate.Text != "")
            {
                DateTime result;
                if (DateTime.TryParse(this.txtFollowUpDate.Text, out result) == false)
                {
                    this.ucMessanger1.ProcessMessage("Follow Up Date: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err, txtFollowUpDate.ID, typeof(TextBox), true);
                    return false;
                }
            }
            switch (ddlOpportunityStatus.SelectedItem.Text)
            {
                case "Identified":
                    break;
                case "Qualified-in":
                    {
                        if (txtNetBrokerageEstimated.Text == "")
                        {
                            this.ucMessanger1.ProcessMessage("Estimated Broking Income: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtQualifyEstimatedBrokingIncome.ID, typeof(TextBox), true);
                            return false;
                        }
                    }
                    break;
                case "Qualified-out":
                    if (txtDateCompleted.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtDateCompleted.ID, typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        DateTime res;
                        if (DateTime.TryParse(txtDateCompleted.Text, out res) == false)
                        {
                            this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err, txtDateCompleted.ID, typeof(TextBox), true);
                            return false;
                        }
                    }
                    break;
                case "Interested":
                    DateTime result;
                    if (txtOpportunityDue.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Renewal Date: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtOpportunityDue.ID, typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        if (DateTime.TryParse(txtOpportunityDue.Text, out result) == false)
                        {
                            this.ucMessanger1.ProcessMessage("Renewal Date: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err, txtOpportunityDue.ID, typeof(TextBox), true);
                            return false;
                        }
                    }
                    if (this.ddlClassification.SelectedValue == "")
                    {
                        //this.ucMessanger1.ProcessMessage("CSS Segment: " + bizM.GetMessageText("ValueNotSelected"), Enums.enMsgType.Err, ddlClassification.ID, typeof(DropDownList), true);
                        //return false;
                    }
                    if (txtNetBrokerageEstimated != null)
                    {
                        if (txtNetBrokerageEstimated.Text == "")
                        {
                            this.ucMessanger1.ProcessMessage("Estimated Broking Income: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtNetBrokerageEstimated.ID, typeof(TextBox), true);
                            return false;
                        }
                    }

                    
                    break;
                case "Not Interested":
                    if (txtDateCompleted.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtDateCompleted.ID, typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        DateTime res;
                        if (DateTime.TryParse(txtDateCompleted.Text, out res) == false)
                        {
                            this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err, txtDateCompleted.ID, typeof(TextBox), true);
                            return false;
                        }
                    }
                    if (txtNetBrokerageEstimated != null)
                    {
                        if (txtNetBrokerageEstimated.Text == "")
                        {
                            this.ucMessanger1.ProcessMessage("Estimated Broking Income: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtQualifyEstimatedBrokingIncome.ID, typeof(TextBox), true);
                            return false;
                        }
                    }
                    break;
                case "Go-to-Market":
                    if (txtNetBrokerageEstimated != null)
                    {
                        if (txtNetBrokerageEstimated.Text == "")
                        {
                            this.ucMessanger1.ProcessMessage("Estimated Broking Income: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtQualifyEstimatedBrokingIncome.ID, typeof(TextBox), true);
                            return false;
                        }
                    }
                    break;
                case "Revisit next year":
                    if (txtDateCompleted.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtDateCompleted.ID, typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        DateTime res;
                        if (DateTime.TryParse(txtDateCompleted.Text, out res) == false)
                        {
                            this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err, txtDateCompleted.ID, typeof(TextBox), true);
                            return false;
                        }
                    }
                    if (txtNetBrokerageEstimated != null)
                    {
                        if (txtNetBrokerageEstimated.Text == "")
                        {
                            this.ucMessanger1.ProcessMessage("Estimated Broking Income: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtQualifyEstimatedBrokingIncome.ID, typeof(TextBox), true);
                            return false;
                        }
                    }
                    break;
                case "Quoted":
                    if (txtInsuredAs != null)
                    {
                        if (txtExtraInsuredAS.Text == "")
                        {
                            this.ucMessanger1.ProcessMessage("Insured As: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtExtraInsuredAS.ID, typeof(TextBox), true);
                            return false;
                        }
                    }
                    if (txtNetBrokerageQuoted.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Broking Income Quoted: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtNetBrokerageQuoted.ID, typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        decimal res;
                        if (Decimal.TryParse(txtNetBrokerageQuoted.Text, out res) == false)
                        {
                            this.ucMessanger1.ProcessMessage("Broking Income Quoted: " + bizM.GetMessageText("FieldNotDecimal"), Enums.enMsgType.Err, txtNetBrokerageQuoted.ID, typeof(TextBox), true);
                            return false;
                        }
                    }
                    if (txtNetBrokerageEstimated != null)
                    {
                        if (txtNetBrokerageEstimated.Text == "")
                        {
                            this.ucMessanger1.ProcessMessage("Estimated Broking Income: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtQualifyEstimatedBrokingIncome.ID, typeof(TextBox), true);
                            return false;
                        }
                    }
                   
                    break;
                case "Can't Place":
                    if (txtNetBrokerageEstimated != null)
                    {
                        if (txtNetBrokerageEstimated.Text == "")
                        {
                            this.ucMessanger1.ProcessMessage("Estimated Broking Income: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtQualifyEstimatedBrokingIncome.ID, typeof(TextBox), true);
                            return false;
                        }
                    }
                    if (txtDateCompleted.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtDateCompleted.ID, typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        DateTime res;
                        if (DateTime.TryParse(txtDateCompleted.Text, out res) == false)
                        {
                            this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err, txtDateCompleted.ID, typeof(TextBox), true);
                            return false;
                        }
                    }
                   
                    break;
                case "Accepted":
                    if (txtInsuredAs != null)
                    {
                        if (txtCompleteExtraInsuredAS.Text == "")
                        {
                            this.ucMessanger1.ProcessMessage("Insured As: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtCompleteExtraInsuredAS.ID, typeof(TextBox), true);
                            return false;
                        }
                    }
                    if (txtNetBrokerageEstimated != null)
                    {
                        if (txtNetBrokerageEstimated.Text == "")
                        {
                            this.ucMessanger1.ProcessMessage("Estimated Broking Income: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtNetBrokerageEstimated.ID, typeof(TextBox), true);
                            return false;
                        }
                        else
                        {
                            decimal res;
                            if (Decimal.TryParse(txtNetBrokerageEstimated.Text, out res) == false)
                            {
                                this.ucMessanger1.ProcessMessage("Estimated Broking Income: " + bizM.GetMessageText("FieldNotDecimal"), Enums.enMsgType.Err, txtNetBrokerageEstimated.ID, typeof(TextBox), true);
                                return false;
                            }
                        }
                    }
                    if (txtNetBrokerageQuoted != null)
                    {
                        if (txtNetBrokerageQuoted.Text == "")
                        {
                            this.ucMessanger1.ProcessMessage("Broking Income Quoted: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtNetBrokerageQuoted.ID, typeof(TextBox), true);
                            return false;
                        }
                        else
                        {
                            decimal res;
                            if (Decimal.TryParse(txtNetBrokerageQuoted.Text, out res) == false)
                            {
                                this.ucMessanger1.ProcessMessage("Broking Income Quoted: " + bizM.GetMessageText("FieldNotDecimal"), Enums.enMsgType.Err, txtNetBrokerageQuoted.ID, typeof(TextBox), true);
                                return false;
                            }
                        }
                    }
                    if (txtNetBrokerageActual.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Actual Broking Income: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtNetBrokerageActual.ID, typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        decimal res;
                        if (Decimal.TryParse(txtNetBrokerageActual.Text, out res) == false)
                        {
                            this.ucMessanger1.ProcessMessage("Actual Broking Income: " + bizM.GetMessageText("FieldNotDecimal"), Enums.enMsgType.Err, txtNetBrokerageActual.ID, typeof(TextBox), true);
                            return false;
                        }
                    }
                   
                    break;
                case "Lost":
                case "Can't process":
                    if (txtInsuredAs != null)
                    {
                        if (txtCompleteExtraInsuredAS.Text == "")
                        {
                            this.ucMessanger1.ProcessMessage("Insured As: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtCompleteExtraInsuredAS.ID, typeof(TextBox), true);
                            return false;
                        }
                    }
                    if (txtDateCompleted.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtDateCompleted.ID, typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        DateTime res;
                        if (DateTime.TryParse(txtDateCompleted.Text, out res) == false)
                        {
                            this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err, txtDateCompleted.ID, typeof(TextBox), true);
                            return false;
                        }
                    }
                    if (txtNetBrokerageEstimated != null)
                    {
                        if (txtNetBrokerageEstimated.Text == "")
                        {
                            this.ucMessanger1.ProcessMessage("Estimated Broking Income: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtNetBrokerageEstimated.ID, typeof(TextBox), true);
                            return false;
                        }
                        else
                        {
                            decimal res;
                            if (Decimal.TryParse(txtNetBrokerageEstimated.Text, out res) == false)
                            {
                                this.ucMessanger1.ProcessMessage("Estimated Broking Income: " + bizM.GetMessageText("FieldNotDecimal"), Enums.enMsgType.Err, txtNetBrokerageEstimated.ID, typeof(TextBox), true);
                                return false;
                            }
                        }
                    }
                    if (txtNetBrokerageQuoted != null)
                    {
                        if (txtNetBrokerageQuoted.Text == "")
                        {
                            this.ucMessanger1.ProcessMessage("Broking Income Quoted: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtNetBrokerageQuoted.ID, typeof(TextBox), true);
                            return false;
                        }
                        else
                        {
                            decimal res;
                            if (Decimal.TryParse(txtNetBrokerageQuoted.Text, out res) == false)
                            {
                                this.ucMessanger1.ProcessMessage("Broking Income Quoted: " + bizM.GetMessageText("FieldNotDecimal"), Enums.enMsgType.Err, txtNetBrokerageQuoted.ID, typeof(TextBox), true);
                                return false;
                            }
                        }
                    }
                   


                    break;
                case "Processed":
                    if (txtInsuredAs != null)
                    {
                        if (txtCompleteExtraInsuredAS.Text == "")
                        {
                            this.ucMessanger1.ProcessMessage("Insured As: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtCompleteExtraInsuredAS.ID, typeof(TextBox), true);
                            return false;
                        }
                    }
                    if (txtDateCompleted.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtDateCompleted.ID, typeof(TextBox), true);
                        return false;
                    }
                    else
                    {
                        DateTime res;
                        if (DateTime.TryParse(txtDateCompleted.Text, out res) == false)
                        {
                            this.ucMessanger1.ProcessMessage("Date Completed: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err, txtDateCompleted.ID, typeof(TextBox), true);
                            return false;
                        }
                    }
                    if (txtMemoNumber.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Memo Number: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtMemoNumber.ID, typeof(TextBox), true);
                        return false;
                    }
                    if (txtClientCode.Text == "")
                    {
                        this.ucMessanger1.ProcessMessage("Client Code: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtClientCode.ID, typeof(TextBox), true);
                        return false;
                    }
                    if (txtNetBrokerageEstimated != null)
                    {
                        if (txtNetBrokerageEstimated.Text == "")
                        {
                            this.ucMessanger1.ProcessMessage("Estimated Broking Income: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtNetBrokerageEstimated.ID, typeof(TextBox), true);
                            return false;
                        }
                        else
                        {
                            decimal res;
                            if (Decimal.TryParse(txtNetBrokerageEstimated.Text, out res) == false)
                            {
                                this.ucMessanger1.ProcessMessage("Estimated Broking Income: " + bizM.GetMessageText("FieldNotDecimal"), Enums.enMsgType.Err, txtNetBrokerageEstimated.ID, typeof(TextBox), true);
                                return false;
                            }
                        }
                    }
                    if (txtNetBrokerageQuoted != null)
                    {
                        if (txtNetBrokerageQuoted.Text == "")
                        {
                            this.ucMessanger1.ProcessMessage("Broking Income Quoted: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, txtNetBrokerageQuoted.ID, typeof(TextBox), true);
                            return false;
                        }
                        else
                        {
                            decimal res;
                            if (Decimal.TryParse(txtNetBrokerageQuoted.Text, out res) == false)
                            {
                                this.ucMessanger1.ProcessMessage("Broking Income Quoted: " + bizM.GetMessageText("FieldNotDecimal"), Enums.enMsgType.Err, txtNetBrokerageQuoted.ID, typeof(TextBox), true);
                                return false;
                            }
                        }
                    }
                 
                    break;
                default: // all pending statuses
                    break;
            }
            return true;
        }

        private void setActivityCorrespondingExpandedPanel(Activity a)
        {
            switch (a.Status.Action)
            {
                case "Qualify":
                    {
                        AccordionCtrl.SelectedIndex = 0;
                        break;
                    }

                case "Contact":
                case "Discover":
                case "Respond":
                    {
                        AccordionCtrl.SelectedIndex = 1;
                        break;
                    }
                case "Agree":
                case "Process":
                    {
                        AccordionCtrl.SelectedIndex = 2;
                        break;
                    }
            }
        }

        private void PopulateActivityDetails()
        {
            int activityId;
            if (Request.QueryString["aid"] == null || !int.TryParse(Request.QueryString["aid"], out activityId))
                return;

            bizMessage bizM = new bizMessage();
            bizSetting bizS = new bizSetting();
            bizActivity biz = new bizActivity();

            Activity a;
            a = biz.GetActivity(activityId);
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);

            if (a == null || a.Status.Action == "Recognise") return;

            //general
            LoadedActivity = a;
            // set the control panel refrences
            setActivityCorrespondingExpandedPanel(a);
            SetSelectedExpandedPanelControls();

            ddlOpportunityStatus.SelectedValue = a.Status.StatusID.ToString();
            //this.hidStatusID.Value = a.OpportunityStatusID.ToString();
            txtActivityNote.Text = a.ActivityNote;
            txtFollowUpDate.Text = string.Format("{0:dd/MM/yyyy}", a.FollowUpDate);
            if (a.ContactID.HasValue == true) ddlContact.SelectedValue = a.ContactID.ToString();

            //audit
            ((Main)Master).HeaderDetails = "Activity added by "
                                                + bizActiveDirectory.GetUserFullName(a.AddedBy)
                                                + " (" + string.Format("{0:dd-MMM-yy}", a.Added) + ")";

            if (a.Modified.HasValue == true)
                ((Main)Master).HeaderDetails += " / modified by "
                    + bizActiveDirectory.GetUserFullName(a.ModifiedBy)
                    + " (" + string.Format("{0:dd-MMM-yy}", a.Modified.Value) + ")";

            ////inactive
            //if (a.Inactive == true)
            //{
            //    //general
            //    this.ddlOpportunityStatus.Enabled = false;
            //    this.txtActivityNote.Enabled = false;
            //    this.txtFollowUpDate.Enabled = false;
            //    this.btnFollowUpDate.Enabled = false;
            //    this.txtFollowUpNote.Enabled = false;
            //    //additional
            //    this.txtIncumbentBroker.Enabled = false;
            //    this.txtIncumbentInsurer.Enabled = false;
            //    this.ddlClassification.Enabled = false;
            //    this.txtNetBrokerageQuoted.Enabled = false;
            //    this.txtNetBrokerageActual.Enabled = false;
            //    this.txtDateCompleted.Enabled = false;
            //    this.btnDateCompleted.Enabled = false;
            //    this.txtMemoNumber.Enabled = false;
            //    this.txtClientCode.Enabled = false;
            //    //buttons
            //    this.btnSave.Enabled = false;
            //}
        }

        private void PopulateAdditionalControls()
        {
            int activityId;
            if ( (Request.QueryString["aid"] == null || !int.TryParse(Request.QueryString["aid"], out activityId))|| LoadedActivity == null )
                return;

            if ((LoadedActivity.Status.Action == "Recognise"))
                return;

            bizMessage bizM = new bizMessage();

            bizOpportunity biz = new bizOpportunity();
            Opportunity o = biz.GetOpportunity(int.Parse(Request.QueryString["oid"]));

            if (o == null) return;
            if (o.NetBrokerageEstimated.HasValue && txtNetBrokerageEstimated!=null)
                txtNetBrokerageEstimated.Text = o.NetBrokerageEstimated.ToString();
            if(txtIncumbentBroker!=null)
                txtIncumbentBroker.Text = o.IncumbentBroker;
            if (txtIncumbentInsurer!=null)
                txtIncumbentInsurer.Text = o.IncumbentInsurer;
            if (o.OpportunityDue.HasValue && txtOpportunityDue!=null)
                txtOpportunityDue.Text = String.Format("{0:dd/MM/yyyy}", o.OpportunityDue);
            if (o.ClassificationID.HasValue && ddlClassification!=null)
                ddlClassification.SelectedValue = o.ClassificationID.ToString();
            if (o.NetBrokerageQuoted.HasValue && txtNetBrokerageQuoted != null)
                txtNetBrokerageQuoted.Text = o.NetBrokerageQuoted.ToString();
            if (o.NetBrokerageActual.HasValue && txtNetBrokerageActual!=null)
                txtNetBrokerageActual.Text = o.NetBrokerageActual.ToString();
            if (o.DateCompleted.HasValue && txtDateCompleted != null)
            txtDateCompleted.Text = String.Format("{0:dd/MM/yyyy}", o.DateCompleted);
            if (txtMemoNumber != null)
            txtMemoNumber.Text = o.MemoNumber;
            if (o.Client!=null && txtClientCode != null)
            txtClientCode.Text = o.Client.ClientCode;
        }
    }
}
