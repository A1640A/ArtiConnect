using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public struct FISCAL_INTEGRITY_t
    {
        public byte Fiscal;

        public byte Event;

        public byte Daily;

        public byte RFU;
    }
}
