using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_TICKET_HEADER
    {
        public string szMerchName1;

        public string szMerchName2;

        public string szMerchAddr1;

        public string szMerchAddr2;

        public string szMerchAddr3;

        public string VATOffice;

        public string VATNumber;

        public string MersisNo;

        public string TicariSicilNo;

        public string WebAddress;

        public int initDateTime;

        public short index;

        public short EJNo;

        public ST_TICKET_HEADER()
        {
            szMerchName1 = "";
            szMerchName2 = "";
            szMerchAddr1 = "";
            szMerchAddr2 = "";
            szMerchAddr3 = "";
            VATOffice = "";
            VATNumber = "";
            MersisNo = "";
            TicariSicilNo = "";
            WebAddress = "";
        }
    }
}
