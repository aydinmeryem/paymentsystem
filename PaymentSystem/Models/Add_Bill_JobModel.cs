using PaymentSystem.StaticModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PaymentSystem.Models
{
    public class Add_Bill_JobModel
    {
        SqlConnection sqlConn { get; set; }
        SqlCommand sqlCom { get; set; }
        SqlDataReader sqlDr { get; set; }      


        public List<Add_Bill_JobModel> ConsumerRawData_ { get; set; }
        public List<Add_Bill_JobModel> EnterpriseRawData_ { get; set; }

        public int bill_id { get; set; }
        public DateTime creationdate { get; set; }
        public DateTime duedate { get; set; }
        public string billamount { get; set; }
        public bool ispaid { get; set; }
        public string tc_id { get; set; }
        public string tax_num { get; set; }
        public int cons_id { get; set; }
        public string username { get; private set; }
        public string pass { get; private set; }
        public bool isactive { get; private set; }
        public int ent_id { get; set; }

        public List<Add_Bill_JobModel> GetConsumerData()
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                ConsumerRawData_ = new List<Add_Bill_JobModel>();
                sqlCom = new SqlCommand(@"SELECT [consumer_id],[consumer_username],[consumer_tc_id],[consumer_pass],[consumer_isactive] FROM [PaymentSystem_DB].[dbo].[consumers]", sqlConn);
                sqlCom.CommandTimeout = 60 * 5;
                sqlDr = sqlCom.ExecuteReader();
                while (sqlDr.Read())
                {
                    ConsumerRawData_.Add(new Add_Bill_JobModel()
                    {
                        cons_id=sqlDr.GetInt32(0),
                        username = sqlDr.GetString(1),
                        tc_id = sqlDr.GetValue(2).ToString(),
                        pass = sqlDr.GetValue(3).ToString(),
                        isactive = sqlDr.GetBoolean(4)
                    });
                }
            }
            return ConsumerRawData_;
        }       


        public void AddNewBillforConsumers(DateTime creation_date, DateTime due_date, string amount, bool ispaid, int cons_id)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();

                sqlCom = new SqlCommand(@"INSERT INTO [dbo].[consumer_bill] ([consumer_bill_creationdate] ,[consumer_bill_duedate] ,[consumer_bill_amount] ,[consumer_bill_ispaid] ,[consumer_bill_consumer_id])
                VALUES('" +creation_date+"','"+due_date+"','"+amount+"','"+ispaid+"','"+cons_id+"')", sqlConn);
                sqlCom.ExecuteReader();
                SqlConnection.ClearPool(sqlConn);
                sqlConn.Close();
            }
        }


        public List<Add_Bill_JobModel> GetEnterpriseData()
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                EnterpriseRawData_ = new List<Add_Bill_JobModel>();
                sqlCom = new SqlCommand(@"SELECT [enterprise_id],[enterprise_username],[enterprise_tax_number], [enterprise_pass],[enterprise_isactive] FROM [PaymentSystem_DB].[dbo].[enterprises]", sqlConn);
                sqlCom.CommandTimeout = 60 * 5;
                sqlDr = sqlCom.ExecuteReader();
                while (sqlDr.Read())
                {
                    EnterpriseRawData_.Add(new Add_Bill_JobModel()
                    {
                        ent_id=sqlDr.GetInt32(0),
                        username = sqlDr.GetString(1),
                        tax_num = sqlDr.GetValue(2).ToString(),
                        pass = sqlDr.GetValue(3).ToString(),
                        isactive = sqlDr.GetBoolean(4)
                    });
                }
            }
            return EnterpriseRawData_;
        }



        public void AddNewBillforEnterprise(DateTime creation_date, DateTime due_date, string amount, bool ispaid, int ent_id)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();

                sqlCom = new SqlCommand(@"INSERT INTO [dbo].[enterprise_bill] ([enterprise_bill_createdate],[enterprise_bill_duedate],[enterprise_amount],[enterprise_ispaid],[enterprise_bill_enterprise_id])
                VALUES('" + creation_date + "','" + due_date + "','" + amount + "','" + ispaid + "','" + ent_id + "')", sqlConn);
                sqlCom.ExecuteReader();
                SqlConnection.ClearPool(sqlConn);
                sqlConn.Close();
            }
        }
    }
}