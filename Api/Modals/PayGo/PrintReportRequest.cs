using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.PayGo
{
    public class PrintReportRequest
    {
        public string AcquirerId { get; set; }
        public string ZNum { get; set; }
        public string ReceiptNum { get; set; }
        public string ReportType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string StartZNo { get; set; }
        public string EndZNo { get; set; }
        public string StartRNo { get; set; }
        public string EndRNo { get; set; }
        public string StartPLUNo { get; set; }
        public string EndPLUNo { get; set; }
    }
}
