using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class SubeUrunResponseModel
    {
        public int Id { get; set; }
        public int? SubeId { get; set; }
        public string StokKodu { get; set; }
        public string BarkodNo { get; set; }
        public int? SubeUrunGrubuId { get; set; }
        public string UrunAdi { get; set; }
        public float AlisFiyat { get; set; }
        public float Fiyat { get; set; }
        public float KdvOrani { get; set; }
        public bool PosEkranindaGoster { get; set; }
        public bool ElTerminalindeGoster { get; set; }
        public DateTime KayitTarihi { get; set; }
        public string Kod { get; set; }
        public int? UrunBirimiId { get; set; }
        public bool Favori { get; set; }

        // İlişkili tablolardan gelen veriler
        public string SubeAdi { get; set; }
        public string UrunGrubuAdi { get; set; }
        public string BirimAdi { get; set; }
    }
}