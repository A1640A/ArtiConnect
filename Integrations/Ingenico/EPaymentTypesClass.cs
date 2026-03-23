using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class EPaymentTypesClass
    {
        public const ulong PAYMENT_ALL = 18442240474083229695uL;

        public const ulong PAYMENT_CASH_TL = 1uL;

        public const ulong PAYMENT_CASH_CURRENCY = 2uL;

        public const ulong PAYMENT_BANK_CARD = 4uL;

        public const ulong PAYMENT_YEMEKCEKI = 8uL;

        public const ulong PAYMENT_MOBILE = 16uL;

        public const ulong PAYMENT_HEDIYE_CEKI = 32uL;

        public const ulong PAYMENT_IKRAM = 64uL;

        public const ulong PAYMENT_ODEMESIZ = 128uL;

        public const ulong PAYMENT_KAPORA = 256uL;

        public const ulong PAYMENT_PUAN = 512uL;

        public const ulong PAYMENT_GIDER_PUSULASI = 1024uL;

        public const ulong PAYMENT_BANKA_TRANSFERI = 2048uL;

        public const ulong PAYMENT_CEK = 4096uL;

        public const ulong PAYMENT_ACIK_HESAP = 8192uL;

        public const ulong PAYMENT_DIGER = 16384uL;

        public const ulong PAYMENT_EXTERNAL_BANK = 32768uL;

        public const ulong PAYMENT_SANAL_POS = 65536uL;

        public const ulong PAYMENT_EPARA_HIZLI_PARA = 131072uL;

        public const ulong PAYMENT_ULASIM_KARTI = 262144uL;

        public const ulong PAYMENT_COMBINED = 524288uL;

        public const ulong PAYMENT_TR_KAREKOD_CARD = 4503599627370496uL;

        public const ulong PAYMENT_TR_KAREKOD_FAST = 9007199254740992uL;

        public const ulong PAYMENT_TR_KAREKOD_MOBIL = 18014398509481984uL;

        public const ulong PAYMENT_TR_KAREKOD_DIGER = 36028797018963968uL;

        public const ulong REVERSE_PAYMENT_ALL = 4503599626321920uL;

        public const ulong REVERSE_PAYMENT_CASH = 1048576uL;

        public const ulong REVERSE_PAYMENT_BANK_CARD_VOID = 2097152uL;

        public const ulong REVERSE_PAYMENT_BANK_CARD_REFUND = 4194304uL;

        public const ulong REVERSE_PAYMENT_YEMEKCEKI = 8388608uL;

        public const ulong REVERSE_PAYMENT_MOBILE = 16777216uL;

        public const ulong REVERSE_PAYMENT_HEDIYE_CEKI = 33554432uL;

        public const ulong REVERSE_PAYMENT_PUAN = 67108864uL;

        public const ulong REVERSE_PAYMENT_ACIK_HESAP = 134217728uL;

        public const ulong REVERSE_PAYMENT_KAPORA = 268435456uL;

        public const ulong REVERSE_PAYMENT_GIDER_PUSULASI = 536870912uL;

        public const ulong REVERSE_PAYMENT_BANKA_TRANSFERI = 1073741824uL;

        public const ulong REVERSE_PAYMENT_CEK = 2147483648uL;

        public const ulong REVERSE_PAYMENT_IKRAM = 4294967296uL;

        public const ulong REVERSE_PAYMENT_ODEMESIZ = 8589934592uL;

        public const ulong REVERSE_PAYMENT_DIGER = 17179869184uL;

        public const ulong REVERSE_TR_KAREKOD_CARD = 34359738368uL;

        public const ulong REVERSE_TR_KAREKOD_FAST = 68719476736uL;

        public const ulong REVERSE_TR_KAREKOD_MOBIL = 137438953472uL;

        public const ulong REVERSE_TR_KAREKOD_DIGER = 274877906944uL;
    }

}
