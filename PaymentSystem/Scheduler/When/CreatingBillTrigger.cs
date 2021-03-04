using PaymentSystem.Scheduler.Do;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentSystem.Scheduler.When
{
    public class CreatingBillTrigger
    {
        public static void Start()
        {
            IScheduler tetikci = StdSchedulerFactory.GetDefaultScheduler().Result;
             tetikci.Start();


            if (!tetikci.IsStarted)
                tetikci.Start();

            IJobDetail duty = JobBuilder.Create<CreateMonthlyBillJob>().Build();

            ICronTrigger tetikleyici = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity("CreateMonthlyBillJob", "null")
                .WithCronSchedule("0 5 9 * * ? *")
                .StartAt(DateTime.UtcNow)
                .Build();

             tetikci.ScheduleJob(duty,tetikleyici);
        }
        
    }
}