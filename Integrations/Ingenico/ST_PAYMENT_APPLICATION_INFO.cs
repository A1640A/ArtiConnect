using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_PAYMENT_APPLICATION_INFO
    {
        public byte[] name;

        public byte index;

        public ushort u16BKMId;

        public ushort Status;

        public ushort Priority;

        public ushort u16AppId;

        public ushort AppType;

        public ushort AppFlag;

        public ST_PAYMENT_APPLICATION_INFO()
        {
            name = new byte[20];
        }
    }

}
