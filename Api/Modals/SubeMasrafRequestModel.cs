using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class SubeMasrafRequestModel
    {
        [Required(ErrorMessage = "Tarih alanı zorunludur.")]
        public DateTime Tarih { get; set; }

        public int? SubeId { get; set; }

        public int? MasrafKategorisiId { get; set; }

        [Required(ErrorMessage = "Açıklama alanı zorunludur.")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string Aciklama { get; set; }

        [Required(ErrorMessage = "Tutar alanı zorunludur.")]
        [Range(0.01, float.MaxValue, ErrorMessage = "Tutar 0'dan büyük olmalıdır.")]
        public float Tutar { get; set; }

        public int? OdemeYontemiId { get; set; }

        public string Kod { get; set; }

        public string Sirket { get; set; }
    }
}