using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices;
using System.Text.RegularExpressions;
using enGage.BL;
using enGage.DL;
using enGage.Web.Helper;

namespace enGage.Web
{
    public partial class FindClientAll : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (Timeline.Capture("FindClientAll.aspx", "ASP.NET"))
                {
                    if (Session["USER"] == null)
                    {
                        bizMessage bizM = new bizMessage();
                        this.ucMessanger1.ProcessMessage("Session: " + bizM.GetMessageText("SessionMissing"), Enums.enMsgType.Err, "", null, true);
                        return;
                    }
                    
                    ((Main)Master).HeaderTitle = "List client search results";
                    LoadClients();
                }
                    
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void LoadClients()
        {
            bizMessage bizM = new bizMessage();
            bizSetting bizS = new bizSetting();
            int MaxRecords = int.Parse(bizS.GetSetting("FindClient.MaxRecords"));

            bizClient biz = new bizClient();

            this.ucMessanger1.ClearMessages();
            if (Request.QueryString["gr"] == "ba")
            {
                int? records = 0;
                List<sp_engage_search_clientResult> clients = biz.FindClientInBA(Request.QueryString["sc"]
                                                                      , Request.QueryString["f1"]
                                                                      , char.Parse(Request.QueryString["f2"])
                                                                      , MaxRecords
                                                                      , ref records);
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);

                if (clients == null) return;

                this.grvClientsBA.DataSource = clients;
                this.grvClientsBA.DataBind();
                this.lblResultCount.Text = clients.Count.ToString();
            }
            else
            {
                int? records = 0;
                List<sp_web_FindClientByFieldResult> clients = biz.FindClientByField(Request.QueryString["sc"]
                                                                      , Request.QueryString["f1"]
                                                                      , char.Parse(Request.QueryString["f2"])
                                                                      , Request.QueryString["gr"]
                                                                      , MaxRecords
                                                                      , ref records);
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);

                if (clients == null) return;

                foreach (sp_web_FindClientByFieldResult c in clients)
                {
                    if (c.AccountExecutiveID != "")
                    {
                        using (Timeline.Capture("bizUser.GetAccountExecutive", "AD"))
                        {
                            var u = bizUser.GetSMIAccountExecutiveIdBOAMPSUserName(c.AccountExecutiveID);
                            bizUser.enGageUser exec = bizUser.GetAccountExecutive(u);
                            if (exec != null)
                            {
                                c.DisplayName = exec.DisplayName;
                            }
                            else
                            {
                                c.DisplayName = c.AccountExecutiveID;
                            }
                        }
                    }
                }
                this.grvClients.DataSource = clients;
                this.grvClients.DataBind();
                this.lblResultCount.Text = clients.Count.ToString();
            }

            this.lblSearch.Text = Request.QueryString["sc"];
            this.btnBack.PostBackUrl = "FindClient.aspx?" +
                                       "sc=" + Request.QueryString["sc"] + "&" +
                                       "f1=" + Request.QueryString["f1"] + "&" +
                                       "f2=" + Request.QueryString["f2"];
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
                    bizUser.enGageUser exec;

                    using (Timeline.Capture("bizUser.GetAccountExecutive", "AD"))
                    {
                        var u = bizUser.GetSMIAccountExecutiveIdBOAMPSUserName(client.AccountExecutiveID);
                        exec = bizUser.GetAccountExecutive(u);
                    }

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
                        e.Row.Cells[5].Text = Regex.Replace(e.Row.Cells[5].Text, Request.QueryString["sc"], "<b>" + Request.QueryString["sc"].ToUpper() + "</b>", RegexOptions.IgnoreCase);
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

        protected void grvClients_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grvClients.PageIndex = e.NewPageIndex;
                LoadClients();
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

        protected void grvClientsBA_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grvClientsBA.PageIndex = e.NewPageIndex;
                LoadClients();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }
    }
}
