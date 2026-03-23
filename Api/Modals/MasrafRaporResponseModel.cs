using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class MasrafRaporResponseModel
    {
        public string GrupAdi { get; set; }
        public int MasrafSayisi { get; set; }
        public float ToplamTutar { get; set; }
    }
}