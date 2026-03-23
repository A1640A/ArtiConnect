using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class DaraSecenegiRequestModel
    {
        [Required(ErrorMessage = "Dara adı alanı zorunludur.")]
        [StringLength(100, ErrorMessage = "Dara adı en fazla 100 karakter olabilir.")]
        public string DaraAdi { get; set; }

        [Required(ErrorMessage = "Gramaj alanı zorunludur.")]
        [Range(0.01, float.MaxValue, ErrorMessage = "Gramaj 0'dan büyük olmalıdır.")]
        public float Gramaj { get; set; }

        public string Kod { get; set; }

        public string Sirket { get; set; }
    }
}