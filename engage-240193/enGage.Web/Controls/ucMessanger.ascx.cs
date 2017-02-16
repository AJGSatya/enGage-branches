using System;
using System.Collections;
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
using System.Drawing;
using enGage.DL;
using enGage.BL;
using enGage.Utilities;
using enGage.Web.Helper;

namespace enGage.Web.Controls
{
    public partial class ucMessanger : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void ClearMessages()
        {
            this.Controls.Clear();
        }

        public void ProcessMessage(string message, Enums.enMsgType messageType, string[] controlNames, Type controlType, bool clearPrevious)
        {
            if (clearPrevious == true)
            {
                this.Controls.Clear();
            }

            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            Label lbl = new Label();
            HtmlGenericControl br = new HtmlGenericControl("BR");
            img.ImageAlign = ImageAlign.TextTop;
            lbl.Text = " " + message;
            switch (messageType)
            {
                case Enums.enMsgType.OK:
                    img.ImageUrl = "~/images/info16x16.png";
                    lbl.ForeColor = Color.Green;
                    break;
                case Enums.enMsgType.Warn:
                    img.ImageUrl = "~/images/warning16x16.png";
                    lbl.ForeColor = Color.FromArgb(206,113,9);
                    break;
                case Enums.enMsgType.Err:
                    img.ImageUrl = "~/images/error16x16.png";
                    lbl.ForeColor = Color.Red;
                    break;
            }
            this.Controls.Add(img);
            this.Controls.Add(lbl);
            this.Controls.Add(br);

            foreach (string controlName in controlNames)
            {
                if (controlName != "")
                {
                    MarkControl(controlName, controlType, messageType);
                }
            }
        }
        
        public void ProcessMessage(string message, Enums.enMsgType messageType, string controlName, Type controlType, bool clearPrevious)
        {
            string[] controlNames = new string[1];
            controlNames[0] = controlName;
            ProcessMessage(message, messageType, controlNames, controlType, clearPrevious);

        }
 
        public void ProcessMessages(Utilities.RTNMessages MSGS, bool clearPrevious)
        {
            if (clearPrevious == true)
            {
                this.Controls.Clear();
            }

            foreach (Utilities.RTNMessage MSG in MSGS)
            {
                System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                Label lbl = new Label();
                HtmlGenericControl br = new HtmlGenericControl("BR");
                img.ImageAlign = ImageAlign.TextTop;
                lbl.Text = " " + MSG.Message;
                switch (MSG.MessageType)
                {
                    case Enums.enMsgType.OK:
                        img.ImageUrl = "~/images/info16x16.png";
                        lbl.ForeColor = Color.Green;
                        break;
                    case Enums.enMsgType.Warn:
                        img.ImageUrl = "~/images/warning16x16.png";
                        lbl.ForeColor = Color.FromArgb(206, 113, 9);
                        break;
                    case Enums.enMsgType.Err:
                        img.ImageUrl = "~/images/error16x16.png";
                        lbl.ForeColor = Color.Red;
                        break;
                }
                this.Controls.Add(img);
                this.Controls.Add(lbl);
                this.Controls.Add(br);

                if (MSG.LinkedControlName != "")
                {
                    MarkControl(MSG.LinkedControlName, MSG.LinkedControlType, MSG.MessageType);
                }
            }
        }

        private void MarkControl(string controlName, Type controlType, Enums.enMsgType messageType)
        {
            switch (controlType.Name)
            {
                case "TextBox":
                    TextBox txt;
                    txt = ((TextBox)this.Page.Master.FindControlRecursive("txt" + controlName));
                    if(txt==null)
                        txt = ((TextBox)this.Page.Master.FindControlRecursive(controlName));
                    if (messageType == Enums.enMsgType.Err)
                    {
                        txt.CssClass = "validation-err";
                    }
                    else // warning
                    {
                        txt.CssClass = "validation-warn";
                    }
                    break;
                case "DropDownList":
                    DropDownList ddl;
                    ddl = ((DropDownList)this.Page.Master.FindControlRecursive("ddl" + controlName));
                    if(ddl==null)
                        ddl = ((DropDownList)this.Page.Master.FindControlRecursive(controlName));
                    if (messageType == Enums.enMsgType.Err)
                    {
                        ddl.CssClass = "validation-err";
                    }
                    else // warning
                    {
                        ddl.CssClass = "validation-warn";
                    }
                    break;
                case "RadioButtonList":
                    RadioButtonList rbl;
                    rbl = ((RadioButtonList)this.Page.Master.FindControlRecursive("rbl" + controlName));
                    if(rbl==null)
                        rbl = ((RadioButtonList)this.Page.Master.FindControlRecursive(controlName));
                    if (messageType == Enums.enMsgType.Err)
                    {
                        rbl.CssClass = "validation-err";
                    }
                    else // warning
                    {
                        rbl.CssClass = "validation-warn";
                    }
                    break;
                default:
                    // do nothing
                    break;
            }
        }

        private void LoopControlsRecursive(Control root)
        {
            switch (root.GetType().Name)
            {
                case "TextBox":
                    TextBox txt;
                    txt = (TextBox)root;
                    txt.CssClass = "control";
                    break;
                case "DropDownList":
                    DropDownList ddl;
                    ddl = (DropDownList)root;
                    ddl.CssClass = "control";
                    break;
                case "RadioButtonList":
                    RadioButtonList rbl;
                    rbl = (RadioButtonList)root;
                    rbl.CssClass = "control";
                    break;
            }

            foreach (Control c in root.Controls)
            {
                LoopControlsRecursive(c);
                
            }

           
        }


        public void UnmarkControls()
        {
            LoopControlsRecursive(this.Page.Master.FindControl("ContentPlaceHolder1"));

            /*
            foreach (Control c in this.Page.Master.FindControl("ContentPlaceHolder1").Controls[0].Controls)
            {
                switch (c.GetType().Name)
                {
                    case "TextBox":
                        TextBox txt;
                        txt = (TextBox)c;
                        txt.CssClass = "control";
                        break;
                    case "DropDownList":
                        DropDownList ddl;
                        ddl = (DropDownList)c;
                        ddl.CssClass = "control";
                        break;
                    case "RadioButtonList":
                        RadioButtonList rbl;
                        rbl = (RadioButtonList)c;
                        rbl.CssClass = "control";
                        break;
                }
            }
             */
        }

    }
}