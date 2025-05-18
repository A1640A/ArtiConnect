using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArtiConnect.Integrations.Ticari1.Modals;

namespace ArtiConnect.Api.Modals.Ticari1
{
    public class EFaturaRequest
    {
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public int CustomerId { get; set; }
        public string SubeNo { get; set; }
        public EFaturaModal EFaturaModal { get; set; }
    }
}
