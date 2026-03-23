using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.Hugin
{
    public class Enums
    {
        public enum FormLanguage
        {
            TR,
            EN
        }

        public enum SubOperationType
        {
            SATIS = 1,
            YBELGE = 10,
            TAKSITLI_SATIS = 04,
            VADE_FARKLI_SATIS = 03,
            PUANLI_SATIS = 16,
            E_IADE = 20,
            KISMI_IADE = 22,
            KONTOR_SATIS = 13,
            PUAN_SORGU = 14,
            KK_BORC_ODEME = 05,
            PREPAID_NAKIT_YUKLEME = 29,
            PREPAID_KARTLI_YUKLEME = 30,
            CASHBACK = 12
        }

        public enum AdjustmentType : sbyte
        {
            Fee,
            PercentFee,
            Discount,
            PercentDiscount
        }
    }
}
