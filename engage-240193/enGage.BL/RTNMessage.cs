using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using enGage.BL;

namespace enGage.Utilities
{
    [Serializable(), System.Diagnostics.DebuggerStepThrough()]
    public class RTNMessage
    {
        public String Message;
        public Enums.enMsgType MessageType;
        public DateTime MessageDate;
        public String MessageSource;
        public String MessageSourceMethod;
        public String StackTrace;
        public String LinkedControlName;
        public Type LinkedControlType;

        public RTNMessage()
        {
            this.Message = "";
        }

        public RTNMessage(string Message, Enums.enMsgType MessageType, System.DateTime MessageDate)
        {
            this.Message = Message;
            this.MessageType = MessageType;
            this.MessageDate = MessageDate;
            this.GetSource();
            this.StackTrace = "";
            this.LinkedControlName = "";
            
            if (this.MessageType == Enums.enMsgType.Err)
            {
                this.StackTrace = new StackTrace(false).ToString().Trim().Replace(System.Environment.NewLine + System.Environment.NewLine, "").Replace("\t", "");
            }
        }

        public RTNMessage(string Message, Enums.enMsgType MessageType, System.DateTime MessageDate, string MessageSource, string MessageSourceMethod)
        {
            this.Message = Message;
            this.MessageType = MessageType;
            this.MessageDate = MessageDate;
            this.MessageSource = MessageSource;
            this.MessageSourceMethod = MessageSourceMethod;
            this.StackTrace = "";
            this.LinkedControlName = "";
            this.LinkedControlType = null;
            if (this.MessageType == Enums.enMsgType.Err)
            {
                this.StackTrace = new StackTrace(false).ToString().Trim().Replace(System.Environment.NewLine + System.Environment.NewLine, "").Replace("\t", "");
            }
        }

        public RTNMessage(string Message, Enums.enMsgType MessageType, System.DateTime MessageDate, string LinkeControlName, Type LinkedControlType)
        {
            this.Message = Message;
            this.MessageType = MessageType;
            this.MessageDate = MessageDate;
            this.MessageSource = "";
            this.MessageSourceMethod = "";
            this.StackTrace = "";
            this.LinkedControlName = LinkeControlName;
            this.LinkedControlType = LinkedControlType;
            if (this.MessageType == Enums.enMsgType.Err)
            {
                this.StackTrace = new StackTrace(false).ToString().Trim().Replace(System.Environment.NewLine + System.Environment.NewLine, "").Replace("\t", "");
            }
        }

        public RTNMessage(string Message, Enums.enMsgType MessageType, System.DateTime MessageDate, string MessageSource, string MessageSourceMethod, string StackTrace)
        {
            this.Message = Message;
            this.MessageType = MessageType;
            this.MessageDate = MessageDate;
            this.MessageSource = MessageSource;
            this.MessageSourceMethod = MessageSourceMethod;
            this.StackTrace = StackTrace;
            this.LinkedControlName = "";
            this.LinkedControlType = null;
        }

        public string MessageTypeName()
        {
            switch (this.MessageType)
            {
                case Enums.enMsgType.Err:
                    return "Error";
                case Enums.enMsgType.OK:
                    return "Success";
                case Enums.enMsgType.Warn:
                    return "Warning";
                default:
                    return "";
            }
        }

        private void GetSource()
        {
            try
            {
                StackTrace ST = new StackTrace(false);
                MessageSource = "Undetermined";
                MessageSourceMethod = "Undetermined";
                for (int I = 1; I <= ST.FrameCount; I++)
                {
                    System.Reflection.MethodBase MB = ST.GetFrame(I).GetMethod();
                    if (MB.DeclaringType.Namespace.ToUpper().IndexOf("OAMPS".ToUpper()) > 0)
                    {
                        if (MB.DeclaringType.Name.ToUpper() != "RTNMESSAGES" & MB.DeclaringType.Name.ToUpper() != "RTNMESSAGE" & MB.DeclaringType.Name.ToUpper() != "CRSP" & MB.Name != ".ctor")
                        {
                            MessageSource = MB.DeclaringType.Name;
                            MessageSourceMethod = MB.Name;
                            break; // TODO: might not be correct. Was : Exit For 
                        }
                    }
                }
            }
            catch
            {
            }
        }
    } 
}
