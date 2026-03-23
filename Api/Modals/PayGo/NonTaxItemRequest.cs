using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.PayGo
{
    public class NonTaxItemRequest
    {
        public string Amount { get; set; }
        public string NonTaxId { get; set; }
        public string ItemName { get; set; }
        public string Tckn { get; set; }
    }
}
