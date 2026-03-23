using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class MasrafKategorisiRequestModel
    {
        [Required(ErrorMessage = "Kategori adı alanı zorunludur.")]
        [StringLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir.")]
        public string KategoriAdi { get; set; }

        public string Kod { get; set; }

        public string Sirket { get; set; }
    }
}