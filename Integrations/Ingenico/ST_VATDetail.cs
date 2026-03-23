using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_VATDetail
    {
        public int u32VAT;

        public int u32Amount;

        public ushort u16VATPercentage;

        public ST_VATDetail()
        {
            u32VAT = 0;
            u32Amount = 0;
            u16VATPercentage = 0;
        }
    }

}
