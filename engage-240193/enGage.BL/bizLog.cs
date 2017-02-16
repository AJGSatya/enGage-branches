using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.ComponentModel;
using enGage.DL;
using System.Diagnostics;

namespace enGage.BL
{
    [DataObject]
    public class bizLog
    {

        public static int InsertExceptionLog(Exception ex)
        {
            try
            {
                enGageDataContext db = new enGageDataContext();
                ExceptionLog el = new ExceptionLog();
                if (ex.Message.Length > 255) el.ShortDescription = ex.Message.Substring(0,255);
                else el.ShortDescription = ex.Message;
                el.LongDescription = ex.StackTrace;
                el.CreatedDate = DateTime.Now;
                el.CreatedBy = bizUser.GetCurrentUserName();
                db.ExceptionLogs.InsertOnSubmit(el);
                db.SubmitChanges();
                return el.ExceptionLogId;
            }
            catch (Exception exc)
            {
                LogEvent(ex.Message + "\n" + ex.StackTrace, Enums.enMsgType.Err, "enGage");
                throw new Exception(exc.Message);
            }
        }

        public IList<sp_web_ListLogsResult> ListLogs(int filter)
        {
            try
            {
                enGageDataContext db = new enGageDataContext();
                List<sp_web_ListLogsResult> list;
                list = db.sp_web_ListLogs(filter).ToList();
                return list;
            }
            catch (Exception ex)
            {
                LogEvent(ex.Message + "\n" + ex.StackTrace, Enums.enMsgType.Err, "enGage");
                throw new Exception(ex.Message);
            }
        }

        public static void LogEvent(string message, Enums.enMsgType messageType, string source)
        {
            try
            {
                EventLogEntryType type = EventLogEntryType.Error;
                switch (messageType)
                {
                    case Enums.enMsgType.OK:
                        type = EventLogEntryType.Information;
                        break;
                    case Enums.enMsgType.Warn:
                        type = EventLogEntryType.Warning;
                        break;
                    case Enums.enMsgType.Err:
                        type = EventLogEntryType.Error;
                        break;
                }
                EventLog.WriteEntry(source, message, type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
