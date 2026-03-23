using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class SubeUrunRequestModel
    {
        [Required(ErrorMessage = "Şube ID alanı zorunludur.")]
        public int? SubeId { get; set; }

        public string StokKodu { get; set; }

        public string BarkodNo { get; set; }

        public int? SubeUrunGrubuId { get; set; }

        [Required(ErrorMessage = "Ürün adı alanı zorunludur.")]
        [StringLength(250, ErrorMessage = "Ürün adı en fazla 250 karakter olabilir.")]
        public string UrunAdi { get; set; }

        [Required(ErrorMessage = "Alış fiyatı alanı zorunludur.")]
        public float AlisFiyat { get; set; }

        [Required(ErrorMessage = "Fiyat alanı zorunludur.")]
        public float Fiyat { get; set; }

        [Required(ErrorMessage = "KDV oranı alanı zorunludur.")]
        public float KdvOrani { get; set; }

        public bool PosEkranindaGoster { get; set; } = true;

        public bool ElTerminalindeGoster { get; set; } = true;

        public string Kod { get; set; }

        public int? UrunBirimiId { get; set; }

        public bool Favori { get; set; } = false;

        public string Sirket { get; set; }
    }
}