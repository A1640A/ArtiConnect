using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.PayGo
{
    public class OpenDocumentRequest
    {
        public string Amount { get; set; }
        public string TranDate { get; set; }
        public string TranTime { get; set; }
        public string DocumentType { get; set; }
        public string Vkn { get; set; }
        public string BillSerialNo { get; set; }
        public string DispatchNote { get; set; }
        public string OrderNo { get; set; }
        public string IsLaterOn { get; set; }
        public string OwnerName { get; set; }
        public string MerchantNo { get; set; }
        public string IsTakeComm { get; set; }
        public string Plate { get; set; }
        public string Title { get; set; }
        public string Commision { get; set; }
    }
}
