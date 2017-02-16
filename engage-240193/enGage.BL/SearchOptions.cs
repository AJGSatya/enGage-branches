using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace enGage.Utilities
{
    public class SearchOptions
    {
        public DateTime DateFrom;
        public DateTime DateTo;
        public String Region;
        public String Branch;
        public String Executive;
        public int Classification;
        public int BusinessType;
        public String Industries;
        public String Sources;
        public String Opportunities;

        public SearchOptions()
        {

        }

        public SearchOptions(DateTime dateFrom, DateTime dateTo, String region, String branch, String executive, int classification, int businessType, String industries, String sources, String opportunities)
        {
            this.DateFrom = dateFrom;
            this.DateTo = dateTo;
            this.Region = region;
            this.Branch = branch;
            this.Executive = executive;
            this.Classification = classification;
            this.BusinessType = businessType;
            this.Industries = industries;
            this.Sources = sources;
            this.Opportunities = opportunities;
        }
    }
}
