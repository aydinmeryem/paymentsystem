using PaymentSystem.StaticModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PaymentSystem.Models
{
    public class AdminDataModel
    {
        SqlConnection sqlConn { get; set; }
        SqlCommand sqlCom { get; set; }
        SqlDataReader sqlDr { get; set; }

        public int admin_id { get; set; }
        public string admin_name { get; set; }
        public string admin_surname { get; set; }
        public string admin_user {get; set; }
        public string admin_pass { get; set; }

        public List<AdminDataModel> AdminData { get; set; }

        public List<AdminDataModel> GetAdminData()
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                AdminData = new List<AdminDataModel>();
                sqlCom = new SqlCommand(@"select * from [PaymentSystem_DB].[dbo].[admins]", sqlConn);
                sqlDr = sqlCom.ExecuteReader();
                while (sqlDr.Read())
                {
                    AdminData.Add(new AdminDataModel()
                    {
                        admin_id = sqlDr.GetInt32(0),
                        admin_name = sqlDr.GetString(1),
                        admin_surname = sqlDr.GetString(2),
                        admin_user = sqlDr.GetString(3),
                        admin_pass=sqlDr.GetString(4)
                    });
                }
            }
            return AdminData;
        }
    }
}