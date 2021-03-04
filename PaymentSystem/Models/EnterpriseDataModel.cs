using PaymentSystem.StaticModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PaymentSystem.Models
{
    public class EnterpriseDataModel
    {
        SqlConnection sqlConn { get; set; }
        SqlCommand sqlCom { get; set; }
        SqlDataReader sqlDr { get; set; }

        #region props
        public int enterprise_id { get; set; }
        public string tax_num { get; set; }
        public string pass { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public int bill_id { get; set; }
        public DateTime creationdate { get; set; }
        public DateTime duedate { get; set; }
        public string billamount { get; set; }
        public bool ispaid { get; set; }
        public bool isactive { get; set; }

        public List<EnterpriseDataModel> EnterpriseData { get; set; }
        public List<EnterpriseDataModel> EnterpriseDetails { get; set; }
        public List<EnterpriseDataModel> InvoiceDetails { get; set; }
        public List<EnterpriseDataModel> EnterprsePaidInvoices { get; set; }
        public List<EnterpriseDataModel> EnterpriseUnpaidInvoices { get; set; }
        public List<EnterpriseDataModel> EnterpriseBillRawData { get; set; }
        #endregion




        public List<EnterpriseDataModel> GetEnterpriseData()
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                EnterpriseData = new List<EnterpriseDataModel>();
                sqlCom = new SqlCommand(@"SELECT [enterprise_username],[enterprise_tax_number], [enterprise_pass],[enterprise_isactive] FROM [PaymentSystem_DB].[dbo].[enterprises]", sqlConn);
                sqlCom.CommandTimeout = 60 * 5;
                sqlDr = sqlCom.ExecuteReader();
                while (sqlDr.Read())
                {
                    EnterpriseData.Add(new EnterpriseDataModel()
                    {
                        username=sqlDr.GetString(0),
                        tax_num = sqlDr.GetValue(1).ToString(),
                        pass = sqlDr.GetValue(2).ToString(),
                        isactive=sqlDr.GetBoolean(3)
                    });
                }
            }
            return EnterpriseData;
        }

        public List<EnterpriseDataModel> GetEnterpriseBillRawData()
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                EnterpriseBillRawData = new List<EnterpriseDataModel>();
                sqlCom = new SqlCommand(@"SELECT enterprise_bill_id,enterprise_bill_createdate,enterprise_bill_duedate,
                enterprise_amount,enterprise_ispaid,enterprise_tax_number
                FROM [PaymentSystem_DB].[dbo].[enterprise_bill] EB 
                LEFT JOIN [PaymentSystem_DB].[dbo].[enterprises] E 
                ON EB.enterprise_bill_enterprise_id=E.enterprise_id WHERE E.enterprise_isactive=1", sqlConn);
                sqlCom.CommandTimeout = 60 * 5;
                sqlDr = sqlCom.ExecuteReader();
                while (sqlDr.Read())
                {
                    EnterpriseBillRawData.Add(new EnterpriseDataModel()
                    {
                        bill_id = sqlDr.GetInt32(0),
                        creationdate = sqlDr.GetDateTime(1),
                        duedate = sqlDr.GetDateTime(2),
                        billamount = sqlDr.GetValue(3).ToString(),
                        ispaid = sqlDr.GetBoolean(4),
                        tax_num = sqlDr.GetString(5)
                    });
                }
            }
            return EnterpriseBillRawData;
        }

        public List<EnterpriseDataModel> GetEnterprisePaidInvoices( string tax)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                EnterprsePaidInvoices = new List<EnterpriseDataModel>();
                sqlCom = new SqlCommand(@"SELECT enterprise_bill_id,enterprise_bill_createdate,enterprise_bill_duedate,
                enterprise_amount,enterprise_ispaid,enterprise_tax_number
                FROM [PaymentSystem_DB].[dbo].[enterprise_bill] EB 
                LEFT JOIN [PaymentSystem_DB].[dbo].[enterprises] E 
                ON EB.enterprise_bill_enterprise_id=E.enterprise_id 
                WHERE E.enterprise_tax_number='" + tax + "' AND E.enterprise_isactive=1 AND EB.enterprise_ispaid=1 ", sqlConn);
                sqlCom.CommandTimeout = 60 * 5;
                sqlDr = sqlCom.ExecuteReader();
                while (sqlDr.Read())
                {
                    EnterprsePaidInvoices.Add(new EnterpriseDataModel()
                    {
                        bill_id = sqlDr.GetInt32(0),
                        creationdate = sqlDr.GetDateTime(1),
                        duedate = sqlDr.GetDateTime(2),
                        billamount = sqlDr.GetValue(3).ToString(),
                        ispaid = sqlDr.GetBoolean(4),
                        tax_num=sqlDr.GetString(5)
                    });
                }
            }
            return EnterprsePaidInvoices;
        }

        public List<EnterpriseDataModel> GetEnterpriseUnpaidInvoices(string tax)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                EnterpriseUnpaidInvoices = new List<EnterpriseDataModel>();
                sqlCom = new SqlCommand(@"SELECT enterprise_bill_id,enterprise_bill_createdate,enterprise_bill_duedate,
                enterprise_amount,enterprise_ispaid ,enterprise_tax_number
                FROM [PaymentSystem_DB].[dbo].[enterprise_bill] EB 
                LEFT JOIN [PaymentSystem_DB].[dbo].[enterprises] E 
                ON EB.enterprise_bill_enterprise_id=E.enterprise_id 
                WHERE E.enterprise_tax_number='" + tax + "' AND E.enterprise_isactive=1 AND EB.enterprise_ispaid=0", sqlConn);
                sqlCom.CommandTimeout = 60 * 5;
                sqlDr = sqlCom.ExecuteReader();
                while (sqlDr.Read())
                {
                    EnterpriseUnpaidInvoices.Add(new EnterpriseDataModel()
                    {
                        bill_id = sqlDr.GetInt32(0),
                        creationdate = sqlDr.GetDateTime(1),
                        duedate = sqlDr.GetDateTime(2),
                        billamount = sqlDr.GetValue(3).ToString(),
                        ispaid = sqlDr.GetBoolean(4),
                        tax_num=sqlDr.GetString(5)
                    });
                }
            }
            return EnterpriseUnpaidInvoices;
        }
        

        public void GetEnterprisePayInvoice(int id)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                sqlCom = new SqlCommand(@"UPDATE [PaymentSystem_DB].[dbo].[enterprise_bill] SET enterprise_ispaid=1 WHERE enterprise_bill_id='" + id + "'", sqlConn);
                sqlCom.ExecuteReader();
                SqlConnection.ClearPool(sqlConn);
                sqlConn.Close();
            }
        }
        
        public List<EnterpriseDataModel> GetEnterpriseDetailsData(string tax)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                EnterpriseDetails = new List<EnterpriseDataModel>();
                sqlCom = new SqlCommand(@"SELECT enterprise_id,enterprise_username,enterprise_name,enterprise_tax_number,enterprise_phone,enterprise_email FROM [PaymentSystem_DB].[dbo].[enterprises] WHERE enterprise_tax_number='" + tax + "' AND enterprise_isactive=1", sqlConn);
                sqlDr = sqlCom.ExecuteReader();
                while (sqlDr.Read())
                {
                    EnterpriseDetails.Add(new EnterpriseDataModel()
                    {
                        enterprise_id = sqlDr.GetInt32(0),
                        username = sqlDr.GetString(1),
                        name = sqlDr.GetString(2),
                        tax_num = sqlDr.GetString(3),
                        phone = sqlDr.GetString(4),
                        email = sqlDr.GetString(5)
                    });
                }
            }
            return EnterpriseDetails;
        }

        public List<EnterpriseDataModel> GetEnterpriseInvoiceDetails(string tax)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                InvoiceDetails = new List<EnterpriseDataModel>();
                sqlCom = new SqlCommand(@"SELECT enterprise_bill_id,enterprise_bill_createdate,enterprise_bill_duedate,enterprise_amount,enterprise_ispaid 
                FROM [PaymentSystem_DB].[dbo].[enterprise_bill] EB 
                LEFT JOIN [PaymentSystem_DB].[dbo].[enterprises] E 
                ON EB.enterprise_bill_enterprise_id=E.enterprise_id 
                WHERE E.enterprise_tax_number='" + tax + "' AND E.enterprise_isactive=1", sqlConn);
                sqlDr = sqlCom.ExecuteReader();
                while (sqlDr.Read())
                {
                    InvoiceDetails.Add(new EnterpriseDataModel()
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


        public void GetEnterprisePayDeposit(string tax)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                sqlCom = new SqlCommand(@"UPDATE [PaymentSystem_DB].[dbo].[enterprise_deposit] SET [enterprise_deposit_ispaid]=1 WHERE [enterprise_deposit_enterprise_tax]='" + tax + "'", sqlConn);
                sqlCom.CommandTimeout = 60 * 5;
                sqlCom.ExecuteReader();
                SqlConnection.ClearPool(sqlConn);
                sqlConn.Close();
            }
        }

        public void GetEnterpriseAccountClose(string tax)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();
                sqlCom = new SqlCommand(@"UPDATE [PaymentSystem_DB].[dbo].[enterprises] SET enterprise_isactive=0 WHERE enterprise_tax_number='" + tax + "'", sqlConn);
                sqlCom.CommandTimeout = 60 * 5;
                sqlCom.ExecuteReader();
                SqlConnection.ClearPool(sqlConn);
                sqlConn.Close();
            }
        }
    }
}