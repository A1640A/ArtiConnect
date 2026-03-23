using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public struct Z_invoice
    {
        public long TotalAmount;

        public long classicTotalAmount;

        public long e_invoiceTotalAmount;

        public long e_archiveTotalAmount;

        public long creditTotalAmount;

        public long cashTotalAmount;
    }

}
