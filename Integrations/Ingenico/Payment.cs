using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class Payment
    {
        public decimal payment_amount { get; set; }

        public decimal payment_type { get; set; }

        public string payment_desc { get; set; }

        public string customer_tax_id { get; set; }
    }

}
