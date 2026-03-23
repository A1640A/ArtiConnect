using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class Z_payment
    {
        public long cashTotal;

        public long creditTotal;

        public long otherTotal;

        public Z_other other;

        public Z_payment()
        {
            other = default(Z_other);
        }
    }

}
