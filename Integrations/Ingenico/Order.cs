using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class Order
    {
        public string item_name { get; set; }

        public string item_tax { get; set; }

        public double item_price { get; set; }

        public decimal item_quantity { get; set; }

        public decimal total_amount { get; set; }

        public string entity { get; set; }

        public string order_id { get; set; }
    }
}
