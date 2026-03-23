using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_TAXFREE_INFO
    {
        public string BuyerName;

        public string BuyerSurname;

        public string VKN;

        public byte[] IDDate;

        public string City;

        public string Country;

        public string CountryCode;

        public string Identifier;

        public string Ettn;

        public ST_TAXFREE_INFO()
        {
            BuyerName = "";
            BuyerSurname = "";
            VKN = "";
            IDDate = new byte[3];
            City = "";
            Country = "";
            CountryCode = "";
            Identifier = "";
            Ettn = "";
        }
    }

}
