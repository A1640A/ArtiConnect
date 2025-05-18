using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.Ticari1
{
    public class EArsivIptalRequest
    {
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public int CustomerId { get; set; }
        public string FaturaKey { get; set; }
    }
}
