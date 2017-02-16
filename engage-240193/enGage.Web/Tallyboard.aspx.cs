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


namespace enGage.Web
{
    public partial class Tallyboard : System.Web.UI.Page
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

        #region Logic
        private void LoadTallyboardReport()
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
            List<proc_rpt_TallyboardResult> rs = biz.GetTallyBoardData(
                                                                        so.DateFrom,
                                                                        so.DateTo,
                                                                        so.Region,
                                                                        so.Branch,
                                                                        so.Executive,
                                                                        so.BusinessType,
                                                                        so.Classification,
                                                                        so.Sources,
                                                                        so.Industries,
                                                                        so.Opportunities);

            CalculateTotal(rs);
            rptTallyboard.DataSource = rs;
            rptTallyboard.DataBind();

            this.pnlResults_CollapsiblePanelExtender.ClientState = "false";
            this.pnlResults_CollapsiblePanelExtender.Collapsed = false;
        }

        private void CalculateTotal(List<proc_rpt_TallyboardResult> rs)
        {
            int totalActivities = 0;
            int totalClients = 0;
            int totalOpportunities = 0;
            foreach (proc_rpt_TallyboardResult r in rs)
            {
                totalActivities += r.Activities != null ? int.Parse(r.Activities.ToString()) : 0;
                totalClients += r.Clients != null ? int.Parse(r.Clients.ToString()) : 0;
                totalOpportunities += r.Opportunities != null ? int.Parse(r.Opportunities.ToString()) : 0;
            }
            TotalActivities = totalActivities;
            TotalClients = totalClients;
            TotalOpportunities = totalOpportunities;
        }
        private void Security()
        {
            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
            if (user.Role == (int)Enums.enUserRole.Executive)
                this.btnDashboardTotals.Visible = false;
        }
        private void SetHeader()
        {
            SearchOptions so = this.ucSearchOptions1.GetSearchOptions();
            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];

            //First set the header according to the user's selection.
            switch (user.Role)
            {
                case (int)Enums.enUserRole.Executive:
                    TallyForHeader = "Account Executive";
                    TotalColumn = so.Executive + " Results";
                    break;
                case (int)Enums.enUserRole.Branch:
                    TallyForHeader = "Account Executive";
                    TotalColumn = so.Branch + " Results";
                    break;
                case (int)Enums.enUserRole.Region:
                    TallyForHeader = "Branch";
                    TotalColumn = so.Region+" Results";
                    break;
                case (int)Enums.enUserRole.Company:
                    TallyForHeader = "Branch (Region)";
                    TotalColumn = "OAMPS Insurance Brokers Ltd Results";
                    break;
                case (int)Enums.enUserRole.Administrator:
                    TallyForHeader = "Branch (Region)";
                    TotalColumn = "OAMPS Insurance Brokers Ltd Results";
                    break;
                default:
                    break;
            }

            if (so != null)
            {
                //Then set the header according to the selection.
                if (so.Region == "(All)" && so.Branch == "(All)" && so.Executive == "(All)")
                {
                    TallyForHeader = "Branch (Region)";
                    TotalColumn = "OAMPS Insurance Brokers Ltd Results";
                }
                else if (so.Branch == "(All)" && so.Executive == "(All)")
                {
                    TallyForHeader = "Branch";
                    TotalColumn = so.Region + " Results";
                }
                else if (so.Executive == "(All)")
                {
                    TallyForHeader = "Account Executive";
                    TotalColumn = so.Branch + " Results";
                }
                else
                {
                    TallyForHeader = "Account Executive";
                    TotalColumn = so.Executive + " Results";
                }
            }
        }
        #endregion

        #region Variables
        private string TallyForHeader
        {
            set
            {
                ltrTallyForHeader.Text = value;
            }
        }
        private string TotalColumn
        {
            set
            {
                ltrTotalColumn.Text = value;
            }
        }
        private int TotalActivities
        {
            set
            {
                ltrTotalActivities.Text = value.ToString();
            }
        }
        private int TotalClients
        {
            set
            {
                ltrTotalClients.Text = value.ToString();
            }
        }
        private int TotalOpportunities
        {
            set
            {
                ltrTotalOpportunities.Text = value.ToString();
            }
        }
        #endregion

        #region Events
        protected void rptTallyboard_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                proc_rpt_TallyboardResult result = (proc_rpt_TallyboardResult)e.Item.DataItem;

                Literal ltrTallyFor = (Literal)e.Item.FindControl("ltrTallyFor");
                Literal ltrActivities = (Literal)e.Item.FindControl("ltrActivities");
                Literal ltrClients = (Literal)e.Item.FindControl("ltrClients");
                Literal ltrOpportunities = (Literal)e.Item.FindControl("ltrOpportunities");

                ltrTallyFor.Text = result.TallyFor;
                ltrActivities.Text = result.Activities != null? result.Activities.ToString():string.Empty;
                ltrClients.Text = result.Clients != null ? result.Clients.ToString() : string.Empty;
                ltrOpportunities.Text = result.Opportunities !=null? result.Clients.ToString():string.Empty;
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


        protected void btnDashBoard_Click(object sender, EventArgs e)
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

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                LoadTallyboardReport();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }
        protected void ucSearchOptions1_OnControlLoaded(object sender, EventArgs e)
        {
            SetHeader();
            LoadTallyboardReport();
        }
        #endregion
    }
}
