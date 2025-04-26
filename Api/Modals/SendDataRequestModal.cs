using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class SendDataRequest
    {
        public string Data { get; set; }
        public bool IsHex { get; set; } = false;
    }
}
