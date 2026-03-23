using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_GMP_PAIR_RESP
    {
        public uint ErrorCode;

        public string szEcrBrand;

        public string szEcrModel;

        public string szEcrSerialNumber;

        public string szVersionNumber;

        public ST_GMP_PAIR_RESP()
        {
            szEcrBrand = "";
            szEcrModel = "";
            szEcrSerialNumber = "";
            ErrorCode = 0u;
            szVersionNumber = "";
        }
    }

}
