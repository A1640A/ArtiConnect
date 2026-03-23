using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_BankSubPaymentInfo
    {
        public ushort type;

        public uint amount;

        public string name;

        public ST_BankSubPaymentInfo()
        {
            name = "";
        }
    }

}
