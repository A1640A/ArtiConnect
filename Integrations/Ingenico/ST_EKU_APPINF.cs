using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_EKU_APPINF
    {
        public byte[] Buffer;

        public uint Amount;

        public uint Vat;

        public byte[] DateTime;

        public byte[] DateTimeDelta;

        public ushort BufLen;

        public ushort RecLen;

        public ushort RemLen;

        public ushort ZNo;

        public ushort FNo;

        public ushort Type;

        public ushort Func;

        public ushort DateTimeCount;

        public ushort RecordStatus;

        public ST_EKU_APPINF()
        {
            Buffer = new byte[1024];
            DateTime = new byte[6];
            DateTimeDelta = new byte[6];
        }
    }

}
