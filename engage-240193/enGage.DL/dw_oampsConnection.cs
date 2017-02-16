using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.Linq.Mapping;

namespace enGage.DL
{
    public partial class dw_oampsDataContext : System.Data.Linq.DataContext
    {
        public dw_oampsDataContext() :
            base(global::System.Configuration.ConfigurationManager.ConnectionStrings["dw_oampsConnectionString"].ConnectionString, mappingSource)
        {
            //OnCreated();
        }
    }
}
