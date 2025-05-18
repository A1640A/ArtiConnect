using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArtiConnect.Integrations.Ticari1.Modals;

namespace ArtiConnect.Api.Modals.Ticari1
{
    public class EFaturaPdfRequest
    {
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public int CustomerId { get; set; }
        public GetEFaturaPdfModal PdfModal { get; set; }
        public string YaziciAdi { get; set; }
        public int KopyaSayisi { get; set; } = 1;
    }
}
