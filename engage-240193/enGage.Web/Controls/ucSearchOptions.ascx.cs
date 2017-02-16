using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using enGage.BL;
using enGage.DL;
using enGage.Utilities;

namespace enGage.Web.Controls
{
    public partial class ucSearchOptions : UserControl
    {
        #region Public Properties

        public bool DisableExecutives { get; set; } = false;

        #endregion

        public event EventHandler ControlLoaded;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    PopulateRegionsBranchesAndExecutives();
                    PopulateDefaults();
                    PopulateClassification();
                    PopulateBusinessType();
                    PopulateSearchOptionsFromSession();
                    SetHeaders();

                    ControlLoaded(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        #region Private Methods

        private void PopulateRegionsBranchesAndExecutives()
        {
            var biz = new bizBranchRegion();
            var user = (bizUser.enGageUser) Session["USER"];

            //#region OBS

            //if (user == null)
            //{
            //    user = new bizUser.enGageUser
            //    {
            //        Branch = "Testing",
            //        DisplayName = "Test User",
            //        Region = "Testing",
            //        Role = 0,
            //        UserName = "testuser"
            //    };
            //}

            //#endregion OBS

            ddlRegion.Items.Clear();
            ddlBranch.Items.Clear();
            ddlExecutive.Items.Clear();

            switch (user.Role)
            {
                case (int) Enums.enUserRole.Executive:
                    ddlRegion.Items.Add(new ListItem(user.Region, user.Region));
                    ddlBranch.Items.Add(new ListItem(user.Branch, user.Branch));
                    if (DisableExecutives == false)
                    {
                        ddlExecutive.Items.Add(new ListItem(user.DisplayName, user.UserName));
                    }
                    else
                    {
                        ddlExecutive.Items.Add(new ListItem("Executives (All)", "(All)"));
                    }
                    break;
                case (int) Enums.enUserRole.Branch:
                    ddlRegion.Items.Add(new ListItem(user.Region, user.Region));
                    ddlBranch.Items.Add(new ListItem(user.Branch, user.Branch));
                    if (DisableExecutives == false)
                    {
                        var bizS = new bizSetting();
                        var execs = bizActiveDirectory.ListAccountExecutivesByBranchForDropDown(ddlBranch.SelectedValue);
                        if (execs == null) return;
                        foreach (string exec in execs)
                        {
                            ddlExecutive.Items.Add(new ListItem(exec, execs[exec]));
                        }
                        SortDropDownList(ref ddlExecutive);
                    }
                    ddlExecutive.Items.Insert(0, new ListItem("Executives (All)", "(All)"));
                    break;
                case (int) Enums.enUserRole.Region:
                    ddlRegion.Items.Add(new ListItem(user.Region, user.Region));
                    var bs = biz.ListBranchesByRegion(ddlRegion.SelectedValue);
                    if (bs == null) return;
                    ddlBranch.Items.Add(new ListItem("Branches (All)", "(All)"));
                    foreach (var b in bs)
                    {
                        ddlBranch.Items.Add(new ListItem(b, b));
                    }
                    ddlExecutive.Items.Add(new ListItem("Executives (All)", "(All)"));
                    break;
                case (int) Enums.enUserRole.Company:
                case (int) Enums.enUserRole.Administrator:
                    var rs = biz.ListRegions();
                    if (rs == null) return;
                    ddlRegion.Items.Add(new ListItem("Regions (All)", "(All)"));
                    foreach (var r in rs)
                    {
                        ddlRegion.Items.Add(new ListItem(r, r));
                    }
                    ddlBranch.Items.Add(new ListItem("Branches (All)", "(All)"));
                    ddlExecutive.Items.Add(new ListItem("Executives (All)", "(All)"));
                    break;
            }
        }

        private void PopulateDefaults()
        {
            txtFrom.Text = string.Format("{0:dd/MM/yyyy}", new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1));
            txtTo.Text = string.Format("{0:dd/MM/yyyy}",
                new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(1).Month, 1).AddDays(-1));
        }

        private void PopulateClassification()
        {
            var bizM = new bizMessage();

            var biz = new bizOpportunity();
            List<Classification> cls;
            cls = biz.ListClassifications();

            if (cls == null) return;

            ddlClassification.Items.Clear();
            ddlClassification.Items.Add(new ListItem("(All)", "0"));
            foreach (var cl in cls)
            {
                ddlClassification.Items.Add(new ListItem(cl.ClassificationName, cl.ClassificationID.ToString()));
            }
        }

        private void PopulateBusinessType()
        {
            var biz = new bizOpportunity();
            var bts = biz.ListBusinessTypes();

            if (bts == null) return;

            ddlBusinessType.Items.Add(new ListItem("(All)", "0"));
            foreach (var bt in bts)
            {
                ddlBusinessType.Items.Add(new ListItem(bt.BusinessTypeName, bt.BusinessTypeID.ToString()));
            }
        }

        private void SortDropDownList(ref DropDownList ddl)
        {
            var listCopy = new List<ListItem>();
            foreach (ListItem item in ddl.Items)
                listCopy.Add(item);
            ddl.Items.Clear();
            foreach (var item in listCopy.OrderBy(item => item.Text))
                ddl.Items.Add(item);
        }

        private string BuildExecsFromDropDownList()
        {
            var execs = "";
            for (var i = 1; i < ddlExecutive.Items.Count; i++)
            {
                execs += "|" + ddlExecutive.Items[i].Value;
            }
            if (execs != "") execs = execs.Remove(0, 1);
            return execs;
        }

        private void SetHeaders()
        {
            lblCriteriaDesc.Text = "<b>" + string.Format("{0:dd-MMM-yy}", DateTime.Parse(txtFrom.Text)) + "</b> to <b>" +
                                   string.Format("{0:dd-MMM-yy}", DateTime.Parse(txtTo.Text)) + "</b>";
            var filter = "";
            if (ddlClassification.SelectedIndex > 0) filter += " and <b>Classification</b>";
            if (ddlBusinessType.SelectedIndex > 0) filter += " and <b>Opportunity Type</b>";
            if (lstIndustry.SelectedIndex != -1) filter += " and <b>ANZSIC</b>";
            if (lstSource.SelectedIndex != -1) filter += " and <b>Source</b>";
            if (lstOpportunity.SelectedIndex != -1) filter += " and <b>Opportunity</b>";
            if (filter != "") filter = filter.Remove(0, 5);
            else filter = "none";
            lblFiltersDesc.Text = filter;
        }

        private string GetIndustries()
        {
            string anzsic = null;
            foreach (ListItem li in lstIndustry.Items)
            {
                if (li.Selected)
                {
                    anzsic += "|" + li.Value;
                }
            }
            if (anzsic != null) anzsic = anzsic.Remove(0, 1).Replace("'", "''");
            return anzsic;
        }

        private void SetIndustries(string industries)
        {
            if (industries == null) return;
            char[] sep = {'|'};
            var anzsic = industries.Split(sep);
            var biz = new bizIndustry();
            var inds = biz.GetIndustriesByAnzsicCodes(anzsic);
            if (inds.Count == 0) return;
            lstIndustry.Items.Clear();
            foreach (var ind in inds)
            {
                var li = new ListItem(ind.IndustryName + " (" + ind.AnzsicCode + ")", ind.AnzsicCode);
                li.Selected = true;
                lstIndustry.Items.Add(li);
            }
            lstIndustry.Visible = true;
            divIndustry.Visible = true;
        }

        private string GetSources()
        {
            string source = null;
            foreach (ListItem li in lstSource.Items)
            {
                if (li.Selected)
                {
                    source += "|" + li.Value;
                }
            }
            if (source != null) source = source.Remove(0, 1).Replace("'", "''");
            return source;
        }

        private string GetOpportunities()
        {
            string opportunity = null;
            foreach (ListItem li in lstOpportunity.Items)
            {
                if (li.Selected)
                {
                    opportunity += "|" + li.Value;
                }
            }
            if (opportunity != null) opportunity = opportunity.Remove(0, 1).Replace("'", "''");
            return opportunity;
        }

        private void SetOpportunities(string opportunities)
        {
            if (opportunities == null) return;
            char[] sep = {'|'};
            var opps = opportunities.Split(sep);
            if (opps.Length == 0) return;
            lstOpportunity.Items.Clear();
            foreach (var opp in opps)
            {
                var li = new ListItem(opp, opp);
                li.Selected = true;
                lstOpportunity.Items.Add(li);
            }
            lstOpportunity.Visible = true;
            divOpportunities.Visible = true;
        }

        private void SetSources(string sources)
        {
            if (sources == null) return;
            char[] sep = {'|'};
            var srcs = sources.Split(sep);
            if (srcs.Length == 0) return;
            lstSource.Items.Clear();
            foreach (var src in srcs)
            {
                var li = new ListItem(src, src);
                li.Selected = true;
                lstSource.Items.Add(li);
            }
            lstSource.Visible = true;
            divSource.Visible = true;
        }

        private bool UIValidation()
        {
            var bizM = new bizMessage();

            if (txtFrom.Text == "")
            {
                ucMessanger1.ProcessMessage("From Date: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err,
                    "From", typeof (TextBox), true);
                return false;
            }
            DateTime res;
            if (DateTime.TryParse(txtFrom.Text, out res) == false)
            {
                ucMessanger1.ProcessMessage("From Date: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err,
                    "From", typeof (TextBox), true);
                return false;
            }
            if (txtTo.Text == "")
            {
                ucMessanger1.ProcessMessage("To Date: " + bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, "To",
                    typeof (TextBox), true);
                return false;
            }
            //DateTime res;
            if (DateTime.TryParse(txtTo.Text, out res) == false)
            {
                ucMessanger1.ProcessMessage("To Date: " + bizM.GetMessageText("FieldNotDate"), Enums.enMsgType.Err, "To",
                    typeof (TextBox), true);
                return false;
            }
            if (DateTime.Parse(txtFrom.Text) > DateTime.Parse(txtTo.Text))
            {
                ucMessanger1.ProcessMessage("From - To: " + bizM.GetMessageText("DateFromGreaterThanDateTo"),
                    Enums.enMsgType.Err, "From", typeof (TextBox), true);
                return false;
            }

            return true;
        }

        private void PopulateSearchOptionsFromSession()
        {
            if (Session["SEARCH_OPTIONS"] == null) return;

            var user = (bizUser.enGageUser) Session["USER"];

            //#region OBS

            //if (user == null)
            //{
            //    user = new bizUser.enGageUser
            //    {
            //        Branch = "Testing",
            //        DisplayName = "Test User",
            //        Region = "Testing",
            //        Role = 0,
            //        UserName = "testuser"
            //    };
            //}

            //#endregion OBS

            var so = (SearchOptions) Session["SEARCH_OPTIONS"];
            txtFrom.Text = string.Format("{0:dd/MM/yyyy}", so.DateFrom);
            txtTo.Text = string.Format("{0:dd/MM/yyyy}", so.DateTo);
            ddlRegion.SelectedValue = so.Region;
            PopulateBranches();
            ddlBranch.SelectedValue = so.Branch;
            PopulateExecutives();
            ddlExecutive.SelectedValue = so.Executive;
            ddlBusinessType.SelectedValue = so.BusinessType.ToString();
            ddlClassification.SelectedValue = so.Classification.ToString();
            SetIndustries(so.Industries);
            SetSources(so.Sources);
            SetOpportunities(so.Opportunities);
        }

        #endregion

        #region Public Methods

        public SearchOptions GetSearchOptions()
        {
            ucMessanger1.ClearMessages();
            ucMessanger1.UnmarkControls();

            if (UIValidation() == false) return null;

            SetHeaders();

            var so = new SearchOptions();
            so.DateFrom = DateTime.Parse(txtFrom.Text);
            so.DateTo = DateTime.Parse(txtTo.Text);
            so.Region = ddlRegion.SelectedValue;
            so.Branch = ddlBranch.SelectedValue;
            so.Executive = ddlExecutive.SelectedValue;
            so.BusinessType = int.Parse(ddlBusinessType.SelectedValue);
            so.Classification = int.Parse(ddlClassification.SelectedValue);
            so.Sources = GetSources();
            so.Industries = GetIndustries();
            so.Opportunities = GetOpportunities();

            pnlCriteria_CollapsiblePanelExtender.ClientState = "true";
            pnlCriteria_CollapsiblePanelExtender.Collapsed = true;
            pnlFilters_CollapsiblePanelExtender.ClientState = "true";
            pnlFilters_CollapsiblePanelExtender.Collapsed = true;

            if (Session["SEARCH_OPTIONS"] == null)
            {
                Session.Add("SEARCH_OPTIONS", so);
            }
            else
            {
                Session["SEARCH_OPTIONS"] = so;
            }

            return so;
        }

        public string GetExecutives()
        {
            var execs = "";
            var user = (bizUser.enGageUser) Session["USER"];
            switch (user.Role)
            {
                case (int) Enums.enUserRole.Executive:
                    execs = ddlExecutive.SelectedValue;
                    break;
                case (int) Enums.enUserRole.Branch:
                    if (ddlRegion.SelectedIndex == 0 && ddlBranch.SelectedIndex == 0 && ddlExecutive.SelectedIndex == 0)
                    {
                        if (DisableExecutives)
                        {
                            var bizS = new bizSetting();
                            execs = bizActiveDirectory.ListAccountExecutivesByRegion(ddlRegion.SelectedValue);
                        }
                        else
                        {
                            execs = BuildExecsFromDropDownList();
                        }
                    }
                    if (ddlRegion.SelectedIndex == 0 && ddlBranch.SelectedIndex == 0 && ddlExecutive.SelectedIndex > 0)
                    {
                        execs = ddlExecutive.SelectedValue;
                    }
                    break;
                case (int) Enums.enUserRole.Region:
                    if (ddlRegion.SelectedIndex == 0 && ddlBranch.SelectedIndex == 0 && ddlExecutive.SelectedIndex == 0)
                    {
                        var bizS = new bizSetting();
                        execs = bizActiveDirectory.ListAccountExecutivesByRegion(ddlRegion.SelectedValue);
                    }
                    if (ddlRegion.SelectedIndex == 0 && ddlBranch.SelectedIndex > 0 && ddlExecutive.SelectedIndex == 0)
                    {
                        var bizS = new bizSetting();
                        execs = bizActiveDirectory.ListAccountExecutivesByBranch(ddlBranch.SelectedValue);
                    }
                    if (ddlRegion.SelectedIndex == 0 && ddlBranch.SelectedIndex > 0 && ddlExecutive.SelectedIndex > 0)
                    {
                        execs = ddlExecutive.SelectedValue;
                    }
                    break;
                case (int) Enums.enUserRole.Company:
                case (int) Enums.enUserRole.Administrator:
                    if (ddlRegion.SelectedIndex == 0 && ddlBranch.SelectedIndex == 0 && ddlExecutive.SelectedIndex == 0)
                    {
                        execs = "(All)";
                    }
                    if (ddlRegion.SelectedIndex > 0 && ddlBranch.SelectedIndex == 0 && ddlExecutive.SelectedIndex == 0)
                    {
                        var bizS = new bizSetting();
                        execs = bizActiveDirectory.ListAccountExecutivesByRegion(ddlRegion.SelectedValue);
                    }
                    if (ddlRegion.SelectedIndex > 0 && ddlBranch.SelectedIndex > 0 && ddlExecutive.SelectedIndex == 0)
                    {
                        execs = BuildExecsFromDropDownList();
                    }
                    if (ddlRegion.SelectedIndex > 0 && ddlBranch.SelectedIndex > 0 && ddlExecutive.SelectedIndex > 0)
                    {
                        execs = ddlExecutive.SelectedValue;
                    }
                    break;
            }
            return execs;
        }

        public string GetExecutiveName(string executive)
        {
            ListItem li;
            li = ddlExecutive.Items.FindByValue(executive);
            if (li == null) return "";
            return li.Text;
        }

        #endregion

        #region Events

        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PopulateBranches();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void PopulateBranches()
        {
            var user = (bizUser.enGageUser) Session["USER"];

            //#region OBS

            //if (user == null)
            //{
            //    user = new bizUser.enGageUser
            //    {
            //        Branch = "Testing",
            //        DisplayName = "Test User",
            //        Region = "Testing",
            //        Role = 0,
            //        UserName = "testuser"
            //    };
            //}

            //#endregion OBS

            switch (user.Role)
            {
                case (int) Enums.enUserRole.Executive:
                case (int) Enums.enUserRole.Branch:
                    break;
                case (int) Enums.enUserRole.Region:
                case (int) Enums.enUserRole.Company:
                case (int) Enums.enUserRole.Administrator:
                    var biz = new bizBranchRegion();
                    var bs = biz.ListBranchesByRegion(ddlRegion.SelectedValue);
                    if (bs == null) return;
                    ddlBranch.Items.Clear();
                    ddlBranch.Items.Add(new ListItem("Branches (All)", "(All)"));
                    foreach (var b in bs)
                    {
                        ddlBranch.Items.Add(new ListItem(b, b));
                    }
                    break;
            }
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PopulateExecutives();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void PopulateExecutives()
        {
            if (DisableExecutives) return;

            var user = (bizUser.enGageUser) Session["USER"];

            //#region OBS

            //if (user == null)
            //{
            //    user = new bizUser.enGageUser
            //    {
            //        Branch = "Testing",
            //        DisplayName = "Test User",
            //        Region = "Testing",
            //        Role = 0,
            //        UserName = "testuser"
            //    };
            //}

            //#endregion OBS

            switch (user.Role)
            {
                case (int) Enums.enUserRole.Executive:
                    break;
                case (int) Enums.enUserRole.Branch:
                case (int) Enums.enUserRole.Region:
                case (int) Enums.enUserRole.Company:
                case (int) Enums.enUserRole.Administrator:
                    var bizS = new bizSetting();
                    var execs = bizActiveDirectory.ListAccountExecutivesByBranchForDropDown(ddlBranch.SelectedValue);
                    if (execs == null) return;
                    ddlExecutive.Items.Clear();
                    foreach (string exec in execs)
                    {
                        ddlExecutive.Items.Add(new ListItem(exec, execs[exec]));
                    }
                    SortDropDownList(ref ddlExecutive);
                    ddlExecutive.Items.Insert(0, new ListItem("Executives (All)", "(All)"));
                    break;
            }
        }

        protected void btnClearCriteria_Click(object sender, EventArgs e)
        {
            try
            {
                PopulateDefaults();
                lblCriteriaDesc.Text = "<b>" + string.Format("{0:dd-MMM-yy}", DateTime.Parse(txtFrom.Text)) +
                                       "</b> to <b>" + string.Format("{0:dd-MMM-yy}", DateTime.Parse(txtTo.Text)) +
                                       "</b>";
                PopulateRegionsBranchesAndExecutives();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnClearFilters_Click(object sender, EventArgs e)
        {
            try
            {
                lblFiltersDesc.Text = "none";
                if (ddlClassification.Items.Count > 0) ddlClassification.SelectedIndex = 0;
                if (ddlBusinessType.Items.Count > 0) ddlBusinessType.SelectedIndex = 0;
                txtFindIndustry.Text = "";
                lblFoundIndustries.Text = "";
                lstIndustry.Items.Clear();
                lstIndustry.Visible = false;
                divIndustry.Visible = false;

                txtFindSource.Text = "";
                lblFoundSources.Text = "";
                lstSource.Items.Clear();
                lstSource.Visible = false;
                divSource.Visible = false;

                txtFindOpportunity.Text = "";
                lblFoundOpportunities.Text = "";
                lstOpportunity.Items.Clear();
                lstOpportunity.Items.Clear();
                divOpportunities.Visible = false;
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnFindIndustry_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFindIndustry.Text == "")
                {
                    lblFoundIndustries.Text = "You have to type a keyword";
                }
                else
                {
                    PopulateIndustries();
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void PopulateIndustries()
        {
            var bizM = new bizMessage();

            var biz = new bizIndustry();
            var inds = biz.ListIndustriesByKeyword(txtFindIndustry.Text);

            lstIndustry.Items.Clear();
            if (txtFindIndustry.Text != "")
            {
                foreach (var ind in inds)
                {
                    lstIndustry.Items.Add(new ListItem(ind.IndustryName + " (" + ind.AnzsicCode + ")", ind.AnzsicCode));
                }
                lstIndustry.Visible = true;
                divIndustry.Visible = true;
                lblFoundIndustries.Text = lstIndustry.Items.Count + " industries found";
            }
            else
            {
                lstIndustry.Visible = false;
                divIndustry.Visible = false;
                lblFoundIndustries.Text = "";
            }
        }

        protected void btnFindSource_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFindSource.Text == "")
                {
                    lblFoundSources.Text = "You have to type something";
                }
                else
                {
                    PopulateSources();
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnFindOpportunity_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFindOpportunity.Text == "")
                {
                    lblFoundOpportunities.Text = "You have to type something";
                }
                else
                {
                    PopulateOpportunities();
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void PopulateSources()
        {
            var bizM = new bizMessage();

            var biz = new bizClient();
            var ss = biz.FindSources(txtFindSource.Text);

            lstSource.Items.Clear();
            if (txtFindSource.Text != "")
            {
                foreach (var s in ss)
                {
                    lstSource.Items.Add(new ListItem(s, s));
                }
                lstSource.Visible = true;
                divSource.Visible = true;
                lblFoundSources.Text = lstSource.Items.Count + " sources found";
            }
            else
            {
                lstSource.Visible = false;
                divSource.Visible = false;
                lblFoundSources.Text = "";
            }
        }

        private void PopulateOpportunities()
        {
            var bizM = new bizMessage();

            var biz = new bizClient();
            var ss = biz.FindOpportunities(txtFindOpportunity.Text);

            lstOpportunity.Items.Clear();
            if (txtFindOpportunity.Text != "")
            {
                foreach (var s in ss)
                {
                    lstOpportunity.Items.Add(new ListItem(s, s));
                }

                lstOpportunity.Visible = true;
                divOpportunities.Visible = true;
                lblFoundOpportunities.Text = lstOpportunity.Items.Count + " opportunities found";
            }
            else
            {
                lstOpportunity.Visible = false;
                divSource.Visible = false;
                lblFoundOpportunities.Text = "";
            }
        }

        #endregion
    }
}