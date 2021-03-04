using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PaymentSystem.Models
{
    public class ConsumerLoginModel
    {
        [Required]
        [DisplayName("TC Kimlik Numaranızı Giriniz")]
        public string TC_id { get; set; }

        [Required]
        [DisplayName("Şifrenizi Giriniz")]
        public string Password { get; set; }
    }
}