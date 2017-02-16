using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Reflection;
using System.Data.Linq;
using System.DirectoryServices;
using enGage.BL;
using enGage.DL;
using System.DirectoryServices.AccountManagement;

namespace enGage.Web
{
    public class Global : System.Web.HttpApplication
    {
        private static string[] GetGroupNames(string userName)
        {
            List<string> result = new List<string>();

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "SMI"))
            {
                using (PrincipalSearchResult<Principal> src = UserPrincipal.FindByIdentity(pc, userName).GetGroups(pc))
                {
                    src.ToList().ForEach(sr => result.Add(sr.SamAccountName));
                }
            }

            return result.ToArray();
        }

        protected void AuthenticateUser()
        {
            if (Session["USER"] != null) return;

            bizSetting biz = new bizSetting();
            var u = bizUser.GetCurrentUserNameWithoutDomain();
            bizUser.enGageUser user = bizUser.GetCurrentUser(u);

            if (user == null)
            {
                Response.Redirect("~/NotAuthorised.aspx", false);
                return;
            }

            //check if it's in one of the correct domains
            string userName = bizUser.GetCurrentUserName().ToUpper();
            if (userName.Contains(biz.GetSetting("Security.DomainName")) ||
                userName.Contains(biz.GetSetting("Security.DomainNameSmi")))
            {
                //ok to proceed
            }
            else
            {
                Response.Redirect("~/NotAuthorised.aspx", false);
                return;
            }

            if (Session["USER"] == null)
            {
                System.Security.Principal.WindowsPrincipal wp = null;
                wp = bizUser.GetCurrentAuthinticatedUserPrincipal();        //(System.Security.Principal.WindowsPrincipal)HttpContext.Current.User;
                string username = wp.Identity.Name;


                var groups = GetGroupNames(username);

                // =======================================================================
                //check user roles/groups
                string ExecutiveGroup = biz.GetSetting("Security.ADExecutiveGroup");
                string ExecutiveGroupSmi = biz.GetSetting("Security.ADExecutiveGroupSmi");

                if(groups.Contains(ExecutiveGroupSmi))
                    user.Role = (int)Enums.enUserRole.Executive;

            


                //try
                //{
                //    if (wp.IsInRole(ExecutiveGroup) || wp.IsInRole(ExecutiveGroupSmi))
                //    {
                //        user.Role = (int)Enums.enUserRole.Executive;
                //    }
                //}
                //catch
                //{
                //    //do nothing
                //}

                // =======================================================================
                string BranchGroup = biz.GetSetting("Security.ADBranchGroup");
                string BranchGroupSmi = biz.GetSetting("Security.ADBranchGroupSmi");

                if (groups.Contains(BranchGroupSmi))
                    user.Role = (int)Enums.enUserRole.Branch;

                //try
                //{
                //    if (wp.IsInRole(BranchGroup) || wp.IsInRole(BranchGroupSmi))
                //    {
                //        user.Role = (int)Enums.enUserRole.Branch;
                //    }
                //}
                //catch
                //{
                //    //do nothing
                //}

                // =======================================================================
                string RegionGroup = biz.GetSetting("Security.ADRegionGroup");
                string RegionGroupSmi = biz.GetSetting("Security.ADRegionGroupSmi");

                if (groups.Contains(RegionGroupSmi))
                    user.Role = (int)Enums.enUserRole.Region;




                //try
                //{
                //    if (wp.IsInRole(RegionGroup) || wp.IsInRole(RegionGroupSmi))
                //    {
                //        user.Role = (int)Enums.enUserRole.Region;
                //    }
                //}
                //catch
                //{
                //    //do nothing
                //}

                // =======================================================================
                string CompanyGroup = biz.GetSetting("Security.ADCompanyGroup");
                string CompanyGroupSmi = biz.GetSetting("Security.ADCompanyGroupSmi");


                if (groups.Contains(CompanyGroupSmi))
                    user.Role = (int)Enums.enUserRole.Company;



                //try
                //{
                //    if (wp.IsInRole(CompanyGroup) || wp.IsInRole(CompanyGroupSmi))
                //    {
                //        user.Role = (int)Enums.enUserRole.Company;
                //    }
                //}
                //catch
                //{
                //    //do nothing
                //}

                // =======================================================================
                string AdminGroup = biz.GetSetting("Security.ADAdminGroup");
                string AdminGroupSmi = biz.GetSetting("Security.ADAdminGroupSmi");

                if (groups.Contains(AdminGroupSmi))
                    user.Role = (int)Enums.enUserRole.Administrator;



                //try
                //{
                //    if (wp.IsInRole(AdminGroup) || wp.IsInRole(AdminGroupSmi))
                //    {
                //        user.Role = (int)Enums.enUserRole.Administrator;
                //    }
                //}
                //catch
                //{
                //    //do nothing
                //}

                if (user.Role == null)
                {
                    Response.Redirect("~/NotAuthorised.aspx", false);
                    return;
                }

                Session.Add("USER", user);
            }
        }


        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            AuthenticateUser();

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}