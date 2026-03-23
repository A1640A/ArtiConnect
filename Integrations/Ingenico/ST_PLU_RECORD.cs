using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public struct ST_PLU_RECORD
    {
        public byte deptIndex;

        public byte unitType;

        public uint flag;

        public uint lastModified;

        public ushort[] currency;

        public uint[] amount;

        public string barcode;

        public string name;

        public uint groupParentId;
    }

}
