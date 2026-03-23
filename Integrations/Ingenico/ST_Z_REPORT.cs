using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_Z_REPORT
    {
        public int StructSize;

        public byte[] Date;

        public byte[] Time;

        public int FNo;

        public int ZNo;

        public int EJNo;

        public Z_countOf countOf;

        public long IncTotAmount;

        public long DecTotAmount;

        public long SaleVoidTotAmount;

        public long CorrectionTotAmount;

        public long InvoiceSaleTotAmount;

        public long FoodRcptTotAmount;

        public long DailyTotAmount;

        public long DailyTotTax;

        public long CumulativeTotAmount;

        public long CumulativeTotTax;

        public long AvansTotalAmount;

        public long OdemeTotalAmount;

        public long TaxRefundTotalAmount;

        public long MatrahsizTotalAmount;

        public long OpenAccountTotalAmount;

        public Z_department[] department;

        public Z_exchange[] exchange;

        public Z_tax[] tax;

        public Z_cashier[] cashier;

        public Z_invoice invoice;

        public Z_payment payment;

        public Z_sectorData sectorData;

        public ST_Z_REPORT()
        {
            Date = new byte[3];
            Time = new byte[2];
            countOf = default(Z_countOf);
            department = new Z_department[12];
            exchange = new Z_exchange[6];
            tax = new Z_tax[8];
            cashier = new Z_cashier[4];
            invoice = default(Z_invoice);
            payment = new Z_payment();
            sectorData = new Z_sectorData();
        }
    }

}
