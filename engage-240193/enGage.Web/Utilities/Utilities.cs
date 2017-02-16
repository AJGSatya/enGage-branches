using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using enGage.DL;
using enGage.BL;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

namespace enGage.Web.Helper
{
    public class CalenderUtilities
    {
        public static string EncodeQuotedPrintable(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            StringBuilder builder = new StringBuilder();

            byte[] bytes = Encoding.UTF8.GetBytes(value);
            foreach (byte v in bytes)
            {
                // The following are not required to be encoded:
                // - Tab (ASCII 9)
                // - Space (ASCII 32)
                // - Characters 33 to 126, except for the equal sign (61).

                if ((v == 9) || ((v >= 32) && (v <= 60)) || ((v >= 62) && (v <= 126)))
                {
                    builder.Append(Convert.ToChar(v));
                }
                else
                {
                    builder.Append('=');
                    builder.Append(v.ToString("X2"));
                }
            }

            char lastChar = builder[builder.Length - 1];
            if (char.IsWhiteSpace(lastChar))
            {
                builder.Remove(builder.Length - 1, 1);
                builder.Append('=');
                builder.Append(((int)lastChar).ToString("X2"));
            }

            return builder.ToString();
        }

        public static DateTime CutOffDate 
        {
            get
            {
                try
                {
                    return string.IsNullOrEmpty(System.Configuration.ConfigurationSettings.AppSettings["CutOffDate"]) ? new DateTime(2013, 1, 1) : DateTime.Parse(System.Configuration.ConfigurationSettings.AppSettings["CutOffDate"]);
                }
                    catch
                {
                    return new DateTime(2013, 1, 1);
                }
            }
        }
    }

    public class GridUtilities
    {
        public static string GetActivityPhase(string statusAction)
        {
            try
            {
                var actionEnum=Enum.Parse(typeof (Enums.ActivityActions), statusAction);
                return GetActivityPhaseEnum((Enums.ActivityActions)actionEnum).ToString();
            }
            catch
            {
                return "";
            }
        }

        public static Enums.OpportunitySteps GetActivityPhaseEnum(Enums.ActivityActions statusAction)
        {
            switch (statusAction)
            {
                case Enums.ActivityActions.Recognise:
                    return Enums.OpportunitySteps.Qualifying;
                case Enums.ActivityActions.Qualify:
                    return Enums.OpportunitySteps.Qualifying;
                case Enums.ActivityActions.Contact:
                case Enums.ActivityActions.Discover:
                case Enums.ActivityActions.Respond:
                    return Enums.OpportunitySteps.Responding;
                case Enums.ActivityActions.Agree:
                case Enums.ActivityActions.Process:
                    return Enums.OpportunitySteps.Completing;
            }
            return default(Enums.OpportunitySteps);
        }

        public static Enums.OpportunitySteps GetActivityPhaseEnum(string statusAction)
        {

            try
            {
                Enums.ActivityActions actionEnum = (Enums.ActivityActions)Enum.Parse(typeof(Enums.ActivityActions), statusAction);
                switch (actionEnum)
                {
                    case Enums.ActivityActions.Recognise:
                        return Enums.OpportunitySteps.Qualifying;
                    case Enums.ActivityActions.Qualify:
                        return Enums.OpportunitySteps.Qualifying;
                    case Enums.ActivityActions.Contact:
                    case Enums.ActivityActions.Discover:
                    case Enums.ActivityActions.Respond:
                        return Enums.OpportunitySteps.Responding;
                    case Enums.ActivityActions.Agree:
                    case Enums.ActivityActions.Process:
                        return Enums.OpportunitySteps.Completing;
                }
                return default(Enums.OpportunitySteps);
            }
            catch
            {
                return default(Enums.OpportunitySteps);
            }
        }

        public static string GetOpportunityDollarValue(object netEstimated, object netQuoted, object netActual, string statusAction)
        {
            try
            {
                
                Enums.ActivityActions actionEnum = (Enums.ActivityActions)Enum.Parse(typeof(Enums.ActivityActions), statusAction);

                switch (actionEnum)
                    {
                        case Enums.ActivityActions.Recognise:
                            return "";
                        case Enums.ActivityActions.Qualify:
                            return (netEstimated==null)? "":String.Format("{0:c}", (decimal)netEstimated ) ;
                        case Enums.ActivityActions.Contact:
                        case Enums.ActivityActions.Discover:
                        case Enums.ActivityActions.Respond:
                            return (netQuoted==null)?    "":String.Format("{0:c}", (decimal)netQuoted);
                        case Enums.ActivityActions.Agree:
                        case Enums.ActivityActions.Process:
                            return (netActual==null) ?   "":String.Format("{0:c}", (decimal)netActual);
                    }
                
                return "";
            }
            catch
            {
                return "";
            }
        }
        
        public static string GetAddedOrModifiedUser(object AddedBy,object ModifiedBy)
        {
           // bizActiveDirectory.GetUserFullName(biz.GetSetting("ActiveDirectory.LDAP"), bizUser.GetCurrentUserName().ToString().Replace(biz.GetSetting("Security.DomainName"), ""))

            bizSetting biz = new bizSetting();

            if (ModifiedBy != null)
                return ((string)ModifiedBy).Replace(biz.GetSetting("Security.DomainName"), "").Replace(biz.GetSetting("Security.DomainNameSmi"), "");

            if (AddedBy != null)
                return ((string)AddedBy).Replace(biz.GetSetting("Security.DomainName"), "").Replace(biz.GetSetting("Security.DomainNameSmi"), "");

            return "";
        }

        public static string GetAddedOrModifiedDate(object AddedDate, object ModifiedDate)
        {
            if (ModifiedDate != null)
                return ((DateTime)ModifiedDate).ToString("dd-MMM-yy hh:mm tt");

            if (AddedDate != null)
                return ((DateTime)AddedDate).ToString("dd-MMM-yy hh:mm tt"); ;

            return "";
        }
        
        public static void SetUserAllowedOpportunities(bizUser.enGageUser user, ref Boolean isRegion, ref Boolean isBranch, ref Boolean isAll)
        {
            isRegion = isBranch = isAll = false;

            switch (user.Role)
            {
                case (int)Enums.enUserRole.Executive:

                    break;
                case (int)Enums.enUserRole.Branch:
                    isBranch = true;
                    break;
                case (int)Enums.enUserRole.Region:
                    isRegion = true;
                    break;
                case (int)Enums.enUserRole.Company:
                case (int)Enums.enUserRole.Administrator:
                    isAll = true;
                    break;
            }
        }
    }

    public class FilterGridsUtilities
    {
        #region regions and excutives
        public static String GetExecutives(bizUser.enGageUser user, DropDownList ddlExecutive, DropDownList ddlRegion, DropDownList ddlBranch, bool DisableExecutives)
        {
            string execs = "";
           
            switch (user.Role)
            {
                case (int)Enums.enUserRole.Executive:
                    execs = ddlExecutive.SelectedValue;
                    break;
                case (int)Enums.enUserRole.Branch:
                    if (ddlRegion.SelectedIndex == 0 && ddlBranch.SelectedIndex == 0 && ddlExecutive.SelectedIndex == 0)
                    {
                        if (DisableExecutives == true)
                        {
                            using (Timeline.Capture("bizActiveDirectory.ListAccountExecutivesByRegion", "AD"))
                            {
                                bizSetting bizS = new bizSetting();
                                execs = bizActiveDirectory.ListAccountExecutivesByRegion(ddlRegion.SelectedValue);
                            }
                        }
                        else
                        {
                            execs = BuildExecsFromDropDownList(ddlExecutive);
                        }
                    }
                    if (ddlRegion.SelectedIndex == 0 && ddlBranch.SelectedIndex == 0 && ddlExecutive.SelectedIndex > 0)
                    {
                        execs = ddlExecutive.SelectedValue;
                    }
                    break;
                case (int)Enums.enUserRole.Region:
                    if (ddlRegion.SelectedIndex == 0 && ddlBranch.SelectedIndex == 0 && ddlExecutive.SelectedIndex == 0)
                    {
                        using (Timeline.Capture("bizActiveDirectory.ListAccountExecutivesByRegion", "AD"))
                        {
                            bizSetting bizS = new bizSetting();
                            execs = bizActiveDirectory.ListAccountExecutivesByRegion(ddlRegion.SelectedValue);
                        }
                    }
                    if (ddlRegion.SelectedIndex == 0 && ddlBranch.SelectedIndex > 0 && ddlExecutive.SelectedIndex == 0)
                    {
                        using (Timeline.Capture("bizActiveDirectory.ListAccountExecutivesByBranch", "AD"))
                        {
                            bizSetting bizS = new bizSetting();
                            execs = bizActiveDirectory.ListAccountExecutivesByBranch(ddlBranch.SelectedValue);
                        }
                    }
                    if (ddlRegion.SelectedIndex == 0 && ddlBranch.SelectedIndex > 0 && ddlExecutive.SelectedIndex > 0)
                    {
                        execs = ddlExecutive.SelectedValue;
                    }
                    break;
                case (int)Enums.enUserRole.Company:
                case (int)Enums.enUserRole.Administrator:
                    if (ddlRegion.SelectedIndex == 0 && ddlBranch.SelectedIndex == 0 && ddlExecutive.SelectedIndex == 0)
                    {
                        execs = "(All)";
                    }
                    if (ddlRegion.SelectedIndex > 0 && ddlBranch.SelectedIndex == 0 && ddlExecutive.SelectedIndex == 0)
                    {
                        using (Timeline.Capture("bizActiveDirectory.ListAccountExecutivesByRegion", "AD"))
                        {
                            bizSetting bizS = new bizSetting();
                            execs = bizActiveDirectory.ListAccountExecutivesByRegion(ddlRegion.SelectedValue);
                        }
                    }
                    if (ddlRegion.SelectedIndex > 0 && ddlBranch.SelectedIndex > 0 && ddlExecutive.SelectedIndex == 0)
                    {
                        execs = BuildExecsFromDropDownList(ddlExecutive);
                    }
                    if (ddlRegion.SelectedIndex > 0 && ddlBranch.SelectedIndex > 0 && ddlExecutive.SelectedIndex > 0)
                    {
                        execs = ddlExecutive.SelectedValue;
                    }
                    break;
            }
            return execs;
        }

        public static String GetExecutiveName(String executive, DropDownList ddlExecutive)
        {
            ListItem li;
            li = ddlExecutive.Items.FindByValue(executive);
            if (li == null) return "";
            else return li.Text;
        }

        public static void PopulateRegionsBranchesAndExecutives(bizUser.enGageUser user, DropDownList ddlRegion, DropDownList ddlBranch, DropDownList ddlExecutive, bool DisableExecutives, bool overrideDefaultRegion)
        {
            bizBranchRegion biz = new bizBranchRegion();

            ddlRegion.Items.Clear();
            ddlBranch.Items.Clear();
            ddlExecutive.Items.Clear();

            switch (user.Role)
            {
                case (int)Enums.enUserRole.Executive:
                    ddlRegion.Items.Add(new ListItem(user.Region, user.Region));
                    ddlBranch.Items.Add(new ListItem(user.Branch, user.Branch));
                    if (DisableExecutives == false)
                    {
                        ddlExecutive.Items.Add(new ListItem(user.DisplayName, user.UserName));
                    }
                    else
                    {
                        ddlExecutive.Items.Add(new ListItem("Executives (All)", "(All)"));
                    }
                    break;
                case (int)Enums.enUserRole.Branch:
                    ddlRegion.Items.Add(new ListItem(user.Region, user.Region));
                    ddlBranch.Items.Add(new ListItem(user.Branch, user.Branch));
                    if (DisableExecutives == false)
                    {
                        using (Timeline.Capture("bizActiveDirectory.ListAccountExecutivesByBranchForDropDown", "AD"))
                        {
                            bizSetting bizS = new bizSetting();
                            NameValueCollection execs = bizActiveDirectory.ListAccountExecutivesByBranchForDropDown(ddlBranch.SelectedValue);
                            if (execs == null) return;
                            foreach (String exec in execs)
                            {
                                ddlExecutive.Items.Add(new ListItem(exec, execs[exec]));
                            }
                            SortDropDownList(ref ddlExecutive);
                        }
                    }

                    ddlExecutive.Items.Insert(0, new ListItem("Executives (All)", "(All)"));
                    break;
                case (int)Enums.enUserRole.Region:
                    ddlRegion.Items.Add(new ListItem(user.Region, user.Region));
                    List<String> bs = biz.ListBranchesByRegion(ddlRegion.SelectedValue);
                    if (bs == null) return;
                    ddlBranch.Items.Add(new ListItem("Branches (All)", "(All)"));
                    foreach (String b in bs)
                    {
                        ddlBranch.Items.Add(new ListItem(b, b));
                    }
                    ddlExecutive.Items.Add(new ListItem("Executives (All)", "(All)"));
                    break;
                case (int)Enums.enUserRole.Company:
                case (int)Enums.enUserRole.Administrator:
                    List<String> rs = biz.ListRegions();
                    if (rs == null) return;
                    ddlRegion.Items.Add(new ListItem("Regions (All)", "(All)"));
                    foreach (String r in rs)
                    {
                        ddlRegion.Items.Add(new ListItem(r, r));
                    }
                    ddlBranch.Items.Add(new ListItem("Branches (All)", "(All)"));
                    ddlExecutive.Items.Add(new ListItem("Executives (All)", "(All)"));

                    if (overrideDefaultRegion)
                    {
                        ddlRegion.SelectedValue = user.Region;
                        ddlRegion.Items.RemoveAt(0);
                    }
                   
                    break;
            }
        }

       public static void PopulateBranches(bizUser.enGageUser user, DropDownList ddlRegion, DropDownList ddlBranch)
        {
            switch (user.Role)
            {
                case (int)Enums.enUserRole.Executive:
                case (int)Enums.enUserRole.Branch:
                    break;
                case (int)Enums.enUserRole.Region:
                case (int)Enums.enUserRole.Company:
                case (int)Enums.enUserRole.Administrator:
                    bizBranchRegion biz = new bizBranchRegion();
                    List<String> bs = biz.ListBranchesByRegion(ddlRegion.SelectedValue);
                    if (bs == null) return;
                    ddlBranch.Items.Clear();
                    ddlBranch.Items.Add(new ListItem("Branches (All)", "(All)"));
                    foreach (String b in bs)
                    {
                        ddlBranch.Items.Add(new ListItem(b, b));
                    }
                    break;
            }
        }

       public static void PopulateExecutives(bizUser.enGageUser user,DropDownList ddlBranch,DropDownList ddlExecutive,  bool DisableExecutives)
        {
            if (DisableExecutives == true) return;

            if (ddlBranch.SelectedIndex == 0 &&
                (user.Role != (int)Enums.enUserRole.Executive)
                &&(user.Role != (int)Enums.enUserRole.Branch)
                
                )
            {
                ddlExecutive.Items.Clear();
                ddlExecutive.Items.Insert(0, new ListItem("Executives (All)", "(All)"));
                return;
            }

            switch (user.Role)
            {
                case (int)Enums.enUserRole.Executive:
                    break;
                case (int)Enums.enUserRole.Branch:
                case (int)Enums.enUserRole.Region:
                case (int)Enums.enUserRole.Company:
                case (int)Enums.enUserRole.Administrator:
                {
                    using (Timeline.Capture("bizActiveDirectory.ListAccountExecutivesByBranchForDropDown", "AD"))
                    {
                        bizSetting bizS = new bizSetting();
                        NameValueCollection execs = bizActiveDirectory.ListAccountExecutivesByBranchForDropDown(ddlBranch.SelectedValue);
                        if (execs == null) return;
                        ddlExecutive.Items.Clear();
                        foreach (String exec in execs)
                        {
                            ddlExecutive.Items.Add(new ListItem(exec, execs[exec]));
                        }
                        SortDropDownList(ref ddlExecutive);
                        ddlExecutive.Items.Insert(0, new ListItem("Executives (All)", "(All)"));
                        break;
                    }
                }
            }
        }

       public static void SortDropDownList(ref DropDownList ddl)
        {
            List<ListItem> listCopy = new List<ListItem>();
            foreach (ListItem item in ddl.Items)
                listCopy.Add(item);
            ddl.Items.Clear();
            foreach (ListItem item in listCopy.OrderBy(item => item.Text))
                ddl.Items.Add(item);
        }

       public static string BuildExecsFromDropDownList(DropDownList ddlExecutive)
        {
            string execs = "";
            for (int i = 1; i < ddlExecutive.Items.Count; i++)
            {
                execs += "|" + ddlExecutive.Items[i].Value;
            }
            if (execs != "") execs = execs.Remove(0, 1);
            return execs;
        }

        #endregion
    }

    public static class ControlExtensions
    {

        public static Control FindControlRecursive(this Control root, string controlID)
        {
            if (root == null) return null;

            //try to find the control at the current level
            Control ctrl = root.FindControl(controlID);
            if (ctrl == null)
            {

                foreach (Control c in root.Controls)
                {
                    ctrl = FindControlRecursive(c, controlID);
                    if (ctrl != null) break;

                }
            }

            return ctrl;

        }
    }
}
