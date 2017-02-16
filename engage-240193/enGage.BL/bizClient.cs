using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Configuration;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Data.SqlClient;
using System.Transactions;
using System.Web.UI.WebControls;
using enGage.DL;

namespace enGage.BL
{
    [DataObject]
    public class bizClient
    {
        public Utilities.RTNMessages MSGS;

        public bizClient()
        {
            MSGS = new Utilities.RTNMessages();
        }       

        #region Validation

        public bool ValidateSuburbPostCodeStateCombination(string suburb, string postCode, string stateCode)
        {
            try
            {
                bizMessage biz = new bizMessage();

                enGageDataContext db = new enGageDataContext();
                AUPostCode pc = db.AUPostCodes.SingleOrDefault(pcr => pcr.Suburb == suburb && pcr.PostCode == postCode && pcr.StateCode == stateCode);
                if (pc == null)
                {
                    this.MSGS.AddMessage(biz.GetMessageText("InvalidSuburbPostCodeState"), Enums.enMsgType.Warn);
                    return false;
                }

                this.MSGS.AddMessage(biz.GetMessageText("ValidSuburbPostCodeState"), Enums.enMsgType.OK);
                return true;
            }
            catch (Exception ex)
            {
                bizMessage biz = new bizMessage();
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return false;
            }
        }

        public int GetAddressMaxLength()
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                int maxLength = db.Clients.Max(x => x.Address.Length);
                return maxLength;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return 0;
            }
        }

        public bool ValidateClient(Client c)
        {
            try
            {
                bizMessage biz = new bizMessage();

                if (c.ClientName == string.Empty)
                {
                    this.MSGS.AddMessage("Client Name: " + biz.GetMessageText("EmptyField"),  Enums.enMsgType.Err, DateTime.Now, "ClientName", typeof(TextBox));
                    return false;
                }
                if (c.ClientID == 0)
                {
                    if (c.AccountExecutiveID == "")
                    {
                        this.MSGS.AddMessage("Account Executive: " + biz.GetMessageText("ValueNotSelected"), Enums.enMsgType.Err, DateTime.Now, "AccountExecutive", typeof(DropDownList));
                        return false;
                    }
                }
                if (c.ABNACN != null)
                {
                    if (c.ABNACN.Length > 20)
                    {
                        this.MSGS.AddMessage("ABN/ACN: " + biz.GetMessageText("ABNACN"), Enums.enMsgType.Err, DateTime.Now, "ABNACN", typeof(TextBox));
                        return false;
                    }
                }
                //if (c.PostCode != string.Empty)
                //{
                //    enGageDataContext db = new enGageDataContext();
                //    AUPostCode pc = db.AUPostCodes.SingleOrDefault(pcr => pcr.Suburb == c.Location && pcr.PostCode == c.PostCode && pcr.StateCode == c.StateCode);
                //    if (pc == null)
                //    {
                //        this.MSGS.AddMessage(biz.GetMessageText("InvalidSuburbPostCodeState"), Enums.enMsgType.Warn);
                //    }
                //}

                return true;
            }
            catch (Exception ex)
            {
                bizMessage biz = new bizMessage();
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return false;
            }
        }

        #endregion

        #region Misc

        public List<sp_engage_search_clientResult> FindClientInBA(string clientName, string clientFilter, char? matchFilter, int maxRecords,ref int? records)
        {
            bizMessage biz = new bizMessage();
            try
            {
                dw_oampsDataContext db = new dw_oampsDataContext();
                var query = db.sp_engage_search_client(clientName, clientFilter, matchFilter, maxRecords, ref records);
                return query.ToList();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("iBAISClientSearchError"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }
        
        public List<sp_web_FindClientByFieldResult> FindClientByField(string clientName, string clientFilter, char? matchFilter, string fieldName, int maxRecords, ref int? records)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                var query = db.sp_web_FindClientByField(clientName, clientFilter, matchFilter, fieldName, maxRecords, ref records);
                return query.ToList();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("enGageClientSearchError"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }
        
        public Client GetClient(int clientID)
        {
            bizMessage biz = new bizMessage();
            try 
	        {
                enGageDataContext db = new enGageDataContext();
	            
                DataLoadOptions loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Client>(o => o.Contacts);
                loadOptions.LoadWith<Client>(o => o.Opportunities);
                db.LoadOptions = loadOptions;
                Client c;
                c = db.Clients.SingleOrDefault(cli => cli.ClientID == clientID);
                if (c == null)
                {
                    this.MSGS.AddMessage(biz.GetMessageText("EmptyRecord"), Enums.enMsgType.Err); 
                }
                return c;
	        }
	        catch (Exception ex)
	        {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err); 
                bizLog.InsertExceptionLog(ex);
                return null;
	        }
        }

        public List<sp_engage_get_clientResult> GetClientFromBA(string clientCode)
        {
            bizMessage biz = new bizMessage();
            try
            {
                dw_oampsDataContext db = new dw_oampsDataContext();
                var query = db.sp_engage_get_client(clientCode);
                if (query == null)
                {
                    this.MSGS.AddMessage(biz.GetMessageText("EmptyRecord"), Enums.enMsgType.Err);
                }
                return query.ToList();
           }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public int InsertClient(Client c)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                db.Clients.InsertOnSubmit(c);
                db.SubmitChanges();
                this.MSGS.AddMessage(biz.GetMessageText("InsertSuccess"), Enums.enMsgType.OK);
                return c.ClientID;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return 0;
            }
        }

        public bool UpdateClient(Client c)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Client ec = db.Clients.Single(cli => cli.ClientID == c.ClientID);
                // general
                if (ec.ClientCode != c.ClientCode)
                {
                    ActionLog al = new ActionLog();
                    al.ID = c.ClientID;
                    al.TableName = "Client";
                    al.Action = "Client code changed";
                    al.Detail = "From " + ec.ClientCode + " to " + c.ClientCode;
                    al.ActionDate = DateTime.Now;
                    al.ActionedBy = bizUser.GetCurrentUserName();
                    db.ActionLogs.InsertOnSubmit(al);
                }
                ec.ClientCode = c.ClientCode;
                if (ec.ClientName != c.ClientName)
                {
                    ActionLog al = new ActionLog();
                    al.ID = c.ClientID;
                    al.TableName = "Client";
                    al.Action = "Client name changed";
                    al.Detail = "From " + ec.ClientName + " to " + c.ClientName;
                    al.ActionDate = DateTime.Now;
                    al.ActionedBy = bizUser.GetCurrentUserName();
                    db.ActionLogs.InsertOnSubmit(al);
                }
                ec.ClientName = c.ClientName;
                ec.RegisteredName = c.RegisteredName;
                ec.InsuredName = c.InsuredName;
                ec.ABNACN = c.ABNACN;
                ec.Source = c.Source;
                ec.OfficeFacsimilie = c.OfficeFacsimilie;
                ec.OfficePhone = c.OfficePhone;
                // address
                ec.Address = c.Address;
                ec.Location = c.Location;
                ec.StateCode = c.StateCode;
                ec.PostCode = c.PostCode;
                // industry
                ec.AnzsicCode = c.AnzsicCode;
                ec.AssociationCode = c.AssociationCode;
                ec.AssociationMemberNumber = c.AssociationMemberNumber;
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
        
        public bool SetInactiveField(int clientID, bool value)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Client ec = db.Clients.Single(cli => cli.ClientID == clientID);

                ec.Inactive = value;
                ec.ModifiedBy = bizUser.GetCurrentUserName();
                ec.Modified = DateTime.Now;

                ActionLog al = new ActionLog();
                al.ID = clientID;
                al.TableName = "Client";
                if (value == true) al.Action = "Inactivate";
                else al.Action = "Activate";
                al.Detail = "";
                al.ActionDate = DateTime.Now;
                al.ActionedBy = bizUser.GetCurrentUserName();
                db.ActionLogs.InsertOnSubmit(al);

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

        public bool SetPrimaryContact(int clientID, int contactID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Client ec = db.Clients.Single(cli => cli.ClientID == clientID);

                ec.PrimaryContactID = contactID;
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

        public bool TransferClient(int clientID, string userName)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Client ec = db.Clients.Single(cli => cli.ClientID == clientID);

                ec.AccountExecutiveID = userName;
                ec.Modified = ec.Modified;
                ec.ModifiedBy = ec.ModifiedBy;

                ActionLog al = new ActionLog();
                al.ID = clientID;
                al.TableName = "Client";
                al.Action = "Transfer";
                al.Detail = "From " + bizUser.GetCurrentUserNameWithoutDomain() + " to " + userName;
                al.ActionDate = DateTime.Now;
                al.ActionedBy = bizUser.GetCurrentUserName();
                db.ActionLogs.InsertOnSubmit(al);

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

        #region Lists

        public IQueryable<Association> ListAssociationsByIndustry(string AnzsicCode)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                var query =
                   from a in db.Associations
                   where a.AnzsicCode == AnzsicCode
                   orderby a.AssociationName ascending
                   select a;
                return query.AsQueryable();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public List<sp_web_ListClientOpenOpportunitiesResult> ListClientOpenOpportunities(int clientID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                return db.sp_web_ListClientOpenOpportunities(clientID).ToList();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public List<sp_web_ListClientClosedOpportunitiesResult> ListClientClosedOpportunities(int clientID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                return db.sp_web_ListClientClosedOpportunities(clientID).ToList();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public List<sp_web_ListClientContactsResult> ListClientContacts(int clientID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                return db.sp_web_ListClientContacts(clientID).ToList();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public List<String> FindSources(string text)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                var query = (from c in db.Clients
                             where c.Source.Contains(text)
                             select c.Source).Distinct();
                return query.ToList();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public List<String> FindOpportunities(string text)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                var query = (from c in db.Opportunities
                             where c.OpportunityName.Contains(text)
                             select c.OpportunityName).Distinct();
                return query.ToList();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        #endregion
    }
}
