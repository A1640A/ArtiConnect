using ArtiConnect.Api.Modals.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class StokKartiResponseModel
    {
        public int Id { get; set; }
        public StokTuru? StokTuru { get; set; }
        public int? StokKartiKategoriId { get; set; }
        public string Barkod { get; set; }
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
        public string Kod { get; set; }
        public DateTime KayitTarihi { get; set; }

        // Navigation properties
        public string StokKategoriAdi { get; set; }
        public string DepoAdi { get; set; }
    }
}