using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using enGage.BL;
using enGage.DL;
using enGage.Web.Helper;
using System.Collections.Specialized;
using System.Diagnostics;

namespace enGage.Web
{
    public partial class FollowUps : System.Web.UI.Page
    {
        public bool DisableExecutives { get; set; }

        List<sp_web_FollowUpsResult> _allIdentify, _allQualify, _allRespond, _allComplete;
        decimal? estimated = 0, quoted = 0, actual = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (Timeline.Capture("FollowUps.aspx", "ASP.NET"))
                {
                    if (!this.IsPostBack)
                    {
                        //((Main)Master).AuthenticateUser();

                        /********************************/
                        /* Start Section Authentication */
                        /********************************/
                        Stopwatch authenticationStopwatch = Stopwatch.StartNew();

                        if (Session["USER"] == null)
                        {
                            bizMessage bizM = new bizMessage();
                            this.ucMessanger1.ProcessMessage("Session: " + bizM.GetMessageText("SessionMissing"), Enums.enMsgType.Err, "", null, true);
                            return;
                        }
                        ((Main)Master).HeaderTitle = "Opportunities on-the-go";

                        bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];

                        authenticationStopwatch.Stop();
                        Debug.WriteLine("Authentication stop watch " + authenticationStopwatch.ElapsedMilliseconds);

                        /*************************/
                        /* Start Section Filters */
                        /*************************/
                        Stopwatch filtersStopWatch = Stopwatch.StartNew();
                        PopulateFilters();
                        filtersStopWatch.Stop();
                        Debug.WriteLine("Filters stop watch " + filtersStopWatch.ElapsedMilliseconds);

                        /*****************/
                        /* Start Regions */
                        /*****************/
                        Stopwatch regionsStopWatch = Stopwatch.StartNew();
                        FilterGridsUtilities.PopulateRegionsBranchesAndExecutives(user, ddlRegion, ddlBranch, ddlExecutive, (user.Role == (int)Enums.enUserRole.Executive || user.Role == (int)Enums.enUserRole.Branch) ? false : true, true);
                        regionsStopWatch.Stop();
                        Debug.WriteLine("Regions stop watch " + regionsStopWatch.ElapsedMilliseconds);

                        /********************/
                        /* Start Follow Ups */
                        /********************/
                        Stopwatch followupsStopWatch = Stopwatch.StartNew();
                        PopulateFollowUps();
                        followupsStopWatch.Stop();
                        Debug.WriteLine("Followups stop watch " + followupsStopWatch.ElapsedMilliseconds);

                        ddlRegion_SelectedIndexChanged(ddlRegion, new EventArgs());
                        ddlBranch_SelectedIndexChanged(ddlRegion, new EventArgs());
                    }
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void PopulateFilters()
        {
            bizOpportunity biz = new bizOpportunity();

            // OAMPS Income
            List<Classification> cls = biz.ListClassifications();
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
            this.ddlOAMPSIncome.Items.Add(new ListItem("OAMPS Income (All)", "0"));
            foreach (Classification cl in cls)
            {
                this.ddlOAMPSIncome.Items.Add(new ListItem(cl.ClassificationName, cl.ClassificationID.ToString()));
            }

            // Business Type
            var excludedTypes = new List<string> { "Quick quote", "Quick win", "Quick call" };
            List<BusinessType> bts = biz.ListBusinessTypes().Where(x => !excludedTypes.Contains(x.BusinessTypeName)).ToList();
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            this.ddlBusinessType.Items.Add(new ListItem("Type (All)", "0"));
            foreach (BusinessType bt in bts)
            {
                this.ddlBusinessType.Items.Add(new ListItem(bt.BusinessTypeName, bt.BusinessTypeID.ToString()));
            }
        }

        private void FilterOpportunities(bizUser.enGageUser user,out string FilteredRegion, out string FilteredBranch,out string FilteredAccountExecutiveID, out bool disableActivities)
        {
            // filter based on user role
            FilteredRegion = FilteredBranch = FilteredAccountExecutiveID = "(All)";

            disableActivities           = (user.Role ==(int)Enums.enUserRole.Executive)? true :false;
            FilteredRegion = (ddlRegion.SelectedIndex != 0) ? ddlRegion.SelectedValue : "(All)";
            FilteredBranch = (ddlBranch.SelectedIndex != 0) ? ddlBranch.SelectedValue : "(All)";
            FilteredAccountExecutiveID = (ddlExecutive.SelectedIndex != 0) ? ddlExecutive.SelectedValue : "(All)";
        }

        
        private void SetUserAllowedOpportunities(bizUser.enGageUser user,Boolean isRegion,Boolean isBranch,Boolean isAll)
        {
            isRegion = isBranch = isAll = false;
            
              switch (user.Role)
            {
                case (int)Enums.enUserRole.Executive:
                   
                    break;
                case (int)Enums.enUserRole.Branch:
                    isBranch = true;
                    break;
                case (int)Enums.enUserRole.Region:
                    isRegion = true;
                    break;
                case (int)Enums.enUserRole.Company:
                case (int)Enums.enUserRole.Administrator:
                    isAll = true;
                    break;
            }
        }
        private void PopulateFollowUps()
        {
            bizMessage bizM = new bizMessage();
            if (Session["USER"] == null)
            {
                this.ucMessanger1.ProcessMessage("Session: " + bizM.GetMessageText("SessionMissing"), Enums.enMsgType.Err, "", null, true);
                return;
            }
            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];

            bizActivity biz = new bizActivity();
            var u = bizUser.GetAccountExecutiveIdBySmiUserName(ddlExecutive.SelectedValue);
            List<sp_web_FollowUpsResult> d1 = biz.GetFollowUps(u, int.Parse(this.ddlOAMPSIncome.SelectedValue), int.Parse(this.ddlBusinessType.SelectedValue), int.Parse(this.ddlFlagged.SelectedValue), string.Join(",", new string[] { Enums.ActivityActions.Qualify.ToString(), Enums.ActivityActions.Recognise.ToString() }), ddlRegion.SelectedValue, ddlBranch.SelectedValue, false);
            List<sp_web_FollowUpsResult> d2 = biz.GetFollowUps(u, int.Parse(this.ddlOAMPSIncome.SelectedValue), int.Parse(this.ddlBusinessType.SelectedValue), int.Parse(this.ddlFlagged.SelectedValue), string.Join(",", new string[] { Enums.ActivityActions.Contact.ToString(), Enums.ActivityActions.Discover.ToString(), Enums.ActivityActions.Respond.ToString() }), ddlRegion.SelectedValue, ddlBranch.SelectedValue, false);
            List<sp_web_FollowUpsResult> d3 = biz.GetFollowUps(u, int.Parse(this.ddlOAMPSIncome.SelectedValue), int.Parse(this.ddlBusinessType.SelectedValue), int.Parse(this.ddlFlagged.SelectedValue), string.Join(",", new string[] { Enums.ActivityActions.Agree.ToString(), Enums.ActivityActions.Process.ToString() }), ddlRegion.SelectedValue, ddlBranch.SelectedValue, false);

            //_allIdentify = d1.Where(x => x.OutcomeType != "C" && x.FollowUpDate >= CalenderUtilities.CutOffDate).ToList();
            _allQualify = d1.Where(x => x.OutcomeType != "C" && x.FollowUpDate >= CalenderUtilities.CutOffDate).ToList();
            _allRespond = d2.Where(x => x.OutcomeType != "C" && x.FollowUpDate >= CalenderUtilities.CutOffDate).ToList();
            _allComplete = d3.Where(x => x.OutcomeType != "C" && x.FollowUpDate >= CalenderUtilities.CutOffDate).ToList();

            estimated = _allQualify.Sum(x => (x.NetBrokerageEstimated.HasValue) ? x.NetBrokerageEstimated : 0);
            quoted = _allRespond.Sum(x => (x.NetBrokerageQuoted.HasValue) ? x.NetBrokerageQuoted : 0);
            actual = _allComplete.Sum(x => (x.NetBrokerageActual.HasValue) ? x.NetBrokerageActual : 0);

            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);


            // combine all results to have a filtered summery for the week
            var allFollowups = _allQualify.Concat(_allRespond).Concat(_allComplete).Where(x => x.OutcomeType != "C" && x.FollowUpDate >= CalenderUtilities.CutOffDate).ToList(); ;
            var weekStartDate = (DateTime.Now.AddDays(-(DateTime.Now.DayOfWeek - DayOfWeek.Monday)));
            var weekEndDate = weekStartDate.AddDays(6);
            var weeklyFilteredFollowUps = allFollowups.Where(x =>
            {
                return ((x.FollowUpDate.HasValue) ?
                    (x.FollowUpDate.Value.Date >= weekStartDate.Date
                    && x.FollowUpDate.Value.Date <= weekEndDate.Date) : false)

                    ||
                    ((x.OpportunityDue.HasValue) ?
                        DateTime.Now.Date >= x.OpportunityDue.Value.Date : false
                    )
                    ;
            }).ToList();

            this.grvSummary.DataSource = weeklyFilteredFollowUps;
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
            this.grvSummary.DataBind();

            // change the way the gridview displays the header
            AlterGridRendering(grvSummary);

            if (weeklyFilteredFollowUps.Count() <= this.grvSummary.PageSize) this.lnkSeeAll0.Enabled = false;
            else this.lnkSeeAll0.Enabled = true;

        }

        protected void AlterGridRendering(GridView grid)
        {
            if (grid.Rows.Count > 0)
            {
                grid.UseAccessibleHeader = true;
                grid.HeaderRow.TableSection = TableRowSection.TableHeader;
                grid.FooterRow.TableSection = TableRowSection.TableFooter;
            }
            
            grid.HeaderStyle.BorderStyle = BorderStyle.None;
            if (grid.HeaderRow != null)
            {
                foreach (TableCell cellObj in grid.HeaderRow.Cells)
                {
                    cellObj.BorderStyle = BorderStyle.None;
                }
            }
        }

        protected void setFollowUpMatrix(Label lblMatrix, decimal? quoted,decimal? actual, int opportunitiesCount, string action  )
        {
            string formattedValue = "" ;
            switch (GridUtilities.GetActivityPhaseEnum(action))
            {
                case Enums.OpportunitySteps.Qualifying:
                    {
                        formattedValue = string.Format("{0} Active, {1:c}", opportunitiesCount, quoted) + " estimated";
                        break;
                    }

                case Enums.OpportunitySteps.Responding:
                    {
                        formattedValue = string.Format("{0} Active, {1:c}", opportunitiesCount, quoted) + " quoted";
                        break;
                    }

                case Enums.OpportunitySteps.Completing:
                    {
                        formattedValue = string.Format("{0} Active, {1:c}", opportunitiesCount,quoted) + " quoted / " + string.Format("{0:c}", actual) + " actual";
                        break;
                    }
            }
            lblMatrix.Text = formattedValue;
        }

        protected void grvIdentified_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Session["USER"] == null) return;

                    bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
                    sp_web_FollowUpsResult suspect = (sp_web_FollowUpsResult)e.Row.DataItem;

                    Image flagged = (Image)e.Row.FindControl("imgFlagged");
                    flagged.Visible = suspect.Flagged;

                    Image type = (Image)e.Row.FindControl("imgBusinessType");
                    switch (suspect.BusinessTypeName)
                    {
                        case Enums.enBusinessType.NewBusiness:
                            type.ImageUrl = "~/images/OpportunityNewBusiness.gif";
                            break;
                        case Enums.enBusinessType.ReclaimedBusiness:
                            type.ImageUrl = "~/images/OpportunityReclaimedBusiness.gif";
                            break;
                        case Enums.enBusinessType.ExistingClients:
                            type.ImageUrl = "~/images/OpportunityExistingClients.gif";
                            break;
                        case Enums.enBusinessType.QuickQuote:
                            type.ImageUrl = "~/images/OpportunityQuickQuote.gif";
                            break;
                        case Enums.enBusinessType.QuickWin:
                            type.ImageUrl = "~/images/OpportunityQuickWin.gif";
                            break;
                        case Enums.enBusinessType.QuickCall:
                            type.ImageUrl = "~/images/OpportunityQuickCall.gif";
                            break;    
                    }

                    if (suspect.ClientName != null)
                    {
                        if (suspect.ClientName.Length > 30) e.Row.Cells[2].Text = suspect.ClientName.Remove(28) + "..";
                    }

                    if (suspect.OpportunityName != null)
                    {
                        if (suspect.OpportunityName.Length > 30) e.Row.Cells[6].Text = suspect.OpportunityName.Remove(28) + "..";
                    }

                    if (suspect.FollowUpDate < DateTime.Today)
                    {
                        e.Row.Cells[8].CssClass = "ochre-bold";
                    }

                    if (suspect.ClassificationName == "OAMPS Income above $20,000")
                    {
                        e.Row.Font.Bold = true;
                    }

                    e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewOpportunity.aspx?cid=" + DataBinder.Eval(e.Row.DataItem, "ClientID") + "&oid=" + DataBinder.Eval(e.Row.DataItem, "OpportunityID") + "'";
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void grvSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Session["USER"] == null) return;

                    bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
                    sp_web_FollowUpsResult suspect = (sp_web_FollowUpsResult)e.Row.DataItem;

                    Image flagged = (Image)e.Row.FindControl("imgFlagged");
                    flagged.Visible = suspect.Flagged;

                    Image type = (Image)e.Row.FindControl("imgBusinessType");
                    switch (suspect.BusinessTypeName)
                    {
                        case Enums.enBusinessType.NewBusiness:
                            type.ImageUrl = "~/images/OpportunityNewBusiness.gif";
                            break;
                        case Enums.enBusinessType.ReclaimedBusiness:
                            type.ImageUrl = "~/images/OpportunityReclaimedBusiness.gif";
                            break;
                        case Enums.enBusinessType.ExistingClients:
                            type.ImageUrl = "~/images/OpportunityExistingClients.gif";
                            break;
                        case Enums.enBusinessType.QuickQuote:
                            type.ImageUrl = "~/images/OpportunityQuickQuote.gif";
                            break;
                        case Enums.enBusinessType.QuickWin:
                            type.ImageUrl = "~/images/OpportunityQuickWin.gif";
                            break;
                        case Enums.enBusinessType.QuickCall:
                            type.ImageUrl = "~/images/OpportunityQuickCall.gif";
                            break;    
                    }

                    if (suspect.ClientName != null)
                    {
                        if (suspect.ClientName.Length > 30) e.Row.Cells[2].Text = suspect.ClientName.Remove(28) + "..";
                    }

                    if (suspect.OpportunityName != null)
                    {
                        if (suspect.OpportunityName.Length > 30) e.Row.Cells[6].Text = suspect.OpportunityName.Remove(28) + "..";
                    }

                    if (suspect.FollowUpDate < DateTime.Today)
                    {
                        e.Row.Cells[9].CssClass = "ochre-bold";
                    }

                    if (suspect.ClassificationName == "OAMPS Income above $20,000")
                    {
                        e.Row.Font.Bold = true;
                    }

                    e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewOpportunity.aspx?cid=" + DataBinder.Eval(e.Row.DataItem, "ClientID") + "&oid=" + DataBinder.Eval(e.Row.DataItem, "OpportunityID") + "'";
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }


        protected void grvSharedSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Session["USER"] == null) return;

                    bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
                    sp_web_FollowUpsResult suspect = (sp_web_FollowUpsResult)e.Row.DataItem;

                    Image flagged = (Image)e.Row.FindControl("imgFlagged");
                    flagged.Visible = suspect.Flagged;

                    Image type = (Image)e.Row.FindControl("imgBusinessType");
                    switch (suspect.BusinessTypeName)
                    {
                        case Enums.enBusinessType.NewBusiness:
                            type.ImageUrl = "~/images/OpportunityNewBusiness.gif";
                            break;
                        case Enums.enBusinessType.ReclaimedBusiness:
                            type.ImageUrl = "~/images/OpportunityReclaimedBusiness.gif";
                            break;
                        case Enums.enBusinessType.ExistingClients:
                            type.ImageUrl = "~/images/OpportunityExistingClients.gif";
                            break;
                        case Enums.enBusinessType.QuickQuote:
                            type.ImageUrl = "~/images/OpportunityQuickQuote.gif";
                            break;
                        case Enums.enBusinessType.QuickWin:
                            type.ImageUrl = "~/images/OpportunityQuickWin.gif";
                            break;
                        case Enums.enBusinessType.QuickCall:
                            type.ImageUrl = "~/images/OpportunityQuickCall.gif";
                            break;    
                    }

                    if (suspect.ClientName != null)
                    {
                        if (suspect.ClientName.Length > 30) e.Row.Cells[2].Text = suspect.ClientName.Remove(28) + "..";
                    }

                    if (suspect.OpportunityName != null)
                    {
                        if (suspect.OpportunityName.Length > 30) e.Row.Cells[6].Text = suspect.OpportunityName.Remove(28) + "..";
                    }

                    if (suspect.FollowUpDate < DateTime.Today)
                    {
                        e.Row.Cells[9].CssClass = "ochre-bold";
                    }

                    if (suspect.ClassificationName == "OAMPS Income above $20,000")
                    {
                        e.Row.Font.Bold = true;
                    }

                    e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewOpportunity.aspx?cid=" + DataBinder.Eval(e.Row.DataItem, "ClientID") + "&oid=" + DataBinder.Eval(e.Row.DataItem, "OpportunityID") + "'";
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        

        protected void grvQualifiedIn_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Session["USER"] == null) return;

                    bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
                    sp_web_FollowUpsResult suspect = (sp_web_FollowUpsResult)e.Row.DataItem;

                    Image flagged = (Image)e.Row.FindControl("imgFlagged");
                    if (suspect.Flagged == true) flagged.Visible = true;
                    else flagged.Visible = false;

                    Image type = (Image)e.Row.FindControl("imgBusinessType");
                    switch (suspect.BusinessTypeName)
                    {
                        case Enums.enBusinessType.NewBusiness:
                            type.ImageUrl = "~/images/OpportunityNewBusiness.gif";
                            break;
                        case Enums.enBusinessType.ReclaimedBusiness:
                            type.ImageUrl = "~/images/OpportunityReclaimedBusiness.gif";
                            break;
                        case Enums.enBusinessType.ExistingClients:
                            type.ImageUrl = "~/images/OpportunityExistingClients.gif";
                            break;
                        case Enums.enBusinessType.QuickQuote:
                            type.ImageUrl = "~/images/OpportunityQuickQuote.gif";
                            break;
                        case Enums.enBusinessType.QuickWin:
                            type.ImageUrl = "~/images/OpportunityQuickWin.gif";
                            break;
                        case Enums.enBusinessType.QuickCall:
                            type.ImageUrl = "~/images/OpportunityQuickCall.gif";
                            break;
                    }

                    if (suspect.ClientName != null)
                    {
                        if (suspect.ClientName.Length > 30) e.Row.Cells[2].Text = suspect.ClientName.Remove(28) + "..";
                    }

                    if (suspect.OpportunityName != null)
                    {
                        if (suspect.OpportunityName.Length > 30) e.Row.Cells[6].Text = suspect.OpportunityName.Remove(28) + "..";
                    }

                    if (suspect.FollowUpDate < DateTime.Today)
                    {
                        e.Row.Cells[9].CssClass = "ochre-bold";
                    }

                    if (suspect.ClassificationName == "OAMPS Income above $20,000")
                    {
                        e.Row.Font.Bold = true;
                    }

                    e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewOpportunity.aspx?cid=" + DataBinder.Eval(e.Row.DataItem, "ClientID") + "&oid=" + DataBinder.Eval(e.Row.DataItem, "OpportunityID") + "'";
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void grvInterested_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Session["USER"] == null) return;

                    bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
                    sp_web_FollowUpsResult suspect = (sp_web_FollowUpsResult)e.Row.DataItem;

                    Image flagged = (Image)e.Row.FindControl("imgFlagged");
                    if (suspect.Flagged == true) flagged.Visible = true;
                    else flagged.Visible = false;

                    Image type = (Image)e.Row.FindControl("imgBusinessType");
                    switch (suspect.BusinessTypeName)
                    {
                        case Enums.enBusinessType.NewBusiness:
                            type.ImageUrl = "~/images/OpportunityNewBusiness.gif";
                            break;
                        case Enums.enBusinessType.ReclaimedBusiness:
                            type.ImageUrl = "~/images/OpportunityReclaimedBusiness.gif";
                            break;
                        case Enums.enBusinessType.ExistingClients:
                            type.ImageUrl = "~/images/OpportunityExistingClients.gif";
                            break;
                        case Enums.enBusinessType.QuickQuote:
                            type.ImageUrl = "~/images/OpportunityQuickQuote.gif";
                            break;
                        case Enums.enBusinessType.QuickWin:
                            type.ImageUrl = "~/images/OpportunityQuickWin.gif";
                            break;
                        case Enums.enBusinessType.QuickCall:
                            type.ImageUrl = "~/images/OpportunityQuickCall.gif";
                            break;
                    }

                    if (suspect.ClientName != null)
                    {
                        if (suspect.ClientName.Length > 30) e.Row.Cells[2].Text = suspect.ClientName.Remove(28) + "..";
                    }

                    if (suspect.OpportunityName != null)
                    {
                        if (suspect.OpportunityName.Length > 30) e.Row.Cells[6].Text = suspect.OpportunityName.Remove(28) + "..";
                    }

                    if (suspect.FollowUpDate < DateTime.Today)
                    {
                        e.Row.Cells[9].CssClass = "ochre-bold";
                    }

                    if (suspect.ClassificationName == "OAMPS Income above $20,000")
                    {
                        e.Row.Font.Bold = true;
                    }

                    e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewOpportunity.aspx?cid=" + DataBinder.Eval(e.Row.DataItem, "ClientID") + "&oid=" + DataBinder.Eval(e.Row.DataItem, "OpportunityID") + "'";
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void grvGoToMarket_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Session["USER"] == null) return;

                    bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
                    sp_web_FollowUpsResult suspect = (sp_web_FollowUpsResult)e.Row.DataItem;

                    Image flagged = (Image)e.Row.FindControl("imgFlagged");
                    if (suspect.Flagged == true) flagged.Visible = true;
                    else flagged.Visible = false;

                    Image type = (Image)e.Row.FindControl("imgBusinessType");
                    switch (suspect.BusinessTypeName)
                    {
                        case Enums.enBusinessType.NewBusiness:
                            type.ImageUrl = "~/images/OpportunityNewBusiness.gif";
                            break;
                        case Enums.enBusinessType.ReclaimedBusiness:
                            type.ImageUrl = "~/images/OpportunityReclaimedBusiness.gif";
                            break;
                        case Enums.enBusinessType.ExistingClients:
                            type.ImageUrl = "~/images/OpportunityExistingClients.gif";
                            break;
                        case Enums.enBusinessType.QuickQuote:
                            type.ImageUrl = "~/images/OpportunityQuickQuote.gif";
                            break;
                        case Enums.enBusinessType.QuickWin:
                            type.ImageUrl = "~/images/OpportunityQuickWin.gif";
                            break;
                        case Enums.enBusinessType.QuickCall:
                            type.ImageUrl = "~/images/OpportunityQuickCall.gif";
                            break;
                    }

                    if (suspect.ClientName != null)
                    {
                        if (suspect.ClientName.Length > 30) e.Row.Cells[2].Text = suspect.ClientName.Remove(28) + "..";
                    }

                    if (suspect.OpportunityName != null)
                    {
                        if (suspect.OpportunityName.Length > 30) e.Row.Cells[6].Text = suspect.OpportunityName.Remove(28) + "..";
                    }

                    if (suspect.FollowUpDate < DateTime.Today)
                    {
                        e.Row.Cells[8].CssClass = "ochre-bold";
                    }

                    if (suspect.ClassificationName == "OAMPS Income above $20,000")
                    {
                        e.Row.Font.Bold = true;
                    }

                    e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewOpportunity.aspx?cid=" + DataBinder.Eval(e.Row.DataItem, "ClientID") + "&oid=" + DataBinder.Eval(e.Row.DataItem, "OpportunityID") + "'";
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void grvQuoted_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Session["USER"] == null) return;

                    bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
                    sp_web_FollowUpsResult suspect = (sp_web_FollowUpsResult)e.Row.DataItem;

                    Image flagged = (Image)e.Row.FindControl("imgFlagged");
                    if (suspect.Flagged == true) flagged.Visible = true;
                    else flagged.Visible = false;

                    Image type = (Image)e.Row.FindControl("imgBusinessType");
                    switch (suspect.BusinessTypeName)
                    {
                        case Enums.enBusinessType.NewBusiness:
                            type.ImageUrl = "~/images/OpportunityNewBusiness.gif";
                            break;
                        case Enums.enBusinessType.ReclaimedBusiness:
                            type.ImageUrl = "~/images/OpportunityReclaimedBusiness.gif";
                            break;
                        case Enums.enBusinessType.ExistingClients:
                            type.ImageUrl = "~/images/OpportunityExistingClients.gif";
                            break;
                        case Enums.enBusinessType.QuickQuote:
                            type.ImageUrl = "~/images/OpportunityQuickQuote.gif";
                            break;
                        case Enums.enBusinessType.QuickWin:
                            type.ImageUrl = "~/images/OpportunityQuickWin.gif";
                            break;
                        case Enums.enBusinessType.QuickCall:
                            type.ImageUrl = "~/images/OpportunityQuickCall.gif";
                            break;
                    }

                    if (suspect.ClientName != null)
                    {
                        if (suspect.ClientName.Length > 30) e.Row.Cells[2].Text = suspect.ClientName.Remove(28) + "..";
                    }

                    if (suspect.OpportunityName != null)
                    {
                        if (suspect.OpportunityName.Length > 30) e.Row.Cells[6].Text = suspect.OpportunityName.Remove(28) + "..";
                    }

                    if (suspect.FollowUpDate < DateTime.Today)
                    {
                        e.Row.Cells[8].CssClass = "ochre-bold";
                    }

                    if (suspect.ClassificationName == "OAMPS Income above $20,000")
                    {
                        e.Row.Font.Bold = true;
                    }

                    e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewOpportunity.aspx?cid=" + DataBinder.Eval(e.Row.DataItem, "ClientID") + "&oid=" + DataBinder.Eval(e.Row.DataItem, "OpportunityID") + "'";
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void grvAccepted_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Session["USER"] == null) return;

                    bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
                    sp_web_FollowUpsResult suspect = (sp_web_FollowUpsResult)e.Row.DataItem;

                    Image flagged = (Image)e.Row.FindControl("imgFlagged");
                    if (suspect.Flagged == true) flagged.Visible = true;
                    else flagged.Visible = false;

                    Image type = (Image)e.Row.FindControl("imgBusinessType");
                    switch (suspect.BusinessTypeName)
                    {
                        case Enums.enBusinessType.NewBusiness:
                            type.ImageUrl = "~/images/OpportunityNewBusiness.gif";
                            break;
                        case Enums.enBusinessType.ReclaimedBusiness:
                            type.ImageUrl = "~/images/OpportunityReclaimedBusiness.gif";
                            break;
                        case Enums.enBusinessType.ExistingClients:
                            type.ImageUrl = "~/images/OpportunityExistingClients.gif";
                            break;
                        case Enums.enBusinessType.QuickQuote:
                            type.ImageUrl = "~/images/OpportunityQuickQuote.gif";
                            break;
                        case Enums.enBusinessType.QuickWin:
                            type.ImageUrl = "~/images/OpportunityQuickWin.gif";
                            break;
                        case Enums.enBusinessType.QuickCall:
                            type.ImageUrl = "~/images/OpportunityQuickCall.gif";
                            break;
                    }

                    if (suspect.ClientName != null)
                    {
                        if (suspect.ClientName.Length > 30) e.Row.Cells[2].Text = suspect.ClientName.Remove(28) + "..";
                    }

                    if (suspect.OpportunityName != null)
                    {
                        if (suspect.OpportunityName.Length > 30) e.Row.Cells[6].Text = suspect.OpportunityName.Remove(28) + "..";
                    }

                    if (suspect.FollowUpDate < DateTime.Today)
                    {
                        e.Row.Cells[9].CssClass = "ochre-bold";
                    }

                    if (suspect.ClassificationName == "OAMPS Income above $20,000")
                    {
                        e.Row.Font.Bold = true;
                    }

                    e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewOpportunity.aspx?cid=" + DataBinder.Eval(e.Row.DataItem, "ClientID") + "&oid=" + DataBinder.Eval(e.Row.DataItem, "OpportunityID") + "'";
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void lnkSeeAllSuspects_Click(object sender, EventArgs e)
        {
            try
            {
                var usertosend = bizUser.GetAccountExecutiveIdBySmiUserName(ddlExecutive.SelectedValue);

                Response.Redirect("FollowUpsAll.aspx?f1=" + this.ddlOAMPSIncome.SelectedValue 
                                + "&f2=" + this.ddlBusinessType.SelectedValue 
                                + "&f3=" + this.ddlFlagged.SelectedValue
                                + "&l1=" + ddlRegion.SelectedValue
                                + "&l2=" + ddlBranch.SelectedValue
                                + "&l3=" + usertosend
                                + "&t=" + summarySortCol.Value
                                + "&gr=" + ((LinkButton)sender).CommandArgument, false);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnFilterOpportunities_Click(object sender, EventArgs e)
        {
            try
            {
                PopulateFollowUps();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }


        #region regions and excutives

        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];

                FilterGridsUtilities.PopulateBranches(user, ddlRegion, ddlBranch);
                FilterGridsUtilities.PopulateExecutives(user, ddlBranch, ddlExecutive, DisableExecutives);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }



        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];

                FilterGridsUtilities.PopulateExecutives(user,ddlBranch,ddlExecutive,DisableExecutives);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }


        #endregion

        protected void btnShareQueue_Click(object sender, EventArgs e)
        {

        }

    }
}
