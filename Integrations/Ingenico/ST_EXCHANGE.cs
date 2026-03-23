using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_EXCHANGE
    {
        public ushort code;

        public string prefix;

        public string sign;

        public byte locationOfSign;

        public byte tousandSeperator;

        public byte centSeperator;

        public byte numberOfCentDigit;

        public ulong rate;

        public ST_EXCHANGE()
        {
            prefix = "";
            sign = "";
        }
    }

}
