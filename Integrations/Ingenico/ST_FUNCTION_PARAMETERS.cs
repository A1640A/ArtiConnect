using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public struct ST_FUNCTION_PARAMETERS
    {
        public uint EKUNo;

        public ST_FUNCTION_PARAMETERS_PASSWORD Password;

        public ST_FUNCTION_PARAMETERS_POINT start;

        public ST_FUNCTION_PARAMETERS_POINT finish;
    }

}
