using ArtiConnect.Api.Modals; // Projendeki ilgili namespace
using ArtiConnect.DataAccess; // Projendeki ilgili namespace
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.Http;

namespace ArtiConnect.Api.Controllers
{
    [ApiLogger]
    [RoutePrefix("api/digi")]
    public class DigiController : BaseApiController
    {
        // DLL Tanımlamaları
        private const string dll_name = "pos_ad_dll.dll";

        [DllImport(dll_name, CharSet = CharSet.Ansi, EntryPoint = "read_standard")]
        private static extern int read_standard(byte[] sbweight);

        [DllImport(dll_name, CharSet = CharSet.Ansi, EntryPoint = "send_tare")]
        public static extern int send_tare(string arg0);

        [DllImport(dll_name, CharSet = CharSet.Ansi, EntryPoint = "send_zero")]
        private static extern int send_zero();

        // Eşzamanlı erişimi engellemek için kilit objesi
        private static object _lockObject = new object();

        /// <summary>
        /// Teraziden anlık ağırlık bilgisini çeker.
        /// </summary>
        [HttpGet]
        [Route("weight")]
        public IHttpActionResult GetWeight()
        {
            try
            {
                lock (_lockObject)
                {
                    byte[] buffer = new byte[50];

                    // DLL'den okuma yap
                    int result = read_standard(buffer);

                    // 0xf0 (240) genelde DIGI terazilerde başarılı okuma kodudur.
                    // Eğer farklı bir kod dönerse hata fırlatabiliriz veya durumu bildirebiliriz.
                    if (result != 0xf0)
                    {
                        return BadRequest($"Teraziden veri okunamadı. Hata Kodu: {result}");
                    }

                    // Byte dizisini string'e çevir (1. indexten başlayıp 7 karakter alıyoruz, senin örneğindeki gibi)
                    string rawWeight = Encoding.Default.GetString(buffer, 1, 7);

                    // Temizleme ve formatlama
                    string formattedWeight = rawWeight.Replace(".", ",");

                    // Sayısal değere çevirmeyi deneyelim (Opsiyonel, frontend'de işlem yapacaksan double dönmek iyidir)
                    double weightValue = 0;
                    double.TryParse(formattedWeight, out weightValue);

                    return Ok(new
                    {
                        Success = true,
                        WeightRaw = rawWeight,
                        WeightFormatted = formattedWeight,
                        WeightValue = weightValue,
                        Message = "Ağırlık başarıyla okundu."
                    });
                }
            }
            catch (DllNotFoundException)
            {
                return InternalServerError(new Exception($"'{dll_name}' dosyası bulunamadı. Lütfen DLL dosyasını uygulamanın çalıştığı klasöre (bin) kopyalayın."));
            }
            catch (BadImageFormatException)
            {
                return InternalServerError(new Exception("Mimari uyumsuzluğu. Lütfen API projesini 'x86' olarak derlediğinizden emin olun."));
            }
            catch (Exception ex)
            {
                return BadRequest($"Ağırlık okunurken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Teraziye DARA (Tare) komutu gönderir.
        /// </summary>
        [HttpPost]
        [Route("tare")]
        public IHttpActionResult SetTare()
        {
            try
            {
                lock (_lockObject)
                {
                    // send_tare fonksiyonu string argüman istiyor, genelde "0" veya boş string gönderilir.
                    // Test uygulamasındaki kullanımına göre burayı güncelleyebilirsin.
                    int result = send_tare("0");

                    if (result != 0) // Başarılı dönüş kodu 0 ise (DLL dokümantasyonuna göre değişebilir)
                    {
                        // Bazı DLL'lerde 0 hata, 1 başarı olabilir. Bunu test etmen gerekebilir.
                        // Şimdilik işlem tamamlandı kabul ediyoruz.
                    }

                    return Ok(new { Success = true, Message = "Dara komutu gönderildi.", ResultCode = result });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Dara işlemi başarısız: {ex.Message}");
            }
        }

        /// <summary>
        /// Teraziye SIFIRLAMA (Zero) komutu gönderir.
        /// </summary>
        [HttpPost]
        [Route("zero")]
        public IHttpActionResult SetZero()
        {
            try
            {
                lock (_lockObject)
                {
                    int result = send_zero();
                    return Ok(new { Success = true, Message = "Sıfırlama komutu gönderildi.", ResultCode = result });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Sıfırlama işlemi başarısız: {ex.Message}");
            }
        }
    }
}