using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.PayGo
{
    public class SetPLURequest
    {
        public string Amount { get; set; }
        public string PLUNo { get; set; }
        public string Barcode { get; set; }
        public string ItemName { get; set; }
        public string GroupNo { get; set; }
        public string StockControl { get; set; }
        public string StockPiece { get; set; }
        public bool PrintPLUSlip { get; set; }
    }
}
