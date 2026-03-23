using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class TransactionHandle
    {
        public string FriendlyName { get; set; }

        public ulong Index { get; set; }

        public bool Active { get; set; }
    }

}
