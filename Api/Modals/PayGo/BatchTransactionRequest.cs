using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.PayGo
{
    public class BatchTransactionRequest
    {
        public List<BatchItem> BatchItems { get; set; }
    }
}
