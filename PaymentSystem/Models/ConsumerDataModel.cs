using PaymentSystem.StaticModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PaymentSystem.Models
{
    public class ConsumerDataModel
    {
        SqlConnection sqlConn { get; set; }
        SqlCommand sqlCom { get; set; }
        SqlDataReader sqlDr { get; set; }
        
        public string tc_id { get; set; }
        public string pass { get; set; }

        public int bill_id { get; set; }
        public DateTime creationdate { get; set; }
        public DateTime duedate { get; set; }
        public string billamount { get; set; }
        public bool ispaid { get; set; }
        public int consumer_id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public bool isactive { get; set; }

        public List<ConsumerDataModel> ConsumerData { get; set; }
        public List<ConsumerDataModel> ConsumerBillRawData { get; set; }
        public List<ConsumerDataModel> ConsumerDetails { get; set; }
        public List<ConsumerDataModel> ConsumerPaidInvoices { get; set; }
        public List<ConsumerDataModel> ConsumerUnpaidInvoices { get; set; }
        public List<ConsumerDataModel> InvoiceDetails { get; set; }
        

        public List<ConsumerDataModel> GetConsumerData()
        {
            using (sqlConn= PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                ConsumerData = new List<ConsumerDataModel>();
                sqlCom = new SqlCommand(@"SELECT [consumer_username],[consumer_tc_id],[consumer_pass],[consumer_isactive] FROM [PaymentSystem_DB].[dbo].[consumers]", sqlConn);
                sqlCom.CommandTimeout = 60 * 5;
                sqlDr = sqlCom.ExecuteReader();
                while (sqlDr.Read())
                {
                    ConsumerData.Add(new ConsumerDataModel()
                    {
                        username=sqlDr.GetString(0),
                        tc_id=sqlDr.GetValue(1).ToString(),
                        pass=sqlDr.GetValue(2).ToString(),
                        isactive=sqlDr.GetBoolean(3)
                    });
                }
            }
            return ConsumerData;
        }

        public List<ConsumerDataModel> GetConsumerBillRawData()
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                ConsumerBillRawData = new List<ConsumerDataModel>();
                sqlCom = new SqlCommand(@"SELECT consumer_bill_id,consumer_bill_creationdate,
                consumer_bill_duedate,consumer_bill_amount,consumer_bill_ispaid,consumer_tc_id 
                FROM [PaymentSystem_DB].[dbo].[consumer_bill] CB 
                LEFT JOIN [PaymentSystem_DB].[dbo].[consumers] C 
                ON CB.consumer_bill_consumer_id=C.consumer_id WHERE consumer_isactive=1", sqlConn);
                sqlCom.CommandTimeout = 60 * 5;
                sqlDr = sqlCom.ExecuteReader();
                while (sqlDr.Read())
                {
                    ConsumerBillRawData.Add(new ConsumerDataModel()
                    {
                        bill_id = sqlDr.GetInt32(0),
                        creationdate = sqlDr.GetDateTime(1),
                        duedate = sqlDr.GetDateTime(2),
                        billamount = sqlDr.GetValue(3).ToString(),
                        ispaid = sqlDr.GetBoolean(4),
                        tc_id=sqlDr.GetString(5)
                    });
                }
            }
            return ConsumerBillRawData;
        }
        

        public List<ConsumerDataModel> GetConsumerPaidInvoices(string tc)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                ConsumerPaidInvoices = new List<ConsumerDataModel>();
                sqlCom = new SqlCommand(@"SELECT consumer_bill_id,consumer_bill_creationdate,
                consumer_bill_duedate,consumer_bill_amount,consumer_bill_ispaid,consumer_tc_id
                FROM [PaymentSystem_DB].[dbo].[consumer_bill] CB 
                LEFT JOIN [PaymentSystem_DB].[dbo].[consumers] C 
                ON CB.consumer_bill_consumer_id=C.consumer_id 
                where C.consumer_tc_id='" + tc + "' AND C.consumer_isactive=1 AND CB.consumer_bill_ispaid=1 AND C.consumer_isactive=1", sqlConn);
                sqlCom.CommandTimeout = 60 * 5;
                sqlDr = sqlCom.ExecuteReader();
                while (sqlDr.Read())
                {
                    ConsumerPaidInvoices.Add(new ConsumerDataModel()
                    {
                        bill_id=sqlDr.GetInt32(0),
                        creationdate = sqlDr.GetDateTime(1),
                        duedate = sqlDr.GetDateTime(2),
                        billamount = sqlDr.GetValue(3).ToString(),
                        ispaid = sqlDr.GetBoolean(4),
                        tc_id=sqlDr.GetString(5)
                    });
                }
            }
            return ConsumerPaidInvoices;
        }

        public List<ConsumerDataModel> GetConsumerUnpaidInvoices(string tc)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                ConsumerUnpaidInvoices = new List<ConsumerDataModel>();
                sqlCom = new SqlCommand(@"SELECT consumer_bill_id,consumer_bill_creationdate,
                consumer_bill_duedate,consumer_bill_amount,consumer_bill_ispaid, consumer_tc_id
                FROM [PaymentSystem_DB].[dbo].[consumer_bill] CB 
                LEFT JOIN [PaymentSystem_DB].[dbo].[consumers] C 
                ON CB.consumer_bill_consumer_id=C.consumer_id 
                where C.consumer_tc_id='" + tc + "' AND C.consumer_isactive=1 AND CB.consumer_bill_ispaid=0 AND C.consumer_isactive=1", sqlConn);
                sqlCom.CommandTimeout = 60 * 5;
                sqlDr = sqlCom.ExecuteReader();
                while (sqlDr.Read())
                {
                    ConsumerUnpaidInvoices.Add(new ConsumerDataModel()
                    {
                        bill_id = sqlDr.GetInt32(0),
                        creationdate = sqlDr.GetDateTime(1),
                        duedate = sqlDr.GetDateTime(2),
                        billamount = sqlDr.GetValue(3).ToString(),
                        ispaid = sqlDr.GetBoolean(4),
                        tc_id = sqlDr.GetString(5)
                    });
                }
            }
            return ConsumerUnpaidInvoices;
        }

        public List<ConsumerDataModel> GetConsumerDetailsData(string tc)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                ConsumerDetails = new List<ConsumerDataModel>();
                sqlCom = new SqlCommand(@"SELECT consumer_id,consumer_username,consumer_name,consumer_surname,consumer_tc_id,consumer_phone,consumer_email FROM [PaymentSystem_DB].[dbo].[consumers] WHERE consumer_tc_id='" + tc + "' AND consumer_isactive=1", sqlConn);
                sqlCom.CommandTimeout = 60 * 5;
                sqlDr = sqlCom.ExecuteReader();
                while (sqlDr.Read())
                {
                    ConsumerDetails.Add(new ConsumerDataModel()
                    {
                        consumer_id = sqlDr.GetInt32(0),
                        username = sqlDr.GetString(1),
                        name = sqlDr.GetString(2),
                        surname = sqlDr.GetString(3),
                        tc_id = sqlDr.GetString(4),
                        phone = sqlDr.GetString(5),
                        email = sqlDr.GetString(6)
                    });
                }
            }
            return ConsumerDetails;
        }

        public List<ConsumerDataModel> GetConsumerInvoiceDetails(string tc)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                InvoiceDetails = new List<ConsumerDataModel>();
                sqlCom = new SqlCommand(@"SELECT consumer_bill_id,consumer_bill_creationdate,
                consumer_bill_duedate,consumer_bill_amount,consumer_bill_ispaid 
                FROM [PaymentSystem_DB].[dbo].[consumer_bill] CB 
                LEFT JOIN [PaymentSystem_DB].[dbo].[consumers] C 
                ON CB.consumer_bill_consumer_id=C.consumer_id 
                where C.consumer_tc_id='" + tc + "' AND C.consumer_isactive=1", sqlConn);
                sqlCom.CommandTimeout = 60 * 5;
                sqlDr = sqlCom.ExecuteReader();
                while (sqlDr.Read())
                {
                    InvoiceDetails.Add(new ConsumerDataModel()
                    {
                        bill_id = sqlDr.GetInt32(0),
                        creationdate = sqlDr.GetDateTime(1),
                        duedate = sqlDr.GetDateTime(2),
                        billamount = sqlDr.GetValue(3).ToString(),
                        ispaid = sqlDr.GetBoolean(4)
                    });
                }
            }
            return InvoiceDetails;
        }

        public void GetConsumerPayInvoice(int id)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                sqlCom = new SqlCommand(@"UPDATE [PaymentSystem_DB].[dbo].[consumer_bill] SET consumer_bill_ispaid=1 WHERE consumer_bill_id='" + id + "'", sqlConn);
                sqlCom.CommandTimeout = 60 * 5;
                sqlCom.ExecuteReader();
                SqlConnection.ClearPool(sqlConn);
                sqlConn.Close();
            }
        }

        public void GetConsumerPayDeposit(string tc)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                sqlCom = new SqlCommand(@"UPDATE [PaymentSystem_DB].[dbo].[consumer_deposit] SET consumer_deposit_ispaid=1 WHERE consumer_deposit_consumer_tc='"+ tc + "'", sqlConn);
                sqlCom.CommandTimeout = 60 * 5;
                sqlCom.ExecuteReader();
                SqlConnection.ClearPool(sqlConn);
                sqlConn.Close();
            }
        }

        public void GetConsumerAccountClose(string tc)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                sqlCom = new SqlCommand(@"UPDATE [PaymentSystem_DB].[dbo].[consumers] SET consumer_isactive=0 WHERE consumer_tc_id='" + tc + "'", sqlConn);
                sqlCom.CommandTimeout = 60 * 5;
                sqlCom.ExecuteReader();
                SqlConnection.ClearPool(sqlConn);
                sqlConn.Close();
            }
        }



    }
}