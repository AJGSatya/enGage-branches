using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using enGage.DL;
using enGage.BL;

namespace enGage.Web
{
    public partial class EditClient : System.Web.UI.Page
    {
        #region Protected Properties

        protected string AddressClientId
        {
            get { return this.txtAddress.ClientID; }
        }
        protected string SuburbClientId
        {
            get { return this.ucAUPSS1.SuburbControl.ClientID; }
        }
        //protected string StateCodeClientId
        //{
        //    get { return this.ucAUPSS1.StateCodeControl.ClientID; }
        //}
        protected string PostCodeClientId
        {
            get { return this.ucAUPSS1.PostCodeControl.ClientID; }
        }

        #endregion

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
                    var master = (Main)Master;
                    if (master != null) master.HeaderTitle = "Edit client";

                    PopulateClientDetails();
                    SetAddressControls();
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void SetAddressControls()
        {
            if (this.rblAddressTypes.SelectedIndex == 1)
            {
                this.trAUPSS.Style.Remove("display");
                this.trAUPSS.Style.Add("display", "none");
            }
            else
            {
                this.trAUPSS.Style.Remove("display");
                this.trAUPSS.Style.Add("display", "");
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

            //read only
            this.lblClientName.Text = c.ClientName;
            this.lblOfficeFacsimilie.Text = c.OfficeFacsimilie;
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

            //executive
            var u = bizUser.GetSMIAccountExecutiveIdBOAMPSUserName(c.AccountExecutiveID);
            bizUser.enGageUser exec = bizUser.GetAccountExecutive(u);
            ucMessanger1.ProcessMessages(biz.MSGS, false);
            lblAccountExecutive.Text = "<b>" + exec.DisplayName + "</b>" + ", " + exec.Branch + " (" + exec.Region + ")";

            //general
            //this.ddlAccountExecutive.SelectedValue = c.AccountExecutiveID.ToString();
            this.txtClientCode.Text = c.ClientCode;
            this.txtClientName.Text = c.ClientName;
            this.txtRegisteredName.Text = c.RegisteredName;
            this.txtInsuredName.Text = c.InsuredName;
            if (c.ABNACN != null) this.txtABNACN.Text = c.ABNACN;
            this.txtSource.Text = c.Source;
            this.txtOfficeFacsimilie.Text = c.OfficeFacsimilie;
            this.txtOfficePhone.Text = c.OfficePhone;

            //address
            this.txtAddress.Text = c.Address;
            if (c.Location != null)
            {
                this.ucAUPSS1.PostCode = c.PostCode;
                this.ucAUPSS1.SetSuburbAndStateCode(c.Location, c.StateCode);
                //this.ucAUPSS1.Suburb = c.Location;
                //this.ucAUPSS1.StateCode = c.StateCode;
                this.rblAddressTypes.SelectedIndex = 0;
            }
            else
            {
                this.rblAddressTypes.SelectedIndex = 1;
            }

            //industry
            if (c.AnzsicCode != null)
            {
                bizIndustry bizI = new bizIndustry();
                Industry ind = bizI.GetIndustry(c.AnzsicCode);
                this.ucMessanger1.ProcessMessages(bizI.MSGS, false);
                this.lstIndustry.Items.Add(new ListItem(ind.IndustryName + " (" + ind.AnzsicCode + ")", ind.AnzsicCode));
                this.lstIndustry.SelectedIndex = 0;
                if (this.lstIndustry.Items.Count > 0) this.lstIndustry.Visible = true;
                PopulateAssociations();
                if (c.AssociationCode == null)
                {
                    this.ddlAssociation.SelectedIndex = 0;
                }
                else
                {
                    this.ddlAssociation.Items.RemoveAt(0);
                    this.ddlAssociation.SelectedValue = c.AssociationCode;
                }
                this.txtAssociationMemberNumber.Text = c.AssociationMemberNumber;
            }

            //audit
            ((Main)Master).HeaderDetails = "Client added by "
                        + bizActiveDirectory.GetUserFullName(c.AddedBy)
                        + " (" + string.Format("{0:dd-MMM-yy}", c.Added) + ")";

            if (c.Modified.HasValue == true)
                ((Main)Master).HeaderDetails += " / modified by "
                    + bizActiveDirectory.GetUserFullName(c.ModifiedBy) 
                    + " (" + string.Format("{0:dd-MMM-yy}", c.Modified.Value) + ")";
        }

        private void PopulateIndustries()
        {
            bizMessage bizM = new bizMessage();

            bizIndustry biz = new bizIndustry();
            IQueryable<Industry> inds = biz.ListIndustriesByKeyword(this.txtFindIndustry.Text);
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);

            this.lstIndustry.Items.Clear();
            if (this.txtFindIndustry.Text != "")
            {
                foreach (Industry ind in inds)
                {
                    this.lstIndustry.Items.Add(new ListItem(ind.IndustryName + " (" + ind.AnzsicCode + ")", ind.AnzsicCode));
                }
                this.lstIndustry.Visible = true;
                this.lblFoundIndustries.Text = this.lstIndustry.Items.Count.ToString() + " industries found";
            }
            else
            {
                this.lstIndustry.Visible = false;
                this.lblFoundIndustries.Text = "";
            }
        }

        private void PopulateAssociations()
        {
            bizMessage bizM = new bizMessage();

            bizClient biz = new bizClient();
            IQueryable<Association> asss = biz.ListAssociationsByIndustry(this.lstIndustry.SelectedValue);
            this.ucMessanger1.ProcessMessages(biz.MSGS, true);

            this.ddlAssociation.Items.Clear();
            this.ddlAssociation.Items.Add(new ListItem("-- Please Select --", ""));
            foreach (Association ass in asss)
            {
                this.ddlAssociation.Items.Add(new ListItem(ass.AssociationName, ass.AssociationCode));
            }
        }

        protected void lstIndustry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PopulateAssociations();
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
                if (this.txtFindIndustry.Text == "")
                {
                    this.lblFoundIndustries.Text = "You have to type a keyword";
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                this.ucMessanger1.ClearMessages();
                this.ucMessanger1.UnmarkControls();

                bizMessage bizM = new bizMessage();
                if (this.txtAddress.Text.Length > this.txtAddress.MaxLength)
                {
                    this.ucMessanger1.ProcessMessage("Address: " + bizM.GetMessageText("ValueGreaterThanMax"), Enums.enMsgType.Err, "", null, true);
                    return;
                }

                Client c = new Client();
                bizClient biz = new bizClient();

                c.ClientID = int.Parse(Request.QueryString["cid"]);
                //if (this.ddlAccountExecutive.SelectedValue != "")
                //    c.AccountExecutiveID = int.Parse(this.ddlAccountExecutive.SelectedValue);
                c.ClientCode = this.txtClientCode.Text;
                c.ClientName = this.txtClientName.Text;
                c.RegisteredName = this.txtRegisteredName.Text;
                c.InsuredName = this.txtInsuredName.Text;
                if (this.txtABNACN.Text != "") c.ABNACN = this.txtABNACN.Text;
                c.Source = this.txtSource.Text;
                c.OfficeFacsimilie = this.txtOfficeFacsimilie.Text;
                c.OfficePhone = this.txtOfficePhone.Text;
                //address
                if (this.txtAddress.Text != "") c.Address = this.txtAddress.Text;
                if (this.rblAddressTypes.SelectedIndex == 0 && this.ucAUPSS1.SuburbControl.SelectedIndex > -1)
                {
                    if (this.ucAUPSS1.PostCode != "") c.PostCode = this.ucAUPSS1.PostCode;
                    if (this.ucAUPSS1.StateCode != "") c.StateCode = this.ucAUPSS1.StateCode;
                    if (this.ucAUPSS1.Suburb != "") c.Location = this.ucAUPSS1.Suburb;
                }
                //industry
                if (this.lstIndustry.SelectedValue != "")
                    c.AnzsicCode = this.lstIndustry.SelectedValue;
                if (this.ddlAssociation.SelectedValue != "")
                    c.AssociationCode = this.ddlAssociation.SelectedValue;
                c.AssociationMemberNumber = this.txtAssociationMemberNumber.Text;
                //audit
                c.ModifiedBy = bizUser.GetCurrentUserName();
                c.Modified = DateTime.Now;
                //action
                if (biz.ValidateClient(c) == false)
                {
                    this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                    return;
                }
                if (biz.UpdateClient(c) == true)
                {
                    Response.Redirect("ViewClient.aspx?cid=" + c.ClientID.ToString(), false);
                }
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ViewClient.aspx?cid=" + Request.QueryString["cid"], false);
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }
    }
}
