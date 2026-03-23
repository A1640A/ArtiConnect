using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_MULTIPLE_RETURN_CODE
    {
        public uint subCommand;

        public uint retcode;

        public uint tag;

        public ushort indexOfSubCommand;

        public ushort lengthOfData;

        public byte[] pData;

        public ST_MULTIPLE_RETURN_CODE()
        {
            pData = new byte[100];
        }
    }

}
