using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_printerDataForOneLine
    {
        public uint Flag;

        public byte lineLen;

        public string line;

        public ST_printerDataForOneLine()
        {
            line = "";
        }
    }

}
