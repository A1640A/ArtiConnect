using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.PayGo
{
    public class DoPaymentRequest
    {
        public string PaymentType { get; set; }
        public string Amount { get; set; }
        public string CurrencyIndex { get; set; }
        public string ExcRate { get; set; }
        public string CurrencyFlag { get; set; }
    }
}
