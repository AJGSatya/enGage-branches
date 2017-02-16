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
using System.Drawing;

namespace enGage.Web
{
    public partial class DashboardTotals : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                   // ((Main)Master).AuthenticateUser();

                    if (Session["USER"] == null)
                    {
                        bizMessage bizM = new bizMessage();
                        this.ucMessanger1.ProcessMessage("Session: " + bizM.GetMessageText("SessionMissing"), Enums.enMsgType.Err, "", null, true);
                        return;
                    }
                    bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
                    if (user.Role == (int)Enums.enUserRole.Executive)
                    {
                        Response.Redirect("~/Dashboard.aspx", false);
                        return;
                    }
                    
                    ((Main)Master).HeaderTitle = "Dashboard Totals";
                    ((HtmlGenericControl)((Main)Master).FindControl("divWrapper")).Attributes["class"] = "wrapper-report";
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void ucSearchOptions1_OnControlLoaded(object sender, EventArgs e)
        {
            LoadDasboardTotalsReport();
        }

        private void LoadDasboardTotalsReport()
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

            bizReport biz = new bizReport();
            List<proc_rpt_DashboardTotalsResult> rs = biz.GetDashboardTotalsData(
                                                      so.DateFrom,
                                                      so.DateTo,
                                                      so.Region,
                                                      so.Branch,
                                                      so.Classification,
                                                      so.BusinessType,
                                                      so.Industries,
                                                      so.Sources,
                                                      so.Opportunities);
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);

            this.ReportViewer1.Reset();
            this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            this.ReportViewer1.LocalReport.EnableHyperlinks = true;
            this.ReportViewer1.LocalReport.ReportPath = "Reports//DashboardTotals.rdlc";
            ReportParameter p1 = new ReportParameter("parDateFrom", so.DateFrom.ToString());
            ReportParameter p2 = new ReportParameter("parDateTo", so.DateTo.ToString());
            ReportParameter p3 = new ReportParameter("parRegion", so.Region);
            ReportParameter p4 = new ReportParameter("parBranch", so.Branch);
            ReportParameter p5 = new ReportParameter("parClassification", so.Classification.ToString());
            ReportParameter p6 = new ReportParameter("parBusinessType", so.BusinessType.ToString());
            ReportParameter p7 = new ReportParameter("parIndustries", so.Industries == null ? "" : so.Industries);
            ReportParameter p8 = new ReportParameter("parSources", so.Sources == null ? "" : so.Sources);
            ReportParameter p9 = new ReportParameter("parOpportunities", so.Opportunities == null ? "" : so.Opportunities);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9 });
            ReportDataSource myRDS = new ReportDataSource("proc_rpt_DashboardTotalsResult", rs);
            this.ReportViewer1.LocalReport.DataSources.Clear();
            this.ReportViewer1.LocalReport.DataSources.Add(myRDS);
            this.ReportViewer1.LocalReport.Refresh();
            this.ReportViewer1.Height = Unit.Point(740);
            this.ReportViewer1.Visible = true;

            this.pnlResults_CollapsiblePanelExtender.ClientState = "false";
            this.pnlResults_CollapsiblePanelExtender.Collapsed = false;
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

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                LoadDasboardTotalsReport();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnDashboard_Click(object sender, EventArgs e)
        {
            try
            {
                // just to set session data
                SearchOptions so = this.ucSearchOptions1.GetSearchOptions();

                Response.Redirect("~/Dashboard.aspx", false);
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
                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                //if (lr.ReportPath.Contains("DashboardTotals") == true)
                //{
                //    ReportParameterInfoCollection pars = e.Report.GetParameters();
                //    bizReport biz = new bizReport();
                //    List<proc_rpt_DashboardTotalsResult> rs = biz.GetDashboardTotalsData(
                //                                              DateTime.Parse(pars[0].Values[0]),
                //                                              DateTime.Parse(pars[1].Values[0]),
                //                                              pars[2].Values[0],
                //                                              pars[3].Values[0],
                //                                              int.Parse(pars[4].Values[0]),
                //                                              int.Parse(pars[5].Values[0]),
                //                                              pars[6].Values[0] == "" ? null : pars[6].Values[0],
                //                                              pars[7].Values[0] == "" ? null : pars[7].Values[0]);
                //    this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                //    ReportDataSource myRDS = new ReportDataSource("proc_rpt_DashboardTotalsResult", rs);
                //    lr.DataSources.Clear();
                //    lr.DataSources.Add(myRDS);
                //    lr.Refresh();
                //    this.ReportViewer1.Height = Unit.Point(740);
                //    this.ReportViewer1.Visible = true;
                //}

                if (lr.ReportPath.Contains("DashboardDetails") == true)
                {
                    SearchOptions so = this.ucSearchOptions1.GetSearchOptions();

                    ReportParameterInfoCollection pars = e.Report.GetParameters();
                    bizReport biz = new bizReport();
                    List<proc_rpt_DashboardDetailResult> rs = biz.GetDashboardDetailData(
                                                              DateTime.Parse(pars[0].Values[0]),
                                                              DateTime.Parse(pars[1].Values[0]),
                                                              pars[2].Values[0],
                                                              int.Parse(pars[3].Values[0]),
                                                              int.Parse(pars[4].Values[0]),
                                                              pars[5].Values[0],
                                                              pars[6].Values[0],
                                                              pars[7].Values[0] == "" ? null : pars[7].Values[0],
                                                              pars[8].Values[0] == "" ? null : pars[8].Values[0],
                                                              so.Opportunities == ""? null : so.Opportunities
                                                              );
                    this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                    ReportDataSource myRDS = new ReportDataSource("proc_rpt_DashboardDetailResult", rs);
                    lr.DataSources.Clear();
                    lr.DataSources.Add(myRDS);
                    lr.Refresh();
                    this.ReportViewer1.Height = Unit.Point(800);
                    this.ReportViewer1.Visible = true;
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
