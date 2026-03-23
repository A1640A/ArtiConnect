using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_PAYMENT_REQUEST
    {
        public ulong typeOfPayment;

        public uint subtypeOfPayment;

        public uint payAmount;

        public uint payAmountBonus;

        public ushort payAmountCurrencyCode;

        public ushort bankBkmId;

        public ushort numberOfinstallments;

        public byte[] terminalId;

        public string BankPaymentUniqueId;

        public _ST_PAYMENT_REQUEST_ORGINAL_DATA OrgTransData;

        public uint batchNo;

        public uint stanNo;

        public ushort rawDataLen;

        public byte[] rawData;

        public string paymentName;

        public string paymentInfo;

        public uint transactionFlag;

        public uint flags;

        public string LoyaltyCustomerId;

        public string PaymentProvisionId;

        public ushort LoyaltyServiceId;

        public ST_PAYMENT_REQUEST()
        {
            terminalId = new byte[8];
            rawDataLen = 0;
            rawData = new byte[512];
            BankPaymentUniqueId = "";
            OrgTransData = new _ST_PAYMENT_REQUEST_ORGINAL_DATA();
        }
    }

}
