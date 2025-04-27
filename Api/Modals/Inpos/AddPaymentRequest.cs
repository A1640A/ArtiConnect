using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.Inpos
{
    public class AddPaymentRequest
    {
        public string PaymentType { get; set; }
        public decimal PaymentAmount { get; set; }
        public int? AcquirerId { get; set; }
    }
}
