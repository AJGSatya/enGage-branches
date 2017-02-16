using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Reflection;
using System.Data.Linq;
using System.Linq;
using System.DirectoryServices;
using enGage.BL;
using enGage.DL;
using System.Collections.Generic;
using enGage.Web.Helper;

namespace enGage.Web
{
    public partial class Main : System.Web.UI.MasterPage
    {
        public string Environment;

        public string HeaderTitle
        {
            get { return this.lblHeaderTitle.Text; }
            set { this.lblHeaderTitle.Text = value; }
        }

        public string HeaderDetails
        {
            get { return this.lblHeaderDetails.Text; }
            set { this.lblHeaderDetails.Text = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (Timeline.Capture("Main.Master", "ASP.NET"))
                {
                    if (this.IsPostBack == false)
                    {
                        SetHeaderAndFooter();
                        //AuthenticateUser();
                        PopuplateFollowUpsSummary();
                    }
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void SetHeaderAndFooter()
        {
            bizSetting biz = new bizSetting();
            this.lblVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.lblReleased.Text = String.Format("{0:dd MMMM yyyy}", System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location));


            bizUser.enGageUser currentUser = (bizUser.enGageUser)Session["USER"];

            var env = Cache.Get("env") as string;
            if (String.IsNullOrEmpty(env))
            {
                this.Environment = biz.GetSetting("Application.Environment");
                Cache.Insert("env",Environment);
            }
           

            if (this.Environment == "Production")
            {
                if (this.GetCurrentPageName().ToLower() == "northsydneyreport.aspx"
                    || this.GetCurrentPageName().ToLower() == "dashboardtotals.aspx"
                    || this.GetCurrentPageName().ToLower() == "dashboard.aspx"
                    || this.GetCurrentPageName().ToLower() == "tallyboard.aspx"
                    )
                {
                    this.header.Attributes["class"] = "header";
                    this.footer.Attributes["class"] = "footer";
                }
                else
                {
                    this.header.Attributes["class"] = "header";
                    this.footer.Attributes["class"] = "footer";
                }
                // this.lblHeaderLine1.CssClass = "header-line1-alt";
                //  this.lblHeaderLine2.CssClass = "header-line2-alt";

                this.lblHeaderLine1.Text = this.lblHeaderLine1.Text.Replace("???", "<span class='header-line1-name' runat='server'>"
                                            + currentUser.DisplayName
                                            + "</span>");
            }
            else
            {
                this.lblEnvironment.Text = this.Environment;
                this.lblHeaderLine1.Text = this.lblHeaderLine1.Text.Replace("???", "<span class='header-line1-name-alt' runat='server'>"
                                            + currentUser.DisplayName
                                             + "</span>");
                if (this.GetCurrentPageName().ToLower() == "northsydneyreport.aspx"
                    || this.GetCurrentPageName().ToLower() == "dashboardtotals.aspx"
                    || this.GetCurrentPageName().ToLower() == "dashboard.aspx"
                    || this.GetCurrentPageName().ToLower() == "tallyboard.aspx"
                    )
                {
                    this.header.Attributes["class"] = "header-wide-alt";
                    this.footer.Attributes["class"] = "footer-wide-alt";
                }
                else
                {
                    this.header.Attributes["class"] = "header-alt";
                    this.footer.Attributes["class"] = "footer-alt";
                }
                this.lblHeaderLine1.CssClass = "header-line1-alt";
                this.lblHeaderLine2.CssClass = "header-line2-alt";

                enGageDataContext db = new enGageDataContext();
                this.lblDatabase.Text = "database " + db.Connection.Database + " on " + db.Connection.DataSource + " server";
            }
        }

        public string GetCurrentPageName()
        {
            string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
            string sRet = oInfo.Name;
            return sRet;
        }

        private void FollowUpsSummary()
        {
            bizActivity biz = new bizActivity();
            int? FollowUpsCount = 0;
            int? FollowUpsOverdue = 0;
            decimal? SubmittedAmount = 0;
            int? SubmittedCount = 0;
            int? SubmittedOverdue = 0;
            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
            if (user == null) return;
            biz.FollowUpsSummary(user.UserName, ref FollowUpsCount, ref FollowUpsOverdue, ref SubmittedCount, ref SubmittedAmount, ref SubmittedOverdue);
            this.lblHeaderLine2.Text = this.lblHeaderLine2.Text.Replace("?1?", FollowUpsCount.ToString());
            if (this.Environment == "Production")
            {
                this.lblHeaderLine2.Text = this.lblHeaderLine2.Text.Replace("?2?", "<span class='header-line2-due'>(" + FollowUpsOverdue.ToString() + " due)</span>");
                this.lblHeaderLine2.Text = this.lblHeaderLine2.Text.Replace("?3?", "<span style='font-weight:bold;'>" + string.Format("{0:c}", SubmittedAmount) + "</span>");
                this.lblHeaderLine2.Text = this.lblHeaderLine2.Text.Replace("?4?", "<span style='font-weight:bold;'>" + SubmittedCount.ToString() + "</span>");
                this.lblHeaderLine2.Text = this.lblHeaderLine2.Text.Replace("?5?", "<span class='header-line2-due'>(" + SubmittedOverdue.ToString() + " due)</span>");
            }
            else
            {
                this.lblHeaderLine2.Text = this.lblHeaderLine2.Text.Replace("?2?", "<span class='header-line2-due-alt'>(" + FollowUpsOverdue.ToString() + " due)</span>");
                this.lblHeaderLine2.Text = this.lblHeaderLine2.Text.Replace("?3?", "<span style='font-weight:bold;'>" + string.Format("{0:c}", SubmittedAmount) + "</span>");
                this.lblHeaderLine2.Text = this.lblHeaderLine2.Text.Replace("?4?", "<span style='font-weight:bold;'>" + SubmittedCount.ToString() + "</span>");
                this.lblHeaderLine2.Text = this.lblHeaderLine2.Text.Replace("?5?", "<span class='header-line2-due-alt'>(" + SubmittedOverdue.ToString() + " due)</span>");
            }
        }
        
        private void PopuplateFollowUpsSummary()
        {
            bizActivity biz = new bizActivity();
            int? FollowUpsCount = 0;
            int? FollowUpsOverdue = 0;
            decimal? SubmittedAmount = 0;
            int? SubmittedCount = 0;
            int? SubmittedOverdue = 0;
            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
            if (user == null) return;

            var allString = "(All)";
            List<sp_web_FollowUpsResult> d1 = biz.GetFollowUps(user.UserName, 0, 0, -1, string.Join(",", new string[] { Enums.ActivityActions.Qualify.ToString(), Enums.ActivityActions.Recognise.ToString() }), allString, allString, true);
            List<sp_web_FollowUpsResult> d2 = biz.GetFollowUps(user.UserName, 0, 0, -1, string.Join(",", new string[] { Enums.ActivityActions.Contact.ToString(), Enums.ActivityActions.Discover.ToString(), Enums.ActivityActions.Respond.ToString() }), allString, allString, false);
            List<sp_web_FollowUpsResult> d3 = biz.GetFollowUps(user.UserName, 0, 0, -1, string.Join(",", new string[] { Enums.ActivityActions.Agree.ToString(), Enums.ActivityActions.Process.ToString() }), allString, allString, false);

            var _allQualify = d1;
            var _allRespond = d2.Where(x => x.FollowUpDate >= CalenderUtilities.CutOffDate).ToList();
            var _allComplete = d3;
            var allOpportunities = _allQualify.Concat(_allRespond).Concat(_allComplete).Where(x => x.OutcomeType != "C" && x.FollowUpDate >= CalenderUtilities.CutOffDate).ToList();
            
            var quoted = _allRespond.Sum(x => (x.NetBrokerageQuoted.HasValue) ? x.NetBrokerageQuoted : 0);
            FollowUpsCount = allOpportunities.Count;
            FollowUpsOverdue = allOpportunities.Where(x=> x.FollowUpDate < DateTime.Today.Date).Count();
            SubmittedAmount = quoted;
            SubmittedCount = _allRespond.Count;
            SubmittedOverdue = _allRespond.Where(x => x.FollowUpDate < DateTime.Today.Date).Count();
        
            //   biz.FollowUpsSummary(user.UserName, ref FollowUpsCount, ref FollowUpsOverdue, ref SubmittedCount, ref SubmittedAmount, ref SubmittedOverdue);
            this.lblHeaderLine2.Text = this.lblHeaderLine2.Text.Replace("?1?", FollowUpsCount.ToString());
            if (this.Environment == "Production")
            {
                this.lblHeaderLine2.Text = this.lblHeaderLine2.Text.Replace("?2?", "<span class='header-line2-due'>(" + FollowUpsOverdue.ToString() + " due)</span>");
                this.lblHeaderLine2.Text = this.lblHeaderLine2.Text.Replace("?3?", "<span style='font-weight:bold;'>" + string.Format("{0:c}", SubmittedAmount) + "</span>");
                this.lblHeaderLine2.Text = this.lblHeaderLine2.Text.Replace("?4?", "<span style='font-weight:bold;'>" + SubmittedCount.ToString() + "</span>");
                this.lblHeaderLine2.Text = this.lblHeaderLine2.Text.Replace("?5?", "<span class='header-line2-due'>(" + SubmittedOverdue.ToString() + " due)</span>");
            }
            else
            {
                this.lblHeaderLine2.Text = this.lblHeaderLine2.Text.Replace("?2?", "<span class='header-line2-due-alt'>(" + FollowUpsOverdue.ToString() + " due)</span>");
                this.lblHeaderLine2.Text = this.lblHeaderLine2.Text.Replace("?3?", "<span style='font-weight:bold;'>" + string.Format("{0:c}", SubmittedAmount) + "</span>");
                this.lblHeaderLine2.Text = this.lblHeaderLine2.Text.Replace("?4?", "<span style='font-weight:bold;'>" + SubmittedCount.ToString() + "</span>");
                this.lblHeaderLine2.Text = this.lblHeaderLine2.Text.Replace("?5?", "<span class='header-line2-due-alt'>(" + SubmittedOverdue.ToString() + " due)</span>");
            }
        }
    }
}
