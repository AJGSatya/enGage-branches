using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace enGage.BL
{
    public class bizActiveDirectory
    {
        //private static DirectoryEntry GetUserDetailsPC(string accountName)
        //{
        //    bizSetting bizS = new bizSetting();
        //    string LDAP = bizS.GetSetting("ActiveDirectory.LDAP");  //what to do with SMI domain ??     TODO

        //    PrincipalContext adPrincipalContext =
        //    new PrincipalContext(
        //        ContextType.Domain,
        //            LDAP.Replace("LDAP://", string.Empty));

        //    UserPrincipal user = new UserPrincipal(adPrincipalContext);
        //    user.Enabled = true;
        //    user.SamAccountName = accountName;

        //    //Test that the wildcard search actually works

        //    PrincipalSearcher pS = new PrincipalSearcher(user);
        //    Principal results = pS.FindOne();
        //    if (results == null) return null;
        //    DirectoryEntry objEntry = (DirectoryEntry)results.GetUnderlyingObject();
        //    return objEntry;
        //}

        //public static DirectoryEntry GetUserDetails(string LDAP, string accountName)
        //{
        //    return GetUserDetailsPC(LDAP, accountName);

        //    /*
        //    DirectoryEntry objEntry = new DirectoryEntry(LDAP);
        //    DirectorySearcher searcher = new DirectorySearcher(objEntry);

        //    searcher.Filter = "(&(objectClass=user)(objectCategory=person)(SAMAccountName=" + accountName + "))";
        //    searcher.PropertiesToLoad.Add("cn");
        //    searcher.PropertiesToLoad.Add("Department");

        //    SearchResult result = null;
        //    result = searcher.FindOne();

        //    if (result == null) return null;

        //    return result.GetDirectoryEntry();
        //     */
        //}

        public static string GetUserFullName(string accountName)
        {
            string returnValue = string.Empty;

            try
            {
                bizSetting bizS = new bizSetting();
                accountName = accountName.Replace(bizS.GetSetting("Security.DomainName"), "");
                accountName = accountName.Replace(bizS.GetSetting("Security.DomainNameSmi"), "");

                //map to the SMI user account name, etc
                //string SmiUserName = bizUser.GetAccountExecutiveIdBySmiUserName(accountName);

                returnValue = bizUser.GetFullNameBySmiUserName(accountName);

                // AD LDS or AD context
                //DirectoryEntry results = GetUserDetailsPC(SmiUserName);
                //if (results == null) return null;

                    //return results.Properties["cn"][0].ToString();
                    //end of code addition

                    /*
                    //DirectoryEntry objEntry = new DirectoryEntry(LDAP);
                    DirectorySearcher searcher = new DirectorySearcher(objEntry);

                    searcher.Filter = "(&(objectClass=user)(objectCategory=person)(sAMAccountName=" + accountName + "))";
                    searcher.PropertiesToLoad.Add("cn");

                    SearchResult result = null;
                    result = searcher.FindOne();

                    if (result == null)
                    {
                        return null;
                    }

                    return result.Properties["cn"][0].ToString();
                     * */
            }
            catch (Exception)
            {
                //return String.Empty;
            }

            return returnValue;

        }

        //public static string GetUserAccountName(string LDAP, string fullName)
        //{

        //    PrincipalContext adPrincipalContext =
        //    new PrincipalContext(
        //        ContextType.Domain,
        //            LDAP.Replace("LDAP://", string.Empty));

        //    UserPrincipal user = new UserPrincipal(adPrincipalContext);
        //    user.Enabled = true;
        //    user.Name = "*" + fullName + "*";

        //    //Test that the wildcard search actually works

        //    PrincipalSearcher pS = new PrincipalSearcher(user);
        //    Principal results = pS.FindOne();
        //    if (results == null) return null;
        //    DirectoryEntry objEntry = (DirectoryEntry)results.GetUnderlyingObject();
        //    return objEntry.Properties["sAMAccountName"][0].ToString();

        //    /*
        //    DirectoryEntry objEntry = new DirectoryEntry(LDAP);
        //    DirectorySearcher searcher = new DirectorySearcher(objEntry);

        //    searcher.Filter = "(&(objectClass=user)(objectCategory=person)(cn=" + fullName + "))";
        //    searcher.PropertiesToLoad.Add("SAMAccountName");

        //    SearchResult result = null;
        //    result = searcher.FindOne();

        //    if (result == null)
        //    {
        //        return null;
        //    }

        //    return result.Properties["sAMAccountName"][0].ToString();
        //     * */
        //}

        public static NameValueCollection ListAccountExecutivesByBranchForDropDown(string branch)
        {
            var bizS = new bizSetting();
            //string LDAP = bizS.GetSetting("ActiveDirectory.LDAP");
            string groupName = bizS.GetSetting("Security.ADExecutiveGroupDisplayName");      //need to have SMI one ??


            if (String.Equals(branch, "Melbourne Commercial and Industry", StringComparison.OrdinalIgnoreCase))
            {
                branch = @"Melbourne Commercial &Industry";
            }

            else if (String.Equals(branch, "Clayton", StringComparison.OrdinalIgnoreCase))
            {
                branch = @"Instrat";
            }

            NameValueCollection nvc = new NameValueCollection();


            /* //Commented to remove hardcoding
            PrincipalContext adPrincipalContext =
            new PrincipalContext(
                ContextType.Domain,
                    // LDAP.Replace("LDAP://", string.Empty), "OU=People,OU=OAMPS User Objects,DC=oamps,DC=com,DC=au");
            */

            PrincipalContext adPrincipalContext = new PrincipalContext(ContextType.Domain,"SMI");
            UserPrincipalEx testuser = new UserPrincipalEx(adPrincipalContext);

            //This is a fix for the hard coding of the AD
            //Searching through AD for the Distinguish Name
            GroupPrincipal qbeGroup = new GroupPrincipal(adPrincipalContext);
            PrincipalSearcher srch = new PrincipalSearcher(qbeGroup);
            var searchPrinciple = new UserPrincipal(adPrincipalContext);
            string groupDn = string.Empty;
            foreach (var found in srch.FindAll())
            {
                GroupPrincipal foundGroup = found as GroupPrincipal;

                if (foundGroup != null)
                {
                    if (foundGroup.Name == groupName)
                    {
                        groupDn = foundGroup.DistinguishedName;
                    }
                }
            }
            groupDn = groupDn != string.Empty ? groupDn : groupName;

            testuser.AdvancedSearchFilter.GroupName(groupDn, MatchType.Equals);
            //Area to change
            testuser.AdvancedSearchFilter.DepartmentName(branch, MatchType.Equals);

            PrincipalSearcher pS = new PrincipalSearcher(testuser);

            DirectorySearcher searcher = (DirectorySearcher)pS.GetUnderlyingSearcher();
            searcher.PropertiesToLoad.Add("DisplayName");
            searcher.PropertiesToLoad.Add("SAMAccountName");

            var results = pS.FindAll();

            foreach (Principal user in results)
                nvc.Add(user.DisplayName, user.SamAccountName);

            return nvc;
        }

        public static NameValueCollection FindAccountExecutive(string name)
        {
            var bizS = new bizSetting();
            string LDAP = bizS.GetSetting("ActiveDirectory.LDAP");
            string groupName = bizS.GetSetting("Security.ADExecutiveGroupDisplayName");

           

            PrincipalContext adPrincipalContext =
            new PrincipalContext(
                ContextType.Domain,
                 LDAP.Replace("LDAP://", string.Empty));

            GroupPrincipal groupP = new GroupPrincipal(adPrincipalContext);

            groupP.Name = groupName;

            //Test that the wildcard search actually works
            PrincipalSearcher pS = new PrincipalSearcher(groupP);
            Principal results = pS.FindOne();
            if (results == null) return null;
            //DirectoryEntry objEntry = (DirectoryEntry)results.GetUnderlyingObject();

            /////////////////////////////////////////////

            NameValueCollection nvc = new NameValueCollection();
            
            
            DirectoryEntry group = (DirectoryEntry)results.GetUnderlyingObject();
             
            object members = group.Invoke("Members", null);
           

            foreach (object member in (IEnumerable)members)
            {
                DirectoryEntry user = new DirectoryEntry(member);
                if (user.Properties["Department"].Value != null)
                {
                    if (user.Properties["DisplayName"].Value.ToString().ToLowerInvariant().Contains(name.ToLowerInvariant()) == true)
                    {
                        nvc.Add(user.Properties["DisplayName"].Value.ToString() + ", " + user.Properties["Department"].Value.ToString(), user.Properties["SAMAccountName"].Value.ToString());
                    }
                }
            }

            return nvc;

            

            //NameValueCollection nvc = new NameValueCollection();
            //bizSetting biz = new bizSetting();

            ////Removing the hard coding of AD I will need to retrieve this value from the database.
            //string adDetails = biz.GetSetting("Security.ADExecutiveDetails");

            //PrincipalContext adPrincipalContext = new PrincipalContext(ContextType.Domain, LDAP.Replace("LDAP://", string.Empty), adDetails);

            //UserPrincipalEx testuser = new UserPrincipalEx(adPrincipalContext);
            //testuser.DisplayName = "*" + name + "*";

            //PrincipalSearcher pS = new PrincipalSearcher(testuser);

            //DirectorySearcher searcher = (DirectorySearcher)pS.GetUnderlyingSearcher();
            //searcher.PropertiesToLoad.Add("DisplayName");
            //searcher.PropertiesToLoad.Add("SAMAccountName");

            //var results = pS.FindAll();

            //foreach (Principal user in results)
            //    nvc.Add(user.DisplayName, user.SamAccountName);

            //return nvc;
        }

        public static String ListAccountExecutivesByRegion(string region)
        {
            var bizS = new bizSetting();
            //string LDAP = bizS.GetSetting("ActiveDirectory.LDAP");
            string groupName = bizS.GetSetting("Security.ADExecutiveGroupDisplayName");

            /////////////////////////////////////////////
            //PrincipalContext adPrincipalContext = new PrincipalContext(ContextType.Domain,LDAP.Replace("LDAP://", string.Empty));
            PrincipalContext adPrincipalContext = new PrincipalContext(ContextType.Domain, "SMI");

            GroupPrincipal group = new GroupPrincipal(adPrincipalContext);
            group.SamAccountName = "*" + groupName + "*";

            //Test that the wildcard search actually works

            PrincipalSearcher pS = new PrincipalSearcher(group);
            Principal results = pS.FindOne();
            if (results == null) return null;
            DirectoryEntry objEntry = (DirectoryEntry)results.GetUnderlyingObject();

            //return objEntry;

            /////////////////////////////////////////////

            /*
           // DirectoryEntry objEntry = new DirectoryEntry(LDAP);
            DirectorySearcher searcher = new DirectorySearcher(objEntry);

            searcher.PropertiesToLoad.Add("member");
            searcher.Filter = "(SAMAccountName=" + groupName + ")";

            SearchResult result = null;
            result = searcher.FindOne();

            if (result == null) return null;

            NameValueCollection nvc = new NameValueCollection();

            DirectoryEntry group = result.GetDirectoryEntry();
            */

            object members = objEntry.Invoke("Members", null);

            String data = "";

            bizBranchRegion biz1 = new bizBranchRegion();
            List<String> branches1 = biz1.ListBranchesByRegion(region);
            foreach (object member in (IEnumerable)members)
            {
                DirectoryEntry user = new DirectoryEntry(member);
                if (user.Properties["Department"].Value != null)
                {
                    foreach (String branch in branches1)
                    {
                        if (user.Properties["Department"].Value.ToString() == branch)
                        {
                            data += "|" + user.Properties["SAMAccountName"].Value.ToString();
                        }
                    }
                }
            }
            if (data != "") data = data.Remove(0, 1);

            return data;
        }

        public static String ListAccountExecutivesByBranch(string branch)
        {
            var bizS = new bizSetting();
            //string LDAP = bizS.GetSetting("ActiveDirectory.LDAP").Replace("LDAP://", string.Empty);
            string groupName = bizS.GetSetting("Security.ADExecutiveGroupDisplayName");

            ////////////////////////////////////////////////

            //PrincipalContext adPrincipalContext = new PrincipalContext(ContextType.Domain, LDAP);
            PrincipalContext adPrincipalContext = new PrincipalContext(ContextType.Domain);

            GroupPrincipal group = new GroupPrincipal(adPrincipalContext);
            group.SamAccountName = "*" + groupName + "*";

            //Test that the wildcard search actually works
            PrincipalSearcher pS = new PrincipalSearcher(group);
            Principal results = pS.FindOne();
            if (results == null) return null;
            DirectoryEntry objEntry = (DirectoryEntry)results.GetUnderlyingObject();
            //return objEntry;

            ////////////////////////////////////////////////
            /*
            // DirectoryEntry objEntry = new DirectoryEntry(LDAP);
            DirectorySearcher searcher = new DirectorySearcher(objEntry);

            searcher.PropertiesToLoad.Add("member");
            searcher.Filter = "(SAMAccountName=" + groupName + ")";

            SearchResult result = null;
            result = searcher.FindOne();

            if (result == null) return null;

            DirectoryEntry group = result.GetDirectoryEntry();
             */
            object members = objEntry.Invoke("Members", null);

            String data = "";

            foreach (object member in (IEnumerable)members)
            {
                DirectoryEntry user = new DirectoryEntry(member);
                if (user.Properties["Department"].Value != null)
                {
                    if (user.Properties["Department"].Value.ToString() == branch)
                    {
                        data += "|" + user.Properties["SAMAccountName"].Value.ToString();
                    }
                }
            }
            if (data != "") data = data.Remove(0, 1);

            return data;
        }
    }


    /// <summary>
    /// //////////////////////////////////////////////////////////////////
    /// </summary>
    /// 
    [DirectoryRdnPrefix("CN")]
    [DirectoryObjectClass("user")]
    public class UserPrincipalEx : UserPrincipal
    {
        // Inplement the constructor using the base class constructor. 
        public UserPrincipalEx(PrincipalContext context)
            : base(context)
        {

        }

        // Implement the constructor with initialization parameters.    
        public UserPrincipalEx(PrincipalContext context,
                             string samAccountName,
                             string password,
                             bool enabled)
            : base(context,
                   samAccountName,
                   password,
                   enabled)
        {
        }

        UserAdvancedFilters searchFilter;

        new public UserAdvancedFilters AdvancedSearchFilter
        {
            get
            {
                if (null == searchFilter)
                    searchFilter = new UserAdvancedFilters(this);

                return searchFilter;
            }
        }

        // Create the Department property.    
        [DirectoryProperty("Department")]
        public string Department
        {
            get
            {
                if (ExtensionGet("Department").Length != 1)
                    return null;

                return (string)ExtensionGet("Department")[0];
            }

            set
            {
                ExtensionSet("Department", value);
            }
        }

        // Create the memberOf property.    
        [DirectoryProperty("memberOf")]
        public string MemberOf
        {
            get
            {
                if (ExtensionGet("memberOf").Length != 1)
                    return null;

                return (string)ExtensionGet("memberOf")[0];
            }

            set
            {
                ExtensionSet("memberOf", value);
            }
        }




        // Implement the overloaded search method FindByIdentity.
        public static new UserPrincipalEx FindByIdentity(PrincipalContext context,
                                                       string identityValue)
        {
            return (UserPrincipalEx)FindByIdentityWithType(context,
                                                         typeof(UserPrincipalEx),
                                                         identityValue);
        }

        // Implement the overloaded search method FindByIdentity. 
        public static new UserPrincipalEx FindByIdentity(PrincipalContext context,
                                                       IdentityType identityType,
                                                       string identityValue)
        {
            return (UserPrincipalEx)FindByIdentityWithType(context,
                                                         typeof(UserPrincipalEx),
                                                         identityType,
                                                         identityValue);
        }



    }



    public class UserAdvancedFilters : AdvancedFilters
    {

        public UserAdvancedFilters(Principal p) : base(p) { }
        public void DepartmentName(string value, MatchType mt)
        {
            this.AdvancedFilterSet("Department", "*" + value + "*", typeof(string), mt);
        }

        public void GroupName(string value, MatchType mt)
        {
            //this.AdvancedFilterSet("memberOf", "CN=" + value + ",OU=OAMPS Security Groups Direct,OU=OAMPS Resources,DC=oamps,DC=com,DC=au", typeof(string), mt);
            this.AdvancedFilterSet("memberOf", value, typeof(string), mt);
        }

    }



    ////////////////////////////////////////////////////////////////////////
}
