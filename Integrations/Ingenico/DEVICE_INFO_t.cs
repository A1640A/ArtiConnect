using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class DEVICE_INFO_t
    {
        public string szSoftVersion = "";

        public string szHardVersion = "";

        public string szCompileDate = "";

        public string szDescription = "";

        public string szHardwareReference = "";

        public string szHardwareSerial = "";

        public string szCpuID = "";

        public string szHash = "";

        public string szBootVersion = "";

        public FISCAL_INTEGRITY_t Integrity;

        public MEMORY_INFO_t Flash1;

        public MEMORY_INFO_t Flash2;

        public MEMORY_INFO_t Fram;

        public ushort CpuCRC;

        public byte Authentication;
    }
}
