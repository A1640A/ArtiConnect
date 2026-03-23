using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class StokSayimKartiResponseModel
    {
        public int Id { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public string SayimKodu { get; set; }
        public int SubeId { get; set; }
        public string SubeAdi { get; set; }
        public DateTime KayitTarihi { get; set; }
        public List<StokSayimUrunResponseModel> StokSayimUrunleri { get; set; }
    }
}
