using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public struct EKU_RECORD_t
    {
        public uint DateTime;

        public uint Amount;

        public uint Vat;

        public ushort FNo;

        public ushort ZNo;

        public ushort Type;

        public ushort Status;
    }
}
