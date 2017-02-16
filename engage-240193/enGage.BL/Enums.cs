using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace enGage.BL
{
    public class Enums
    {
        public enum OpportunitySteps
        {
            Recognise = 0,
            Qualifying = 1,
            Responding = 2,
            Completing = 3
        }

        public enum ActivityActions
        {
            Recognise,
            Qualify,
            Contact,
            Discover,
            Respond,
            Agree,
            Process
        }

        public enum enMsgType
        {
            OK = 0,
            Warn = 1,
            Err = 2
        }

        public struct enStateCode
        {
            public const string ACT = "ACT";
            public const string NSW = "NSW";
            public const string NT = "NT";
            public const string QLD = "QLD";
            public const string SA = "SA";
            public const string TAS = "TAS";
            public const string VIC = "VIC";
            public const string WA = "WA";
            public const string OS = "OS";
        }

        public enum enUserRole 
        { 
            Executive,
            Branch,
            Region,
            Company,
            Administrator
        };

        public struct enBusinessType
        {
            public const string NewBusiness = "New business";
            public const string ReclaimedBusiness = "Reclaimed business";
            public const string ExistingClients = "New business (Existing clients)";
            public const string QuickQuote = "Quick quote";
            public const string QuickWin = "Quick win";
            public const string QuickCall = "Quick call";
        }

        public enum enEditMode { Insert, Update };
        public enum enSortOrder { Ascending, Descending };
        public enum enYesNo { Unknown, Yes, No };

        public static T NumToEnum<T>(int number)
        {
            return (T)Enum.ToObject(typeof(T), number);
        }

    }
}
