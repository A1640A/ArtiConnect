using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public enum EPaymentSubtypes
    {
        PAYMENT_SUBTYPE_PROCESS_ON_POS,
        PAYMENT_SUBTYPE_SALE,
        PAYMENT_SUBTYPE_INSTALMENT_SALE,
        PAYMENT_SUBTYPE_LOYALTY_PUAN,
        PAYMENT_SUBTYPE_ADVANCE_REFUND,
        PAYMENT_SUBTYPE_INSTALLMENT_REFUND,
        PAYMENT_SUBTYPE_REFERENCED_REFUND,
        PAYMENT_SUBTYPE_REFERENCED_REFUND_WITH_CARD,
        PAYMENT_SUBTYPE_REFERENCED_REFUND_WITHOUT_CARD
    }

}
