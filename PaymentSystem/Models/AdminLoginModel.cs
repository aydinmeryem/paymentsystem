using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PaymentSystem.Models
{
    public class AdminLoginModel
    {
        [Required]
        [DisplayName("Admin Kullanıcı Adınızı Giriniz.")]
        public string admin_username { get; set; }

        [Required]
        [DisplayName("Şifrenizi Giriniz")]
        public string admin_password { get; set; }
    }
}