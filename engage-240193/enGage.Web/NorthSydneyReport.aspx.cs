using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using enGage.BL;
using enGage.DL;
using enGage.Utilities;
using Microsoft.Reporting.WebForms;

namespace enGage.Web
{
    public partial class NorthSydneyReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    //((Main)Master).AuthenticateUser();

                    bizMessage bizM = new bizMessage();
                    if (Session["USER"] == null)
                    {
                        this.ucMessanger1.ProcessMessage("Session: " + bizM.GetMessageText("SessionMissing"), Enums.enMsgType.Err, "", null, true);
                        return;
                    }
                    ((Main)Master).HeaderTitle = "Report";
                    ((HtmlGenericControl)((Main)Master).FindControl("divWrapper")).Attributes["class"] = "wrapper-report";

                    bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
                    bizSetting bizS = new bizSetting();

                    PopulateTeams();
                    PopulateDefaults();
                    LoadNorthSydneyReport();
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void PopulateTeams()
        {
            bizReport biz = new bizReport();
            List<String> teams = biz.GetNorthSydneyTeams();
            this.ddlTeam.Items.Clear();
            this.ddlTeam.Items.Add("(All)");
            foreach (String team in teams)
            {
                this.ddlTeam.Items.Add(team);
            }
        }

        private void PopulateDefaults()
        {
            this.txtFrom.Text = string.Format("{0:dd/MM/yyyy}", new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1));
            this.txtTo.Text = string.Format("{0:dd/MM/yyyy}", new DateTime(DateTime.Today.AddMonths(1).Year, DateTime.Today.AddMonths(1).Month, 1).AddDays(-1));
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                LoadNorthSydneyReport();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void LoadNorthSydneyReport()
        {
            bizReport biz = new bizReport();
            List<proc_rpt_DashboardNorthSydneyResult> rs = biz.GetNorthSydneyReportData(
                                                           DateTime.Parse(this.txtFrom.Text),
                                                           DateTime.Parse(this.txtTo.Text),
                                                           this.ddlTeam.SelectedValue);
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);

            this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            //this.ReportViewer1.LocalReport.ReportEmbeddedResource = "enGage.Web.Reports.NorthSydneyReport.rdlc";
            //this.ReportViewer1.LocalReport.ReportPath = MapPath("Reports/NorthSydneyReport.rdlc");
            ReportParameter p1 = new ReportParameter("parDateFrom", this.txtFrom.Text);
            ReportParameter p2 = new ReportParameter("parDateTo", this.txtTo.Text);
            ReportParameter p3 = new ReportParameter("parTeamName", this.ddlTeam.SelectedValue);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });
            ReportDataSource myRDS = new ReportDataSource("proc_rpt_DashboardNorthSydneyResult", rs);
            this.ReportViewer1.LocalReport.DataSources.Clear();
            this.ReportViewer1.LocalReport.DataSources.Add(myRDS);
            this.ReportViewer1.LocalReport.Refresh();
            this.ReportViewer1.Visible = true;
        }
    }
}
