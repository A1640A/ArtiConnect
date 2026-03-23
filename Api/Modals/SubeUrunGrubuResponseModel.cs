using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class SubeUrunGrubuResponseModel
    {
        public int Id { get; set; }
        public int? SubeId { get; set; }
        public string UrunGrubuAdi { get; set; }
        public DateTime KayitTarihi { get; set; }
        public string Kod { get; set; }
        public int? SiraNo { get; set; }

        // İlişkili tablolardan gelen veriler
        public string SubeAdi { get; set; }
    }
}
