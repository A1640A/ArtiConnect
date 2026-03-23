using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.PayGo
{
    public class DrawerRequest
    {
        public string Amount { get; set; }
        public string DrawerStatus { get; set; }
        public FreeTextInfo FreeText { get; set; }
    }
}
