using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class Root
    {
        public int ticket_id { get; set; }

        public string ticket_uid { get; set; }

        public string ticket_no { get; set; }

        public bool is_invoice { get; set; }

        public bool first_payment { get; set; }

        public List<Order> orders { get; set; }

        public Payment payment { get; set; }

        public Discount discount { get; set; }

        public Calculation[] calculation { get; set; }

        public Customer customer { get; set; }
    }
}
