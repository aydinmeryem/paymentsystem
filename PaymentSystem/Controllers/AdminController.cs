using PaymentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PaymentSystem.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(AdminLoginModel ad)
        {
            var admindata = new AdminDataModel();

            admindata.GetAdminData();

            admindata.admin_user = ad.admin_username;
            admindata.admin_pass = ad.admin_password;
            if (admindata!=null && admindata.admin_pass==ad.admin_password)
            {
                FormsAuthentication.SetAuthCookie("admin_password", false);
                return RedirectToAction("Index", "AdminOperations");
            }
            else
            {
                ViewBag.ErrorMessage = "Kullanıcı bulunamadı. Lütfen blgilerinizi kontrol ediniz.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}