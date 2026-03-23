using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class StokSayimKartiRequestModel
    {
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public string SayimKodu { get; set; }
        public int SubeId { get; set; }
        public List<StokSayimUrunRequestModel> StokSayimUrunleri { get; set; }
    }
}
