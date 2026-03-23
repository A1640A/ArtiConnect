using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_HANDLE_LIST
    {
        public ulong Handle;

        public string szMasterOkcSicilNo;

        public string szUserDefinedTranGroup;

        public string szUserDefinedTranSubGroup;

        public uint ReceiptAmount;

        public byte Status;

        public ST_HANDLE_LIST()
        {
            szMasterOkcSicilNo = "";
            szUserDefinedTranGroup = "";
            szUserDefinedTranSubGroup = "";
        }
    }

}
