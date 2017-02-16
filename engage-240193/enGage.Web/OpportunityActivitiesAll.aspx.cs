using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using enGage.DL;
using enGage.BL;

namespace enGage.Web
{
    public partial class OpportunityActivitiesAll : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    if (Session["USER"] == null)
                    {
                        bizMessage bizM = new bizMessage();
                        this.ucMessanger1.ProcessMessage("Session: " + bizM.GetMessageText("SessionMissing"), Enums.enMsgType.Err, "", null, true);
                        return;
                    }
                }
                ((Main)Master).HeaderTitle = "Client Contacts";
                PopulateClientDetails();
                PopulateActivities();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void PopulateClientDetails()
        {
            bizMessage bizM = new bizMessage();

            bizClient biz = new bizClient();
            Client c;
            c = biz.GetClient(int.Parse(Request.QueryString["cid"]));
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);

            if (c == null) return;

            //client details
            this.lblClientName.Text = c.ClientName;
            this.lblOfficePhone.Text = c.OfficePhone;
            if (c.Address == null)
            {
                this.lblAddress.Text = "no address";
                this.lblAddress.CssClass = "page-text-nodata";
            }
            else
            {
                this.lblAddress.Text = c.Address + ", " + c.Location + " " + c.StateCode + " " + c.PostCode;
            }
            if (c.Industry != null) this.lblAssociation.Text = c.Industry.IndustryName + "(" + c.Industry.AnzsicCode + ")";
            if (c.AssociationCode != null) this.lblAssociation.Text = c.Association.AssociationName;

            //executive
            var u = bizUser.GetSMIAccountExecutiveIdBOAMPSUserName(c.AccountExecutiveID);
            bizUser.enGageUser exec = bizUser.GetAccountExecutive(u);
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            this.lblAccountExecutive.Text = "<b>" + exec.DisplayName + "</b>" + ", " + exec.Branch + " (" + exec.Region + ")";
        }

        private void PopulateActivities()
        {
            bizOpportunity biz = new bizOpportunity();

            //active activities
            List<sp_web_ListOpportunityActiveActivitiesResult> aa = biz.ListOpportunityActiveActivites(int.Parse(Request.QueryString["oid"]));
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            this.grvActiveActivities.DataSource = aa;
            this.grvActiveActivities.DataBind();
            this.lblActive.Text = aa.Count.ToString();

            //inactive activities
            List<sp_web_ListOpportunityInactiveActivitiesResult> ia = biz.ListOpportunityInactiveActivites(int.Parse(Request.QueryString["oid"]));
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            this.grvInactiveActivities.DataSource = ia;
            this.grvInactiveActivities.DataBind();
            this.lblInactive.Text = ia.Count.ToString();

            //buttons
            this.btnBack.PostBackUrl = "ViewOpportunity.aspx?cid=" + Request.QueryString["cid"] + "&oid=" + Request.QueryString["oid"];
        }

        protected void grvActivities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewActivity.aspx?cid=" + Request.QueryString["cid"] + "&oid=" + Request.QueryString["oid"] + "&aid=" + DataBinder.Eval(e.Row.DataItem, "ActivityID") + "'";
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
