using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Principal;
using enGage.DL;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace enGage.BL
{
    public class bizUser
    {
        public class enGageUser
        {
            private string _SMIUserName;
            private string _DisplayName;
            private string _UserName;
            private string _Branch;
            private string _Region;
            private int _Role;

            public enGageUser()
            {
            }

            public string DisplayName
            {
                get
                {
                    return this._DisplayName;
                }
                set
                {
                    if ((this._DisplayName != value))
                    {
                        this._DisplayName = value;
                    }
                }
            }

            public string UserName
            {
                get
                {
                    return this._UserName;
                }
                set
                {
                    if ((this._UserName != value))
                    {
                        this._UserName = value;
                    }
                }
            }
            public string SMIUserName
            {
                get
                {
                    return this._SMIUserName;
                }
                set
                {
                    if ((this._SMIUserName != value))
                    {
                        this._SMIUserName = value;
                    }
                }
            }

            public string Branch
            {
                get
                {
                    return this._Branch;
                }
                set
                {
                    if ((this._Branch != value))
                    {
                        this._Branch = value;
                    }
                }
            }

            public string Region
            {
                get
                {
                    return this._Region;
                }
                set
                {
                    if ((this._Region != value))
                    {
                        this._Region = value;
                    }
                }
            }

            public int Role
            {
                get
                {
                    return this._Role;
                }
                set
                {
                    if ((this._Role != value))
                    {
                        this._Role = value;
                    }
                }
            }
        }

        public static string GetCurrentUserName()
        {
            String User = "";
            if ((System.Web.HttpContext.Current != null))
            {
                User = System.Web.HttpContext.Current.User.Identity.Name;
                if (string.IsNullOrEmpty(User))
                    User = WindowsIdentity.GetCurrent().Name;
                return User;
            }
            else
            {
                User = WindowsIdentity.GetCurrent().Name;
                return User;
            }
        }

        public static string GetCurrentUserNameWithoutDomain()
        {
            String User = "";
            if ((System.Web.HttpContext.Current != null))
            {
                User = System.Web.HttpContext.Current.User.Identity.Name;
                if (string.IsNullOrEmpty(User))
                    User = WindowsIdentity.GetCurrent().Name;
                User = User.Remove(0, User.IndexOf("\\") + 1);
                return User;
            }

            else
            {

                User = WindowsIdentity.GetCurrent().Name;
                User = User.Remove(0, User.IndexOf("\\") + 1);
                return User;
            }
        }

        public static WindowsPrincipal GetCurrentAuthinticatedUserPrincipal()
        {
            var user = default(System.Security.Principal.WindowsPrincipal);
            if ((System.Web.HttpContext.Current != null))
            {
                user = (System.Security.Principal.WindowsPrincipal)System.Web.HttpContext.Current.User;
                if (string.IsNullOrEmpty(user.Identity.Name))
                    user = new WindowsPrincipal(WindowsIdentity.GetCurrent());

                return user;
            }

            else
            {

                user = new WindowsPrincipal(WindowsIdentity.GetCurrent());

                return user;
            }
        }

        public static enGageUser GetCurrentUser(string currentLogonWithoutAdPrefix)
        {
            //get the SMI user name
            string SmiUserName = GetAccountExecutiveIdBySmiUserName(currentLogonWithoutAdPrefix);

            System.Security.Principal.WindowsImpersonationContext impersonationContext;
            impersonationContext = ((System.Security.Principal.WindowsIdentity)WindowsIdentity.GetCurrent()).Impersonate();

            //get the adctive directory connection
            bizSetting biz = new bizSetting();
            string LDAP = biz.GetSetting("ActiveDirectory.LDAP").Replace("LDAP://", string.Empty);

            PrincipalContext adPrincipalContext = new PrincipalContext(ContextType.Domain, LDAP, null, null);
            UserPrincipal userP = new UserPrincipal(adPrincipalContext);
            userP.Enabled = true;
            userP.SamAccountName =  currentLogonWithoutAdPrefix;    //updated to use the new SMI user name sourced from SQL

            //Test that the wildcard search actually works
            PrincipalSearcher pS = new PrincipalSearcher(userP);
            Principal results = pS.FindOne();
            if (results == null) return null;
            DirectoryEntry de = (DirectoryEntry)results.GetUnderlyingObject();

            /*
                bizSetting biz = new bizSetting();
                System.DirectoryServices.DirectoryEntry de =
               // bizActiveDirectory.GetUserDetails(biz.GetSetting("ActiveDirectory.LDAP"),
                HttpContext.Current.User.Identity.Name.Replace("OAMPSINS\\", ""));
             */

            if (de == null) return null;

            enGageUser user = new enGageUser();
            user.SMIUserName = currentLogonWithoutAdPrefix; //yes, this is correct, save the current domain user logon
            user.UserName = SmiUserName;    //for mapping over to AD, need to use the 'translated from SQL' logon
            user.DisplayName = de.Properties["cn"].Value.ToString();
            user.Branch = de.Properties["Department"].Value.ToString();
            user.Region = GetRegionForBranch(user.Branch);
            user.Role = 0;

            return user;


            //// FOR TESTING PURPOSES
            //enGageUser user = new enGageUser
            //{
            //    SMIUserName = username,
            //    UserName = GetAccountExecutiveIdBySmiUserName(username),
            //    DisplayName = "Testing Account",
            //    Branch = "Testing Branch",
            //    Region = "Testing Region",
            //    Role = 0
            //};

            // return user;
        }
        public static string GetSMIAccountExecutiveIdBOAMPSUserName(string username)
        {
            string returnValue = string.Empty;

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["enGageConnectionString"].ConnectionString;

                using (SqlConnection cn = new SqlConnection(connString))
                {
                    cn.Open();

                    //call off to sql server.
                    string sql = "SELECT SMIAccountExecutiveID FROM AccountExecutive WHERE AccountExecutiveID  = '{0}'";
                    sql = string.Format(sql, username);

                    SqlCommand cmd = new SqlCommand(sql, cn);

                    //returnValue = "Aadeel.Mills";
                    returnValue = (string)cmd.ExecuteScalar() ?? username;

                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
            }

            return returnValue;
        }

        public static string GetAccountExecutiveIdBySmiUserName(string username)
        {
            string returnValue = string.Empty;

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["enGageConnectionString"].ConnectionString;

                using (SqlConnection cn = new SqlConnection(connString))
                {
                    cn.Open();

                    //call off to sql server.
                    string sql = "SELECT AccountExecutiveID FROM AccountExecutive WHERE SMIAccountExecutiveID = '{0}'";
                    sql = string.Format(sql, username);

                    SqlCommand cmd = new SqlCommand(sql, cn);

                    //returnValue = "Aadeel.Mills";
                    returnValue = (string)cmd.ExecuteScalar() ?? username;

                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
            }

            return returnValue;
        }

        public static string GetFullNameBySmiUserName(string username)
        {
            string returnValueFullName = string.Empty;

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["enGageConnectionString"].ConnectionString;

                using (SqlConnection cn = new SqlConnection(connString))
                {
                    cn.Open();

                    //call off to sql server.
                    string sql = "SELECT FullName FROM AccountExecutive WHERE SMIAccountExecutiveID = '{0}'";
                    sql = string.Format(sql, username);

                    SqlCommand cmd = new SqlCommand(sql, cn);

                    //returnValue = "Aadeel Mills";
                    returnValueFullName = (string)cmd.ExecuteScalar() ?? username;

                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex);
            }

            return returnValueFullName;
        }

        public static enGageUser GetAccountExecutive(string username)
        {
            //bizSetting biz = new bizSetting();
            //string LDAP = biz.GetSetting("ActiveDirectory.LDAP").Replace("LDAP://", string.Empty);

            PrincipalContext adPrincipalContext = new PrincipalContext(ContextType.Domain, "SMI");
            UserPrincipal userP = new UserPrincipal(adPrincipalContext);
            userP.Enabled = true;
            userP.SamAccountName = username;

            //Test that the wildcard search actually works

            PrincipalSearcher pS = new PrincipalSearcher(userP);
            Principal results = pS.FindOne();
            if (results == null) return null;
            DirectoryEntry de = (DirectoryEntry)results.GetUnderlyingObject();

            /*
            bizSetting biz = new bizSetting();
            System.DirectoryServices.DirectoryEntry de =
            //bizActiveDirectory.GetUserDetails(biz.GetSetting("ActiveDirectory.LDAP"),username);
             */

            if (de == null) return null;
            enGageUser user = new enGageUser();
            user.UserName = username;
            user.DisplayName = de.Properties["cn"].Value.ToString();
            user.Branch = de.Properties["Department"].Value.ToString();
            user.Region = GetRegionForBranch(user.Branch);
            user.Role = 0;
            return user;
        }

        public static Dictionary<string, enGageUser> GetUsersAccountExecutives(List<string> usernames)
        {
            bizSetting biz = new bizSetting();
            // List of sample results.
            List<string> loadedUsers = new List<string>();
            Dictionary<string, enGageUser> enGageUsersResults = new Dictionary<string, enGageUser>();

            if (usernames == null || usernames.Count == 0)
                return enGageUsersResults;

            string LDAP = biz.GetSetting("ActiveDirectory.LDAP").Replace("LDAP://", string.Empty);

            using (DirectorySearcher searcher = new DirectorySearcher(LDAP))
            {
                StringBuilder filterStringBuilder = new StringBuilder();

                // Just create a single LDAP query for all user SIDs
                filterStringBuilder.Append("(&(objectClass=user)(|");
                foreach (string userSid in usernames)
                {
                    filterStringBuilder.AppendFormat("({0}={1})", "SamAccountName", userSid);
                }

                filterStringBuilder.Append("))");

                searcher.PageSize = 1000; // Very important to have it here. Otherwise you'll get only 1000 at all. Please refere to DirectorySearcher documentation

                searcher.Filter = filterStringBuilder.ToString();

                // We do not want to go beyond GC
                searcher.ReferralChasing = ReferralChasingOption.None;

                searcher.PropertiesToLoad.AddRange(
                    new[] { "DistinguishedName", "cn", "Department", "SamAccountName" });

                SearchResultCollection results = searcher.FindAll();

                // 1- get all departement branches
                Dictionary<string, string> departementRegions = new Dictionary<string, string>();
                List<string> tempDepartments = new List<string>();
                foreach (SearchResult searchResult in results)
                {
                    var deptKey = searchResult.Properties["Department"][0].ToString();
                    if (!departementRegions.ContainsKey(deptKey))
                        departementRegions.Add(deptKey, GetRegionForBranch(deptKey));

                    // add the result in one go
                    var userKey = searchResult.Properties["SamAccountName"][0].ToString();

                    if (!enGageUsersResults.ContainsKey(userKey))
                        enGageUsersResults.Add(userKey, new enGageUser
                        {
                            DisplayName = searchResult.Properties["cn"][0].ToString(),
                            UserName = userKey,
                            Branch = deptKey,
                            Region = departementRegions[deptKey],
                            Role = 0
                        });
                };



                //foreach (SearchResult searchResult in results)
                //{
                //    string distinguishedName = searchResult.Properties["DistinguishedName"][0].ToString();
                //    loadedUsers.Add(distinguishedName);
                //}
            }



            return enGageUsersResults;

        }

        //public static List<sp_web_FindUsersResult> FindUsers(string name)
        //{
        //    StaffDataContext db = new StaffDataContext();
        //    List<sp_web_FindUsersResult> users;
        //    users = db.sp_web_FindUsers(name).ToList();
        //    return users;
        //}

        private static string GetRegionForBranch(string branch)
        {
            enGageDataContext db = new enGageDataContext();
            BranchRegion br = db.BranchRegions.SingleOrDefault(b => b.Branch == branch);
            if (br == null) return "";
            else return br.Region;
        }
    }
}
