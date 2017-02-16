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
    public class bizBranchRegion
    {
        public Utilities.RTNMessages MSGS;

        public bizBranchRegion()
        {
            MSGS = new Utilities.RTNMessages();
        }

        public List<String> ListRegions()
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                var query = (from br in db.BranchRegions
                            select br.Region).Distinct();
                return query.ToList();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public List<String> ListBranches()
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                var query = (from br in db.BranchRegions
                             select br.Branch);
                return query.ToList();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }

        public List<String> ListBranchesByRegion(string region)
        {
            bizMessage biz = new bizMessage();
            try
            {
                enGageDataContext db = new enGageDataContext();
                var query = (from br in db.BranchRegions
                             where br.Region == region
                             select br.Branch).Distinct();
                return query.ToList();
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(biz.GetMessageText("Exception"), Enums.enMsgType.Err);
                bizLog.InsertExceptionLog(ex);
                return null;
            }
        }
    }
}
