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
using System.Text;

namespace enGage.Web
{
    public partial class Dashboard2 : System.Web.UI.Page
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
                    ((Main)Master).HeaderTitle = "Dashboard";
                    ((HtmlGenericControl)((Main)Master).FindControl("divWrapper")).Attributes["class"] = "wrapper-report";
                    Security();
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
            if (user.Role == (int)Enums.enUserRole.Executive)
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

        private void LoadDashboardReport()
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

            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];

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

            StringBuilder sb = new StringBuilder();

            rptDashboard.DataSource = rs;
            rptDashboard.DataBind();

            //Do the sums for the other fields.
            Decimal sumActivitiesQuoted = 0;
            Decimal sumActivitiesActual = 0;
            Decimal sumFollowUpsQuoted = 0;
            Decimal sumFollowUpsActual = 0;
            Decimal sumPipelineQuoted = 0;
            Decimal sumPipelineActual = 0;
            Decimal sumToWinQuoted = 0;
            Decimal sumWonActual = 0;
            Decimal sumLostQuoted = 0;
            Decimal sumDueQuoted = 0;
            Decimal sumDueActual = 0;

            foreach(proc_rpt_DashboardResult r in rs)
            {
                sumActivitiesQuoted += r.ActivitiesQuoted != null ? (decimal)r.ActivitiesQuoted : 0;
                sumActivitiesActual += r.ActivitiesActual != null ? (decimal)r.ActivitiesActual : 0;
                 sumFollowUpsQuoted += r.FollowUpsQuoted != null ? (decimal)r.FollowUpsQuoted : 0;
                 sumFollowUpsActual += r.FollowUpsActual != null ? (decimal)r.FollowUpsActual : 0;
                 sumPipelineQuoted += r.PipelineQuoted != null ? (decimal)r.PipelineQuoted : 0;
                 sumPipelineActual += r.PipelineActual != null ? (decimal)r.PipelineActual : 0;
                 sumToWinQuoted += r.ToWinQuoted != null ? (decimal)r.ToWinQuoted : 0;
                 sumWonActual += r.WonActual != null ? (decimal)r.WonActual : 0;
                 sumLostQuoted += r.LostQuoted != null ? (decimal)r.LostQuoted : 0;
                 sumDueQuoted += r.DueQuoted != null ? (decimal)r.DueQuoted : 0;
                 sumDueActual += r.DueActual != null ? (decimal)r.DueActual : 0;
                //sb.AppendLine(r.Action + " "+r.Activities+" "+r.ActivitiesActual+" "+r.ActivitiesQuoted+" "+r.CompleteOutcomes+" "+r.DueActual+" "+r.DueOutcomes+" "+r.DueQuoted+" "+r.FollowUps+" "+r.FollowUpsActual+" "+r.FollowUpsQuoted+" "+r.LostQuoted+" "+r.PipelineActual+" "+r.PipelineOutcomes+" "+r.PipelineQuoted+" "+r.PreviousStatusID+" "+r.SuccessOutcomes+" "+r.ToWinQuoted+" "+r.WonActual+"<br/>");
            }

            

            /*
            foreach(proc_rpt_DashboardResult r in rs)
            {
                sb.AppendLine(r.Action + " "+r.Activities+" "+r.ActivitiesActual+" "+r.ActivitiesQuoted+" "+r.CompleteOutcomes+" "+r.DueActual+" "+r.DueOutcomes+" "+r.DueQuoted+" "+r.FollowUps+" "+r.FollowUpsActual+" "+r.FollowUpsQuoted+" "+r.LostQuoted+" "+r.PipelineActual+" "+r.PipelineOutcomes+" "+r.PipelineQuoted+" "+r.PreviousStatusID+" "+r.SuccessOutcomes+" "+r.ToWinQuoted+" "+r.WonActual+"<br/>");
            }
            ltrText.Text = sb.ToString();
            */

            /*
            this.ReportViewer1.Reset();
            this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
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

            this.pnlResults_CollapsiblePanelExtender.ClientState = "false";
            this.pnlResults_CollapsiblePanelExtender.Collapsed = false;
             * */
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

        protected void ReportViewer1_Drillthrough(object sender, DrillthroughEventArgs e)
        {
            try
            {
                LocalReport lr = (LocalReport)e.Report;
                /*
                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                if (lr.ReportPath.Contains("DashboardDetails") == true)
                {
                    ReportParameterInfoCollection pars = e.Report.GetParameters();
                    bizReport biz = new bizReport();
                    List<proc_rpt_DashboardDetailResult> rs = biz.GetDashboardDetailData(
                                                              DateTime.Parse(pars[0].Values[0]),
                                                              DateTime.Parse(pars[1].Values[0]),
                                                              pars[2].Values[0],
                                                              int.Parse(pars[4].Values[0]),
                                                              int.Parse(pars[3].Values[0]),
                                                              pars[5].Values[0],
                                                              pars[6].Values[0],
                                                              pars[7].Values[0] == "" ? null : pars[7].Values[0],
                                                              pars[8].Values[0] == "" ? null : pars[8].Values[0]);
                    this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                    ReportDataSource myRDS = new ReportDataSource("proc_rpt_DashboardDetailResult", rs);
                    lr.DataSources.Clear();
                    lr.DataSources.Add(myRDS);
                    lr.Refresh();
                    this.ReportViewer1.Height = Unit.Point(800);
                    this.ReportViewer1.Visible = true;
                 
                }
                 */
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void rptDashboard_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                proc_rpt_DashboardResult r = (proc_rpt_DashboardResult)e.Item.DataItem;
                Literal ltrAction = (Literal)e.Item.FindControl("ltrAction");
                Literal ltrActivities = (Literal)e.Item.FindControl("ltrActivities");
                Literal ltrFollowUps = (Literal)e.Item.FindControl("ltrFollowUps");
                Literal ltrAction2 = (Literal)e.Item.FindControl("ltrAction2");
                Literal ltrPipelineOutcomes = (Literal)e.Item.FindControl("ltrPipelineOutcomes");
                Literal ltrSuccessOutcomes = (Literal)e.Item.FindControl("ltrSuccessOutcomes");
                Literal ltrCompleteOutcomes = (Literal)e.Item.FindControl("ltrCompleteOutcomes");
                Literal ltrDuesOutcomes = (Literal)e.Item.FindControl("ltrDuesOutcomes");

                //ltrAction.Text = r.Phase;
                ltrActivities.Text = r.Activities != null ? r.Activities.ToString(): string.Empty;
                ltrFollowUps.Text = r.FollowUps != null ? r.FollowUps.ToString() : string.Empty;

               /* switch (r.Action)
                {
                    case "Recognise":
                        ltrAction2.Text = "Identified";
                        break;
                    case "Qualify":
                        ltrAction2.Text = "Qualified-in";
                        break;
                    case "Contact":
                        ltrAction2.Text = "Interested";
                        break;
                    case "Discover":
                        ltrAction2.Text = "Go-to-Market";
                        break;
                    case "Respond":
                        ltrAction2.Text = "Quoted";
                        break;
                    case "Agree":
                        ltrAction2.Text = "Accepted";
                        break;
                    case "Process":
                        ltrAction2.Text = "Processed";
                        break;
                    default:
                        ltrAction2.Text = string.Empty;
                        break;
                }
                * */
                ltrPipelineOutcomes.Text = r.PipelineOutcomes != null ? r.PipelineOutcomes.ToString() : string.Empty;
                ltrSuccessOutcomes.Text = r.SuccessOutcomes != null ? r.SuccessOutcomes.ToString() : string.Empty;
                ltrCompleteOutcomes.Text = r.CompleteOutcomes != null ? r.CompleteOutcomes.ToString() : string.Empty;
                ltrDuesOutcomes.Text = r.DueOutcomes != null ? r.DueOutcomes.ToString() : string.Empty;
            }

            
        }
    }
}