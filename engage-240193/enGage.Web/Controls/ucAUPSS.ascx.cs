using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using enGage.DL;
using enGage.BL;

namespace enGage.Web.Controls
{
    public partial class ucAUPSS : System.Web.UI.UserControl
    {

        #region Public Methods

        public void ClearControl()
        {
            this.txtPostCode.Text = "";
            this.ddlSuburb.Items.Clear();
        }

        #endregion

        #region Public Propertis

        public bool Enabled
        {
            get
            {
                return this.txtPostCode.Enabled;
            }
            set
            {
                this.txtPostCode.Enabled = value;
                this.ddlSuburb.Enabled = value;
            }
        }

        public string PostCode
        {
            get
            {
                return this.txtPostCode.Text.Trim();
            }
            set
            {
                this.txtPostCode.Text = value.Trim();
                LoadSuburbsAndState();
            }
        }

        public string Suburb
        {
            get
            {
                char[] sep = { '#' };
                if (this.ddlSuburb.SelectedIndex == -1) return "";
                else return this.ddlSuburb.SelectedValue.Split(sep)[0].Trim();
            }
        }

        public string StateCode
        {
            get
            {
                char[] sep = { '#' };
                if (this.ddlSuburb.SelectedIndex == -1) return "";
                else return this.ddlSuburb.SelectedValue.Split(sep)[1].Trim();
            }
        }

        public void SetSuburbAndStateCode(string suburb, string stateCode)
        {
            this.ddlSuburb.SelectedValue = suburb + "#" + stateCode;
            this.hidSuburb.Value = this.ddlSuburb.SelectedIndex.ToString();
        }

        public string CssClass
        {
            get
            {
                return this.txtPostCode.CssClass;
            }
            set
            {
                this.txtPostCode.CssClass = value;
                this.ddlSuburb.CssClass = value;
            }
        }


        public DropDownList SuburbControl
        {
            get { return this.ddlSuburb; }
        }

        public TextBox PostCodeControl
        {
            get { return this.txtPostCode; }
        }

        #endregion

        #region Load

        protected void Page_Load(object sender, EventArgs e)
        {
            String cbReference;
            cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript;
            callbackScript = "function CallServer(arg, context)" +
                             "{ " + "arg = document.getElementsByName('"+ this.txtPostCode.ClientID + "')[0].value; " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

            this.txtPostCode.Attributes.Add("onkeydown", "if (event.keyCode == 9) CallServer('', '');");

            if (this.txtPostCode.Text != "")
            {
                LoadSuburbsAndState();
                if (this.hidSuburb.Value != "")
                {
                    if (this.ddlSuburb.Items.Count > Convert.ToInt32(this.hidSuburb.Value))
                    {
                        this.ddlSuburb.SelectedIndex = Convert.ToInt32(this.hidSuburb.Value);
                    }
                }
            }
        }

        private void LoadSuburbsAndState()
        {
            try
            {
                bizAUPostCode biz = new bizAUPostCode();
                IList<AUPostCode> pss;
                pss = biz.ListSuburbState(this.txtPostCode.Text.Trim());
                this.ddlSuburb.Items.Clear();
                if (pss.Count > 0)
                {
                    foreach (AUPostCode au in pss)
                    {
                        this.ddlSuburb.Items.Add(new ListItem(au.Suburb + " (" + au.StateCode + ")", au.Suburb + "#" + au.StateCode));
                    }
                    this.ddlSuburb.CssClass = "control";
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                Response.Redirect("~/General/ErrorPage.aspx", false);
            }
        }

        #endregion

        #region Callback

        protected String returnValue;

        public void RaiseCallbackEvent(String eventArgument)
        {
            bizAUPostCode biz = new bizAUPostCode();
            IList<AUPostCode> pss;
            pss = biz.ListSuburbState(eventArgument);
            foreach (AUPostCode au in pss)
            {
                returnValue += au.Suburb + "#" + au.StateCode + ";";
            }
        }

        public string GetCallbackResult()
        {
            return returnValue;
        }

        #endregion

    }
}