using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ticari1
{
    public class Modals
    {
        public class ResultModal
        {
            public string MSG { get; set; }
            public string RESPONSE_LNK { get; set; }
            public int RESP_CODE { get; set; }
            public string INVOICE_TYPE { get; set; }
            public string INVOICE_NUMBER { get; set; }
            public bool IS_OK { get; set; }
            public string RESPONSE_ID { get; set; }
            public string ERR_DETAIL { get; set; }
            public bool IsSuccess { get; set; }
        }


        public class EFaturaModal
        {
            public string GondericiVkn { get; set; }
            public string AliciVkn { get; set; }
            public string AliciUnvani { get; set; }
            public string AliciAdi { get; set; }
            public string AliciSoyadi { get; set; }
            public string AliciVergiDairesi { get; set; }
            public string AliciCaddeSokak { get; set; }
            public string AliciIlce { get; set; }
            public string AliciIl { get; set; }
            public string AliciUlke { get; set; }
            public string AliciMail { get; set; }
            public string AliciTelefon { get; set; }
            public string FaturaTarihi { get; set; }

            public List<EFaturaSatiri> FaturaSatirlari = new List<EFaturaSatiri>();
            public List<Tic1_Description> AciklamaSatirlari = new List<Tic1_Description>();
        }

        public class EFaturaSatiri
        {
            public string UrunAdi { get; set; }
            public float Miktari { get; set; }
            public float BirimFiyati { get; set; }
            public string Birimi { get; set; }
            public float KdvOrani { get; set; }

            public float Tutar
            {
                get
                {
                    return Miktari * BirimFiyati;
                }
            }

            public float KdvTutari
            {
                get
                {
                    return (Tutar * KdvOrani) / (100 + KdvOrani);
                }
            }
        }

        public class GetEFaturaPdfModal
        {
            public string Vkn { get; set; }
            public string FaturaKey { get; set; }
            public double FaturaTuru { get; set; } = 1;
            public string FaturaNo { get; set; }
        }


        public class Tic1_InvoiceReq
        {
            public string API_KEY { get; set; }
            public string SECRET_KEY { get; set; }
            public int MUSTERI_ID { get; set; }
            public IList<Tic1_Invoice> invoices = new List<Tic1_Invoice>();
        }
        public class Tic1_Invoice
        {
            public string GONDERICI_VKN_TCKN { get; set; }
            public int GONDERIM_SEKLI { get; set; } = 1;
            public string REFERANS_BELGE_ID { get; set; }
            public string ALICI_VKN_TCKN { get; set; }
            public string ALICI_UNVANI { get; set; }
            public string ALICI_AD { get; set; }
            public string ALICI_SOYAD { get; set; }
            public string FATURA_TARIHI { get; set; }
            public float FATURA_MAL_HIZMET_TOPLAMI { get; set; }
            public float FATURA_ALTI_INDIRIM { get; set; }
            public float FATURA_TOPLAM_INDIRIM { get; set; }
            public float FATURA_KDV_TOPLAMI { get; set; }
            public float FATURA_YUVARLAMA_TUTARI { get; set; }
            public float FATURA_ODENECEK_TOPLAM { get; set; }
            public string FATURA_TIPI { get; set; }
            public string SENARYO_TURU { get; set; } = "TEMEL";
            public string FATURA_TURU { get; set; }
            public string FATURA_PB { get; set; }
            public string FATURA_PB_KURU { get; set; }
            public string ALICI_VERGI_DAIRESI { get; set; }
            public string ALICI_CADDE_SOKAK { get; set; }
            public string ALICI_BINA_ADI { get; set; }
            public string ALICI_BINA_NO { get; set; }
            public string ALICI_KAPI_NO { get; set; }
            public string ALICI_SEMT { get; set; }
            public string ALICI_ILCE { get; set; }
            public string ALICI_IL { get; set; }
            public string ALICI_ULKE { get; set; }
            public string ALICI_MAIL { get; set; }
            public string ALICI_TELEFON { get; set; }
            public IList<Tic1_InvRow> FATURA_SATIRLARI = new List<Tic1_InvRow>();
            public IList<Tic1_Description> FATURA_ACIKLAMASI = new List<Tic1_Description>();
            public string DEPARTMAN_ID { get; set; }
        }
        public class Tic1_InvRow
        {
            public string URUN_ADI { get; set; }
            public float URUN_MIKTARI { get; set; }
            public string URUN_BIRIMI { get; set; }
            public float URUN_BIRIM_FIYATI { get; set; }
            public string URUN_FIYAT_PB { get; set; }
            public float URUN_KDV_ORANI { get; set; }
            public float KDV_TUTARI { get; set; }
            public float MAL_HIZMET_TUTARI { get; set; }
            public float ISKONTO_ORANI { get; set; }
            public float ISKONTO_TUTARI { get; set; }
        }

        public class Tic1_Description
        {
            public string FATURA_NOTU { get; set; }
        }
    }
}
