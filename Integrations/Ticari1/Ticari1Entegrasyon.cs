using ArtiConnect.com.ticari1.ews;
using DevExpress.XtraPdfViewer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArtiConnect.Integrations.Ticari1.Modals;
using System.Windows.Forms;
using ArtiConnect.Extensions;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ArtiConnect.Integrations.Ticari1
{
    public static class Ticari1Entegrasyon
    {
        public static string StringDuzelt(string metin)
        {
            if (string.IsNullOrEmpty(metin))
                return metin;

            return metin.Replace(" ", "").Replace("\n", "").Trim();
        }

        public static ResultModal EFaturaOlustur(string apiKey, string secretKey, int customerId, string subeNo, EFaturaModal eFaturaModal)
        {
            apiKey = StringDuzelt(apiKey);
            secretKey = StringDuzelt(secretKey);
            subeNo = StringDuzelt(subeNo);

            if (!eFaturaModal.FaturaSatirlari.Any()) return null;

            var t1WS = new com.ticari1.ews.Ticari1_eInvoice_Con_Web_Services();
            var checkResponse = t1WS.checkService();
            var loginResponse = t1WS.userLogin(apiKey, secretKey, customerId);

            var invObj = new Tic1_InvoiceReq()
            {
                API_KEY = apiKey,
                SECRET_KEY = secretKey,
                MUSTERI_ID = customerId,
            };

            var Invoice = new Tic1_Invoice()
            {
                GONDERICI_VKN_TCKN = eFaturaModal.GondericiVkn,
                GONDERIM_SEKLI = 1,
                REFERANS_BELGE_ID = "",
                ALICI_VKN_TCKN = eFaturaModal.AliciVkn,
                ALICI_UNVANI = eFaturaModal.AliciUnvani,
                ALICI_AD = eFaturaModal.AliciAdi,
                ALICI_SOYAD = eFaturaModal.AliciSoyadi,
                ALICI_VERGI_DAIRESI = eFaturaModal.AliciVergiDairesi,
                ALICI_CADDE_SOKAK = eFaturaModal.AliciCaddeSokak,
                ALICI_BINA_ADI = "",
                ALICI_BINA_NO = "",
                ALICI_KAPI_NO = "",
                ALICI_SEMT = "",
                ALICI_ILCE = eFaturaModal.AliciIlce,
                ALICI_IL = eFaturaModal.AliciIl,
                ALICI_ULKE = eFaturaModal.AliciUlke,
                ALICI_MAIL = eFaturaModal.AliciMail,
                ALICI_TELEFON = eFaturaModal.AliciTelefon,
                FATURA_TARIHI = eFaturaModal.FaturaTarihi,
                FATURA_TIPI = "FATURA",
                SENARYO_TURU = "",
                FATURA_TURU = "SATIS",
                FATURA_PB = "TRY",
                FATURA_PB_KURU = "TRY",
                FATURA_ACIKLAMASI = eFaturaModal.AciklamaSatirlari,
                DEPARTMAN_ID = subeNo
            };

            foreach (var faturaSatiri in eFaturaModal.FaturaSatirlari)
                Invoice.FATURA_SATIRLARI.Add(new Tic1_InvRow()
                {
                    URUN_ADI = faturaSatiri.UrunAdi,
                    URUN_MIKTARI = faturaSatiri.Miktari,
                    URUN_BIRIM_FIYATI = (faturaSatiri.BirimFiyati - (faturaSatiri.KdvTutari / faturaSatiri.Miktari)).ToString("n2").ToFloat(),
                    URUN_FIYAT_PB = "TRY",
                    URUN_BIRIMI = faturaSatiri.Birimi,
                    URUN_KDV_ORANI = faturaSatiri.KdvOrani,
                    KDV_TUTARI = faturaSatiri.KdvTutari.ToString("n2").ToFloat(),
                    MAL_HIZMET_TUTARI = (faturaSatiri.Tutar - faturaSatiri.KdvTutari).ToString("n2").ToFloat(),
                    ISKONTO_ORANI = 0F,
                    ISKONTO_TUTARI = 0F,
                });
            invObj.invoices.Add(Invoice);

            var invJSon = JsonConvert.SerializeObject(invObj);
            File.WriteAllText($"ticari1Response_{DateTime.Now.ToString("dd.MM.yyyy HH.mm.ssss")}", invJSon);
            var t1WsResponse = t1WS.postInvoiceJson(invJSon);
            var ws_resp = JObject.Parse(t1WsResponse);

            var invoicesResponse = ws_resp["RESP_INVOICES"][0].ToString();
            var response = JsonConvert.DeserializeObject<ResultModal>(invoicesResponse);
            response.IsSuccess = (bool)ws_resp["IS_SUCCESS"];

            return response;
        }

        [Obsolete]
        public static void PrintPdf(string filePath, string YaziciAdi, int KopyaSayisi = 1)
        {
            try
            {
                using (PdfViewer pdfViewer = new PdfViewer())
                {
                    pdfViewer.LoadDocument(filePath);

                    // DevExpress.Pdf.PdfPrinterSettings yerine PrinterSettings kullanalım
                    var printerSettings = new System.Drawing.Printing.PrinterSettings
                    {
                        PrinterName = YaziciAdi,
                        Copies = (short)KopyaSayisi
                    };

                    // Siyah beyaz yazdırma Ayarı
                    printerSettings.DefaultPageSettings.Color = false;

                    // Print işlemini gerçekleştir
                    pdfViewer.Print(printerSettings);
                }
            }
            catch (Exception)
            { }
        }

        [Obsolete]
        public static bool GetEFaturaPdf(string apiKey, string secretKey, int customerId, GetEFaturaPdfModal getEFaturaPdfModal, string yaziciAdi, int kopyaSayisi = 1)
        {
            apiKey = StringDuzelt(apiKey);
            secretKey = StringDuzelt(secretKey);

            var t1WS = new com.ticari1.ews.Ticari1_eInvoice_Con_Web_Services();
            var checkResponse = t1WS.checkService();
            var loginResponse = t1WS.userLogin(apiKey, secretKey, customerId);

            var invObj = new Tic1_InvoiceReq()
            {
                API_KEY = apiKey,
                SECRET_KEY = secretKey,
                MUSTERI_ID = customerId,
            };

            var t1WsResponse = t1WS.getInvPDF(apiKey, secretKey, customerId, getEFaturaPdfModal.Vkn, getEFaturaPdfModal.FaturaKey, getEFaturaPdfModal.FaturaTuru, "", "", getEFaturaPdfModal.FaturaNo);

            if (t1WsResponse.eDoc_Pdf != null)
            {
                string fileName = "tempTicari1EFatura.pdf";
                File.WriteAllBytes(fileName, t1WsResponse.eDoc_Pdf);

                if (!string.IsNullOrEmpty(yaziciAdi))
                    PrintPdf(fileName, yaziciAdi, kopyaSayisi);
            }

            var r = $"{t1WsResponse.err_detail}\n{t1WsResponse.resp_msg}";
            return t1WsResponse.resp_code == "200" ? true : false;
        }

        [Obsolete]
        public static (string, bool) EArsivIptal(string apiKey, string secretKey, int customerId, string faturaKey)
        {
            apiKey = StringDuzelt(apiKey);
            secretKey = StringDuzelt(secretKey);

            var t1WS = new com.ticari1.ews.Ticari1_eInvoice_Con_Web_Services();
            var checkResponse = t1WS.checkService();
            var loginResponse = t1WS.userLogin(apiKey, secretKey, customerId);

            var response = t1WS.getInvCancel(apiKey, secretKey, customerId, faturaKey, DateTime.Now.ToString("yyyy-MM-dd"), "İptal Edildi", "ARTI", "ARTI");
            return (response.resp_msg, response.resp_status.HasValue ? response.resp_status.Value : false);
        }

        [Obsolete]
        public static EinvGetInvXml GetFaturaXml(string apiKey, string secretKey, int customerId, string vkn, string faturaKey)
        {
            apiKey = StringDuzelt(apiKey);
            secretKey = StringDuzelt(secretKey);

            var t1WS = new com.ticari1.ews.Ticari1_eInvoice_Con_Web_Services();
            var checkResponse = t1WS.checkService();
            var loginResponse = t1WS.userLogin(apiKey, secretKey, customerId);

            var response = t1WS.getInvXML(apiKey, secretKey, customerId, vkn, faturaKey, 1, "ARTI", "ARTI", "");
            return response;
        }

        [Obsolete]
        public static EinvGetInvXmlColl GetFaturaXmlColl(string apiKey, string secretKey, int customerId, string vkn, DateTime fDate, DateTime lDate)
        {
            apiKey = StringDuzelt(apiKey);
            secretKey = StringDuzelt(secretKey);

            var t1WS = new com.ticari1.ews.Ticari1_eInvoice_Con_Web_Services();
            var checkResponse = t1WS.checkService();
            var loginResponse = t1WS.userLogin(apiKey, secretKey, customerId);

            var response = t1WS.getInvXMLColl(apiKey, secretKey, customerId, vkn, fDate, lDate, 0, "ARTI", "ARTI");
            return response;
        }

        public class Ticari1Fatura
        {
            public string Vkn { get; set; }
            public string FaturaNo { get; set; }
            public string Senaryo { get; set; } //profile
            public DateTime Tarih { get; set; }
            public string FirmaAdi { get; set; }
            public string Durum { get; set; }
            public string Tip { get; set; } //name
            public string Tur { get; set; } //code
            public float Matrah { get; set; }
            public float Toplam { get; set; }
            public float Kdv { get; set; }
            public string Identifier { get; set; }
        }

        static object GetPropertyByName(object obj, string propertyName)
        {
            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo != null)
            {
                var result = propertyInfo.GetValue(obj);
                return result;
            }
            else
            {
                return null;
            }
        }

        [Obsolete]
        public static List<Ticari1Fatura> GetGelenFaturaList(string apiKey, string secretKey, int customerId, DateTime fDate, DateTime lDate)
        {
            apiKey = StringDuzelt(apiKey);
            secretKey = StringDuzelt(secretKey);

            var t1WS = new com.ticari1.ews.Ticari1_eInvoice_Con_Web_Services();
            var checkResponse = t1WS.checkService();
            var loginResponse = t1WS.userLogin(apiKey, secretKey, customerId);

            var invObj = new Tic1_InvoiceReq()
            {
                API_KEY = apiKey,
                SECRET_KEY = secretKey,
                MUSTERI_ID = customerId,
            };

            var response = t1WS.getInvList(apiKey, secretKey, customerId, "", "", fDate, lDate, 0, 0, 0);

            if (!response.Invoices.Any()) return null;

            var result = new List<Ticari1Fatura>();
            foreach (var fatura in response.Invoices)
                result.Add(
                    new Ticari1Fatura()
                    {
                        Vkn = GetPropertyByName(fatura, "doc_sender_receiver_tax") as string,
                        FaturaNo = GetPropertyByName(fatura, "doc_number") as string,
                        Senaryo = GetPropertyByName(fatura, "doc_profile") as string,
                        Tarih = Convert.ToDateTime(GetPropertyByName(fatura, "doc_issue_date")),
                        FirmaAdi = GetPropertyByName(fatura, "doc_sender_receiver_name") as string,
                        Durum = GetPropertyByName(fatura, "doc_status") as string,
                        Tip = GetPropertyByName(fatura, "doc_type_name") as string,
                        Tur = GetPropertyByName(fatura, "doc_type_code") as string,
                        Matrah = GetPropertyByName(fatura, "line_ext_amount").ToString().ToFloat(),
                        Toplam = GetPropertyByName(fatura, "pay_amount").ToString().ToFloat(),
                        Kdv = GetPropertyByName(fatura, "taxamount").ToString().ToFloat(),
                        Identifier = GetPropertyByName(fatura, "doc_identifier").ToString(),
                    }
                    );

            return result;
        }
    }
}
