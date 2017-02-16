using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices;
using System.Text.RegularExpressions;
using enGage.BL;
using enGage.DL;

namespace enGage.Web
{
    public partial class FindClient : System.Web.UI.Page
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
                    ((Main)Master).HeaderTitle = "List client search results";
                    this.txtSearchCriteria.Focus();
                    PopulateControls();
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void PopulateControls()
        {
            if (this.Request.QueryString["sc"] != null)
            {
                this.txtSearchCriteria.Text = this.Request.QueryString["sc"].ToString();
                this.ddlClient.SelectedValue = this.Request.QueryString["f1"].ToString();
                this.ddlMatch.SelectedValue = this.Request.QueryString["f2"].ToString();
                LoadClients();
                LoadClientsBA();
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                LoadClients();
                LoadClientsBA();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void LoadClients()
        {
            bizMessage biz = new bizMessage();
            this.ucMessanger1.ClearMessages();
            this.txtSearchCriteria.CssClass = "control";
            //this.ucMessanger1.UnmarkControls(); // todo: doesn't work
            this.grvClientsClientName.Visible = false;
            this.grvClientsAddress.Visible = false;
            this.grvClientsIndustry.Visible = false;

            if (this.txtSearchCriteria.Text == "")
            {
                this.ucMessanger1.ProcessMessage(biz.GetMessageText("EmptyField"), Enums.enMsgType.Err, "SearchCriteria", typeof(TextBox), false);
                return;
            }

            bizSetting bizS = new bizSetting();
            int MaxRecords = int.Parse(bizS.GetSetting("FindClient.MaxRecords"));

            bizClient bizC = new bizClient();
            int? records = 0;
            List<sp_web_FindClientByFieldResult> clients = bizC.FindClientByField(this.txtSearchCriteria.Text
                                                                  , this.ddlClient.SelectedValue
                                                                  , char.Parse(this.ddlMatch.SelectedValue)
                                                                  , null
                                                                  , MaxRecords
                                                                  , ref records);
            this.ucMessanger1.ProcessMessages(bizC.MSGS, true);

            if (clients == null) return;


            // get all users in AD
            
           var allusersResult= bizUser.GetUsersAccountExecutives(clients.Select<sp_web_FindClientByFieldResult, string>(x => x.AccountExecutiveID).Distinct().ToList());

            // change all clients
            clients.ForEach(
                x =>
                {
                    if (x.AccountExecutiveID != "" && allusersResult.ContainsKey(x.AccountExecutiveID))
                    {
                        bizUser.enGageUser exec = allusersResult[x.AccountExecutiveID];//bizUser.GetAccountExecutive(x.AccountExecutiveID);
                        if (exec != null)
                            x.DisplayName = exec.DisplayName;
                        else
                            x.DisplayName = x.AccountExecutiveID;
                    }
                }

                );

            List<sp_web_FindClientByFieldResult> cn = clients.Where(c => c.Match == "client").ToList();
            List<sp_web_FindClientByFieldResult> add = clients.Where(c => c.Match == "address").ToList();
            List<sp_web_FindClientByFieldResult> ind = clients.Where(c => c.Match == "industry").ToList();

            if (records == 0)
            {
                this.btnAdd.Visible = true;

                this.tdHeaderCN.Visible = false;
                this.tdFooterCN.Visible = false;
                this.tdHeaderAD.Visible = false;
                this.tdFooterAD.Visible = false;
                this.tdHeaderIND.Visible = false;
                this.tdFooterIND.Visible = false;
                this.ucMessanger1.ProcessMessage("enGage: " + biz.GetMessageText("NoClientsFound"), Enums.enMsgType.Warn, "", null, false);
                // change the new postpack url
                btnAdd.PostBackUrl += "?name=" + HttpUtility.UrlEncode(this.txtSearchCriteria.Text);
                return;
            }

            if (records > 0 && records <= MaxRecords)
            {

                
                this.btnAdd.Visible = true;

                /*foreach (sp_web_FindClientByFieldResult c in cn)
                {
                    if (c.AccountExecutiveID != "")
                    {
                        bizUser.enGageUser exec = bizUser.GetAccountExecutive(c.AccountExecutiveID);
                        if (exec != null)
                            c.DisplayName = exec.DisplayName;
                        else
                            c.DisplayName = c.AccountExecutiveID;
                    }
                }
                foreach (sp_web_FindClientByFieldResult c in add)
                {
                    if (c.AccountExecutiveID != "")
                    {
                        bizUser.enGageUser exec = bizUser.GetAccountExecutive(c.AccountExecutiveID);
                        if (exec != null)
                            c.DisplayName = exec.DisplayName;
                        else
                            c.DisplayName = c.AccountExecutiveID;
                    }
                }
                foreach (sp_web_FindClientByFieldResult c in ind)
                {
                    if (c.AccountExecutiveID != "")
                    {
                        bizUser.enGageUser exec = bizUser.GetAccountExecutive(c.AccountExecutiveID);
                        if (exec != null)
                            c.DisplayName = exec.DisplayName;
                        else
                            c.DisplayName = c.AccountExecutiveID;
                    }
                }*/

                if (cn.Count == 0)
                {
                    this.tdHeaderCN.Visible = false;
                    this.grvClientsClientName.Visible = false;
                    this.tdFooterCN.Visible = false;
                }
                else
                {
                    this.lblResultCountCN.Text = cn.Count.ToString();
                    this.lblSearchCN.Text = this.txtSearchCriteria.Text;
                    this.tdHeaderCN.Visible = true;
                    this.grvClientsClientName.DataSource = cn;
                    this.grvClientsClientName.DataBind();
                    this.grvClientsClientName.Visible = true;
                    this.tdFooterCN.Visible = true;
                    this.lnkCN.Enabled = true;
                    if (cn.Count <= this.grvClientsClientName.PageSize) this.lnkCN.Enabled = false;
                }
                if (add.Count == 0)
                {
                    this.tdHeaderAD.Visible = false;
                    this.grvClientsAddress.Visible = false;
                    this.tdFooterAD.Visible = false;
                }
                else
                {
                    this.lblResultCountAD.Text = add.Count.ToString();
                    this.lblSearchAD.Text = this.txtSearchCriteria.Text;
                    this.tdHeaderAD.Visible = true;
                    this.grvClientsAddress.DataSource = add;
                    this.grvClientsAddress.DataBind();
                    this.grvClientsAddress.Visible = true;
                    this.tdFooterAD.Visible = true;
                    this.lnkAD.Enabled = true;
                    if (add.Count <= this.grvClientsAddress.PageSize) this.lnkAD.Enabled = false;
                }
                if (ind.Count == 0)
                {
                    this.tdHeaderIND.Visible = false;
                    this.grvClientsIndustry.Visible = false;
                    this.tdFooterIND.Visible = false;
                }
                else
                {
                    this.lblResultCountIND.Text = ind.Count.ToString();
                    this.lblSearchIND.Text = this.txtSearchCriteria.Text;
                    this.tdHeaderIND.Visible = true;
                    this.grvClientsIndustry.DataSource = ind;
                    this.grvClientsIndustry.DataBind();
                    this.grvClientsIndustry.Visible = true;
                    this.tdFooterIND.Visible = true;
                    this.lnkIND.Enabled = true;
                    if (ind.Count <= this.grvClientsIndustry.PageSize) this.lnkIND.Enabled = false;
                }

                if (records == MaxRecords)
                {
                    this.ucMessanger1.ProcessMessage("enGage: " + biz.GetMessageText("MaxClientsReached"), Enums.enMsgType.Warn, "", null, false);
                }

                return;
            }
            if (records > MaxRecords)
            {
                this.btnAdd.Visible = false;

                this.tdHeaderCN.Visible = false;
                this.tdFooterCN.Visible = false;
                this.tdHeaderAD.Visible = false;
                this.tdFooterAD.Visible = false;
                this.tdHeaderIND.Visible = false;
                this.tdFooterIND.Visible = false;
                this.ucMessanger1.ProcessMessage("enGage: " + biz.GetMessageText("TooManyClientsFound"), Enums.enMsgType.Warn, "", null, false);
                return;
            }
        }

        private void LoadClientsBA()
        {
            bizMessage bizM = new bizMessage();
            //this.ucMessanger1.ClearMessages();

            if (this.txtSearchCriteria.Text == "")
            {
                this.ucMessanger1.ProcessMessage(bizM.GetMessageText("EmptyField"), Enums.enMsgType.Err, "SearchCriteria", typeof(TextBox), false);
                return;
            }

            bizSetting bizS = new bizSetting();
            int MaxRecords = int.Parse(bizS.GetSetting("FindClient.MaxRecords"));

            bizClient biz = new bizClient();
            int? records = 0;
            List<sp_engage_search_clientResult> clients = biz.FindClientInBA(this.txtSearchCriteria.Text
                                                                      , this.ddlClient.SelectedValue
                                                                      , char.Parse(this.ddlMatch.SelectedValue)
                                                                      , MaxRecords
                                                                      , ref records);
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);

            if (clients == null) return;

            if (records == 0)
            {
                this.btnAdd.Visible = true;
                this.tdHeaderBA.Visible = false;
                this.grvClientsBA.Visible = false;
                this.tdFooterBA.Visible = false;
                this.ucMessanger1.ProcessMessage("iBAIS: " + bizM.GetMessageText("NoClientsFound"), Enums.enMsgType.Warn, "", null, false);
            }
            if (records > 0 && records <= MaxRecords)
            {
                this.btnAdd.Visible = true;
                this.lblResultCountBA.Text = clients.Count.ToString();
                this.lblSearchBA.Text = this.txtSearchCriteria.Text;
                this.tdHeaderBA.Visible = true;
                this.grvClientsBA.DataSource = clients;
                this.grvClientsBA.DataBind();
                this.grvClientsBA.Visible = true;
                this.tdFooterBA.Visible = true;
                this.lnkBA.Enabled = true;
                if (clients.Count <= this.grvClientsBA.PageSize) this.lnkBA.Enabled = false;
                if (records == MaxRecords)
                {
                    this.ucMessanger1.ProcessMessage("iBAIS: " + bizM.GetMessageText("MaxClientsReached"), Enums.enMsgType.Warn, "", null, false);
                }
            }
            if (records > MaxRecords)
            {
                this.btnAdd.Visible = false;
                this.tdHeaderBA.Visible = false;
                this.grvClientsBA.Visible = false;
                this.tdFooterBA.Visible = false;
                this.ucMessanger1.ProcessMessage("iBAIS: " + bizM.GetMessageText("TooManyClientsFound"), Enums.enMsgType.Warn, "", null, false);
            }
        }

        protected void grvClients_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Session["USER"] == null) return;

                    bizUser.enGageUser user = (bizUser.enGageUser)Session["USER"];
                    sp_web_FindClientByFieldResult client = (sp_web_FindClientByFieldResult)e.Row.DataItem;
                    bizClient biz = new bizClient();
                    var u = bizUser.GetSMIAccountExecutiveIdBOAMPSUserName(client.AccountExecutiveID);
                    bizUser.enGageUser exec = bizUser.GetAccountExecutive(u);

                    //if (client.ClientName != null)
                    //{
                    //    if (client.ClientName.Length > 35) e.Row.Cells[2].Text = client.ClientName.Remove(33) + "..";
                    //}

                    //if (client.IndustryName != null)
                    //{
                    //    if (client.IndustryName.Length > 35) e.Row.Cells[3].Text = client.IndustryName.Remove(33) + "..";
                    //}

                    //if (e.Row.Cells[4].Text != "")
                    //{
                    //    if (e.Row.Cells[4].Text.Length > 15) e.Row.Cells[4].Text = e.Row.Cells[4].Text.Remove(13) + "..";
                    //}

                    //if (e.Row.Cells[5].Text != "")
                    //{
                    //    if (e.Row.Cells[5].Text.Length > 15) e.Row.Cells[5].Text = e.Row.Cells[5].Text.Remove(13) + "..";
                    //}

                    if (e.Row.Cells[5].Text != "")
                    {
                        e.Row.Cells[5].Text = Regex.Replace(e.Row.Cells[5].Text, this.txtSearchCriteria.Text, "<b>" + this.txtSearchCriteria.Text.ToUpper() + "</b>", RegexOptions.IgnoreCase);
                    }

                    if (client.Inactive == true) e.Row.CssClass = "lightgrey";
                    else
                    {
                        if (client.FollowUpDate < DateTime.Now) e.Row.Cells[2].CssClass = "ochre-bold";
                    }

                    // SECURITY
                    Image img = (Image)e.Row.FindControl("imgArrow");
                    switch (user.Role)
                    {
                        case (int)Enums.enUserRole.Executive:
                            if (exec != null)
                            {
                                if (user.DisplayName != exec.DisplayName)
                                {
                                    e.Row.CssClass = "darkgrey";
                                }
                                if (user.Branch != exec.Branch)
                                {
                                    img.ImageUrl = "~/images/ArrowHollowSmall.gif";
                                    e.Row.Enabled = false;
                                    if (client.Inactive == true) e.Row.CssClass = "lightgrey-italic";
                                    else e.Row.CssClass = "darkgrey";
                                }
                            }
                            else
                            {
                                e.Row.CssClass = "darkgrey";
                                img.ImageUrl = "~/images/ArrowHollowSmall.gif";
                                e.Row.Enabled = false;
                                if (client.Inactive == true) e.Row.CssClass = "lightgrey-italic";
                                else e.Row.CssClass = "darkgrey";
                            }
                            break;
                        case (int)Enums.enUserRole.Branch:
                            if (exec != null)
                            {
                                if (user.DisplayName != exec.DisplayName)
                                {
                                    e.Row.CssClass = "darkgrey";
                                }
                                if (user.Branch != exec.Branch)
                                {
                                    img.ImageUrl = "~/images/ArrowHollowSmall.gif";
                                    e.Row.Enabled = false;
                                    if (client.Inactive == true) e.Row.CssClass = "lightgrey-italic";
                                    else e.Row.CssClass = "darkgrey";
                                }
                            }
                            else
                            {
                                e.Row.CssClass = "darkgrey";
                                img.ImageUrl = "~/images/ArrowHollowSmall.gif";
                                //e.Row.Enabled = false;
                                if (client.Inactive == true) e.Row.CssClass = "lightgrey-italic";
                                else e.Row.CssClass = "darkgrey";
                            }
                            break;
                        case (int)Enums.enUserRole.Region:
                            if (exec != null)
                            {
                                if (user.DisplayName != exec.DisplayName)
                                {
                                    e.Row.CssClass = "darkgrey";
                                }
                                if (user.Region != exec.Region)
                                {
                                    img.ImageUrl = "~/images/ArrowHollowSmall.gif";
                                    e.Row.Enabled = false;
                                    if (client.Inactive == true) e.Row.CssClass = "lightgrey-italic";
                                    else e.Row.CssClass = "darkgrey";
                                }
                            }
                            else
                            {

                                e.Row.CssClass = "darkgrey";
                                img.ImageUrl = "~/images/ArrowHollowSmall.gif";
                                e.Row.Enabled = false;
                                if (client.Inactive == true) e.Row.CssClass = "lightgrey-italic";
                                else e.Row.CssClass = "darkgrey";
                            }
                            break;
                        case (int)Enums.enUserRole.Company:
                            if (exec != null)
                            {
                                if (user.DisplayName != exec.DisplayName)
                                {
                                    e.Row.CssClass = "darkgrey";
                                }
                            }
                            else
                            {
                                e.Row.CssClass = "darkgrey";
                                img.ImageUrl = "~/images/ArrowHollowSmall.gif";
                                e.Row.Enabled = false;
                                if (client.Inactive == true) e.Row.CssClass = "lightgrey-italic";
                                else e.Row.CssClass = "darkgrey";
                            }
                            break;
                        case (int)Enums.enUserRole.Administrator:
                            // do nothing
                            if (exec == null)
                            {
                                //e.Row.CssClass = "darkgrey";
                                img.ImageUrl = "~/images/ArrowHollowSmall.gif";
                                e.Row.Cells[4].Enabled = false;
                                //if (client.Inactive == true) e.Row.CssClass = "lightgrey-italic";
                                //else e.Row.CssClass = "darkgrey";
                            }
                            break;
                    }

                    e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewClient.aspx?cid=" + DataBinder.Eval(e.Row.DataItem, "ClientID") + "'";
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void lnkCN_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("FindClientAll.aspx?sc=" + this.txtSearchCriteria.Text + "&f1=" + this.ddlClient.SelectedValue + "&f2=" + this.ddlMatch.SelectedValue + "&gr=" + ((LinkButton)sender).CommandArgument, false);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void grvClientsBA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HyperLink lnk = (HyperLink)e.Row.FindControl("lnkCopy");
                    lnk.NavigateUrl = "AddClient.aspx?cc=" + e.Row.Cells[2].Text;

                    sp_engage_search_clientResult client = (sp_engage_search_clientResult)e.Row.DataItem;
                    if (client.Inactive == true) e.Row.CssClass = "lightgrey";
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
