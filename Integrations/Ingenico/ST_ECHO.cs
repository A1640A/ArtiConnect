using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_ECHO
    {
        public uint retcode;

        public uint status;

        public byte[] kvc;

        public byte ecrMode;

        public ushort mtuSize;

        public byte[] ecrVersion;

        public ST_DATE date;

        public ST_TIME time;

        public ST_CASHIER activeCashier;

        public ST_ECHO()
        {
            kvc = new byte[8];
            ecrMode = 0;
            ecrVersion = new byte[16];
            activeCashier = new ST_CASHIER();
            date = default(ST_DATE);
            time = default(ST_TIME);
        }
    }

}
