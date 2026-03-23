using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_INVIOCE_INFO
    {
        public byte source;

        public byte[] no;

        public byte[] date;

        public byte[] tck_no;

        public byte[] vk_no;

        public ulong amount;

        public ushort currency;

        public uint flag;

        public ST_INVIOCE_INFO()
        {
            no = new byte[25];
            date = new byte[3];
            tck_no = new byte[12];
            vk_no = new byte[12];
        }
    }

}
