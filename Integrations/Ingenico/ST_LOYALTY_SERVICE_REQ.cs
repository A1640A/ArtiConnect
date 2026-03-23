using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_LOYALTY_SERVICE_REQ
    {
        public byte[] name;

        public string CustomerId;

        public ushort ServiceId;

        public ushort u16AppId;

        public ushort CustomerIdType;

        public uint Amount;

        public byte[] rawData;

        public ushort rawDataLen;

        public ST_LOYALTY_SERVICE_REQ()
        {
            name = new byte[24];
            rawData = new byte[512];
            CustomerId = "";
        }
    }

}
