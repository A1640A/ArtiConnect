using Inpos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.Inpos
{ 
    public class AddSaleItemRequest
    {
        public string ItemName { get; set; }
        public UInt64 UnitPrice { get; set; }
        public UInt32 Multiplier { get; set; }
        public Int32 DiscountRate { get; set; }
        public UInt64 DiscountAmount { get; set; }
        public byte Section { get; set; }
        public Unit Unit { get; set; }
    }
}
