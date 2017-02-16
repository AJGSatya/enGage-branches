using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using enGage.BL;
using enGage.DL;
using enGage.Web.Helper;

namespace enGage.Web
{
    public partial class AssignQueue : Page
    {
        private List<sp_web_FollowUpsResult> _allIdentify, _allQualify, _allRespond, _allComplete;
        public bool DisableExecutives { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (Timeline.Capture("AssignQueue.aspx", "ASP.NET"))
                {
                    if (!IsPostBack)
                    {
                        //((Main)Master).AuthenticateUser();

                        if (Session["USER"] == null)
                        {
                            var bizM = new bizMessage();
                            ucMessanger1.ProcessMessage("Session: " + bizM.GetMessageText("SessionMissing"),
                                Enums.enMsgType.Err, "", null, true);
                            return;
                        }

                        ((Main) Master).HeaderTitle = "Assign Oportunities ";
                        var user = (bizUser.enGageUser) Session["USER"];

                        PopulateFilters();
                        FilterGridsUtilities.PopulateRegionsBranchesAndExecutives(user, ddlFilterRegion, ddlFilterBranch,
                            ddlFilterExcutives,
                            (user.Role == (int) Enums.enUserRole.Executive || user.Role == (int) Enums.enUserRole.Branch)
                                ? false
                                : true, true);
                        FilterGridsUtilities.PopulateRegionsBranchesAndExecutives(user, ddlRegion, ddlBranch,
                            ddlExecutive,
                            (user.Role == (int) Enums.enUserRole.Executive || user.Role == (int) Enums.enUserRole.Branch)
                                ? false
                                : true, false);

                        ddlFilterRegion_SelectedIndexChanged(ddlRegion, new EventArgs());
                        ddlFilterBranch_SelectedIndexChanged(ddlRegion, new EventArgs());
                        PopulateFollowUps();
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
            var biz = new bizOpportunity();

            // OAMPS Income
            var cls = biz.ListClassifications();
            ucMessanger1.ProcessMessages(biz.MSGS, true);
            ddlOAMPSIncome.Items.Add(new ListItem("OAMPS Income (All)", "0"));
            foreach (var cl in cls)
            {
                ddlOAMPSIncome.Items.Add(new ListItem(cl.ClassificationName, cl.ClassificationID.ToString()));
            }

            // Business Type
            var bts = biz.ListBusinessTypes();
            ucMessanger1.ProcessMessages(biz.MSGS, false);
            ddlBusinessType.Items.Add(new ListItem("Type (All)", "0"));
            foreach (var bt in bts)
            {
                ddlBusinessType.Items.Add(new ListItem(bt.BusinessTypeName, bt.BusinessTypeID.ToString()));
            }
        }

        private void PopulateFollowUps()
        {
            var bizM = new bizMessage();
            if (Session["USER"] == null)
            {
                ucMessanger1.ProcessMessage("Session: " + bizM.GetMessageText("SessionMissing"), Enums.enMsgType.Err, "",
                    null, true);
                return;
            }
            var user = (bizUser.enGageUser) Session["USER"];

            var biz = new bizActivity();

            bool isRegion, isBranch, isAll;
            isRegion = isBranch = isAll = false;
            GridUtilities.SetUserAllowedOpportunities(user, ref isRegion, ref isBranch, ref isAll);

            var d1 = biz.GetFollowUps(ddlFilterExcutives.SelectedValue, int.Parse(ddlOAMPSIncome.SelectedValue),
                int.Parse(ddlBusinessType.SelectedValue), int.Parse(ddlFlagged.SelectedValue), "Recognise",
                ddlFilterRegion.SelectedValue, ddlFilterBranch.SelectedValue, isAll);
            var d2 = biz.GetFollowUps(ddlFilterExcutives.SelectedValue, int.Parse(ddlOAMPSIncome.SelectedValue),
                int.Parse(ddlBusinessType.SelectedValue), int.Parse(ddlFlagged.SelectedValue), "Qualify",
                ddlFilterRegion.SelectedValue, ddlFilterBranch.SelectedValue, isAll);
            var d3 = biz.GetFollowUps(ddlFilterExcutives.SelectedValue, int.Parse(ddlOAMPSIncome.SelectedValue),
                int.Parse(ddlBusinessType.SelectedValue), int.Parse(ddlFlagged.SelectedValue), "Contact",
                ddlFilterRegion.SelectedValue, ddlFilterBranch.SelectedValue, isAll);
            var d4 = biz.GetFollowUps(ddlFilterExcutives.SelectedValue, int.Parse(ddlOAMPSIncome.SelectedValue),
                int.Parse(ddlBusinessType.SelectedValue), int.Parse(ddlFlagged.SelectedValue), "Discover",
                ddlFilterRegion.SelectedValue, ddlFilterBranch.SelectedValue, isAll);
            var d5 = biz.GetFollowUps(ddlFilterExcutives.SelectedValue, int.Parse(ddlOAMPSIncome.SelectedValue),
                int.Parse(ddlBusinessType.SelectedValue), int.Parse(ddlFlagged.SelectedValue), "Respond",
                ddlFilterRegion.SelectedValue, ddlFilterBranch.SelectedValue, isAll);
            var d6 = biz.GetFollowUps(ddlFilterExcutives.SelectedValue, int.Parse(ddlOAMPSIncome.SelectedValue),
                int.Parse(ddlBusinessType.SelectedValue), int.Parse(ddlFlagged.SelectedValue), "Agree",
                ddlFilterRegion.SelectedValue, ddlFilterBranch.SelectedValue, isAll);
            var d7 = biz.GetFollowUps(ddlFilterExcutives.SelectedValue, int.Parse(ddlOAMPSIncome.SelectedValue),
                int.Parse(ddlBusinessType.SelectedValue), int.Parse(ddlFlagged.SelectedValue), "Process",
                ddlFilterRegion.SelectedValue, ddlFilterBranch.SelectedValue, isAll);


            _allIdentify =
                d1.Where(x => x.OutcomeType != "C" && x.FollowUpDate >= CalenderUtilities.CutOffDate).ToList();
            _allQualify =
                d2.Concat(d1)
                    .Where(x => x.OutcomeType != "C" && x.FollowUpDate >= CalenderUtilities.CutOffDate)
                    .ToList();
            _allRespond =
                d3.Concat(d4)
                    .Concat(d5)
                    .Where(x => x.OutcomeType != "C" && x.FollowUpDate >= CalenderUtilities.CutOffDate)
                    .ToList();
            _allComplete =
                d6.Concat(d7)
                    .Where(x => x.OutcomeType != "C" && x.FollowUpDate >= CalenderUtilities.CutOffDate)
                    .ToList();

            // get the summary matrix for opprtunities
            decimal? estimated = 0, quoted = 0, actual = 0;
            int? noOfOpportunities = 0;

            estimated = _allQualify.Sum(x => (x.NetBrokerageEstimated.HasValue) ? x.NetBrokerageEstimated : 0);
            quoted = _allRespond.Sum(x => (x.NetBrokerageQuoted.HasValue) ? x.NetBrokerageQuoted : 0);
            actual = _allComplete.Sum(x => (x.NetBrokerageActual.HasValue) ? x.NetBrokerageActual : 0);


            //biz.FollowUpsMatrix(user.UserName, string.Join(",", new string[] { Enums.ActivityActions.Qualify.ToString(), Enums.ActivityActions.Recognise.ToString() }),false,CalenderUtilities.CutOffDate, ref noOfOpportunities, ref estimated, ref quoted, ref actual);
            setFollowUpMatrix(lblQualifyMatrix, estimated, actual, _allQualify.Count,
                Enums.ActivityActions.Qualify.ToString());

            //biz.FollowUpsMatrix(user.UserName, string.Join(",", new string[] { Enums.ActivityActions.Contact.ToString(), Enums.ActivityActions.Discover.ToString(), Enums.ActivityActions.Respond.ToString() }),false,CalenderUtilities.CutOffDate, ref noOfOpportunities, ref estimated, ref quoted, ref actual);
            setFollowUpMatrix(lblRespondMatrix, quoted, actual, _allRespond.Count,
                Enums.ActivityActions.Contact.ToString());

            //biz.FollowUpsMatrix(user.UserName, string.Join(",", new string[] { Enums.ActivityActions.Agree.ToString(), Enums.ActivityActions.Process.ToString() }),false,CalenderUtilities.CutOffDate, ref noOfOpportunities, ref estimated, ref quoted, ref actual);
            setFollowUpMatrix(lblCompleteMatrix, quoted, actual, _allComplete.Count,
                Enums.ActivityActions.Agree.ToString());


            //this.grvIdentified.DataSource = allIdentify;
            //this.ucMessanger1.ProcessMessages(biz.MSGS, true);
            //this.grvIdentified.DataBind();

            grvQualifiedIn.DataSource = _allQualify;
            ucMessanger1.ProcessMessages(biz.MSGS, true);
            grvQualifiedIn.DataBind();

            grvInterested.DataSource = _allRespond;
            ucMessanger1.ProcessMessages(biz.MSGS, true);
            grvInterested.DataBind();

            grvAccepted.DataSource = _allComplete;
            ucMessanger1.ProcessMessages(biz.MSGS, true);
            grvAccepted.DataBind();


            // combine all results to have a filtered summery for the week
            var allFollowups =
                d1.Concat(d2)
                    .Concat(d3)
                    .Concat(d4)
                    .Concat(d5)
                    .Concat(d6)
                    .Where(x => x.OutcomeType != "C" && x.FollowUpDate >= CalenderUtilities.CutOffDate)
                    .ToList();
            ;
            var weekStartDate = (DateTime.Now.AddDays(-(DateTime.Now.DayOfWeek - DayOfWeek.Monday)));
            var weekEndDate = weekStartDate.AddDays(6);
            var weeklyFilteredFollowUps = allFollowups.Where(x =>
            {
                return ((x.FollowUpDate.HasValue)
                    ? (x.FollowUpDate.Value.Date >= weekStartDate.Date
                       && x.FollowUpDate.Value.Date <= weekEndDate.Date)
                    : false)
                       ||
                       ((x.OpportunityDue.HasValue)
                           ? DateTime.Now.Date >= x.OpportunityDue.Value.Date
                           : false
                           )
                    ;
            }).ToList();

            grvSummary.DataSource = weeklyFilteredFollowUps;
            ucMessanger1.ProcessMessages(biz.MSGS, true);
            grvSummary.DataBind();

            // change the way the gridview displays the header
            AlterGridRendering(grvSummary);
            AlterGridRendering(grvQualifiedIn);
            AlterGridRendering(grvInterested);
            AlterGridRendering(grvAccepted);


            if (weeklyFilteredFollowUps.Count() <= grvSummary.PageSize) lnkSeeAll0.Enabled = false;
            else lnkSeeAll0.Enabled = true;
            lblQualifiedIn.Text = "Qualifying"; // + _allQualify.Count.ToString() + " active)";
            lnkSeeAll2.Enabled = true;
            lblInterested.Text = "Responding"; // + _allRespond.Count.ToString() + " active)";
            lnkSeeAll3.Enabled = true;
            lblAccepted.Text = "Completing"; // + _allComplete.Count.ToString() + " active)";
            lnkSeeAll6.Enabled = true;
        }


        protected void AlterGridRendering(GridView grid)
        {
            if (grid.Rows.Count > 0)
            {
                grid.UseAccessibleHeader = true;
                grid.HeaderRow.TableSection = TableRowSection.TableHeader;
                grid.FooterRow.TableSection = TableRowSection.TableFooter;
            }

            //foreach (TableRow row in grid.Rows)
            //    foreach (TableCell cell in row.Cells)
            //{
            //    cell.ToolTip = cell.Text;
            //}
        }

        protected void setFollowUpMatrix(Label lblMatrix, decimal? quoted, decimal? actual, int opportunitiesCount,
            string action)
        {
            var formattedValue = "";
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
                    formattedValue = string.Format("{0} Active, {1:c}", opportunitiesCount, quoted) + " quoted / " +
                                     string.Format("{0:c}", actual) + " actual";
                    break;
                }
            }
            lblMatrix.Text = formattedValue;
        }


        protected void btnShowAll_Click(object sender, EventArgs e)
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

        protected void grvIdentified_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Session["USER"] == null) return;

                    var user = (bizUser.enGageUser) Session["USER"];
                    var suspect = (sp_web_FollowUpsResult) e.Row.DataItem;

                    var flagged = (Image) e.Row.FindControl("imgFlagged");
                    flagged.Visible = suspect.Flagged;

                    var type = (Image) e.Row.FindControl("imgBusinessType");
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
                        if (suspect.OpportunityName.Length > 30)
                            e.Row.Cells[6].Text = suspect.OpportunityName.Remove(28) + "..";
                    }

                    if (suspect.FollowUpDate < DateTime.Today)
                    {
                        e.Row.Cells[8].CssClass = "ochre-bold";
                    }

                    if (suspect.ClassificationName == "OAMPS Income above $20,000")
                    {
                        e.Row.Font.Bold = true;
                    }

                    e.Row.Attributes.Add("OnMouseOver",
                        "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewOpportunity.aspx?cid=" +
                                                  DataBinder.Eval(e.Row.DataItem, "ClientID") + "&oid=" +
                                                  DataBinder.Eval(e.Row.DataItem, "OpportunityID") + "'";
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

                    var user = (bizUser.enGageUser) Session["USER"];
                    var suspect = (sp_web_FollowUpsResult) e.Row.DataItem;

                    var flagged = (Image) e.Row.FindControl("imgFlagged");
                    flagged.Visible = suspect.Flagged;

                    var type = (Image) e.Row.FindControl("imgBusinessType");
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
                        if (suspect.OpportunityName.Length > 30)
                            e.Row.Cells[6].Text = suspect.OpportunityName.Remove(28) + "..";
                    }

                    if (suspect.FollowUpDate < DateTime.Today)
                    {
                        e.Row.Cells[8].CssClass = "ochre-bold";
                    }

                    if (suspect.ClassificationName == "OAMPS Income above $20,000")
                    {
                        e.Row.Font.Bold = true;
                    }

                    e.Row.Attributes.Add("OnMouseOver",
                        "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewOpportunity.aspx?cid=" +
                                                  DataBinder.Eval(e.Row.DataItem, "ClientID") + "&oid=" +
                                                  DataBinder.Eval(e.Row.DataItem, "OpportunityID") + "'";
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

                    var user = (bizUser.enGageUser) Session["USER"];
                    var suspect = (sp_web_FollowUpsResult) e.Row.DataItem;

                    var flagged = (Image) e.Row.FindControl("imgFlagged");
                    if (suspect.Flagged) flagged.Visible = true;
                    else flagged.Visible = false;

                    var type = (Image) e.Row.FindControl("imgBusinessType");
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
                        if (suspect.OpportunityName.Length > 30)
                            e.Row.Cells[6].Text = suspect.OpportunityName.Remove(28) + "..";
                    }

                    if (suspect.FollowUpDate < DateTime.Today)
                    {
                        e.Row.Cells[8].CssClass = "ochre-bold";
                    }

                    if (suspect.ClassificationName == "OAMPS Income above $20,000")
                    {
                        e.Row.Font.Bold = true;
                    }

                    e.Row.Attributes.Add("OnMouseOver",
                        "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewOpportunity.aspx?cid=" +
                                                  DataBinder.Eval(e.Row.DataItem, "ClientID") + "&oid=" +
                                                  DataBinder.Eval(e.Row.DataItem, "OpportunityID") + "'";
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

                    var user = (bizUser.enGageUser) Session["USER"];
                    var suspect = (sp_web_FollowUpsResult) e.Row.DataItem;

                    var flagged = (Image) e.Row.FindControl("imgFlagged");
                    if (suspect.Flagged) flagged.Visible = true;
                    else flagged.Visible = false;

                    var type = (Image) e.Row.FindControl("imgBusinessType");
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
                        if (suspect.OpportunityName.Length > 30)
                            e.Row.Cells[6].Text = suspect.OpportunityName.Remove(28) + "..";
                    }

                    if (suspect.FollowUpDate < DateTime.Today)
                    {
                        e.Row.Cells[8].CssClass = "ochre-bold";
                    }

                    if (suspect.ClassificationName == "OAMPS Income above $20,000")
                    {
                        e.Row.Font.Bold = true;
                    }

                    e.Row.Attributes.Add("OnMouseOver",
                        "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewOpportunity.aspx?cid=" +
                                                  DataBinder.Eval(e.Row.DataItem, "ClientID") + "&oid=" +
                                                  DataBinder.Eval(e.Row.DataItem, "OpportunityID") + "'";
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

                    var user = (bizUser.enGageUser) Session["USER"];
                    var suspect = (sp_web_FollowUpsResult) e.Row.DataItem;

                    var flagged = (Image) e.Row.FindControl("imgFlagged");
                    if (suspect.Flagged) flagged.Visible = true;
                    else flagged.Visible = false;

                    var type = (Image) e.Row.FindControl("imgBusinessType");
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
                        if (suspect.OpportunityName.Length > 30)
                            e.Row.Cells[6].Text = suspect.OpportunityName.Remove(28) + "..";
                    }

                    if (suspect.FollowUpDate < DateTime.Today)
                    {
                        e.Row.Cells[8].CssClass = "ochre-bold";
                    }

                    if (suspect.ClassificationName == "OAMPS Income above $20,000")
                    {
                        e.Row.Font.Bold = true;
                    }

                    e.Row.Attributes.Add("OnMouseOver",
                        "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewOpportunity.aspx?cid=" +
                                                  DataBinder.Eval(e.Row.DataItem, "ClientID") + "&oid=" +
                                                  DataBinder.Eval(e.Row.DataItem, "OpportunityID") + "'";
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

                    var user = (bizUser.enGageUser) Session["USER"];
                    var suspect = (sp_web_FollowUpsResult) e.Row.DataItem;

                    var flagged = (Image) e.Row.FindControl("imgFlagged");
                    if (suspect.Flagged) flagged.Visible = true;
                    else flagged.Visible = false;

                    var type = (Image) e.Row.FindControl("imgBusinessType");
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
                        if (suspect.OpportunityName.Length > 30)
                            e.Row.Cells[6].Text = suspect.OpportunityName.Remove(28) + "..";
                    }

                    if (suspect.FollowUpDate < DateTime.Today)
                    {
                        e.Row.Cells[8].CssClass = "ochre-bold";
                    }

                    if (suspect.ClassificationName == "OAMPS Income above $20,000")
                    {
                        e.Row.Font.Bold = true;
                    }

                    e.Row.Attributes.Add("OnMouseOver",
                        "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewOpportunity.aspx?cid=" +
                                                  DataBinder.Eval(e.Row.DataItem, "ClientID") + "&oid=" +
                                                  DataBinder.Eval(e.Row.DataItem, "OpportunityID") + "'";
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

                    var user = (bizUser.enGageUser) Session["USER"];
                    var suspect = (sp_web_FollowUpsResult) e.Row.DataItem;

                    var flagged = (Image) e.Row.FindControl("imgFlagged");
                    if (suspect.Flagged) flagged.Visible = true;
                    else flagged.Visible = false;

                    var type = (Image) e.Row.FindControl("imgBusinessType");
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
                        if (suspect.OpportunityName.Length > 30)
                            e.Row.Cells[6].Text = suspect.OpportunityName.Remove(28) + "..";
                    }

                    if (suspect.FollowUpDate < DateTime.Today)
                    {
                        e.Row.Cells[8].CssClass = "ochre-bold";
                    }

                    if (suspect.ClassificationName == "OAMPS Income above $20,000")
                    {
                        e.Row.Font.Bold = true;
                    }

                    e.Row.Attributes.Add("OnMouseOver",
                        "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewOpportunity.aspx?cid=" +
                                                  DataBinder.Eval(e.Row.DataItem, "ClientID") + "&oid=" +
                                                  DataBinder.Eval(e.Row.DataItem, "OpportunityID") + "'";
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
                Response.Redirect("FollowUpsAll.aspx?f1=" + ddlOAMPSIncome.SelectedValue
                                  + "&f2=" + ddlBusinessType.SelectedValue
                                  + "&f3=" + ddlFlagged.SelectedValue
                                  + "&t=" + summarySortCol.Value
                                  + "&gr=" + ((LinkButton) sender).CommandArgument, false);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected List<string> GetSelectedOpportunitiesIDs(GridView grid)
        {
            var selectedOppoIDs = new List<string>();
            // get all selected opportunities
            foreach (GridViewRow row in grid.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    var chk = row.Cells[0].FindControl("chkbOpprtunitySelect") as CheckBox;
                    if (chk != null && chk.Checked)
                    {
                        selectedOppoIDs.Add(grvSummary.DataKeys[row.RowIndex].Value.ToString());
                    }
                }
            }

            return selectedOppoIDs;
        }

        protected void btnassignSelectedOpportunities_Click(object sender, EventArgs e)
        {
            if (ddlExecutive.SelectedIndex > 0)
            {
                var user = (bizUser.enGageUser) Session["USER"];

                var AssignedToExcutiveId = ddlExecutive.SelectedValue;
                // get all selected opportunitiesids
                var selectedOpprtunityIDs =
                    selectedOpportunitiesIDs.Value.Split('#').Where(x => x.Trim() != "").Distinct();
                var opportunitiesList = string.Join(",", selectedOpprtunityIDs.ToArray());

                // assign the Oppprtunities
                var biz = new bizOpportunity();
                biz.AssignQueueOppportunitiesToExcutive(user.UserName, AssignedToExcutiveId, opportunitiesList);

                // refresh the grids
                PopulateFollowUps();
            }
            else
            {
                ucMessanger2.ProcessMessage("Please Select an Executive", Enums.enMsgType.Err, "", null, true);
                // refresh the grids
                PopulateFollowUps();
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

        protected void ddlFilterRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var user = (bizUser.enGageUser) Session["USER"];

                FilterGridsUtilities.PopulateBranches(user, ddlFilterRegion, ddlFilterBranch);
                FilterGridsUtilities.PopulateExecutives(user, ddlFilterBranch, ddlFilterExcutives, DisableExecutives);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void ddlFilterBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var user = (bizUser.enGageUser) Session["USER"];

                FilterGridsUtilities.PopulateExecutives(user, ddlFilterBranch, ddlFilterExcutives, DisableExecutives);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }


        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var user = (bizUser.enGageUser) Session["USER"];

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
                var user = (bizUser.enGageUser) Session["USER"];

                FilterGridsUtilities.PopulateExecutives(user, ddlBranch, ddlExecutive, DisableExecutives);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        #endregion
    }
}