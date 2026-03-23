using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public enum ETransactionFiscalType
    {
        TRANSACTION_FISCAL_TYPE_SALE,
        TRANSACTION_FISCAL_TYPE_REFUND,
        TRANSACTION_FISCAL_TYPE_VOID,
        TRANSACTION_FISCAL_TYPE_NON_FISCAL_SALE,
        TRANSACTION_FISCAL_TYPE_INFO
    }

}
