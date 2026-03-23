using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public struct ST_BilgiFisleri
    {
        public ST_BilgiFisi stFaturaBilgi;

        public ST_BilgiFisi stYemekKarti;

        public ST_BilgiFisi stAvans;

        public ST_BilgiFisi stFaturaTahsilati;

        public ST_BilgiFisi stCariHesap;

        public ST_BilgiFisi stDiger;

        public ST_BilgiFisi stGenelToplam;

        public string OtoparkFisiAdedi;

        public string MaliFisYemekKartiTutari;

        public string MaliFisFaturaTahsilatTutari;

        public string MaliFisDigerMatrahsiz;
    }

}
