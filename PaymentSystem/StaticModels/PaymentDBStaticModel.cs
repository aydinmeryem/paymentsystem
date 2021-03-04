using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PaymentSystem.StaticModels
{
    public static class PaymentDBStaticModel
    {
        public static SqlConnection ConnectionInfo()
        {
            SqlConnection conn = new SqlConnection(@"server=(localdb)\MSSQLLocalDB; database=ProductCatalog; integrated security=true");
            return conn;
        }
    }
}