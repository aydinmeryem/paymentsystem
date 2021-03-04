using PaymentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace PaymentSystem.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [OutputCache(Duration =180,VaryByParam ="*",Location =OutputCacheLocation.Server)]
        public ActionResult ConsumerLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ConsumerLogin(ConsumerLoginModel cons)
        {
            var consdata = new ConsumerDataModel();
            System.Threading.Thread.Sleep(200);
            consdata.GetConsumerData();
            var searchuser = consdata.ConsumerData.Where(x => x.tc_id == cons.TC_id).SingleOrDefault();

            if (searchuser != null)
            {
                FormsAuthentication.SetAuthCookie("TC_id", false);
                return RedirectToAction("Index", "ConsumerBill", new { tc = searchuser.tc_id.ToString()});
            }
            {
                ViewBag.ErrorMessage = "Kullanıcı bulunamadı. Lütfen blgilerinizi kontrol ediniz.";
                return RedirectToAction("Index", "Home");
            }
        }



        [HttpGet]
        [OutputCache(Duration = 180, VaryByParam = "*", Location = OutputCacheLocation.Server)]
        public ActionResult EnterpriseLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EnterpriseLogin(EnterpriseLoginModel ens)
        {
            var ensdata = new EnterpriseDataModel();
            System.Threading.Thread.Sleep(200);
            ensdata.GetEnterpriseData();
            var searchuser = ensdata.EnterpriseData.Where(x => x.tax_num == ens.Tax_No).SingleOrDefault();

            if (searchuser != null)
            {
                FormsAuthentication.SetAuthCookie("Tax_No", false);
                return RedirectToAction("Index", "EnterpriseBill", new { tax = searchuser.tax_num.ToString() });
            }
            else
            {
                ViewBag.ErrorMessage = "Kullanıcı bulunamadı. Lütfen blgilerinizi kontrol ediniz.";
                return RedirectToAction("Index", "Home");
            }
        }      


    }
}