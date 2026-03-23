using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public struct EKU_INFO_t
    {
        public EKU_RECORD_t LastRecord;

        public uint MapFreeArea;

        public uint MapUsedArea;

        public uint DataFreeArea;

        public uint DataUsedArea;

        public ushort HeaderUsed;

        public ushort HeaderTotal;

        public EKU_STATUS_t Status;

        public ushort CpuCRC;
    }

}
