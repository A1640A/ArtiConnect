using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public enum EVasType
    {
        TLV_OKC_ASSIST_VAS_TYPE_ADISYON = 1,
        TLV_OKC_ASSIST_VAS_TYPE_IN_FLIGHT = 2,
        TLV_OKC_ASSIST_VAS_TYPE_INGENICO = 3,
        TLV_OKC_ASSIST_VAS_TYPE_OTHER = 4,
        TLV_OKC_ASSIST_VAS_TYPE_AKTIFNOKTA = 5,
        TLV_OKC_ASSIST_VAS_TYPE_MOBIL_ODEME = 6,
        TLV_OKC_ASSIST_VAS_TYPE_OTOPARK = 7,
        TLV_OKC_ASSIST_VAS_TYPE_YEMEKCEKI = 8,
        TLV_OKC_ASSIST_VAS_TYPE_LOYALTY = 9,
        TLV_OKC_ASSIST_VAS_TYPE_PAYMENT = 10,
        TLV_OKC_ASSIST_VAS_TYPE_ALL = 256
    }
}
