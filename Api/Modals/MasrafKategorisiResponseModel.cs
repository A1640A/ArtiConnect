using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class MasrafKategorisiResponseModel
    {
        public int Id { get; set; }
        public string KategoriAdi { get; set; }
        public DateTime KayitTarihi { get; set; }
        public DateTime? GuncellemeTarihi { get; set; }
        public string Kod { get; set; }
    }
}