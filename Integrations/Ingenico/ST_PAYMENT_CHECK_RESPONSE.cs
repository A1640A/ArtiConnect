using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_PAYMENT_CHECK_RESPONSE
    {
        public string odemeOnayKod;

        public string refundRNN;

        public byte[] uniqueId;

        public byte TaksitSayisi;

        public string CardHolderName;

        public byte ReaderTypes;

        public byte[] CardType;

        public uint Tutar;

        public ushort BankName;

        public ushort BKMIdU16;

        public uint BatchNo;

        public uint STAN;

        public string TerminalId;

        public string MerchantId;

        public byte ProvisionId;

        public ushort BKMId;

        public ST_PAYMENT_CHECK_RESPONSE()
        {
            CardType = new byte[3];
        }
    }

}
