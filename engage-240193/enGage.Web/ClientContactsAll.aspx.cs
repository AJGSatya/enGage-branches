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
    public partial class ClientContactsAll : System.Web.UI.Page
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
                ((Main)Master).HeaderTitle = "Client Contacts All";
                PopulateClientDetails();
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
            bizSetting bizS = new bizSetting();

            bizClient biz = new bizClient();
            Client c;
            c = biz.GetClient(Convert.ToInt32(Request.QueryString["cid"]));
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);

            if (c == null) return;

            //general
            this.lblClientName.Text = c.ClientName;
            this.lblOfficeFacsimilie.Text = c.OfficeFacsimilie;
            this.lblOfficePhone.Text = c.OfficePhone;

            //address
            this.lblAddress.Text = c.Address + ", " + c.Location + " " + c.StateCode + " " + c.PostCode;

            //executive
            var u = bizUser.GetSMIAccountExecutiveIdBOAMPSUserName(c.AccountExecutiveID);
            bizUser.enGageUser exec = bizUser.GetAccountExecutive(u);
            this.ucMessanger1.ProcessMessages(biz.MSGS, false);
            this.lblAccountExecutive.Text = "<b>" + exec.DisplayName + "</b>" + ", " + exec.Branch + " (" + exec.Region + ")";

            //active contacts
            bizContact bizA = new bizContact();
            List<Contact> acons = c.Contacts.Where(co => co.Inactive == false).ToList();
            this.grvActiveContacts.DataSource = acons;
            this.ucMessanger1.ProcessMessages(bizA.MSGS, false);
            this.grvActiveContacts.DataBind();
            this.lblActiveContacts.Text = acons.Count.ToString();

            //inactive contacts
            bizContact bizI = new bizContact();
            List<Contact> icons = c.Contacts.Where(co => co.Inactive == true).ToList();
            this.grvInactiveContacts.DataSource = icons;
            this.ucMessanger1.ProcessMessages(bizI.MSGS, false);
            this.grvInactiveContacts.DataBind();
            this.lblInactiveContacts.Text = icons.Count.ToString();

            //buttons
            this.btnBack.PostBackUrl = "ViewClient.aspx?cid=" + c.ClientID.ToString();
        }

        protected void grvContacts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Contact c = (Contact)e.Row.DataItem;

                    if (c.Mobile == "")
                    {
                        e.Row.Cells[4].Text = "no mobile";
                        e.Row.Cells[4].CssClass = "lightgrey-italic";
                    }
                    if (c.DirectLine == "")
                    {
                        e.Row.Cells[5].Text = "no direct line";
                        e.Row.Cells[5].CssClass = "lightgrey-italic";
                    }
                    if (c.Email == "")
                    {
                        e.Row.Cells[6].Text = "no email address";
                        e.Row.Cells[6].CssClass = "lightgrey-italic";
                    }

                    e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#F4F3F0';this.style.cursor='hand';");
                    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = 'white';");
                    e.Row.Attributes["onClick"] = "location.href='ViewContact.aspx?cid=" + DataBinder.Eval(e.Row.DataItem, "ClientID") + "&coid=" + DataBinder.Eval(e.Row.DataItem, "ContactID") + "'";
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
