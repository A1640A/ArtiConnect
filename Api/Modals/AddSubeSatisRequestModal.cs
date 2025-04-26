using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class AddSubeSatisRequestModal
    {
        public string SubeKodu { get; set; }
        public string StokKodu { get; set; }
        public double Miktar { get; set; }
        public double Fiyat { get; set; }
        public DateTime Tarih { get; set; }
        public string FisId { get; set; }
        public string AdisyonNo { get; set; }
        public DateTime FisTarihi { get; set; }
        public string FisNoDurumu { get; set; }
        public string KullaniciAdi { get; set; }
        public string UrunAdi { get; set; }
        public string UrunGrubuAdi { get; set; }
        public bool IsIade { get; set; }
        public bool IsZayi { get; set; }
        public bool IsKaporaSatis { get; set; }
        public string Aciklama { get; set; }
    }
}
