using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class SubeMasrafResponseModel
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public int? SubeId { get; set; }
        public int? MasrafKategorisiId { get; set; }
        public string Aciklama { get; set; }
        public float Tutar { get; set; }
        public int? OdemeYontemiId { get; set; }
        public DateTime KayitTarihi { get; set; }
        public string Kod { get; set; }

        // İlişkili tablolardan gelen veriler
        public string SubeAdi { get; set; }
        public string MasrafKategoriAdi { get; set; }
        public string OdemeYontemiAdi { get; set; }
    }
}