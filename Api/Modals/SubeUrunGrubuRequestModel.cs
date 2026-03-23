using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class SubeUrunGrubuRequestModel
    {
        [Required(ErrorMessage = "Şube ID alanı zorunludur.")]
        public int? SubeId { get; set; }

        [Required(ErrorMessage = "Ürün grubu adı alanı zorunludur.")]
        [StringLength(250, ErrorMessage = "Ürün grubu adı en fazla 250 karakter olabilir.")]
        public string UrunGrubuAdi { get; set; }

        public string Kod { get; set; }

        public string Sirket { get; set; }
    }
}