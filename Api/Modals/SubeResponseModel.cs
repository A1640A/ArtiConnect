using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class SubeResponseModel
    {
        public int Id { get; set; }
        public string SubeAdi { get; set; }
        public string Adres { get; set; }
        public string Yetkili { get; set; }
        public string Telefon { get; set; }
        public string Kod { get; set; }
        public DateTime KayitTarihi { get; set; }
    }
}
