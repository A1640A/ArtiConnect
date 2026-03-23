using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.PayGo
{
    public class AddBatchItemRequest
    {
        public string BatchId { get; set; }
        public BatchItem Item { get; set; }
    }
}
