using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.PayGo
{
    public class DoTransactionRequest
    {
        public string Amount { get; set; }
        public string ProcessType { get; set; }
        public string DepartmentId { get; set; }
        public string PLUID { get; set; }
        public string NonTaxId { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
        public string Barcode { get; set; }
        public string Rate { get; set; }
        public string ItemName { get; set; }
        public string TranId { get; set; }
        public string CollectionId { get; set; }
        public FreeTextInfo FreeText { get; set; }
    }
}
