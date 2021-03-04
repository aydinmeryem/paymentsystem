using PaymentSystem.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PaymentSystem.Scheduler.Do
{
    public class CreateMonthlyBillJob : IJob
    {
        Add_Bill_JobModel addmodel;
        public CreateMonthlyBillJob()
        {
            addmodel = new Add_Bill_JobModel();
        }
        public Task Execute(IJobExecutionContext context)
        {

            if (context!=null)
            {
                AddConsumerBill();
                AddEnterpriseBill();                
            }
            throw new NotImplementedException();
            

        }

        void AddConsumerBill()
        {
            addmodel = new Add_Bill_JobModel();

            addmodel.GetConsumerData();

            DateTime creation = DateTime.Now;

            DateTime creation_date = creation;
            DateTime due_date = creation.AddMonths(+1);
            string amount = "400";
            bool ispaid = false;
            int cons_id;
            var users = addmodel.ConsumerRawData_;

            

            for (int i = 0; i < users.Count;)
            {
                foreach (var item in users)
                {
                    cons_id = item.cons_id;
                    addmodel.AddNewBillforConsumers(creation_date, due_date, amount, ispaid, cons_id);
                }
                
                i++;
            }
        }


        void AddEnterpriseBill()
        {
            addmodel = new Add_Bill_JobModel();
            addmodel.GetEnterpriseData();

            DateTime creation = DateTime.Today;

            DateTime creation_date = creation;
            DateTime due_date = creation.AddMonths(+1);
            string amount = "600";
            bool ispaid = false;

            var users = addmodel.EnterpriseRawData_;

            for (int i = 0; i < users.Count;)
            {
                foreach (var item in users)
                {
                    int ent_id = item.ent_id;
                    addmodel.AddNewBillforEnterprise(creation_date, due_date, amount, ispaid, ent_id);
                }

                i++;
            }

        }
    }
}