using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.Printer
{
    public class PrintLabelRequestModal
    {
        public string ProductName { get; set; }
        public string Barcode { get; set; }
        public float Price { get; set; }

        public string FileName { get; set; }
        public int CopyNumber { get; set; }
        public string PrinterName { get; set; }
    }
}