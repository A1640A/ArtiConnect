using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_DM_REPORT
    {
        public int StructSize;

        public ushort versiyon;

        public string IsyeriVKN;

        public string RaporUretilmeTarihi;

        public string RaporUretilmeSaati;

        public string AylikRaporNo;

        public string GunlukRaporNo;

        public string RaporDonemiBaslangicTarihi;

        public string RaporDonemiBitisTarihi;

        public string KDV_GrupAdedi;

        public ST_KDV_Grubu[] stKDV_Grubu = new ST_KDV_Grubu[8];

        public string ToplamKDV_Tutari;

        public string ToplamSatisTutari;

        public string BeyanEdilecekKDV_Tutari;

        public string IndirimToplamTutari;

        public string ArtirimToplamTutari;

        public string KumulatifToplamKDV_Tutari;

        public string KumulatifToplamSatisTutari;

        public string IptalEdilenBelgeAdedi;

        public string IptalEdilenBelgeToplamTutari;

        public ST_OKC_Belge stOKC_Belge;

        public ST_OdemeToplami stOdemeToplami;

        public string EkuNo;
    }

}
