using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    [Serializable]
    public struct ST_TAX_RATE
    {
        [MarshalAs(UnmanagedType.U2)]
        public ushort taxRate;
    } 
}
