using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_TICKET
    {
        public uint TransactionFlags;

        public uint OptionFlags;

        public ushort ZNo;

        public ushort FNo;

        public ushort EJNo;

        public uint TotalReceiptAmount;

        public uint TotalReceiptTax;

        public uint TotalReceiptDiscount;

        public uint TotalReceiptIncrement;

        public uint CashBackAmount;

        public uint TotalReceiptItemCancel;

        public uint TotalReceiptPayment;

        public uint TotalReceiptReversedPayment;

        public uint KasaAvansAmount;

        public uint KasaPaymentAmount;

        public uint invoiceAmount;

        public uint invoiceAmountCurrency;

        public uint KatkiPayiAmount;

        public uint TaxFreeRefund;

        public uint TaxFreeCalculated;

        public string szTicketDate;

        public string szTicketTime;

        public ushort SourceVasAppID;

        public ushort PaymentVasAppID;

        public ushort BankVasAppID;

        public byte ticketType;

        public ushort totalNumberOfItems;

        public ushort numberOfItemsInThis;

        public ushort totalNumberOfPayments;

        public ushort numberOfPaymentsInThis;

        public ushort numberOfLoyaltyInThis;

        public string TckNo;

        public string invoiceNo;

        public uint invoiceDate;

        public byte invoiceType;

        public int totalNumberOfPrinterLines;

        public int numberOfPrinterLinesInThis;

        public byte[] uniqueId;

        public byte[] rawData;

        public ushort rawDataLen;

        public string LastPaymentErrorCode;

        public string LastPaymentErrorMsg;

        public string BankPaymentUniqueId;

        public ST_SALEINFO[] SaleInfo;

        public ST_PAYMENT[] stPayment;

        public ST_VATDetail[] stTaxDetails;

        public ST_printerDataForOneLine[] stPrinterCopy;

        public byte[] UserData;

        public ST_LOYALTY_SERVICE_INFO[] stLoyaltyService;

        public int CurrencyProfileIndex;

        public ST_TICKET()
        {
            TckNo = "";
            invoiceNo = "";
            szTicketDate = "";
            szTicketTime = "";
            uniqueId = new byte[24];
            rawData = new byte[512];
            SaleInfo = new ST_SALEINFO[512];
            stPayment = new ST_PAYMENT[24];
            stTaxDetails = new ST_VATDetail[8];
            stPrinterCopy = new ST_printerDataForOneLine[1024];
            stLoyaltyService = new ST_LOYALTY_SERVICE_INFO[8];
        }
    }

}
