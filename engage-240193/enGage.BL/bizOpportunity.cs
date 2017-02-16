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
    public class bizOpportunity
    {
        public Utilities.RTNMessages MSGS;

        public bizOpportunity()
        {
            MSGS = new Utilities.RTNMessages();
        }

        #region Validation

        public bool ValidateOpportunity(Opportunity o)
        {
             try
            {
                bizMessage biz = new bizMessage();

                if (o.OpportunityName == string.Empty)
                {
                    this.MSGS.AddMessage("Opportunity Name: " + biz.GetMessageText("EmptyField"), Enums.enMsgType.Err, DateTime.Now, "OpportunityName", typeof(TextBox));
                    return false;
                }

                if (o.BusinessTypeID == null)
                {
                    this.MSGS.AddMessage("Opportunity Type: " + biz.GetMessageText("EmptyField"), Enums.enMsgType.Err, DateTime.Now, "BusinessType", typeof(DropDownList));
                    return false;
                }

                if (o.Flagged == null)
                {
                    this.MSGS.AddMessage("Flagged: " + biz.GetMessageText("EmptyField"), Enums.enMsgType.Err, DateTime.Now, "Flagged", typeof(DropDownList));
                    return false;
                }

                if (o.OpportunityDue.HasValue == true)
                {
                    if (o.OpportunityDue.Value < new DateTime(1900, 1, 1) ||
                        o.OpportunityDue.Value > new DateTime(2079, 6, 6))
                    {
                        this.MSGS.AddMessage("Renewal Date: " + biz.GetMessageText("DateOutOfRange"), Enums.enMsgType.Err, DateTime.Now, "OpportunityDue", typeof(TextBox));
                        return false;
                    }
                }

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

        public bool ValidateQuickQuoteOpportunity(Opportunity o)
        {
            bizMessage biz = new bizMessage();
            try
            {

                if (o.OpportunityName == string.Empty)
                {
                    this.MSGS.AddMessage("Opportunity Name: " + biz.GetMessageText("EmptyField"), Enums.enMsgType.Err, DateTime.Now, "OpportunityName", typeof(TextBox));
                    return false;
                }

                if (o.BusinessTypeID == null)
                {
                    this.MSGS.AddMessage("Opportunity Type: " + biz.GetMessageText("EmptyField"), Enums.enMsgType.Err, DateTime.Now, "BusinessType", typeof(DropDownList));
                    return false;
                }

                if (o.Flagged == null)
                {
                    this.MSGS.AddMessage("Flagged: " + biz.GetMessageText("EmptyField"), Enums.enMsgType.Err, DateTime.Now, "Flagged", typeof(DropDownList));
                    return false;
                }

                if (o.OpportunityDue.HasValue == true)
                {
                    if (o.OpportunityDue.Value < new DateTime(1900, 1, 1) ||
                        o.OpportunityDue.Value > new DateTime(2079, 6, 6))
                    {
                        this.MSGS.AddMessage("Renewal Date: " + biz.GetMessageText("DateOutOfRange"), Enums.enMsgType.Err, DateTime.Now, "OpportunityDue", typeof(TextBox));
                        return false;
                    }
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

        public bool ValidateQuickWinOpportunity(Opportunity o)
        {
            bizMessage biz = new bizMessage();
            try
            {

                if (o.OpportunityName == string.Empty)
                {
                    this.MSGS.AddMessage("Opportunity Name: " + biz.GetMessageText("EmptyField"), Enums.enMsgType.Err, DateTime.Now, "OpportunityName", typeof(TextBox));
                    return false;
                }

                if (o.BusinessTypeID == null)
                {
                    this.MSGS.AddMessage("Opportunity Type: " + biz.GetMessageText("EmptyField"), Enums.enMsgType.Err, DateTime.Now, "BusinessType", typeof(DropDownList));
                    return false;
                }

                if (o.Flagged == null)
                {
                    this.MSGS.AddMessage("Flagged: " + biz.GetMessageText("EmptyField"), Enums.enMsgType.Err, DateTime.Now, "Flagged", typeof(DropDownList));
                    return false;
                }

                if (o.OpportunityDue.HasValue == true)
                {
                    if (o.OpportunityDue.Value < new DateTime(1900, 1, 1) ||
                        o.OpportunityDue.Value > new DateTime(2079, 6, 6))
                    {
                        this.MSGS.AddMessage("Renewal Date: " + biz.GetMessageText("DateOutOfRange"), Enums.enMsgType.Err, DateTime.Now, "OpportunityDue", typeof(TextBox));
                        return false;
                    }
                }

                if (o.NetBrokerageQuoted == null)
                {
                    this.MSGS.AddMessage("Broking Income Quoted: " + biz.GetMessageText("EmptyField"), Enums.enMsgType.Err, DateTime.Now, "NetBrokerageQuoted", typeof(TextBox));
                    return false;
                }

                if (o.NetBrokerageActual == null)
                {
                    this.MSGS.AddMessage("Expected OAMPS income Actual: " + biz.GetMessageText("EmptyField"), Enums.enMsgType.Err, DateTime.Now, "NetBrokerageActual", typeof(TextBox));
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

        #region CRUD

        public Opportunity GetOpportunity(int opportunityID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                DataLoadOptions loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Opportunity>(op => op.Client);
                loadOptions.LoadWith<Opportunity>(op => op.Activities);
                db.LoadOptions = loadOptions;
                Opportunity o;
                o = db.Opportunities.SingleOrDefault(opp => opp.OpportunityID == opportunityID);
                if (o == null)
                {
                    this.MSGS.AddMessage(biz.GetMessageText("EmptyRecord"), Enums.enMsgType.Err);
                }
                return o;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public int InsertOpportunity(Opportunity o)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                db.Opportunities.InsertOnSubmit(o);
                db.SubmitChanges();
                this.MSGS.AddMessage(biz.GetMessageText("InsertSuccess"), Enums.enMsgType.OK);
                return o.OpportunityID;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return 0;
            }
        }

        public bool UpdateOpportunity(Opportunity o)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Opportunity eo = db.Opportunities.Single(opp => opp.OpportunityID == o.OpportunityID);

                if (eo.OpportunityDue != o.OpportunityDue)
                {
                    ActionLog al = new ActionLog();
                    al.ID = o.OpportunityID;
                    al.TableName = "Opportunity";
                    al.Action = "Due date changed";
                    al.Detail = "From " + String.Format("{0:dd-MMM-yy}", eo.OpportunityDue) + " to " + String.Format("{0:dd-MMM-yy}", o.OpportunityDue);
                    al.ActionDate = DateTime.Now;
                    al.ActionedBy = bizUser.GetCurrentUserName();
                    db.ActionLogs.InsertOnSubmit(al);
                }

                if (eo.NetBrokerageEstimated != o.NetBrokerageEstimated)
                {
                    ActionLog al = new ActionLog();
                    al.ID = o.OpportunityID;
                    al.TableName = "Opportunity";
                    al.Action = "Estimated Broking Income changed";
                    al.Detail = "From " + String.Format("{0:C2}", eo.NetBrokerageEstimated) + " to " + String.Format("{0:C2}", o.NetBrokerageEstimated);
                    al.ActionDate = DateTime.Now;
                    al.ActionedBy = bizUser.GetCurrentUserName();
                    db.ActionLogs.InsertOnSubmit(al);
                }

                if (eo.NetBrokerageQuoted != o.NetBrokerageQuoted)
                {
                    ActionLog al = new ActionLog();
                    al.ID = o.OpportunityID;
                    al.TableName = "Opportunity";
                    al.Action = "Broking Income Quoted changed";
                    al.Detail = "From " + String.Format("{0:C2}", eo.NetBrokerageQuoted) + " to " + String.Format("{0:C2}", o.NetBrokerageQuoted);
                    al.ActionDate = DateTime.Now;
                    al.ActionedBy = bizUser.GetCurrentUserName();
                    db.ActionLogs.InsertOnSubmit(al);
                }

                if (eo.NetBrokerageActual != o.NetBrokerageActual)
                {
                    ActionLog al = new ActionLog();
                    al.ID = o.OpportunityID;
                    al.TableName = "Opportunity";
                    al.Action = "Actual Broking Income changed";
                    al.Detail = "From " + String.Format("{0:C2}", eo.NetBrokerageActual) + " to " + String.Format("{0:C2}", o.NetBrokerageActual);
                    al.ActionDate = DateTime.Now;
                    al.ActionedBy = bizUser.GetCurrentUserName();
                    db.ActionLogs.InsertOnSubmit(al);
                }

                if (eo.MemoNumber != o.MemoNumber)
                {
                    ActionLog al = new ActionLog();
                    al.ID = o.OpportunityID;
                    al.TableName = "Opportunity";
                    al.Action = "Memo number changed";
                    al.Detail = "From " + String.Format("{0:C2}", eo.MemoNumber) + " to " + String.Format("{0:C2}", o.MemoNumber);
                    al.ActionDate = DateTime.Now;
                    al.ActionedBy = bizUser.GetCurrentUserName();
                    db.ActionLogs.InsertOnSubmit(al);
                }

                //general
                eo.OpportunityName = o.OpportunityName;
                eo.OpportunityDue = o.OpportunityDue;
                eo.Flagged = o.Flagged;
                eo.IncumbentBroker = o.IncumbentBroker;
                eo.IncumbentInsurer = o.IncumbentInsurer;
                eo.ClassificationID = o.ClassificationID;
                eo.BusinessTypeID = o.BusinessTypeID;
                eo.ContactID = o.ContactID;
                eo.NetBrokerageEstimated = o.NetBrokerageEstimated;
                eo.NetBrokerageQuoted = o.NetBrokerageQuoted;
                eo.NetBrokerageActual = o.NetBrokerageActual;
                eo.DateCompleted = o.DateCompleted;
                eo.MemoNumber = o.MemoNumber;
                // audit
                eo.Modified = o.Modified;
                eo.ModifiedBy = o.ModifiedBy;

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

        public bool IsOpportunityCompleted(int? opportunityID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                bool? result = false;
                db.sp_web_IsOpportunityCompleted(opportunityID, ref result);
                return (bool)result;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return false;
            }
        }

        public List<sp_web_GetCurrentOpportunityStatusResult> GetCurrentOpportunityStatus(int opportunityID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                List<sp_web_GetCurrentOpportunityStatusResult> s = db.sp_web_GetCurrentOpportunityStatus(opportunityID).ToList();
                if (s.Count() == 0)
                {
                    this.MSGS.AddMessage(biz.GetMessageText("EmptyRecord"), Enums.enMsgType.Err);
                    return null;
                }
                return s;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public DateTime? GetCurrentFollowUpDate(int opportunityID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                DateTime? d = null;
                db.sp_web_GetCurrentFollowUpDate(opportunityID, ref d);
                return d;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public bool SetToInactive(int opportunityID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Opportunity eo = db.Opportunities.Single(op => op.OpportunityID == opportunityID);

                eo.Inactive = true;
                eo.ModifiedBy = bizUser.GetCurrentUserName();
                eo.Modified = DateTime.Now;

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

        public bool SetToActive(int opportunityID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Opportunity eo = db.Opportunities.Single(op => op.OpportunityID == opportunityID);

                eo.Inactive = false;
                eo.ModifiedBy = bizUser.GetCurrentUserName();
                eo.Modified = DateTime.Now;

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

        public bool SetFlag(int opportunityID, bool flag)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Opportunity eo = db.Opportunities.Single(op => op.OpportunityID == opportunityID);

                eo.Flagged = flag;
                eo.ModifiedBy = bizUser.GetCurrentUserName();
                eo.Modified = DateTime.Now;

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

        public List<sp_web_ListOpportunityActiveActivitiesResult> ListOpportunityActiveActivites(int opportunityID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                List<sp_web_ListOpportunityActiveActivitiesResult> a = db.sp_web_ListOpportunityActiveActivities(opportunityID).ToList();
                if (a.Count() == 0)
                {
                    this.MSGS.AddMessage(biz.GetMessageText("EmptyRecord"), Enums.enMsgType.Err);
                    return null;
                }
                return a;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public List<sp_web_ListOpportunityInactiveActivitiesResult> ListOpportunityInactiveActivites(int opportunityID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                List<sp_web_ListOpportunityInactiveActivitiesResult> a = db.sp_web_ListOpportunityInactiveActivities(opportunityID).ToList();
                if (a.Count() == 0)
                {
                    this.MSGS.AddMessage(biz.GetMessageText("EmptyRecord"), Enums.enMsgType.Err);
                    return null;
                }
                return a;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        #endregion

        #region Classification

        public Classification GetClassification(int classificationID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Classification c = db.Classifications.SingleOrDefault(cl => cl.ClassificationID == classificationID);
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

        public List<Classification> ListClassifications()
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                var query = db.Classifications.ToList();
                if (query == null)
                {
                    this.MSGS.AddMessage(biz.GetMessageText("EmptyRecord"), Enums.enMsgType.Err);
                }
                return query;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        #endregion

        #region Business Type

        public List<BusinessType> ListBusinessTypes()
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                var query = db.BusinessTypes.ToList();
                if (query == null)
                {
                    this.MSGS.AddMessage(biz.GetMessageText("EmptyRecord"), Enums.enMsgType.Err);
                }
                return query;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        #endregion

        #region assignQueue

        public void AssignQueueOppportunitiesToExcutive(string assignFromExecutiveID, string assignToExecutiveID, string opportunityIDs)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                db.sp_web_AssignQueue(assignFromExecutiveID,assignToExecutiveID,opportunityIDs);
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
            }
        }
        #endregion

        #region ShareQueue

        public void ShareQueueOppportunitiesWithExcutive(string shareFromExecutiveID, string shareToExecutiveID, string opportunityIDs)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                db.sp_web_ShareQueue(shareFromExecutiveID, shareToExecutiveID, opportunityIDs);
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
            }
        }
        #endregion

    }
}
