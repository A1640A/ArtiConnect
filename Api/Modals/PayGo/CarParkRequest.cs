using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.PayGo
{
    public class CarParkRequest
    {
        public string Amount { get; set; }
        public string PaymentType { get; set; }
        public string PlateNo { get; set; }
    }
}
