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
    public class bizActivity
    {
        public Utilities.RTNMessages MSGS;

        public bizActivity()
        {
            MSGS = new Utilities.RTNMessages();
        }

        #region Validation

        public bool ValidateActivity(Activity a)
        {
            bizMessage biz = new bizMessage();
            try
            {
                if (a.OpportunityStatusID == 0)
                {
                    this.MSGS.AddMessage("Stage of Sales / Aspire Cycle: " + biz.GetMessageText("ValueNotSelected"), Enums.enMsgType.Err, DateTime.Now, "OpportunityStatus", typeof(DropDownList));
                    return false;
                }

                var activityStatus = this.GetStatusByID(a.OpportunityStatusID);
                var txtPrefix = "";
                txtPrefix = (activityStatus.Action == Enums.ActivityActions.Qualify.ToString() || activityStatus.Action == Enums.ActivityActions.Recognise.ToString()) ? "Qualify" : txtPrefix;
                 txtPrefix = (new string[] { Enums.ActivityActions.Contact.ToString(), Enums.ActivityActions.Discover.ToString(), Enums.ActivityActions.Respond.ToString() }.Contains(activityStatus.Action)) ? "Respond" : txtPrefix;
                 txtPrefix = (new string[] { Enums.ActivityActions.Agree.ToString(), Enums.ActivityActions.Process.ToString() }.Contains(activityStatus.Action)) ? "Complete" : txtPrefix;

                if (activityStatus.OutcomeType != "C")
                {
                    if (a.FollowUpDate.HasValue == false)
                    {
                        this.MSGS.AddMessage("Follow-up Date: " + biz.GetMessageText("EmptyField"), Enums.enMsgType.Err, DateTime.Now, txtPrefix+"FollowUpDate", typeof(TextBox));
                        return false;
                    }
                    else
                    {
                        if (a.FollowUpDate.Value < new DateTime(1900,1,1) ||
                            a.FollowUpDate.Value > new DateTime(2079,6,6))
                        {
                            this.MSGS.AddMessage("Follow-up Date: " + biz.GetMessageText("DateOutOfRange"), Enums.enMsgType.Err, DateTime.Now, "FollowUpDate", typeof(TextBox));
                            return false;
                        }
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

        public bool ValidateActivity(Activity a,bool newClient)
        {
            bizMessage biz = new bizMessage();
            try
            {
                if (a.OpportunityStatusID == 0)
                {
                    this.MSGS.AddMessage("Stage of Sales / Aspire Cycle: " + biz.GetMessageText("ValueNotSelected"), Enums.enMsgType.Err, DateTime.Now, "OpportunityStatus", typeof(DropDownList));
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

        public Activity GetActivity(int activityID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                DataLoadOptions loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Activity>(act => act.Opportunity);
                loadOptions.LoadWith<Opportunity>(opp => opp.Client);
                db.LoadOptions = loadOptions;
                Activity a;
                a = db.Activities.SingleOrDefault(ac => ac.ActivityID == activityID);
                if (a == null)
                {
                    this.MSGS.AddMessage(biz.GetMessageText("EmptyRecord"), Enums.enMsgType.Err);
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

        private void SaveExtraMissingControls(Opportunity currentOpportunityToSave,Opportunity extraFieldsOppo, string statusName)
        {
            switch (statusName)
            {
                case "Identified":
                    break;
                case "Qualified-in":
                    break;
                case "Qualified-out":
                    break;
                case "Interested":
                case "Not Interested":
                case "Go-to-Market":
                case "Revisit next year":
                case "Quoted":
                case "Can't Place":
                    if (extraFieldsOppo.Client!=null &&!string.IsNullOrEmpty(extraFieldsOppo.Client.InsuredName))
                    {
                        currentOpportunityToSave.Client.InsuredName = extraFieldsOppo.Client.InsuredName;
                    }

                    if (extraFieldsOppo.NetBrokerageEstimated.HasValue )
                    {
                        // save the estimated Income
                        currentOpportunityToSave.NetBrokerageEstimated = extraFieldsOppo.NetBrokerageEstimated;

                    }
                    break;
                case "Accepted":
                case "Lost":
                case "Can't process":
                case "Processed":
                    if (extraFieldsOppo.Client!=null&&!string.IsNullOrEmpty(extraFieldsOppo.Client.InsuredName))
                    {
                        currentOpportunityToSave.Client.InsuredName = extraFieldsOppo.Client.InsuredName;
                    }

                    if (extraFieldsOppo.NetBrokerageEstimated.HasValue)
                    {
                        // save the estimated Income
                        currentOpportunityToSave.NetBrokerageEstimated = extraFieldsOppo.NetBrokerageEstimated;

                    }
                    if (extraFieldsOppo.NetBrokerageQuoted.HasValue )
                    {
                        // save the quoted Income
                        currentOpportunityToSave.NetBrokerageQuoted = extraFieldsOppo.NetBrokerageQuoted;

                    }

                    if (extraFieldsOppo.NetBrokerageActual.HasValue)
                    {
                        // save the quoted Income
                        currentOpportunityToSave.NetBrokerageActual = extraFieldsOppo.NetBrokerageActual;

                    }
                    break;
                default: // all pending statuses
                    break;
            }

            // check for extra fields to show depending on missing data


        }

        public int InsertActivity(Activity a, Opportunity o, Client c,Opportunity extraFieldsOppo)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                db.Activities.InsertOnSubmit(a);

                Opportunity eo = db.Opportunities.SingleOrDefault(opp => opp.OpportunityID == a.OpportunityID);
                Client ec = db.Clients.SingleOrDefault(cl => cl.ClientID == eo.ClientID);
                Status es = db.Status.SingleOrDefault(st => st.StatusID == a.OpportunityStatusID);
                
                switch (es.StatusName)
                {
                    case "Identified":
                        break;
                    case "Qualified-in":
                        eo.NetBrokerageEstimated = o.NetBrokerageEstimated;
                        // as a special condition, the classification will always change depending on the dollar value
                        eo.ClassificationID = o.ClassificationID;
                        break;
                    case "Qualified-out":
                        eo.DateCompleted = o.DateCompleted;
                        break;
                    case "Interested":
                        eo.OpportunityDue = o.OpportunityDue;
                        eo.IncumbentBroker = o.IncumbentBroker;
                        eo.IncumbentInsurer = o.IncumbentInsurer;
                        eo.ClassificationID = o.ClassificationID;
                        break;
                    case "Not Interested":
                        eo.DateCompleted = o.DateCompleted;
                        break;
                    case "Go-to-Market":
                        break;
                    case "Revisit next year":
                        eo.DateCompleted = o.DateCompleted;
                        break;
                    case "Quoted":
                        eo.NetBrokerageQuoted = o.NetBrokerageQuoted;
                        eo.ClassificationID = o.ClassificationID;
                        break;
                    case "Can't place":
                        eo.DateCompleted = o.DateCompleted;
                        break;
                    case "Accepted":
                        eo.NetBrokerageActual = o.NetBrokerageActual;
                        eo.ClassificationID = o.ClassificationID;
                        break;
                    case "Lost":
                        eo.DateCompleted = o.DateCompleted;
                        break;
                    case "Processed":
                        eo.DateCompleted = o.DateCompleted;
                        eo.MemoNumber = o.MemoNumber;
                        ec.ClientCode = c.ClientCode;
                        break;
                    case "Can't process":
                        eo.DateCompleted = o.DateCompleted;

                        break;
                    default: // all pending statuses
                        break;
                }
                // save extra fields
                SaveExtraMissingControls(eo, extraFieldsOppo, es.StatusName);

                db.SubmitChanges();

                this.MSGS.AddMessage(biz.GetMessageText("InsertSuccess"), Enums.enMsgType.OK);
                return a.ActivityID;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return 0;
            }
        }

        public bool UpdateActivity(Activity a, Opportunity extraFieldsOppo)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Activity ea = db.Activities.SingleOrDefault(act => act.ActivityID == a.ActivityID);

                //general
                ea.OpportunityID = a.OpportunityID;
                ea.OpportunityStatusID = a.OpportunityStatusID;
                ea.ActivityNote = a.ActivityNote;
                ea.FollowUpDate = a.FollowUpDate;
                ea.Inactive = a.Inactive;
                ea.ContactID = a.ContactID;

                // audit
                ea.Modified = a.Modified;
                ea.ModifiedBy = a.ModifiedBy;

                Opportunity eo = db.Opportunities.SingleOrDefault(opp => opp.OpportunityID == a.OpportunityID);
                
                switch (ea.Status.StatusName)
                    {
                        case "Identified":
                            break;
                        case "Qualified-in":
                            if (eo.NetBrokerageEstimated != a.Opportunity.NetBrokerageEstimated)
                            {
                                ActionLog al = new ActionLog();
                                al.ID = a.ActivityID;
                                al.TableName = "Opportunity";
                                al.Action = "Estimated Broking Income changed";
                                al.Detail = "From " + String.Format("{0:C2}", eo.NetBrokerageEstimated) + " to " + String.Format("{0:C2}", a.Opportunity.NetBrokerageEstimated);
                                al.ActionDate = DateTime.Now;
                                al.ActionedBy = bizUser.GetCurrentUserName();
                                db.ActionLogs.InsertOnSubmit(al);
                            }
                            eo.NetBrokerageEstimated = a.Opportunity.NetBrokerageEstimated;
                            eo.ClassificationID = a.Opportunity.ClassificationID;
                            break;
                        case "Qualified-out":
                            eo.DateCompleted = a.Opportunity.DateCompleted;
                            break;
                        case "Interested":
                            if (eo.OpportunityDue != a.Opportunity.OpportunityDue)
                            {
                                ActionLog al = new ActionLog();
                                al.ID = a.ActivityID;
                                al.TableName = "Opportunity";
                                al.Action = "Due date changed";
                                al.Detail = "From " + String.Format("{0:dd-MMM-yy}", eo.OpportunityDue) + " to " + String.Format("{0:dd-MMM-yy}", a.Opportunity.OpportunityDue);
                                al.ActionDate = DateTime.Now;
                                al.ActionedBy = bizUser.GetCurrentUserName();
                                db.ActionLogs.InsertOnSubmit(al);
                            }
                            eo.OpportunityDue = a.Opportunity.OpportunityDue;
                            eo.IncumbentBroker = a.Opportunity.IncumbentBroker;
                            eo.IncumbentInsurer = a.Opportunity.IncumbentInsurer;
                            eo.ClassificationID = a.Opportunity.ClassificationID;
                            break;
                        case "Not Interested":
                            eo.DateCompleted = a.Opportunity.DateCompleted;
                            break;
                        case "Go-to-Market":
                            break;
                        case "Revisit next year":
                            eo.DateCompleted = a.Opportunity.DateCompleted;
                            break;
                        case "Quoted":
                            if (eo.NetBrokerageQuoted != a.Opportunity.NetBrokerageQuoted)
                            {
                                ActionLog al = new ActionLog();
                                al.ID = a.ActivityID;
                                al.TableName = "Opportunity";
                                al.Action = "Broking Income Quoted changed";
                                al.Detail = "From " + String.Format("{0:C2}", eo.NetBrokerageQuoted) + " to " + String.Format("{0:C2}", a.Opportunity.NetBrokerageQuoted);
                                al.ActionDate = DateTime.Now;
                                al.ActionedBy = bizUser.GetCurrentUserName();
                                db.ActionLogs.InsertOnSubmit(al);
                            }
                            eo.NetBrokerageQuoted = a.Opportunity.NetBrokerageQuoted;
                            eo.ClassificationID = a.Opportunity.ClassificationID;
                            break;
                        case "Can't place":
                            eo.DateCompleted = a.Opportunity.DateCompleted;
                            break;
                        case "Accepted":
                            if (eo.NetBrokerageActual != a.Opportunity.NetBrokerageActual)
                            {
                                ActionLog al = new ActionLog();
                                al.ID = a.ActivityID;
                                al.TableName = "Opportunity";
                                al.Action = "Actual Broking Income changed";
                                al.Detail = "From " + String.Format("{0:C2}", eo.NetBrokerageActual) + " to " + String.Format("{0:C2}", a.Opportunity.NetBrokerageActual);
                                al.ActionDate = DateTime.Now;
                                al.ActionedBy = bizUser.GetCurrentUserName();
                                db.ActionLogs.InsertOnSubmit(al);
                            }
                            eo.NetBrokerageActual = a.Opportunity.NetBrokerageActual;
                            eo.ClassificationID = a.Opportunity.ClassificationID;
                            break;
                        case "Lost":
                            eo.DateCompleted = a.Opportunity.DateCompleted;
                            break;
                        case "Processed":
                            eo.DateCompleted = a.Opportunity.DateCompleted;
                            if (eo.MemoNumber != a.Opportunity.MemoNumber)
                            {
                                ActionLog al = new ActionLog();
                                al.ID = a.Opportunity.OpportunityID;
                                al.TableName = "Opportunity";
                                al.Action = "Memo number changed";
                                al.Detail = "From " + String.Format("{0:C2}", eo.MemoNumber) + " to " + String.Format("{0:C2}", a.Opportunity.MemoNumber);
                                al.ActionDate = DateTime.Now;
                                al.ActionedBy = bizUser.GetCurrentUserName();
                                db.ActionLogs.InsertOnSubmit(al);
                            }
                            eo.MemoNumber = a.Opportunity.MemoNumber;
                            if (eo.Client.ClientCode != a.Opportunity.Client.ClientCode)
                            {
                                ActionLog al = new ActionLog();
                                al.ID = a.Opportunity.Client.ClientID;
                                al.TableName = "Client";
                                al.Action = "Client code changed";
                                al.Detail = "From " + String.Format("{0:C2}", eo.Client.ClientCode) + " to " + String.Format("{0:C2}", a.Opportunity.Client.ClientCode);
                                al.ActionDate = DateTime.Now;
                                al.ActionedBy = bizUser.GetCurrentUserName();
                                db.ActionLogs.InsertOnSubmit(al);
                            }
                            eo.Client.ClientCode = a.Opportunity.Client.ClientCode;
                            break;
                        case "Can't process":
                            eo.DateCompleted = a.Opportunity.DateCompleted;
                            break;
                        default: // all pending statuses
                            break;
                    }
                // save extra fields
                SaveExtraMissingControls(eo, extraFieldsOppo, ea.Status.StatusName);

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

        public bool SetToInactive(int activityID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Activity ea = db.Activities.Single(act => act.ActivityID == activityID);

                ea.Inactive = true;
                ea.ModifiedBy = bizUser.GetCurrentUserName();
                ea.Modified = DateTime.Now;

                ActionLog al = new ActionLog();
                al.ID = activityID;
                al.TableName = "Activity";
                al.Action = "Inactivate";
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

        public List<sp_web_ListNextOpportunityStatusesResult> ListNextOpportunityStatuses(int opportunityID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                var query = db.sp_web_ListNextOpportunityStatuses(opportunityID);
                return query.ToList();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public List<sp_web_ListCurrentOpportunityStatusesResult> ListCurrentOpportunityStatuses(int opportunityID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                var query = db.sp_web_ListCurrentOpportunityStatuses(opportunityID);
                return query.ToList();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public bool IsThisLatestActivity(int opportunityID, int activityID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Activity a = db.Activities.Where(act => act.OpportunityID == opportunityID && act.Inactive == false).OrderByDescending(ac => ac.Added).First();
                if (a.ActivityID == activityID)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return false;
            }
        }

        #endregion

        #region Status

        public IEnumerable<Status> GetOppprtunityStepStatuses(enGage.BL.Enums.OpportunitySteps currentSteps)
        {
            
             enGageDataContext db = new enGageDataContext();

             switch (currentSteps)
             {
                 case Enums.OpportunitySteps.Qualifying:
                     {
                         return db.Status.ToList().Where(st => st.Action == "Qualify");
                         break;
                     }

                 case Enums.OpportunitySteps.Responding:
                     {
                         return db.Status.ToList().Where(st => { return new string[] { "Contact", "Discover", "Respond" }.Contains(st.Action); });
                         break;
                     }

                 case Enums.OpportunitySteps.Completing:
                     {
                         return db.Status.ToList().Where(st => { return new string[] { "Agree", "Process" }.Contains(st.Action); });
                         break;
                     }
             }
                
            return null;
             
        }

        public Status GetInitialStatus()
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Status s = db.Status.SingleOrDefault(st => st.PreviousStatusID == null);
                if (s == null)
                {
                    this.MSGS.AddMessage(biz.GetMessageText("EmptyRecord"), Enums.enMsgType.Err);
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

        public List<Status> GetNextStatuses(int statusID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                List<Status> s = db.Status.Where(st => st.PreviousStatusID == statusID).ToList();
                //if (s == null)
                //{
                //    this.MSGS.AddMessage(biz.GetMessageText("EmptyRecord"), Enums.enMsgType.Err);
                //}
                return s;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public Status GetStatusByID(int statusID)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Status s = db.Status.SingleOrDefault(st => st.StatusID == statusID);
                if (s == null)
                {
                    this.MSGS.AddMessage(biz.GetMessageText("EmptyRecord"), Enums.enMsgType.Err);
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

        public Status GetStatusByName(string statusName)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                Status s = db.Status.SingleOrDefault(st => st.StatusName == statusName);
                if (s == null)
                {
                    this.MSGS.AddMessage(biz.GetMessageText("EmptyRecord"), Enums.enMsgType.Err);
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

        public List<sp_web_ListStatusesByOutcomeTypeResult> ListStatusesByOutcomeType(char outcomeType)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                var query = db.sp_web_ListStatusesByOutcomeType(outcomeType);
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

        #endregion

        #region FollowUps

        public void FollowUpsSummary(string accountExecutiveID
                                   , ref int? followUpsCount
                                   , ref int? followUpsOverdue
                                   , ref int? submittedCount
                                   , ref decimal? submittedAmount
                                   , ref int? submittedOverdue)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                db.sp_web_FollowUpsSummary(accountExecutiveID
                                         , ref followUpsCount
                                         , ref followUpsOverdue
                                         , ref submittedCount
                                         , ref submittedAmount
                                         , ref submittedOverdue);
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
            }
        }

        public void FollowUpsMatrix(string accountExecutiveID
                                   , string ActionNames
                                   , bool IsOnlyInPipeline
                                   , DateTime CutOffDate
                                   , ref int? followUpsCount
                                   , ref decimal? followUpsEstimated
                                   , ref decimal? followUpsQuoted
                                   , ref decimal? followUpsActual
                                   )
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                var dbResult=db.sp_web_FollowUpsMatrix(accountExecutiveID
                                         , ActionNames, CutOffDate.ToString("dd/MM/yyyy") , IsOnlyInPipeline).ToList();

                if (dbResult != null && dbResult.Count != 0)
                {
                    var record=dbResult.FirstOrDefault();
                    followUpsCount = record.NoOppo;
                    followUpsEstimated = record.Estimated;
                    followUpsQuoted = record.Quoted;
                    followUpsActual = record.Actual;

                }
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
            }
        }


        public List<sp_web_FollowUpsResult> GetFollowUps(string accountExecutiveID
                                                          , int estimatedIncomeID
                                                          , int businessTypeID
                                                          , int flagged
                                                          , string action
                                                          ,	string FilteredRegion
                                                          , string FilteredBranch
                                                          , bool IsAll
                                                          
            )
        {
            bizMessage biz = new bizMessage();
            try
            {
                //if(String.Equals(FilteredBranch, "Melbourne Commercial and Industry", StringComparison.OrdinalIgnoreCase)){
                //    FilteredBranch = @"Melbourne Commercial &Industry";
                //}
                //else if (String.Equals(FilteredBranch, "Clayton", StringComparison.OrdinalIgnoreCase))
                //{
                //    FilteredBranch = @"Instrat";
                //}

                enGageDataContext db = new enGageDataContext();
                List<sp_web_FollowUpsResult> f = db.sp_web_FollowUps(accountExecutiveID, estimatedIncomeID, businessTypeID, flagged, action, FilteredRegion, FilteredBranch, IsAll).ToList();
                if (f == null)
                {
                    this.MSGS.AddMessage(biz.GetMessageText("EmptyRecord"), Enums.enMsgType.Err);
                }
                return f;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }
        public List<sp_web_SharedFollowUpsResult> GetSharedFollowUps(string accountExecutiveID
                                                         , int estimatedIncomeID
                                                         , int businessTypeID
                                                         , int flagged
                                                         , string action
                                                           , DateTime cutOffDate)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                List<sp_web_SharedFollowUpsResult> f = db.sp_web_SharedFollowUps(accountExecutiveID, estimatedIncomeID, businessTypeID, flagged, action, cutOffDate.ToString("yyyy-M-dd")).ToList();
                if (f == null)
                {
                    this.MSGS.AddMessage(biz.GetMessageText("EmptyRecord"), Enums.enMsgType.Err);
                }
                return f;
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
