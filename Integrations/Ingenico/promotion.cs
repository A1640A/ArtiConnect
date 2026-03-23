using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class promotion
    {
        public byte type;

        public int amount;

        public string ticketMsg;

        public promotion()
        {
            ticketMsg = "";
        }
    }

}
