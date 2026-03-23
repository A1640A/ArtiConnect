using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_ONLINE_INVIOCE_INFO
    {
        public string CustomerName;

        public string VKN;

        public string HomeAddress;

        public string District;

        public string City;

        public string Country;

        public string Mail;

        public string WebSite;

        public string Phone;

        public string TaxOffice;

        public string Ettn;

        public string DespatchNo;

        public string Identifier;

        public string OrderNo;

        public byte[] Type;

        public byte[] OrderDate;

        public byte[] DespatchDate;

        public string SellerIdentifier_OnlineInvoice;

        public string SellerIdentifier_OnlineArchive;

        public ushort rawDataLen;

        public byte[] rawData;

        public ST_ONLINE_INVIOCE_INFO()
        {
            CustomerName = "";
            VKN = "";
            HomeAddress = "";
            District = "";
            City = "";
            Country = "";
            Mail = "";
            WebSite = "";
            Phone = "";
            TaxOffice = "";
            Ettn = "";
            DespatchNo = "";
            Identifier = "";
            OrderNo = "";
            Type = new byte[2];
            OrderDate = new byte[7];
            DespatchDate = new byte[7];
            SellerIdentifier_OnlineArchive = "";
            SellerIdentifier_OnlineInvoice = "";
            rawDataLen = 0;
            rawData = new byte[512];
        }
    }

}
