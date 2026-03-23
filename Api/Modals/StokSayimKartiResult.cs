using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class StokSayimKartiResult
    {
        public string Sonuc { get; set; }
        public string HataMesaji { get; set; }
        public int? HataSeverity { get; set; }
        public int? HataState { get; set; }
        public bool IsSuccess => string.IsNullOrEmpty(HataMesaji);
    }
}
