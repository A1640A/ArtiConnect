using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class AddSubeOdemeRequestModal
    {
        public string SubeKodu { get; set; }
        public int FisId { get; set; }
        public string AdisyonNo { get; set; }
        public DateTime FisTarihi { get; set; }
        public string KullaniciAdi { get; set; }
        public string OdemeYontemiAdi { get; set; }
        public double Miktar { get; set; }
        public DateTime Tarih { get; set; }
    }
}
