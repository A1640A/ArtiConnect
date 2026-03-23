using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_ITEM
    {
        public byte type;

        public byte subType;

        public byte deptIndex;

        public byte unitType;

        public uint amount;

        public ushort currency;

        public uint count;

        public uint flag;

        public byte countPrecition;

        public byte pluPriceIndex;

        public string name;

        public string barcode;

        public string firm;

        public string invoiceNo;

        public string subscriberId;

        public string tckno;

        public uint Reserved;

        public byte[] Date;

        public promotion promotion;

        public ushort OnlineInvoiceItemExceptionCode;

        public ST_ITEM()
        {
            name = "";
            barcode = "";
            firm = "";
            invoiceNo = "";
            subscriberId = "";
            tckno = "";
            promotion = new promotion();
            Date = new byte[3];
        }
    }
}
