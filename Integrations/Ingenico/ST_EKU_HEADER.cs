using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public struct ST_EKU_HEADER
    {
        public byte[] SicilNo;

        public byte[] TerminalSerialNo;

        public byte[] TerminalProductCode;

        public byte[] SoftwareVersion;

        public byte[] MerchantName;

        public byte[] MerchantAddress;

        public byte[] VATOffice;

        public byte[] VATNumber;

        public EKU_INI_CLS_t Init;

        public EKU_INI_CLS_t Close;

        public ushort Active;

        public ushort EkuCount;

        public ushort HeaderIndex;

        public ushort HeaderTotal;

        public byte[] MersisNo;

        public byte[] TicariSicilNo;

        public byte[] WebAddress;

        public byte[] ApplicationUse;
    }

}
