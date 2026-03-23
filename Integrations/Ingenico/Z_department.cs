using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class Z_department
    {
        public long totalAmount;

        public long totalQuantity;

        public byte[] name;

        public Z_department()
        {
            name = new byte[25];
        }
    }

}
