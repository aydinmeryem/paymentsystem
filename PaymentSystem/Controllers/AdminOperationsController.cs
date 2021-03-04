using PaymentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaymentSystem.Controllers
{
    public class AdminOperationsController : Controller
    {
        // GET: AdminOperations
        
        public ActionResult Index()
        {
            return View();
        }


        #region abone yapma işlemleri
        [HttpGet]
        public ActionResult CreateNewSubscriber()
        {
            return View();
        }

        public ActionResult CreateNewConsumerSubscriber(string username1, string conname, string surname, string tc, string conphone, string conemail, string conpass, bool conisactive)
        {
            CreateDeleteSubscriberModel sub = new CreateDeleteSubscriberModel();
            ConsumerDataModel consdata = new ConsumerDataModel();
            consdata.GetConsumerData();

            System.Threading.Thread.Sleep(200);
            var searchuser = consdata.ConsumerData.Where(x => x.username == username1).SingleOrDefault();

            if (searchuser != null)
            {
                TempData["Message"] = "<script>alert('Bu kayıt zaten mevcut!');</script>";
                return RedirectToAction("CreateNewSubscriber");
            }
            else
            {
                sub.AddNewConsumer(username1, conname, surname, tc, conphone, conemail, conpass, conisactive);
                sub.AddNewConsumerInDeposit(tc);
                TempData["Message"] = "<script>alert('Başarılı bir şekilde sisteme eklendi');</script>";
                return RedirectToAction("CreateNewSubscriber");
            }
        }

        public ActionResult CreateNewEnterpriseSubscriber(string username2, string entname, string tax, string entphone, string entemail, string entpass, bool entisactive)
        {
            CreateDeleteSubscriberModel sub = new CreateDeleteSubscriberModel();
            EnterpriseDataModel entdata = new EnterpriseDataModel();
            entdata.GetEnterpriseData();

            System.Threading.Thread.Sleep(200);
            var searchuser2 = entdata.EnterpriseData.Where(y => y.username == username2).SingleOrDefault();

            if (searchuser2 != null)
            {
                TempData["Message"] = "<script>alert('Bu kayıt zaten mevcut!');</script>";
                return RedirectToAction("CreateNewSubscriber");
            }
            else
            {
                sub.AddNewEnterprise(username2, entname, tax, entphone, entemail, entpass, entisactive);
                sub.AddNewEnterpriseInDeposit(tax);
                TempData["Message"] = "<script>alert('Başarılı bir şekilde sisteme eklendi');</script>";
                return RedirectToAction("CreateNewSubscriber");
            }
        }
        #endregion


        #region abone sorgulama ve fatura ödeme işlemleri
        [HttpGet]
        public ActionResult SearchConsumerSubscriber()
        {
            return View();
        }

        public ActionResult SearchConsumerSubscriberResult(string tc_)
        {
            ConsumerDataModel consdata = new ConsumerDataModel();

            System.Threading.Thread.Sleep(200);
            consdata.GetConsumerDetailsData(tc_);
            var searchuser = consdata.ConsumerDetails.Where(x => x.tc_id == tc_).FirstOrDefault();

            if (searchuser != null)
            {
                consdata.GetConsumerInvoiceDetails(tc_);
                TempData["result"] = "Username:" + searchuser.username + ", Name:" + searchuser.name + ", Surname:" + searchuser.surname + ", TCno:" + searchuser.tc_id + ", Phone:" + searchuser.phone + ", Email:" + searchuser.email;
                return View(consdata);
            }
            else
            {
                TempData["result"] = "<script>alert('Kullanıcı bulunamadı');</script>";
                return RedirectToAction("SearchConsumerSubscriber");
            }
        }

        [HttpGet]
        public ActionResult SearchEnterpriseSubscriber()
        {
            return View();
        }

        public ActionResult SearchEnterpriseSubscriberResult(string tax_)
        {
            EnterpriseDataModel entdata = new EnterpriseDataModel();

            System.Threading.Thread.Sleep(200);
            entdata.GetEnterpriseDetailsData(tax_);
            var searchuser = entdata.EnterpriseDetails.Where(x => x.tax_num == tax_).FirstOrDefault();

            if (searchuser != null)
            {
                entdata.GetEnterpriseInvoiceDetails(tax_);
                TempData["result"] = "Username:" + searchuser.username + ", Name:" + searchuser.name + ", TaxNo:" + searchuser.tax_num + ", Phone:" + searchuser.phone + ", Email:" + searchuser.email;
                return View(entdata);
            }
            else
            {
                TempData["result"] = "<script>alert('Kullanıcı bulunamadı');</script>";
                return RedirectToAction("SearchEnterpriseSubscriber");
            }
        }

        public ActionResult PayInvoiceConsumer_(string tc_, int id)
        {
            ConsumerDataModel consmodel = new ConsumerDataModel();
            System.Threading.Thread.Sleep(200);
            consmodel.GetConsumerBillRawData();
            var searchuser = consmodel.ConsumerBillRawData.Where(x => x.bill_id == id).FirstOrDefault();
            var searchuser2 = consmodel.ConsumerBillRawData.Where(y => y.tc_id == tc_).FirstOrDefault();

            if (searchuser != null && searchuser2 != null)
            {
                consmodel.GetConsumerPayInvoice(id);
                consmodel.GetConsumerPaidInvoices(tc_);
                consmodel.GetConsumerUnpaidInvoices(tc_);
                TempData["Message"] = "<script>alert('Fatura ödeme işlemi tamamlandı');</script>";
                ViewBag.IsReload = true;
                return RedirectToAction("SearchConsumerSubscriberResult", "AdminOperations", new { tc_ = searchuser2.tc_id.ToString() });
            }
            else
            {
                return RedirectToAction("SearchConsumerSubscriberResult", "AdminOperations", new { tc_ = searchuser2.tc_id.ToString() });
            }
        }

        public ActionResult PayInvoiceEnterprise_(string tax_, int id)
        {
            EnterpriseDataModel entmodel = new EnterpriseDataModel();
            System.Threading.Thread.Sleep(200);
            entmodel.GetEnterpriseBillRawData();
            var searchuser = entmodel.EnterpriseBillRawData.Where(x => x.bill_id == id).FirstOrDefault();
            var searchuser2 = entmodel.EnterpriseBillRawData.Where(y => y.tax_num == tax_).FirstOrDefault();

            if (searchuser != null && searchuser2 != null)
            {
                entmodel.GetEnterprisePayInvoice(id);
                entmodel.GetEnterprisePaidInvoices(tax_);
                entmodel.GetEnterpriseUnpaidInvoices(tax_);
                TempData["Message"] = "<script>alert('Fatura ödeme işlemi tamamlandı');</script>";
                ViewBag.IsReload = true;
                return RedirectToAction("SearchEnterpriseSubscriberResult", "AdminOperations", new { tax_ = searchuser2.tax_num.ToString() });
            }
            else
            {
                return RedirectToAction("SearchEnterpriseSubscriberResult", "AdminOperations", new { tax_ = searchuser2.tax_num.ToString() });
            }
        }
        #endregion


        #region abonelik aç/kapa işlemleri
        [HttpGet]
        public ActionResult SubscriptionTransaction()
        {
            return View();
        }

        public ActionResult SubscriptionTransactionConsumerResult(string tc_)
        {
            ConsumerDataModel consdata = new ConsumerDataModel();

            System.Threading.Thread.Sleep(200);
            consdata.GetConsumerDetailsData(tc_);
            var searchuser = consdata.ConsumerDetails.Where(x => x.tc_id == tc_).FirstOrDefault();

            if (searchuser != null)
            {
                consdata.GetConsumerInvoiceDetails(tc_);
                consdata.GetConsumerUnpaidInvoices(tc_);


                ViewBag.usern = searchuser.username;
                ViewBag.name = searchuser.name;
                ViewBag.surn = searchuser.surname;
                ViewBag.tckn = searchuser.tc_id;
                ViewBag.phn = searchuser.phone;
                ViewBag.mail = searchuser.email;
                ViewBag.unpaidcount = consdata.ConsumerUnpaidInvoices.Count(y => y.ispaid == false);

                return View(consdata);
            }            
            else
            {
                TempData["result"] = "<script>alert('Kullanıcı bulunamadı');</script>";
                return RedirectToAction("SearchConsumerSubscriber");
            }
        }

        public ActionResult CloseConsumerAccount(string tc_)
        {
            ConsumerDataModel consdata = new ConsumerDataModel();

            System.Threading.Thread.Sleep(200);
            consdata.GetConsumerBillRawData();
            var searchuser = consdata.ConsumerBillRawData.Where(y => y.tc_id == tc_).FirstOrDefault();


            if (searchuser != null)
            {
                consdata.GetConsumerPayDeposit(tc_);
                consdata.GetConsumerDetailsData(tc_);
                consdata.GetConsumerAccountClose(tc_);
                ViewBag.IsReload = true;
                return RedirectToAction("SubscriptionTransaction", "AdminOperations");
            }
            else
            {
                ViewBag.message = "<p style='color:red;'>Abonelik kapatma işlemi başarısız oldu.</p>";
                return RedirectToAction("SubscriptionTransactionConsumerResult", "AdminOperations", new { tc_ = searchuser.tc_id.ToString() });
            }

        }

        public ActionResult SubscriptionTransactionEnterpriseResult(string tax_)
        {
            EnterpriseDataModel consdata = new EnterpriseDataModel();

            System.Threading.Thread.Sleep(200);
            consdata.GetEnterpriseDetailsData(tax_);
            var searchuser = consdata.EnterpriseDetails.Where(x => x.tax_num == tax_).FirstOrDefault();

            if (searchuser != null)
            {
                consdata.GetEnterpriseInvoiceDetails(tax_);
                consdata.GetEnterpriseUnpaidInvoices(tax_);


                ViewBag.usern = searchuser.username;
                ViewBag.name = searchuser.name;
                ViewBag.tckn = searchuser.tax_num;
                ViewBag.phn = searchuser.phone;
                ViewBag.mail = searchuser.email;
                ViewBag.unpaidcount = consdata.EnterpriseUnpaidInvoices.Count(y => y.ispaid == false);

                return View(consdata);
            }
            else
            {
                TempData["result"] = "<script>alert('Kullanıcı bulunamadı');</script>";
                return RedirectToAction("SearchConsumerSubscriber");
            }
        }

        public ActionResult CloseEnterpriseAccount(string tax_)
        {
            EnterpriseDataModel ensdata = new EnterpriseDataModel();

            System.Threading.Thread.Sleep(200);
            ensdata.GetEnterpriseBillRawData();

            var searchuser = ensdata.EnterpriseBillRawData.Where(y => y.tax_num == tax_).FirstOrDefault();

            if (searchuser != null)
            {
                ensdata.GetEnterprisePayDeposit(tax_);
                ensdata.GetEnterpriseDetailsData(tax_);
                ensdata.GetEnterpriseAccountClose(tax_);
                ViewBag.IsReload = true;
                return RedirectToAction("SubscriptionTransaction", "AdminOperations");
            }
            else
            {
                ViewBag.message = "<p style='color:red;'>Abonelik kapatma işlemi başarısız oldu.</p>";
                return RedirectToAction("SubscriptionTransactionEnterpriseResult", "AdminOperations", new { tax_ = searchuser.tax_num.ToString() });
            }

        }
        #endregion

       


    }
}