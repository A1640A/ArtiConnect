using ArtiConnect.Api.Modals.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class StokKartiRequestModel
    {
        public StokTuru? StokTuru { get; set; }
        public int? StokKartiKategoriId { get; set; }

        [StringLength(50)]
        public string Barkod { get; set; }

        [Required(ErrorMessage = "Stok adı zorunludur")]
        [StringLength(100)]
        public string StokAdi { get; set; }

        public float AlisKdv { get; set; }
        public float SatisKdv { get; set; }
        public int? DepoId { get; set; }

        public Birim? Birim1 { get; set; }
        public float BirimCarpani1 { get; set; }
        public Birim? Birim2 { get; set; }
        public float BirimCarpani2 { get; set; }
        public Birim? Birim3 { get; set; }
        public float BirimCarpani3 { get; set; }
        public Birim? Birim4 { get; set; }
        public float BirimCarpani4 { get; set; }

        [StringLength(50)]
        public string Kod { get; set; }
    }
}