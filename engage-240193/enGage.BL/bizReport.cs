using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Data.SqlClient;
using System.Transactions;
using System.Web.UI.WebControls;
using enGage.DL;

namespace enGage.BL
{
    [DataObject]
    public class bizReport
    {
        public Utilities.RTNMessages MSGS;

        public bizReport()
        {
            MSGS = new Utilities.RTNMessages();
        }

        #region Validation

        public bool ValidateParameters(DateTime reportStart, DateTime reportEnd)
        {
            try
            {
                bizMessage biz = new bizMessage();

                if (reportStart > reportEnd)
                {
                    this.MSGS.AddMessage("From - To: " + biz.GetMessageText("DateFromGreaterThanDateTo"), Enums.enMsgType.Err, DateTime.Now, "From", typeof(TextBox));
                    return false;
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

        public List<proc_rpt_DashboardResult> GetDashboardData(DateTime reportStart, 
                                                    DateTime reportEnd, 
                                                    string region,
                                                    string branch,
                                                    string executive,
                                                    int? businessTypeID, 
                                                    int? classificationID, 
                                                    string source, 
                                                    string anzsic,
                                                    string opportunities)
        {
            bizMessage biz = new bizMessage();
            try
            {
                if (businessTypeID == 0) businessTypeID = null;
                if (classificationID == 0) classificationID = null;

                enGageDataContext db = new enGageDataContext();
                db.CommandTimeout = 60;
                return db.proc_rpt_Dashboard(reportStart, reportEnd, region, branch, executive, businessTypeID, classificationID, source, anzsic, opportunities).ToList();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public List<proc_rpt_DashboardTotalsResult> GetDashboardTotalsData(DateTime reportStart,
                                                    DateTime reportEnd,
                                                    string region,
                                                    string branch,
                                                    int? businessTypeID,
                                                    int? classificationID,
                                                    string source,
                                                    string anzsic,
                                                    string opportunities)
        {
            bizMessage biz = new bizMessage();
            try
            {
                if (businessTypeID == 0) businessTypeID = null;
                if (classificationID == 0) classificationID = null;
                enGageDataContext db = new enGageDataContext();
                db.CommandTimeout = 240;
                return db.proc_rpt_DashboardTotals(reportStart, reportEnd, region, branch, businessTypeID, classificationID, source, anzsic, opportunities).ToList();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public List<proc_rpt_DashboardDetailResult> GetDashboardDetailData(DateTime reportStart,
                                                     DateTime reportEnd,
                                                     string executiveID,
                                                     int? businessTypeID,
                                                     int? classificationID,
                                                     string source,
                                                     string anzsic,
                                                     string actionSet,
                                                     string resultSet,
                                                     string opportunities)
        {
            bizMessage biz = new bizMessage();
            try
            {
                if (businessTypeID == 0) businessTypeID = null;
                if (classificationID == 0) classificationID = null;
                if (source == "") source = null;
                if (anzsic == "") anzsic = null;
                enGageDataContext db = new enGageDataContext();
                return db.proc_rpt_DashboardDetail(reportStart, reportEnd, executiveID, businessTypeID, classificationID, source, anzsic, actionSet, resultSet, opportunities).ToList();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        #region North Sydney
        
        public List<proc_rpt_DashboardNorthSydneyResult> GetNorthSydneyReportData(DateTime reportStart, DateTime reportEnd, String teamLeaderName)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                return db.proc_rpt_DashboardNorthSydney(reportStart, reportEnd, teamLeaderName).ToList();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public List<String> GetNorthSydneyTeams()
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                var query = (from t in db._ibais_TeamLists
                             orderby t.Team_Name ascending
                             select t.TeamLeaderName).Distinct();
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

        public List<proc_rpt_TallyboardResult> GetTallyBoardData(
                                                    DateTime reportStart,
                                                    DateTime reportEnd,
                                                    string region,
                                                    string branch,
                                                    string executiveID,
                                                    int? businessTypeID,
                                                    int? classificationID,
                                                    string source,
                                                    string anzsic,
                                                    string opportunities)
        {
            bizMessage biz = new bizMessage();
            try
            {
                if (businessTypeID == 0) businessTypeID = null;
                if (classificationID == 0) classificationID = null;
                enGageDataContext db = new enGageDataContext();
                db.CommandTimeout = 240;
                return db.proc_rpt_Tallyboard(reportStart, reportEnd, region, branch, executiveID, businessTypeID, classificationID, source, anzsic, opportunities).ToList();
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
