using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_UNIQUE_ID
    {
        public byte[] uniqueId;

        public ushort reserved1;

        public uint reserved2;

        public ST_UNIQUE_ID()
        {
            uniqueId = new byte[24];
        }
    }

}
