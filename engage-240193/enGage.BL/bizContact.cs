using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Web.UI.WebControls;
using enGage.DL;

namespace enGage.BL
{
    [DataObject]
    public class bizContact
    {
        public Utilities.RTNMessages MSGS;

        public bizContact()
        {
            MSGS = new Utilities.RTNMessages();
        }

        #region CRUD

        public Contact GetContact(int contactID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Contact con = db.Contacts.SingleOrDefault(c => c.ContactID == contactID);
                if (con == null)
                {
                    this.MSGS.AddMessage(biz.GetMessageText("EmptyRecord"), Enums.enMsgType.Err);
                }
                return con;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public int InsertContact(Contact c)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                db.Contacts.InsertOnSubmit(c);
                db.SubmitChanges();
                this.MSGS.AddMessage(biz.GetMessageText("InsertSuccess"), Enums.enMsgType.OK);
                return c.ContactID;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return 0;
            }
        }

        public bool UpdateContact(Contact c)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Contact ec = db.Contacts.Single(cli => cli.ContactID == c.ContactID);
                //general
                ec.ContactName = c.ContactName;
                ec.Title = c.Title;
                ec.Mobile = c.Mobile;
                ec.DirectLine = c.DirectLine;
                ec.Email = c.Email;
                // audit
                ec.Modified = c.Modified;
                ec.ModifiedBy = c.ModifiedBy;

                db.SubmitChanges();
                
                this.MSGS.AddMessage(biz.GetMessageText("UpdateSuccess"), Enums.enMsgType.OK);
                return true;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return false;
            }
        }

        #endregion

        #region Misc

        public bool SetToActiveInactive(int contactID, bool inactive)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Contact ec = db.Contacts.Single(con => con.ContactID == contactID);

                ec.Inactive = inactive;
                ec.ModifiedBy = bizUser.GetCurrentUserName();
                ec.Modified = DateTime.Now;

                db.SubmitChanges();

                this.MSGS.AddMessage(biz.GetMessageText("UpdateSuccess"), Enums.enMsgType.OK);
                return true;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return false;
            }
        }

        #endregion

        #region Validation

        public bool ValidateContact(Contact c)
        {
            bizMessage biz = new bizMessage();
            try
            {
                if (c.ContactName == string.Empty)
                {
                    this.MSGS.AddMessage("Contact Name: " + biz.GetMessageText("EmptyField"), Enums.enMsgType.Err, DateTime.Now, "ContactName", typeof(TextBox));
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return false;
            }
        }

        #endregion
    }
}
