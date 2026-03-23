using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_BANK_PAYMENT_INFO
    {
        public uint batchNo;

        public uint stan;

        public uint balance;

        public ushort bankBkmId;

        public byte numberOfdiscount;

        public byte numberOfbonus;

        public string authorizeCode;

        public byte[] transFlag;

        public string terminalId;

        public string rrn;

        public string referenceCodeOfTransaction;

        public string merchantId;

        public string bankName;

        public byte numberOfInstallments;

        public byte numberOfsubPayment;

        public byte numberOferrorMessage;

        public ST_BankSubPaymentInfo[] stBankSubPaymentInfo;

        public ST_CARD_INFO stCard;

        public ST_PaymentErrMessage stPaymentErrMessage;

        public ST_BANK_PAYMENT_INFO()
        {
            authorizeCode = "";
            transFlag = new byte[2];
            terminalId = "";
            merchantId = "";
            bankName = "";
            stBankSubPaymentInfo = new ST_BankSubPaymentInfo[12];
            stCard = new ST_CARD_INFO();
            stPaymentErrMessage = new ST_PaymentErrMessage();
        }
    }

}
