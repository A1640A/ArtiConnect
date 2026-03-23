using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_PaymentErrMessage
    {
        public string ErrorCode;

        public string ErrorMsg;

        public string AppErrorCode;

        public string AppErrorMsg;

        public ST_PaymentErrMessage()
        {
            ErrorCode = "";
            ErrorMsg = "";
            AppErrorCode = "";
            AppErrorMsg = "";
        }
    }

}
