using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class PaymentStatus
    {
        public readonly string PaymentType;

        public decimal TotalItemAmount { get; set; }

        public decimal TicketAmount { get; set; }

        public decimal CurrentAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        public bool IsDiscount { get; set; }

        public decimal TotalAmountPaid { get; set; }

        public bool IsPartialPayment { get; set; }

        public bool IsLastPayment { get; set; }

        public bool IsFirsPayment { get; set; }

        public PaymentStatus(decimal totalItemAmount, decimal ticketAmount, decimal paymentAmount, decimal discountAmount, string paymentType, ST_TICKET incompleteTicket = null)
        {
            if (incompleteTicket == null)
            {
                TotalItemAmount = totalItemAmount;
                TicketAmount = ticketAmount;
                CurrentAmount = paymentAmount;
                TotalAmountPaid = paymentAmount;
                DiscountAmount = discountAmount;
                IsPartialPayment = TicketAmount != CurrentAmount;
                IsLastPayment = TicketAmount - TotalAmountPaid == 0m;
                IsFirsPayment = true;
                IsDiscount = discountAmount > 0m;
                PaymentType = paymentType;
            }
            else
            {
                TotalItemAmount = (decimal)incompleteTicket.TotalReceiptAmount / 100m + (decimal)incompleteTicket.TotalReceiptDiscount / 100m;
                TicketAmount = (decimal)incompleteTicket.TotalReceiptAmount / 100m;
                CurrentAmount = paymentAmount;
                TotalAmountPaid = (decimal)incompleteTicket.TotalReceiptPayment / 100m + CurrentAmount;
                DiscountAmount = (decimal)incompleteTicket.TotalReceiptDiscount / 100m;
                IsPartialPayment = true;
                IsFirsPayment = false;
                IsLastPayment = TicketAmount - TotalAmountPaid == 0m;
                IsDiscount = DiscountAmount > 0m;
                PaymentType = incompleteTicket.stPayment[0].typeOfPayment.ToString();
            }
        }

        public void UpdatePaidAmount(decimal amount)
        {
            if (!(TotalAmountPaid + amount > TicketAmount))
            {
                TotalAmountPaid += amount;
                CurrentAmount = amount;
                IsFirsPayment = false;
                IsLastPayment = TotalAmountPaid + DiscountAmount == TotalItemAmount;
            }
            else
            {
                IsFirsPayment = false;
            }
        }

        public void ReverseUpdate()
        {
            TotalAmountPaid -= CurrentAmount;
            CurrentAmount = 0m;
            IsLastPayment = false;
        }

        public void FirstPartialPaymentFail()
        {
            IsFirsPayment = false;
            CurrentAmount = 0m;
            TotalAmountPaid = 0m;
        }

        public void RollbackFailedPayment()
        {
            TotalAmountPaid -= CurrentAmount;
        }
    }

}
