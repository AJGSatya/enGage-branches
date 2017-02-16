using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using enGage.DL;

namespace enGage.BL
{
    [DataObject]
    public class bizSetting
    {

        public Utilities.RTNMessages MSGS;

        public bizSetting()
        {
            MSGS = new Utilities.RTNMessages();
        }

        public Setting GetSettingObject(String code)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Setting set = db.Settings.SingleOrDefault(p => p.SettingCode == code);
                if (set == null)
                {
                    return null;
                }
                else
                {
                    return set;
                }
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public string GetSetting(String code)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Setting pre = db.Settings.SingleOrDefault(p => p.SettingCode == code);
                if (pre == null)
                {
                    return "";
                }
                else
                {
                    return pre.SettingValue;
                }
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return "";
            }
        }

    }
}
