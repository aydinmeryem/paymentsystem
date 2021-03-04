using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PaymentSystem.Models
{
    public class EnterpriseLoginModel
    {
        [Required]
        [DisplayName("Vergi Numaranızı Giriniz")]
        public string Tax_No { get; set; }

        [Required]
        [DisplayName("Şifrenizi Giriniz")]
        public string Password { get; set; }
    }
}