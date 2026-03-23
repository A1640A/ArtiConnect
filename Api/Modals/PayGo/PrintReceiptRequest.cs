using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.PayGo
{
    public class PrintReceiptRequest
    {
        public CashierLoginRequest CashierLogin { get; set; }
        public OpenDocumentRequest OpenDocument { get; set; }
        public List<BatchItem> BatchItems { get; set; }
        public DoPaymentRequest Payment { get; set; }
        public CloseDocumentRequest CloseDocument { get; set; }
    }
}
