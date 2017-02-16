using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using enGage.DL;
using enGage.BL;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace enGage.Web
{
    public partial class ViewActivity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    ((Main)Master).HeaderTitle = "Work with activity";
                    PopulateClientDetails();
                    PopulateOpportunityDetails();
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
            if (s != null) this.lblOpportunityStatus.Text = s.First().StatusName;

            if (biz.IsOpportunityCompleted(o.OpportunityID) == true)
            {
                this.btnAddActivity.Visible = false;
            }
        }

        private void PopulateActivityDetails()
        {
            bizMessage bizM = new bizMessage();
            bizSetting bizS = new bizSetting();
            bizActivity biz = new bizActivity();

            Activity a;
            a = biz.GetActivity(Convert.ToInt32(Request.QueryString["aid"]));
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);

            if (a == null) return;

            this.lblActivityStatus.Text = a.Status.StatusName;
            this.lblActivityNote.Text = a.ActivityNote.Replace("\n", "<br />");
            this.lblFollowUpDate.Text = string.Format("{0:dd-MMM-yy}", a.FollowUpDate);
            if (a.ContactID.HasValue == true) this.lblContact.Text = a.Contact.ContactName;

            ((Main)Master).HeaderDetails = "Activity added by "
                                            + bizActiveDirectory.GetUserFullName(a.AddedBy)
                                            + " (" + string.Format("{0:dd-MMM-yy}", a.Added) + ")";

            if (a.Modified.HasValue == true)
                ((Main)Master).HeaderDetails += " / modified by "
                        + bizActiveDirectory.GetUserFullName(a.ModifiedBy)
                        + " (" + string.Format("{0:dd-MMM-yy}", a.Modified.Value) + ")";

            //inactive
            if (a.Inactive == true)
            {
                this.tblActivity.Disabled = true;
                this.btnInactivate.Visible = false;
                this.btnSendEmail.Visible = false;
                this.btnAddActivity.Visible = false;
                this.btnEditActivity.Visible = false;
            }
            else
            {
                if (biz.GetInitialStatus().StatusID == a.OpportunityStatusID) this.btnInactivate.Visible = false;

                if (this.lblOpportunityStatus.Text != this.lblActivityStatus.Text) this.btnInactivate.Visible = false;

                this.btnSendEmail.Visible = a.ContactID.HasValue;
                if (a.ContactID.HasValue == true)
                {
                    if (a.Contact.Email != "") this.btnSendEmail.OnClientClick = "javascript:location.href='mailto:" + a.Contact.Email + "'";
                }
            }
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

            switch (this.lblActivityStatus.Text)
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

            this.lblIncumbentBroker.Text = o.IncumbentBroker;
            this.lblIncumbentInsurer.Text = o.IncumbentInsurer;
            this.lblOpportunityDue2.Text = String.Format("{0:dd-MMM-yy}", o.OpportunityDue);
            if (o.ClassificationID.HasValue == true) this.lblClassification.Text = o.Classification.ClassificationName;
            this.lblNetBrokerageQuoted.Text = string.Format("{0:C}", o.NetBrokerageQuoted);
            this.lblNetBrokerageActual.Text = string.Format("{0:C}", o.NetBrokerageActual);
            this.lblDateCompleted.Text = String.Format("{0:dd-MMM-yy}", o.DateCompleted);
            this.lblMemoNumber.Text = o.MemoNumber;
            this.lblClientCode.Text = o.Client.ClientCode;
        }

        protected void btnEditActivity_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("EditActivity.aspx?cid=" + Request.QueryString["cid"]
                                + "&oid=" + Request.QueryString["oid"]
                                + "&aid=" + Request.QueryString["aid"], false);
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
                Response.Redirect("AddActivity.aspx?cid=" + Request.QueryString["cid"]
                                + "&oid=" + Request.QueryString["oid"], false);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnInactivate_Click(object sender, EventArgs e)
        {
            try
            {
                bizActivity biz = new bizActivity();
                if (biz.SetToInactive(int.Parse(Request.QueryString["aid"])) == true)
                {
                    PopulateActivityDetails();
                }
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
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

        protected void btnOpportunity_Click(object sender, EventArgs e)
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

        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            try
            {
                // todo:
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnSetReminder_Click(object sender, EventArgs e)
        {

            bizClient biz = new bizClient();
            Client c;
            c = biz.GetClient(int.Parse(Request.QueryString["cid"]));

            bizOpportunity biz2 = new bizOpportunity();
            Opportunity o;
            o = biz2.GetOpportunity(int.Parse(Request.QueryString["oid"]));

            bizActivity biz3 = new bizActivity();
            Activity a;
            a = biz3.GetActivity(Convert.ToInt32(Request.QueryString["aid"]));

            string subject = string.Format("enGage Follow-up: {0},{1} - {2}", c.ClientName, o.OpportunityName, a.Status.StatusName);

            //To do - set this to the name of the activity.

            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;

            if (a.FollowUpDate != null)
            {
                startDate = DateTime.Parse(string.Format("{0} 08:00 AM", string.Format("{0:dd/MM/yyyy}", a.FollowUpDate)));
                endDate = DateTime.Parse(string.Format("{0} 08:05 AM", string.Format("{0:dd/MM/yyyy}", a.FollowUpDate)));
            }

            string urlPort=(HttpContext.Current.Request.Url.IsDefaultPort) ? "":":"+HttpContext.Current.Request.Url.Port.ToString();
            string activityURL = "http://" + HttpContext.Current.Request.Url.Host + urlPort+ "/ViewActivity.aspx?";

            List<string> queryStringList = new List<string>();

            if(!string.IsNullOrEmpty(Request.QueryString["cid"]))
            {
                queryStringList.Add(String.Format("cid={0}",Request.QueryString["cid"]));
            }

            if (!string.IsNullOrEmpty(Request.QueryString["oid"]))
            {
                queryStringList.Add(String.Format("oid={0}", Request.QueryString["oid"]));
            }

            if (!string.IsNullOrEmpty(Request.QueryString["aid"]))
            {
                queryStringList.Add(String.Format("aid={0}", Request.QueryString["aid"]));
            }


            activityURL += string.Join("&", queryStringList.ToArray());

            string t = "BEGIN:VCALENDAR\n"+
"PRODID:-//Microsoft Corporation//Outlook MIMEDIR//EN\n" +
"VERSION:2.0\n" +
"BEGIN:VEVENT\n" +
"DTSTART:" + startDate.ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + "\n" +
"DTEND:" + endDate.ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + "\n" +
//"LOCATION:My office\n" +
//"CATEGORIES:Business\n" +
"DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + Helper.CalenderUtilities.EncodeQuotedPrintable("Activity Note: " + a.ActivityNote +"\n\n"+ activityURL) + "=0D=0A\n" +
"SUMMARY:"+subject+"\n" +
"PRIORITY:3\n" +
"END:VEVENT\n" +
"END:VCALENDAR";
            Response.Clear();
            Response.ContentType = "application/VCS";
            Response.AddHeader("content-disposition", "attachment; filename=\"calendar.vcs\"");
            Response.Write(t.ToString());
            Response.End();

            
            
            

            /*
            
            */
        }
    }
}