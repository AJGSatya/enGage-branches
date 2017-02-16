using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using enGage.BL;
using enGage.DL;
using enGage.Web.Helper;

namespace enGage.Web
{
    public partial class FollowUpsAll : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    bizMessage bizM = new bizMessage();
                    if (Session["USER"] == null)
                    {
                        this.ucMessanger1.ProcessMessage("Session: " + bizM.GetMessageText("SessionMissing"), Enums.enMsgType.Err, "", null, true);
                        return;
                    }

                    if (Request.QueryString["gr"] == null) return;

                    string see = Request.QueryString["gr"];
                    switch (see)
                    {
                        case "0":
                            Populatesummary();
                            break;
                        case "1":
                            PopulateIdentified();
                            break;
                        case "2":
                            PopulateQualifiedIn();
                            break;
                        case "3":
                            PopulateInterested();
                            break;
                        case "4":
                            PopulateGoToMarket();
                            break;
                        case "5":
                            PopulateQuoted();
                            break;
                        case "6":
                            PopulateAccepted();
                            break;
                        case "7":
                            PopulateSharedSummary();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
           /* // sort the grid with the passed parameter
            if (!string.IsNullOrEmpty(Request.QueryString["t"]))
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Sort_SUMMARY", " var sorting=" + Request.QueryString["t"] + "; setTimeout( function(){ " +
                "$(\"#" + grvSummary.ClientID + "\").trigger(\"sorton\",[sorting])" +
                "}, 100);", true);


            if (!string.IsNullOrEmpty(Request.QueryString["ts"]))
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Sort_SharedSUMMARY", " var sortingShared=" + Request.QueryString["t"] + "; setTimeout( function(){ " +
                "$(\"#" + grvSharedSummary.ClientID + "\").trigger(\"sorton\",[sortingShared])" +
                "}, 100);", true);   
            */

            base.OnPreRender(e);
        }

        private void SetUserAllowedOpportunities(bizUser.enGageUser user, Boolean isRegion, Boolean isBranch, Boolean isAll)
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

        private void Populatesummary()
        {

            ((Main)Master).HeaderTitle = "Current Activities";

            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
            bizActivity biz = new bizActivity();
            bool isRegion, isBranch, isAll;
            isRegion = isBranch = isAll = false;
            GridUtilities.SetUserAllowedOpportunities(user, ref isRegion, ref isBranch, ref isAll);


            List<sp_web_FollowUpsResult> d = biz.GetFollowUps(Request.QueryString["l3"]
                                             , int.Parse(Request.QueryString["f1"])
                                             , int.Parse(Request.QueryString["f2"])
                                             , int.Parse(Request.QueryString["f3"])
                                             , "Recognise"
                                             , Request.QueryString["l1"]
                                             , Request.QueryString["l2"]
                                             , isAll);

            List<sp_web_FollowUpsResult> d1 = biz.GetFollowUps(Request.QueryString["l3"]
                                             , int.Parse(Request.QueryString["f1"])
                                             , int.Parse(Request.QueryString["f2"])
                                             , int.Parse(Request.QueryString["f3"])
                                             , "Qualify"
                                             , Request.QueryString["l1"]
                                             , Request.QueryString["l2"]
                                             , isAll);

            List<sp_web_FollowUpsResult> d2 = biz.GetFollowUps(Request.QueryString["l3"]
                                              , int.Parse(Request.QueryString["f1"])
                                              , int.Parse(Request.QueryString["f2"])
                                              , int.Parse(Request.QueryString["f3"])
                                              , "Contact"
                                              , Request.QueryString["l1"]
                                              , Request.QueryString["l2"]
                                              , isAll);

            List<sp_web_FollowUpsResult> d3 = biz.GetFollowUps(Request.QueryString["l3"]
                                           
                                               , int.Parse(Request.QueryString["f1"])
                                               , int.Parse(Request.QueryString["f2"])
                                               , int.Parse(Request.QueryString["f3"])
                                               , "Discover"
                                               , Request.QueryString["l1"]
                                               , Request.QueryString["l2"]
                                               , isAll);
            List<sp_web_FollowUpsResult> d4 = biz.GetFollowUps(Request.QueryString["l3"]
                                            
                                               , int.Parse(Request.QueryString["f1"])
                                               , int.Parse(Request.QueryString["f2"])
                                               , int.Parse(Request.QueryString["f3"])
                                               , "Respond"
                                               , Request.QueryString["l2"]
                                               , Request.QueryString["l3"]
                                               , isAll);

            List<sp_web_FollowUpsResult> d5 = biz.GetFollowUps( Request.QueryString["l3"]
                                             , int.Parse(Request.QueryString["f1"])
                                             , int.Parse(Request.QueryString["f2"])
                                             , int.Parse(Request.QueryString["f3"])
                                             , "Agree"
                                             , Request.QueryString["l1"]
                                             , Request.QueryString["l2"]
                                             , isAll);


            // combine all results to have a filtered summery for the week
            var allList = d.Concat(d1).Concat(d2).Concat(d3).Concat(d4).Concat(d5).ToList();
            allList = allList.Where(x => x.OutcomeType != "C" && x.FollowUpDate >= CalenderUtilities.CutOffDate).ToList();
            var weekStartDate = (DateTime.Now.AddDays(-(DateTime.Now.DayOfWeek - DayOfWeek.Monday)));
            var weekEndDate = weekStartDate.AddDays(6);
            var weeklyFilteredFollowUps = allList.Where(x =>
            {
                return (
                    (x.OutcomeType!="C")&&(x.FollowUpDate.HasValue) ?
                    (x.FollowUpDate.Value >= weekStartDate
                    && x.FollowUpDate.Value <= weekEndDate)
                   
                    : false)
                 ||
                    (  (x.OpportunityDue.HasValue)?
                        DateTime.Now.Date>=x.OpportunityDue.Value.Date:false
                    )
                    ;
            }).ToList();

            this.grvSummary.DataSource = weeklyFilteredFollowUps;
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
            this.grvSummary.DataBind();

            // change the way the gridview displays the header

            if (grvSummary.Rows.Count > 0)
            {
                grvSummary.UseAccessibleHeader = true;
                grvSummary.HeaderRow.TableSection = TableRowSection.TableHeader;
                grvSummary.FooterRow.TableSection = TableRowSection.TableFooter;
            }

            this.lblTitle.Text = "This Week's scheduled Activities (" + weeklyFilteredFollowUps.Count.ToString() + " active)";

        }

        private void PopulateSharedSummary()
        {
            bizMessage bizM = new bizMessage();
            if (Session["USER"] == null)
            {
                this.ucMessanger1.ProcessMessage("Session: " + bizM.GetMessageText("SessionMissing"), Enums.enMsgType.Err, "", null, true);
                return;
            }
            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];

            bizActivity biz = new bizActivity();



            List<sp_web_SharedFollowUpsResult> d1 = biz.GetSharedFollowUps(user.UserName, int.Parse(Request.QueryString["f1"]), int.Parse(Request.QueryString["f2"]), int.Parse(Request.QueryString["f3"]), "Recognise", CalenderUtilities.CutOffDate);

            // combine all results to have a filtered summery for the week
            var allFollowups = d1 ;
            var weekStartDate = (DateTime.Now.AddDays(-(DateTime.Now.DayOfWeek - DayOfWeek.Monday)));
            var weekEndDate = weekStartDate.AddDays(6);
            var weeklyFilteredFollowUps = allFollowups.Where(x =>
            {
                return ((x.OutcomeType != "C")&&(x.FollowUpDate.HasValue) ?
                    (x.FollowUpDate.Value.Date >= weekStartDate.Date
                    && x.FollowUpDate.Value.Date <= weekEndDate.Date)
                    && x.FollowUpDate >= CalenderUtilities.CutOffDate
                    : false)

                    ||
                    ((x.OpportunityDue.HasValue) ?
                        DateTime.Now.Date >= x.OpportunityDue.Value.Date : false
                    )
                    ;
            }).ToList();

            this.grvSharedSummary.DataSource = allFollowups;//weeklyFilteredFollowUps;
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
            this.grvSharedSummary.DataBind();

            // change the way the gridview displays the header

            if (grvSharedSummary.Rows.Count > 0)
            {
                grvSharedSummary.UseAccessibleHeader = true;
                grvSharedSummary.HeaderRow.TableSection = TableRowSection.TableHeader;
                grvSharedSummary.FooterRow.TableSection = TableRowSection.TableFooter;
            }

            this.lblTitle.Text = "Activities shared With Me";
        }

        private void PopulateIdentified()
        {
            ((Main)Master).HeaderTitle = "Identified list";

            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
            bizActivity biz = new bizActivity();

            bool isRegion, isBranch, isAll;
            isRegion = isBranch = isAll = false;
            GridUtilities.SetUserAllowedOpportunities(user, ref isRegion, ref isBranch, ref isAll);

            List<sp_web_FollowUpsResult> d = biz.GetFollowUps(Request.QueryString["l3"]
                                              ,int.Parse(Request.QueryString["f1"])
                                              ,int.Parse(Request.QueryString["f2"])
                                              ,int.Parse(Request.QueryString["f3"])
                                              ,"Recognise"
                                              ,Request.QueryString["l1"]
                                              ,Request.QueryString["l2"]
                                              ,isAll);
            this.grvIdentified.DataSource = d.Where(x => (x.OutcomeType != "C") && x.FollowUpDate >= CalenderUtilities.CutOffDate).ToList();
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
            this.grvIdentified.DataBind();

            this.lblTitle.Text = "Identified to Qualify (" + d.Count.ToString() + " active)";
        }

        private void PopulateQualifiedIn()
        {
            ((Main)Master).HeaderTitle = "Qualify list";

            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
            bizActivity biz = new bizActivity();

            bool isRegion, isBranch, isAll;
            isRegion = isBranch = isAll = false;
            GridUtilities.SetUserAllowedOpportunities(user, ref isRegion, ref isBranch, ref isAll);

            List<sp_web_FollowUpsResult> d1 = biz.GetFollowUps(Request.QueryString["l3"]
                                             ,int.Parse(Request.QueryString["f1"])
                                             ,int.Parse(Request.QueryString["f2"])
                                             ,int.Parse(Request.QueryString["f3"])
                                             , "Recognise"
                                             , Request.QueryString["l1"]
                                              , Request.QueryString["l2"]
                                             ,isAll);

            List<sp_web_FollowUpsResult> d = biz.GetFollowUps(Request.QueryString["l3"]
                                             ,int.Parse(Request.QueryString["f1"])
                                             ,int.Parse(Request.QueryString["f2"])
                                             ,int.Parse(Request.QueryString["f3"])
                                             ,"Qualify"
                                             , Request.QueryString["l1"]
                                              , Request.QueryString["l2"]
                                             ,isAll);

            

            var allList = d.Concat(d1).Where(x=>(x.OutcomeType != "C") && x.FollowUpDate >= CalenderUtilities.CutOffDate).ToList();
            this.grvQualifiedIn.DataSource = allList;
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
            this.grvQualifiedIn.DataBind();

            AlterGridRendering(grvQualifiedIn);

            this.lblTitle.Text = "Qualifying (" + allList.Where(x=>x.OutcomeType!="C").Count().ToString() + " active)";
        }

        private void PopulateInterested()
        {
            ((Main)Master).HeaderTitle = "Respond list";
            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
            bizActivity biz = new bizActivity();

            bool isRegion, isBranch, isAll;
            isRegion = isBranch = isAll = false;
            GridUtilities.SetUserAllowedOpportunities(user, ref isRegion, ref isBranch, ref isAll);

            List<sp_web_FollowUpsResult> d = biz.GetFollowUps(Request.QueryString["l3"]
                                               ,int.Parse(Request.QueryString["f1"])
                                               ,int.Parse(Request.QueryString["f2"])
                                               ,int.Parse(Request.QueryString["f3"])
                                               ,"Contact"
                                               , Request.QueryString["l1"]
                                               , Request.QueryString["l2"]
                                               ,isAll);

            List<sp_web_FollowUpsResult> d2 = biz.GetFollowUps(Request.QueryString["l3"]
                                               ,int.Parse(Request.QueryString["f1"])
                                               ,int.Parse(Request.QueryString["f2"])
                                               ,int.Parse(Request.QueryString["f3"])
                                               , "Discover"
                                               , Request.QueryString["l1"]
                                                , Request.QueryString["l2"]
                                               ,isAll);
            List<sp_web_FollowUpsResult> d3 = biz.GetFollowUps(Request.QueryString["l3"]
                                   , int.Parse(Request.QueryString["f1"])
                                   , int.Parse(Request.QueryString["f2"])
                                   , int.Parse(Request.QueryString["f3"])
                                   , "Respond"
                                   , Request.QueryString["l1"]
                                   , Request.QueryString["l2"]
                                   ,isAll);

            

            var allList = d.Concat(d2).Concat(d3).Where(x=> (x.OutcomeType != "C") && x.FollowUpDate >= CalenderUtilities.CutOffDate).ToList();
            this.grvInterested.DataSource = allList;
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
            this.grvInterested.DataBind();

            AlterGridRendering(grvInterested);

            this.lblTitle.Text = "Responding (" + allList.Where(x=>x.OutcomeType!="C").Count().ToString() + " active)";
        }

        private void PopulateGoToMarket()
        {
            ((Main)Master).HeaderTitle = "Go-to-Market list";

            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
            bizActivity biz = new bizActivity();

            bool isRegion, isBranch, isAll;
            isRegion = isBranch = isAll = false;
            GridUtilities.SetUserAllowedOpportunities(user, ref isRegion, ref isBranch, ref isAll);

            List<sp_web_FollowUpsResult> d = biz.GetFollowUps(Request.QueryString["l3"]
                                              ,int.Parse(Request.QueryString["f1"])
                                              ,int.Parse(Request.QueryString["f2"])
                                              ,int.Parse(Request.QueryString["f3"])
                                              ,"Discover"
                                                 , Request.QueryString["l1"]
                                                , Request.QueryString["l2"]
                                              , isAll);
            this.grvIdentified.DataSource = d;
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
            this.grvIdentified.DataBind();

            this.lblTitle.Text = "Go-to-Market to Respond (" + d.Count.ToString() + " active)";
        }

        private void PopulateQuoted()
        {
            ((Main)Master).HeaderTitle = "Quoted list";

            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
            bizActivity biz = new bizActivity();

            bool isRegion, isBranch, isAll;
            isRegion = isBranch = isAll = false;
            GridUtilities.SetUserAllowedOpportunities(user, ref isRegion, ref isBranch, ref isAll);


            List<sp_web_FollowUpsResult> d = biz.GetFollowUps(Request.QueryString["l3"]
                                              ,int.Parse(Request.QueryString["f1"])
                                              ,int.Parse(Request.QueryString["f2"])
                                              ,int.Parse(Request.QueryString["f3"])
                                              ,"Respond"
                                               , Request.QueryString["l1"]
                                                , Request.QueryString["l2"]
                                              , isAll);
            this.grvQuoted.DataSource = d.Where(x => (x.OutcomeType != "C") && x.FollowUpDate >= CalenderUtilities.CutOffDate).ToList(); ;
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
            this.grvQuoted.DataBind();

            this.lblTitle.Text = "Quoted to Agree (" + d.Where(x=>x.OutcomeType!="C").Count().ToString() + " active)";
        }

        private void PopulateAccepted()
        {
            ((Main)Master).HeaderTitle = "Complete list";

            bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
            bizActivity biz = new bizActivity();

            bool isRegion, isBranch, isAll;
            isRegion = isBranch = isAll = false;
            GridUtilities.SetUserAllowedOpportunities(user, ref isRegion, ref isBranch, ref isAll);


            List<sp_web_FollowUpsResult> d = biz.GetFollowUps(Request.QueryString["l3"]
                                              ,int.Parse(Request.QueryString["f1"])
                                              ,int.Parse(Request.QueryString["f2"])
                                              ,int.Parse(Request.QueryString["f3"])
                                              ,"Agree"
                                               , Request.QueryString["l1"]
                                                , Request.QueryString["l2"]
                                              ,isAll);
            List<sp_web_FollowUpsResult> d2 = biz.GetFollowUps(Request.QueryString["l3"]
                                             , int.Parse(Request.QueryString["f1"])
                                             , int.Parse(Request.QueryString["f2"])
                                             , int.Parse(Request.QueryString["f3"])
                                             , "Process"
                                               , Request.QueryString["l1"]
                                                , Request.QueryString["l2"]
                                              , isAll);

            

            var allList = d.Concat(d2).Where(x=> (x.OutcomeType != "C") && x.FollowUpDate >= CalenderUtilities.CutOffDate).ToList();
            this.grvAccepted.DataSource = allList;
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);
            this.grvAccepted.DataBind();

            AlterGridRendering(grvAccepted);

            this.lblTitle.Text = "Completing (" + allList.Where(x => x.OutcomeType != "C").Count().ToString() + " active)";
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
                        if (suspect.ClientName.Length > 35) e.Row.Cells[2].Text = suspect.ClientName.Remove(33) + "..";
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
                        if (suspect.ClientName.Length > 35) e.Row.Cells[2].Text = suspect.ClientName.Remove(33) + "..";
                    }

                    if (suspect.FollowUpDate < DateTime.Today)
                    {
                        e.Row.Cells[10].CssClass = "ochre-bold";
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
                        if (suspect.ClientName.Length > 35) e.Row.Cells[2].Text = suspect.ClientName.Remove(33) + "..";
                    }

                    if (suspect.FollowUpDate < DateTime.Today)
                    {
                        e.Row.Cells[10].CssClass = "ochre-bold";
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
                        if (suspect.ClientName.Length > 35) e.Row.Cells[2].Text = suspect.ClientName.Remove(33) + "..";
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
                        if (suspect.ClientName.Length > 35) e.Row.Cells[2].Text = suspect.ClientName.Remove(33) + "..";
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
                        if (suspect.ClientName.Length > 35) e.Row.Cells[2].Text = suspect.ClientName.Remove(33) + "..";
                    }

                    if (suspect.FollowUpDate < DateTime.Today)
                    {
                        e.Row.Cells[10].CssClass = "ochre-bold";
                    }

                    if (suspect.ClassificationName == "OAMPS Income above $20,000")
                    {
                        e.Row.Font.Bold = true;
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

        protected void grvIdentified_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grvIdentified.PageIndex = e.NewPageIndex;
                PopulateIdentified();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void grvQualifiedIn_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grvQualifiedIn.PageIndex = e.NewPageIndex;
                PopulateQualifiedIn();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void grvInterested_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grvInterested.PageIndex = e.NewPageIndex;
                PopulateInterested();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void grvGoToMarket_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grvGoToMarket.PageIndex = e.NewPageIndex;
                PopulateGoToMarket();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void grvQuoted_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grvQuoted.PageIndex = e.NewPageIndex;
                PopulateQuoted();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void grvAccepted_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grvAccepted.PageIndex = e.NewPageIndex;
                PopulateAccepted();
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

        protected void AlterGridRendering(GridView grid)
        {
            if (grid.Rows.Count > 0)
            {
                grid.UseAccessibleHeader = true;
                grid.HeaderRow.TableSection = TableRowSection.TableHeader;
                grid.FooterRow.TableSection = TableRowSection.TableFooter;
            

            
                grid.HeaderStyle.BorderStyle = BorderStyle.None;
                foreach (TableCell cellObj in grid.HeaderRow.Cells)
                {
                    cellObj.BorderStyle = BorderStyle.None;
                }
            }

        }

    }
}
