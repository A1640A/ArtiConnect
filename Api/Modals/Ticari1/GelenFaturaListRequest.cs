using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.Ticari1
{
    public class GelenFaturaListRequest
    {
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public int CustomerId { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
    }
}
