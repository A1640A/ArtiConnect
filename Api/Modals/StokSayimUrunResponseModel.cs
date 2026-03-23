using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class StokSayimUrunResponseModel
    {
        public int Id { get; set; }
        public int StokSayimKartiId { get; set; }
        public int StokKartiId { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public DateTime Tarih { get; set; }
        public double Devir { get; set; }
        public double Giren { get; set; }
        public double Cikan { get; set; }
        public double Kalan { get; set; }
        public double Sayim { get; set; }
        public double Fark { get; set; }
    }
}
