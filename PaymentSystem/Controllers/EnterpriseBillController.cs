using PaymentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaymentSystem.Controllers
{
    public class EnterpriseBillController : Controller
    {
        // GET: EnterpriseBill
        [HttpGet]
        public ActionResult Index(string tax)
        {
            EnterpriseDataModel entmodel = new EnterpriseDataModel();

            System.Threading.Thread.Sleep(200);
            entmodel.GetEnterpriseBillRawData();
            var searchuser2 = entmodel.EnterpriseBillRawData.Where(y => y.tax_num == tax).FirstOrDefault();

            if (searchuser2 != null)
            {
                entmodel.GetEnterprisePaidInvoices(tax);
                entmodel.GetEnterpriseUnpaidInvoices(tax);
            }
            else
            {
                TempData["Message"] = "<script>alert('Kayıt bulunamadı');</script>";
            }

            return View(entmodel);
        }


        public ActionResult PayInvoiceEnterprise(string tax, int id)
        {
            EnterpriseDataModel entmodel = new EnterpriseDataModel();
            System.Threading.Thread.Sleep(200);
            entmodel.GetEnterpriseBillRawData();
            var searchuser = entmodel.EnterpriseBillRawData.Where(x => x.bill_id == id).FirstOrDefault();
            var searchuser2 = entmodel.EnterpriseBillRawData.Where(y => y.tax_num == tax).FirstOrDefault();

            if (searchuser != null && searchuser2 != null)
            {
                entmodel.GetEnterprisePayInvoice(id);
                entmodel.GetEnterprisePaidInvoices(tax);
                entmodel.GetEnterpriseUnpaidInvoices(tax);
                TempData["Message"] = "<script>alert('Fatura ödeme işlemi tamamlandı');</script>";
                ViewBag.IsReload = true;
                return RedirectToAction("Index", "EnterpriseBill", new { tax = searchuser2.tax_num.ToString() });
            }
            else
            {
                return RedirectToAction("Index", "EnterpriseBill", new { tax = searchuser2.tax_num.ToString() });
            }
        }

       
    }
}