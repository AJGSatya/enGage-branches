using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using Glimpse.Ado.AlternateType;

namespace enGage.DL
{
    public partial class enGageDataContext : System.Data.Linq.DataContext
    {
        public enGageDataContext() :
            base(new GlimpseDbConnection(new SqlConnection(global::System.Configuration.ConfigurationManager.ConnectionStrings["enGageConnectionString"].ConnectionString)), mappingSource)
        {
            OnCreated();
        }
    }
}
