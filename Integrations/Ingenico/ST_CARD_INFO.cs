using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_CARD_INFO
    {
        public byte inputType;

        public string pan;

        public string holderName;

        public byte[] type;

        public ST_CARD_INFO()
        {
            inputType = 0;
            pan = "";
            holderName = "";
            type = new byte[3];
        }
    }

}
