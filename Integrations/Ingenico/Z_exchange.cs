using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class Z_exchange
    {
        public long totalAmount;

        public long totalAmountInTL;

        public byte[] name;

        public Z_exchange()
        {
            name = new byte[13];
        }
    }

}
