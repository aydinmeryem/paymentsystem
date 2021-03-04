using PaymentSystem.StaticModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PaymentSystem.Models
{
    public class CreateDeleteSubscriberModel
    {
        SqlConnection sqlConn { get; set; }
        SqlCommand sqlCom { get; set; }
        SqlDataReader sqlDr { get; set; }

        public void AddNewConsumer(string username1, string conname, string surname, string tc,string conphone,string conemail,string conpass, bool conisactive)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();

                sqlCom = new SqlCommand(@"INSERT INTO [PaymentSystem_DB].[dbo].[consumers](consumer_username,consumer_name,
                consumer_surname,consumer_tc_id,consumer_phone,consumer_email,consumer_pass,consumer_isactive)
                VALUES('"+ username1 + "', '"+ conname + "', '"+surname+"', '"+tc+"', '"+ conphone + "', '"+ conemail + "', '"+ conpass + "', '"+ conisactive + "')",sqlConn);
                sqlCom.ExecuteReader();
                SqlConnection.ClearPool(sqlConn);
                sqlConn.Close();
            }
        }

        public void AddNewConsumerInDeposit(string tc)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();

                sqlCom = new SqlCommand(@"  INSERT INTO [PaymentSystem_DB].[dbo].[consumer_deposit](consumer_deposit_amount,consumer_deposit_ispaid,consumer_deposit_consumer_tc,consumer_deposit_paid_date)
                VALUES('400','0','" + tc + "',null)", sqlConn);
                sqlCom.ExecuteReader();
                SqlConnection.ClearPool(sqlConn);
                sqlConn.Close();
            }
        }

        public void AddNewEnterprise(string username2, string entname, string tax, string entphone, string entemail, string entpass, bool entisactive)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();

                sqlCom = new SqlCommand(@"INSERT INTO [PaymentSystem_DB].[dbo].[enterprises]
                (enterprise_username,enterprise_name,enterprise_tax_number,enterprise_phone,enterprise_email,enterprise_pass,enterprise_isactive)
                VALUES('"+username2+"','"+ entname + "','"+tax+"','"+entphone+"','"+entemail+"','"+entpass+"','"+entisactive+"')", sqlConn);
                sqlCom.ExecuteReader();
                SqlConnection.ClearPool(sqlConn);
                sqlConn.Close();
            }
        }


        public void AddNewEnterpriseInDeposit(string tax)
        {
            using (sqlConn = PaymentDBStaticModel.ConnectionInfo())
            {
                sqlConn.Open();

                sqlCom = new SqlCommand(@"INSERT INTO [PaymentSystem_DB].[dbo].[enterprise_deposit](enterprise_deposit_amount,enterprise_deposit_ispaid,enterprise_deposit_enterprise_tax,enterprise_deposit_paid_date)
                VALUES('600','0','"+tax+"',null)", sqlConn);
                sqlCom.ExecuteReader();
                SqlConnection.ClearPool(sqlConn);
                sqlConn.Close();
            }
        }
    }
}