using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using enGage.BL;
using enGage.DL;

namespace enGage.Web
{
    public partial class OpenReminder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetReminder();
           // ClientScript.RegisterClientScriptBlock(this.GetType(), "REFREH_WINDOW", "setTimeout( window.opener.location=\"" + Request.Url.AbsoluteUri + "\", 0); window.close();", true);   
        }


        protected void SetReminder()
        {

            bizClient biz = new bizClient();
            Client c;
            c = biz.GetClient(int.Parse(Request.QueryString["cid"]));

            bizOpportunity biz2 = new bizOpportunity();
            Opportunity o;
            o = biz2.GetOpportunity(int.Parse(Request.QueryString["oid"]));

            bizActivity biz3 = new bizActivity();
            Activity a;
            a = biz3.GetActivity(int.Parse(Request.QueryString["aid"]));

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
            string activityURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path).Replace(Request.Url.Segments[Request.Url.Segments.Length - 1], "") +"ViewOpportunity.aspx?";

            List<string> queryStringList = new List<string>();

            if (!string.IsNullOrEmpty(Request.QueryString["cid"]))
            {
                queryStringList.Add(String.Format("cid={0}", Request.QueryString["cid"]));
            }

            if (!string.IsNullOrEmpty(Request.QueryString["oid"]))
            {
                queryStringList.Add(String.Format("oid={0}", Request.QueryString["oid"]));
            }

            queryStringList.Add(String.Format("aid={0}", Request.QueryString["aid"]));



            activityURL += string.Join("&", queryStringList.ToArray());
            var htmltab = @"&nbsp\;";
            var opportunityDue = ((a.Opportunity.OpportunityDue.HasValue) ? @"<br><B>Renewal Date :</B> " + htmltab + a.Opportunity.OpportunityDue.Value.ToString("dd/MM/yyyy") + "<br>" : "<br>");

            string t = "BEGIN:VCALENDAR\n" +
"PRODID:-//Microsoft Corporation//Outlook MIMEDIR//EN\n" +
"VERSION:2.0\n" +
"BEGIN:VEVENT\n" +
"DTSTART:" + startDate.ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + "\n" +
"DTEND:" + endDate.ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + "\n" +
                //"LOCATION:My office\n" +
                //"CATEGORIES:Business\n" +
"DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + Helper.CalenderUtilities.EncodeQuotedPrintable("Currently at:" + a.Status.StatusName + "\n" +
((a.Opportunity.OpportunityDue.HasValue) ? "Renewal Date : " + a.Opportunity.OpportunityDue.Value.ToString("dd/MM/yyyy") + "\n" : "") +
                                                                                           "Activity Note: " + a.ActivityNote + "\n\n" + activityURL) + "=0D=0A\n" +
"SUMMARY:" + subject + "\n" +
"PRIORITY:0\n" +
"X-ALT-DESC;FMTTYPE=text/html:" + @"<!DOCTYPE HTML PUBLIC \""-//W3C//DTD HTML 3.2//EN\"">\n<HTML>\n<HEAD>\n<TITLE></TITLE>\n</HEAD>\n<BODY>\n" +
"<b>Currently at:</b> " + htmltab + a.Status.StatusName +
opportunityDue +
@"<br><b>Activity Note:</b> " + htmltab + a.ActivityNote + "<br><br>" + "<A HREF=\"" + HttpUtility.UrlEncode(activityURL) + "\">" + activityURL + "</A>" + "<br>" + "</BODY>\n</HTML> " +
"X-MICROSOFT-CDO-BUSYSTATUS:FREE"+
"TRIGGER:-PT15M"+
"END:VEVENT" +
"END:VCALENDAR";

            var tempPage = new Page();
            Response.Clear();
            Response.ContentType = "application/VCS";
            Response.AddHeader("content-disposition", "attachment; filename=\"calendar.vcs\"");
            Response.Write(t.ToString());
            Response.Flush();
           // Response.End();

        }
    }
}
