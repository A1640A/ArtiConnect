using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.PayGo
{
    public class SetDepartmentRequest
    {
        public string Amount { get; set; }
        public string DepartmentId { get; set; }
        public string VatGroup { get; set; }
        public string ItemName { get; set; }
        public string DepLimitAmount { get; set; }
    }
}
