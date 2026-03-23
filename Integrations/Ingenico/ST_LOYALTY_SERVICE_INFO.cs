using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_LOYALTY_SERVICE_INFO
    {
        public byte[] name;

        public string CustomerId;

        public ushort ServiceId;

        public ushort u16AppId;

        public ushort CustomerIdType;

        public uint TotalDiscountAmount;

        public ST_LOYALTY_SERVICE_INFO()
        {
            name = new byte[24];
            CustomerId = "";
        }
    }
}
