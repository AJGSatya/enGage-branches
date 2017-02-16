using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using enGage.DL;

namespace enGage.BL
{
    [DataObject]
    public class bizAUPostCode
    {
        public Utilities.RTNMessages MSGS;

        public bizAUPostCode()
        {
            MSGS = new Utilities.RTNMessages();
        }

        //public AUPostCode GetAUPostCodeBySPS(string suburb, string postcode, string statecode)
        //{
        //    bizMessage bizMessage = new bizMessage();
        //    try
        //    {
        //        enGageDataContext db = new enGageDataContext();
        //        var query = from pc in db.AUPostCodes
        //                    where pc.Suburb.Equals(suburb) && pc.PostCode.Equals(postcode) && pc.StateCode.Equals(statecode)
        //                    select pc;
        //        return query.SingleOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        this.MSGS.AddMessage(bizMessage.GetMessageText("Exception"), Enums.enMsgType.Err);
        //        bizLog.InsertExceptionLog(ex);
        //        return null;
        //    }
        //}

        public IList<AUPostCode> ListSuburbState(string postcode)
        {
            bizMessage bizMessage = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                var query = from pc in db.AUPostCodes
                            where pc.PostCode.Equals(postcode)
                            select pc;
                return query.ToList();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(bizMessage.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }
 
    }
}
