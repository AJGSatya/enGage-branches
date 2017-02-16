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
    public class bizIndustry
    {
        public Utilities.RTNMessages MSGS;

        public bizIndustry()
        {
            MSGS = new Utilities.RTNMessages();
        }     

        //public IQueryable<Industry> ListIndustries()
        //{
        //    bizMessage biz = new bizMessage();
        //    try
        //    {
        //        enGageDataContext db = new enGageDataContext();
        //        var query =
        //           from i in db.Industries
        //           orderby i.IndustryName ascending
        //           select i;
        //        return query.AsQueryable();
        //    }
        //    catch (Exception ex)
        //    {
        //        this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
        //        bizLog.InsertExceptionLog(ex);
        //        return null;
        //    }
        //}

        public IQueryable<Industry> ListIndustriesByKeyword(string keyword)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                var query =
                   from i in db.Industries
                   where i.IndustryKeyWords.Contains(keyword)
                   orderby i.IndustryName ascending
                   select i;
                return query.AsQueryable();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public Industry GetIndustry(string AnzsicCode)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Industry ind = db.Industries.SingleOrDefault(i => i.AnzsicCode == AnzsicCode);
                if (ind == null)
                {
                    this.MSGS.AddMessage(biz.GetMessageText("EmptyRecord"), Enums.enMsgType.Err);
                }
                return ind;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public List<Industry> GetIndustriesByAnzsicCodes(string[] AnzsicCode)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                var query = 
                   from i in db.Industries
                   where AnzsicCode.Contains(i.AnzsicCode)
                   orderby i.IndustryName ascending
                   select i;
                return query.ToList();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }
    }
}
