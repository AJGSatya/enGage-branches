using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.DirectoryServices;
using enGage.BL;
using enGage.DL;
using enGage.Utilities;
using enGage.Web.Pages;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using enGage.Web.Helper;

namespace enGage.Web
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    //((Main)Master).AuthenticateUser();

                    if (Session["USER"] == null)
                    {
                        bizMessage bizM = new bizMessage();
                        this.ucMessanger1.ProcessMessage("Session: " + bizM.GetMessageText("SessionMissing"), Enums.enMsgType.Err, "", null, true);
                        return;
                    }

                    using (Timeline.Capture("Dashboard.aspx", "ASP.NET"))
                    {
                        ((Main)Master).HeaderTitle = "Dashboard";
                        ((HtmlGenericControl)((Main)Master).FindControl("divWrapper")).Attributes["class"] = "wrapper-report";
                        Security();
                    }
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void Security()
        {
            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
            if (user != null && user.Role == (int)Enums.enUserRole.Executive)
                this.btnDashboardTotals.Visible = false;
        }

        protected void ucSearchOptions1_OnControlLoaded(object sender, EventArgs e)
        {
            LoadDashboardReport();
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                LoadDashboardReport();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnTallyBoard_Click(object sender, EventArgs e)
        {
            try
            {
                // just to set session data
                SearchOptions so = this.ucSearchOptions1.GetSearchOptions();

                Response.Redirect("~/Tallyboard.aspx", false);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void LoadDashboardReport()
        {
            using (Timeline.Capture("Dashboard.aspx: LoadDashboardReport", "ASP.NET"))
            {
                this.ucMessanger1.ClearMessages();
                this.ucMessanger1.UnmarkControls();

                SearchOptions so = this.ucSearchOptions1.GetSearchOptions();
                if (so == null)
                {
                    this.pnlResults_CollapsiblePanelExtender.ClientState = "true";
                    this.pnlResults_CollapsiblePanelExtender.Collapsed = true;
                    return;
                }

                bizMessage bizM = new bizMessage();

                if (Session["USER"] == null)
                {
                    this.ucMessanger1.ProcessMessage("Session: " + bizM.GetMessageText("SessionMissing"), Enums.enMsgType.Err, "", null, true);
                    this.pnlResults_CollapsiblePanelExtender.ClientState = "true";
                    this.pnlResults_CollapsiblePanelExtender.Collapsed = true;
                    return;
                }

                bizUser.enGageUser user = (bizUser.enGageUser) Session["USER"];
                
                bizReport biz = new bizReport();
                List<proc_rpt_DashboardResult> rs = biz.GetDashboardData(
                    so.DateFrom,
                    so.DateTo,
                    so.Region,
                    so.Branch,
                    so.Executive,
                    so.Classification,
                    so.BusinessType,
                    so.Industries,
                    so.Sources,
                    so.Opportunities);
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);

                if (rs == null) return;

                this.ReportViewer1.Reset();
                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                string ssrsURL = ConfigurationSettings.AppSettings["SSRS_URL"];
                if (string.IsNullOrEmpty(ssrsURL))
                {
                    throw new Exception("Error: Please set the SSRS_URL in the AppSetting in web.config");
                }
                this.ReportViewer1.ServerReport.ReportServerUrl = new Uri(ssrsURL);
                this.ReportViewer1.ServerReport.ReportPath = "/enGage/Dashboard";

                using (Timeline.Capture("ServerReport.Refresh()", "Reports"))
                {
                    this.ReportViewer1.ServerReport.Refresh();
                }

                this.ReportViewer1.Height = Unit.Point(480);
                this.ReportViewer1.Visible = true;
                /* Start */

                /*
                    ReportParameter p1 = new ReportParameter("ReportStart", so.DateFrom.ToString("dd MMM yyyy"));
                    ReportParameter p2 = new ReportParameter("ReportEnd", so.DateTo.ToString("dd MMM yyyy"));
                    ReportParameter p3 = new ReportParameter("Region", so.Region);
                    ReportParameter p4 = new ReportParameter("Branch", so.Branch);
                    ReportParameter p5 = new ReportParameter("AccountExecutiveID", so.Executive);
                    ReportParameter p6 = new ReportParameter("ClassificationID", so.Classification.ToString());
                    ReportParameter p7 = new ReportParameter("BusinessTypeID", so.BusinessType.ToString());
                    ReportParameter p8 = new ReportParameter("ANZSICDelimitedList", so.Industries == null ? "" : so.Industries);
                    ReportParameter p9 = new ReportParameter("SourceDelimitedList", so.Sources == null ? "" : so.Sources);
                    string ExecutiveName = this.ucSearchOptions1.GetExecutiveName(so.Executive);
                    string label = so.Executive != "(All)" ? ExecutiveName : so.Branch != "(All)" && so.Executive == "(All)" ? so.Region + " (" + so.Branch + ")" : so.Region != "(All)" && so.Branch == "(All)" ? so.Region : so.Region == "(All)" ? "OAMPS Insurance Brokers Ltd" : "";
                    ReportParameter p10 = new ReportParameter("OpportunitiesDelimitedList", label);
                */

                /* End */

                ReportParameter p11 = new ReportParameter("parDateFrom", so.DateFrom.ToString("dd MMM yyyy"));
                ReportParameter p12 = new ReportParameter("parDateTo", so.DateTo.ToString("dd MMM yyyy"));
                ReportParameter p13 = new ReportParameter("parRegion", so.Region);
                ReportParameter p14 = new ReportParameter("parBranch", so.Branch);
                ReportParameter p15 = new ReportParameter("parExecutive", so.Executive);

                //Uncommented after the fix up of the NULL values.
                ReportParameter p16 = new ReportParameter("parClassification",
                    so.Classification.ToString() == string.Empty || so.Classification.ToString() == "0"
                        ? null
                        : so.Classification.ToString());
                ReportParameter p17 = new ReportParameter("parBusinessType",
                    so.BusinessType.ToString() == string.Empty || so.BusinessType.ToString() == "0"
                        ? null
                        : so.BusinessType.ToString());
                ReportParameter p18 = new ReportParameter("parIndustries",
                    string.IsNullOrEmpty(so.Industries) ? null : so.Industries);
                ReportParameter p19 = new ReportParameter("parSources",
                    string.IsNullOrEmpty(so.Sources) ? null : so.Sources);

                string ExecutiveName = this.ucSearchOptions1.GetExecutiveName(so.Executive);
                string label = so.Executive != "(All)"
                    ? ExecutiveName
                    : so.Branch != "(All)" && so.Executive == "(All)"
                        ? so.Region + " (" + so.Branch + ")"
                        : so.Region != "(All)" && so.Branch == "(All)"
                            ? so.Region
                            : so.Region == "(All)" ? "OAMPS Insurance Brokers Ltd" : "";
                ReportParameter p20 = new ReportParameter("parReportLabel", label);
                //ReportParameter p21 = new ReportParameter("parURL", HttpContext.Current.Request.Url.Authority);
                ReportParameter p22 = new ReportParameter("parOpportunities", so.Opportunities);

                this.ReportViewer1.ServerReport.SetParameters(new ReportParameter[]
                {p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p22});

                using (Timeline.Capture("ServerReport.Refresh()", "Reports"))
                {
                    this.ReportViewer1.ServerReport.Refresh();
                }

                this.ReportViewer1.Height = Unit.Point(480);
                this.ReportViewer1.Visible = true;

                /*
            this.ReportViewer1.LocalReport.EnableHyperlinks = true;
            this.ReportViewer1.LocalReport.ReportPath = "Reports//Dashboard.rdlc";
            ReportParameter p1 = new ReportParameter("parDateFrom", so.DateFrom.ToString());
            ReportParameter p2 = new ReportParameter("parDateTo", so.DateTo.ToString());
            ReportParameter p3 = new ReportParameter("parRegion", so.Region);
            ReportParameter p4 = new ReportParameter("parBranch", so.Branch);
            ReportParameter p5 = new ReportParameter("parExecutive", so.Executive);
            ReportParameter p6 = new ReportParameter("parClassification", so.Classification.ToString());
            ReportParameter p7 = new ReportParameter("parBusinessType", so.BusinessType.ToString());
            ReportParameter p8 = new ReportParameter("parIndustries", so.Industries == null ? "" : so.Industries);
            ReportParameter p9 = new ReportParameter("parSources", so.Sources == null ? "" : so.Sources);
            string ExecutiveName = this.ucSearchOptions1.GetExecutiveName(so.Executive);
            string label = so.Executive != "(All)" ? ExecutiveName : so.Branch != "(All)" && so.Executive == "(All)" ? so.Region + " (" + so.Branch + ")" : so.Region != "(All)" && so.Branch == "(All)" ? so.Region : so.Region == "(All)" ? "OAMPS Insurance Brokers Ltd" : "";
            ReportParameter p10 = new ReportParameter("parReportLabel", label);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 });
            ReportDataSource myRDS = new ReportDataSource("proc_rpt_DashboardResult", rs);
            this.ReportViewer1.LocalReport.DataSources.Clear();
            this.ReportViewer1.LocalReport.DataSources.Add(myRDS);
            this.ReportViewer1.LocalReport.Refresh();
            this.ReportViewer1.Height = Unit.Point(480);
            this.ReportViewer1.Visible = true;
            */

                this.pnlResults_CollapsiblePanelExtender.ClientState = "false";
                this.pnlResults_CollapsiblePanelExtender.Collapsed = false;
            }
        }

        protected void btnDashboardTotals_Click(object sender, EventArgs e)
        {
            try
            {
                // just to set session data
                SearchOptions so = this.ucSearchOptions1.GetSearchOptions();

                Response.Redirect("~/DashboardTotals.aspx", false);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        /*
        protected void ReportViewer1_Drillthrough(object sender, DrillthroughEventArgs e)
        {
            try
            {
                ServerReport sr = (ServerReport)e.Report;
                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                if (sr.ReportPath.Contains("DashboardDetails") == true)
                {

                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }
         * */

        
        protected void ReportViewer1_Drillthrough(object sender, DrillthroughEventArgs e)
        {
            try
            {
                SearchOptions so = this.ucSearchOptions1.GetSearchOptions();
                if (so == null)
                {
                    this.pnlResults_CollapsiblePanelExtender.ClientState = "true";
                    this.pnlResults_CollapsiblePanelExtender.Collapsed = true;
                    return;
                }

                this.ReportViewer1.Reset();
                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                string ssrsURL = ConfigurationSettings.AppSettings["SSRS_URL"];
                if (string.IsNullOrEmpty(ssrsURL))
                {
                    throw new Exception("Error: Please set the SSRS_URL in the AppSetting in web.config");
                }
                this.ReportViewer1.ServerReport.ReportServerUrl = new Uri(ssrsURL);
                this.ReportViewer1.ServerReport.ReportPath = "/enGage/DashboardDetails";

                using (Timeline.Capture("ServerReport.Refresh()", "Reports"))
                {
                    this.ReportViewer1.ServerReport.Refresh();
                }

                this.ReportViewer1.Height = Unit.Point(480);
                this.ReportViewer1.Visible = true;

                ReportParameter p11 = new ReportParameter("parDateFrom", so.DateFrom.ToString("dd MMM yyyy"));
                ReportParameter p12 = new ReportParameter("parDateTo", so.DateTo.ToString("dd MMM yyyy"));
                //ReportParameter p13 = new ReportParameter("parRegion", so.Region);
                //ReportParameter p14 = new ReportParameter("parBranch", so.Branch);
                ReportParameter p15 = new ReportParameter("parExecutive", so.Executive);

                ReportParameter p16 = new ReportParameter("parClassification", so.Classification.ToString());
                ReportParameter p17 = new ReportParameter("parBusinessType", so.BusinessType.ToString());
                ReportParameter p18 = new ReportParameter("parIndustries", so.Industries == null ? "" : so.Industries);
                ReportParameter p19 = new ReportParameter("parSources", so.Sources == null ? "" : so.Sources);
                string ExecutiveName = this.ucSearchOptions1.GetExecutiveName(so.Executive);
                string label = so.Executive != "(All)" ? ExecutiveName : so.Branch != "(All)" && so.Executive == "(All)" ? so.Region + " (" + so.Branch + ")" : so.Region != "(All)" && so.Branch == "(All)" ? so.Region : so.Region == "(All)" ? "OAMPS Insurance Brokers Ltd" : "";
                ReportParameter p20 = new ReportParameter("parReportLabel", label);
                //ReportParameter p21 = new ReportParameter("parOpportunities", so.Opportunities);

                this.ReportViewer1.ServerReport.SetParameters(new ReportParameter[] { p11, p12,p15, p16, p17, p18, p19, p20});

                using (Timeline.Capture("ServerReport.Refresh()", "Reports"))
                {
                    this.ReportViewer1.ServerReport.Refresh();
                }

                this.ReportViewer1.Height = Unit.Point(480);
                this.ReportViewer1.Visible = true;
                this.pnlResults_CollapsiblePanelExtender.ClientState = "false";
                this.pnlResults_CollapsiblePanelExtender.Collapsed = false;
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }
    }
}
