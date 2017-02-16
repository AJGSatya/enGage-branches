using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data.Linq;
using enGage.DL;

namespace enGage.BL
{
    [DataObject]
    public class bizMessage
    {

        public string GetMessageText(string messageKey)
        {
            try
            {
                enGageDataContext db = new enGageDataContext();
                string msgtext;
                msgtext = db.Messages.SingleOrDefault(m => m.MessageKey == messageKey).MessageText;
                return msgtext;
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
                return "";
            }
        }

    }
}
