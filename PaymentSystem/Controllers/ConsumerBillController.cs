using PaymentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace PaymentSystem.Controllers
{
    public class ConsumerBillController : Controller
    {
        // GET: ConsumerBill
        [HttpGet]
        //[OutputCache(Duration = 180, VaryByParam = "*", Location = System.Web.UI.OutputCacheLocation.Server)]
        public ActionResult Index(string tc)
        {
            ConsumerDataModel consmodel = new ConsumerDataModel();

            System.Threading.Thread.Sleep(200);
            consmodel.GetConsumerBillRawData();
            var searchuser2 = consmodel.ConsumerBillRawData.Where(y => y.tc_id == tc).FirstOrDefault();

            if (searchuser2 != null)
            {
                consmodel.GetConsumerPaidInvoices(tc);
                consmodel.GetConsumerUnpaidInvoices(tc);
            }
            else
            {
                TempData["Message"] = "<script>alert('Kayıt bulunamadı');</script>";
            }


                                 
            return View(consmodel);
        }





        public ActionResult PayInvoiceConsumer(string tc, int id)
        {
            ConsumerDataModel consmodel = new ConsumerDataModel();
            System.Threading.Thread.Sleep(200);
            consmodel.GetConsumerBillRawData();
            var searchuser = consmodel.ConsumerBillRawData.Where(x => x.bill_id == id).FirstOrDefault();
            var searchuser2 = consmodel.ConsumerBillRawData.Where(y => y.tc_id == tc).FirstOrDefault();

            if (searchuser != null && searchuser2 != null)
            {
                consmodel.GetConsumerPayInvoice(id);
                consmodel.GetConsumerPaidInvoices(tc);
                consmodel.GetConsumerUnpaidInvoices(tc);
                TempData["Message"] = "<script>alert('Fatura ödeme işlemi tamamlandı');</script>";
                ViewBag.IsReload = true;
                return RedirectToAction("Index", "ConsumerBill", new { tc = searchuser2.tc_id.ToString() });
            }
            else
            {
                return RedirectToAction("Index", "ConsumerBill", new { tc = searchuser2.tc_id.ToString() });
            }
        }






    }
}