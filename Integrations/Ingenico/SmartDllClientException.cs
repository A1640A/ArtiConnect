using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class SmartDllClientException : Exception
    {
        public uint DllErrorCode { get; set; }

        public SmartDllClientException(uint dllErrorCode)
        {
            DllErrorCode = dllErrorCode;
        }
    }

}
