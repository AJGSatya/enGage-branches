using System;
using System.IO;
using System.Xml;
using System.Web;
using System.Collections;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Reflection;
using enGage.BL;

namespace enGage.Utilities
{
        [Serializable(), System.Diagnostics.DebuggerStepThrough()]
        public class RTNMessages : ICollection
        {
            public string CollectionName;
            private ArrayList MsgArray = new ArrayList();

            #region Misc
            
            public RTNMessage Item(int index)
            {
                return (RTNMessage)MsgArray[index];
            }

            public void Merge(RTNMessage[] Message)
            {
                foreach (RTNMessage M in Message)
                {
                    this.AddMessage(M.Message, M.MessageType, M.MessageDate, M.MessageSource, M.MessageSourceMethod, M.StackTrace);
                }
            }
            
            public void Clear()
            {
                this.MsgArray.Clear();
            }

            public void ClearMessages()
            {
                this.MsgArray.Clear();
            }
                  
            public void AddExceptionMessage(System.Exception Ex)
            {
                this.AddMessage(Ex.Message, Enums.enMsgType.Err, System.DateTime.Now, Assembly.GetCallingAssembly().FullName, MethodInfo.GetCurrentMethod().Name, Ex.StackTrace);
            }

            #endregion

            #region Add

            public int Add(RTNMessage Message)
            {
                MsgArray.Add(Message);
                return MsgArray.Count;
            }
            public int AddMessage(String Message, Enums.enMsgType MessageType)
            {
                RTNMessage MSG = new RTNMessage(Message, MessageType, System.DateTime.Now);
                MsgArray.Add(MSG);
                return MsgArray.Count;
            }
            public int AddMessage(String Message, Enums.enMsgType MessageType, System.DateTime MessageDate)
            {
                RTNMessage MSG = new RTNMessage(Message, MessageType, MessageDate);
                MsgArray.Add(MSG);
                return MsgArray.Count;
            }
            public int AddMessage(String Message, Enums.enMsgType MessageType, System.DateTime MessageDate, String MessageSource, String MessageMethod)
            {
                RTNMessage MSG = new RTNMessage(Message, MessageType, MessageDate, MessageSource, MessageMethod);
                MsgArray.Add(MSG);
                return MsgArray.Count;
            }
            public int AddMessage(string Message, Enums.enMsgType MessageType, System.DateTime MessageDate, String LinkedControlName, Type LinkedControlType)
            {
                RTNMessage MSG = new RTNMessage(Message, MessageType, MessageDate, LinkedControlName, LinkedControlType);
                MsgArray.Add(MSG);
                return MsgArray.Count;
            }
            public int AddMessage(string Message, Enums.enMsgType MessageType, System.DateTime MessageDate, String MessageSource, String MessageMethod, String StackTrace)
            {
                RTNMessage MSG = new RTNMessage(Message, MessageType, MessageDate, MessageSource, MessageMethod, StackTrace);
                MsgArray.Add(MSG);
                return MsgArray.Count;
            }
            public int AddMessage(string Message, Enums.enMsgType MessageType, System.DateTime MessageDate, String MessageSource, String MessageMethod, String StackTrace, String LinkedControlName, Type LinkedControlType)
            {
                RTNMessage MSG = new RTNMessage(Message, MessageType, MessageDate, MessageSource, MessageMethod, StackTrace);
                MsgArray.Add(MSG);
                return MsgArray.Count;
            }

            public int Add(RTNMessages Messages)
            {
                foreach (RTNMessage M in Messages)
                {
                    this.AddMessage(M.Message, M.MessageType, M.MessageDate, M.MessageSource, M.MessageSourceMethod, M.StackTrace, M.LinkedControlName, M.LinkedControlType);
                }
                return MsgArray.Count;
            }

            #endregion

            #region Interface Methods

            public void CopyTo(System.Array array, int index)
            {
                MsgArray.CopyTo(array, index);
            }

            public int Count
            {
                get { return MsgArray.Count; }
            }

            public bool IsSynchronized
            {
                get { return false; }
            }

            public object SyncRoot
            {
                get { return this; }
            }

            public IEnumerator GetEnumerator()
            {
                return MsgArray.GetEnumerator();
            }

            #endregion

            #region Counts

            public int CountError()
            {
                int C = 0;
                foreach (RTNMessage MSG in this.MsgArray)
                {
                    if (MSG.MessageType == Enums.enMsgType.Err)
                    {
                        C += 1;
                    }
                }
                return C;
            }

            public int CountWarning()
            {
                int C = 0;
                foreach (RTNMessage MSG in this.MsgArray)
                {
                    if (MSG.MessageType == Enums.enMsgType.Warn)
                    {
                        C += 1;
                    }
                }
                return C;
            }

            public int CountSuccess()
            {
                int C = 0;
                foreach (RTNMessage MSG in this.MsgArray)
                {
                    if (MSG.MessageType == Enums.enMsgType.OK)
                    {
                        C += 1;
                    }
                }
                return C;
            }

            #endregion

            #region XML

            public string WriteXML()
            {
                string RTN;
                StringWriter SW = new StringWriter();
                XmlTextWriter XW = new XmlTextWriter(SW);
                XmlConvert XC = new XmlConvert();
                System.Text.StringBuilder St = new System.Text.StringBuilder();
                XW.Formatting = Formatting.Indented;
                XW.WriteStartElement("Messages");
                XW.WriteAttributeString("Version", XmlConvert.ToString(1));
                foreach (RTNMessage M in this.MsgArray)
                {
                    XW.WriteStartElement("Message");
                    XW.WriteAttributeString("Message", HttpUtility.HtmlEncode(M.Message));
                    XW.WriteAttributeString("MessageType", XC.ToString());
                    XW.WriteAttributeString("MessageDate", XC.ToString());
                    XW.WriteAttributeString("MessageSource", HttpUtility.HtmlEncode(M.MessageSource));
                    XW.WriteAttributeString("MessageSourceMethod", HttpUtility.HtmlEncode(M.MessageSourceMethod));
                    XW.WriteAttributeString("StackTrace", HttpUtility.HtmlEncode(M.StackTrace));
                    XW.WriteEndElement();
                }
                XW.WriteEndElement();
                XW.Flush();
                XW.Close();
                XW = null;
                SW.Flush();
                SW.Close();
                RTN = SW.ToString();
                SW = null;
                return RTN;
            }

            public bool ReadXML(string XML)
            {
                try
                {
                    XML = System.Web.HttpUtility.HtmlDecode(XML);
                    StringReader SR = new StringReader(XML);
                    XmlDocument XD = new XmlDocument();
                    XD.Load(SR);
                    SR.Close();
                    SR = null;
                    if (XD.GetElementsByTagName("Messages") == null)
                    {
                        return false;
                    }
                    XmlNode XN = XD.GetElementsByTagName("Messages").Item(0);
                    foreach (XmlNode N in XN.ChildNodes)
                    {
                        RTNMessage M = new RTNMessage();
                        if ((N.Attributes["Message"] != null))
                        {
                            M.Message = HttpUtility.HtmlDecode(N.Attributes["Message"].Value);
                        }
                        if ((N.Attributes["MessageType"] != null))
                        {
                            M.MessageType = (Enums.enMsgType)Enum.Parse(typeof(Enums.enMsgType), N.Attributes["MessageType"].Value);
                        }
                        if ((N.Attributes["MessageDate"] != null))
                        {
                            M.MessageDate = System.DateTime.Parse(N.Attributes["MessageDate"].Value);
                        }
                        if ((N.Attributes["MessageSource"] != null))
                        {
                            M.MessageSource = HttpUtility.HtmlDecode(N.Attributes["MessageSource"].Value);
                        }
                        if ((N.Attributes["MessageSourceMethod"] != null))
                        {
                            M.MessageSourceMethod = HttpUtility.HtmlDecode(N.Attributes["MessageSourceMethod"].Value);
                        }
                        if ((N.Attributes["StackTrace"] != null))
                        {
                            M.StackTrace = HttpUtility.HtmlDecode(N.Attributes["StackTrace"].Value);
                        }
                        this.Add(M);
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            #endregion

        } 
}
