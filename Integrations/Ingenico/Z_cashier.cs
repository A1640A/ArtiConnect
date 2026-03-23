using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class Z_cashier
    {
        public long totalAmount;

        public byte[] name;

        public Z_cashier()
        {
            name = new byte[12];
        }
    }

}
