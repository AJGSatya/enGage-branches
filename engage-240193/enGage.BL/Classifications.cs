using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace enGage.BL
{
    public class ClassificationsRanges
    {
        public ClassificationsRanges()
        {
            MinRange=MaxRange=0;
            PersonalLinesOnly=false;
        }
        public decimal MinRange {get; set;}
        public decimal MaxRange{get; set;}
        public int ID{get; set;}
        public bool PersonalLinesOnly{get; set;}

    }

    public static class Classifications
    {

        //for refrence
        //        ClassificationID	ClassificationName	                    FollowUpDefault
        //        2	                Personal Lines Only	                    6
        //        3	                Micro SME Clients < $1K	                6
        //        4	                Small Commercial Clients $1K - $5K	    6
        //        5	                Medium Commercial Clients $5K - $10K	6
        //        6	                Large Commercial Clients $10K - $20K	6
        //        8	                Corporate Clients $20K - $50K	        6
        //        9	                Corporate Clients $50K - $100K	        12
        //        11	            Corporate Clients $100K - $200K	        12
        //        12	            Corporate Clients $200K+	            12

        public static List<ClassificationsRanges> MappedClassificationsRanges
        {

            get
            {
                return new List<ClassificationsRanges>()
                {
                    new ClassificationsRanges(){ID=1,PersonalLinesOnly=true},
                    new ClassificationsRanges(){ID=2,MaxRange=1 },
                    new ClassificationsRanges(){ID=3,MinRange=1, MaxRange=5 },
                    new ClassificationsRanges(){ID=4,MinRange=5, MaxRange=10 },
                    new ClassificationsRanges(){ID=5,MinRange=10, MaxRange=20 },
                    new ClassificationsRanges(){ID=6,MinRange=20, MaxRange=50 },
                    new ClassificationsRanges(){ID=7,MinRange=50, MaxRange=100 },
                    new ClassificationsRanges(){ID=8,MinRange=100, MaxRange=200 },
                    new ClassificationsRanges(){ID=9,MinRange=200, MaxRange=decimal.MaxValue }    
                };
            }

        }
    }
}
