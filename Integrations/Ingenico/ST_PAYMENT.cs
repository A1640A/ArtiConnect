using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_PAYMENT
    {
        public byte flags;

        public uint dateOfPayment;

        public uint typeOfPayment;

        public byte subtypeOfPayment;

        public uint orgAmount;

        public ushort orgAmountCurrencyCode;

        public uint payAmount;

        public ushort payAmountCurrencyCode;

        public uint cashBackAmountInTL;

        public uint cashBackAmountInDoviz;

        public string paymentName;

        public string paymentInfo;

        public ST_BANK_PAYMENT_INFO stBankPayment;

        public ST_PAYMENT()
        {
            paymentName = "";
            paymentInfo = "";
            stBankPayment = new ST_BANK_PAYMENT_INFO();
        }
    }

}
