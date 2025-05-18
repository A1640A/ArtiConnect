using ArtiConnect.DataAccess;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Windows.Forms;

namespace ArtiConnect.Api.Controllers
{
    [ApiLogger]
    [RoutePrefix("api/yemeksepeti")]
    public class YemekSepetiController : BaseApiController
    {
        private AppDbContext db = new AppDbContext();
        private static Process pythonProcess;
        private static string bearerToken;
        private static readonly object lockObject = new object();

        /// <summary>
        /// Yemeksepeti oturum açma işlemini görünür modda başlatır
        /// </summary>
        [HttpGet]
        [Route("login/show")]
        public async Task<IHttpActionResult> RunPythonScriptShow()
        {
            return await RunPythonScriptInternal("--show");
        }

        /// <summary>
        /// Yemeksepeti oturum açma işlemini gizli modda başlatır
        /// </summary>
        [HttpGet]
        [Route("login/hide")]
        public async Task<IHttpActionResult> RunPythonScriptHide()
        {
            return await RunPythonScriptInternal("--hide");
        }

        /// <summary>
        /// Mevcut Bearer token'ı getirir
        /// </summary>
        [HttpGet]
        [Route("token")]
        public IHttpActionResult GetBearerToken()
        {
            try
            {
                string bearerFilePath = GetBearerFilePath();

                if (File.Exists(bearerFilePath))
                {
                    string token = File.ReadAllText(bearerFilePath).Trim();
                    return Ok(new { Success = true, Token = token });
                }
                else
                {
                    return BadRequest("Bearer token dosyası bulunamadı. Önce login işlemi yapmalısınız.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Bearer token alınırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Yemeksepeti teslimatları getirir
        /// </summary>
        [HttpGet]
        [Route("deliveries")]
        public IHttpActionResult GetDeliveries(DateTime? fromDate = null)
        {
            try
            {
                if (!fromDate.HasValue)
                {
                    fromDate = DateTime.UtcNow.AddDays(-1);
                }

                string bearerFilePath = GetBearerFilePath();

                if (!File.Exists(bearerFilePath))
                {
                    return BadRequest("Bearer token dosyası bulunamadı. Önce login işlemi yapmalısınız.");
                }

                string token = File.ReadAllText(bearerFilePath).Trim();

                // Tarih formatını ISO 8601 formatına dönüştürme ve URL kodlama
                string formattedDate = Uri.EscapeDataString(fromDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));

                // RestClient ayarlarını yapılandırma
                var options = new RestClientOptions("https://vendor-api-tr.me.restaurant-partners.com")
                {
                    MaxTimeout = -1,
                };

                // RestClient oluşturma
                var client = new RestClient(options);

                // İstek oluşturma
                var request = new RestRequest($"/api/2/deliveries-web?from={formattedDate}", Method.Get);

                // Header'ları ekleme
                request.AddHeader("Accept-Language", "tr-TR,tr;q=0.9");
                request.AddHeader("authorization", $"{token}");

                // İsteği gönderme ve yanıtı alma
                RestResponse response = client.Execute(request);

                // Yanıt başarılıysa içeriği döndürme
                if (response.IsSuccessful)
                {
                    return Ok(new { Success = true, Data = response.Content });
                }
                else
                {
                    // Hata durumunda hata mesajını döndürme
                    return BadRequest($"Hata: ({(int)response.StatusCode}) {response.StatusCode} - {response.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                // İstisna durumunda hata mesajını döndürme
                return BadRequest($"İstek sırasında bir hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Python işlemini durdurur
        /// </summary>
        [HttpPost]
        [Route("stop")]
        public IHttpActionResult StopPythonProcess()
        {
            try
            {
                if (pythonProcess != null && !pythonProcess.HasExited)
                {
                    pythonProcess.Kill();
                    pythonProcess = null;
                    return Ok(new { Success = true, Message = "Python işlemi başarıyla durduruldu." });
                }
                else
                {
                    return Ok(new { Success = false, Message = "Çalışan bir Python işlemi bulunamadı." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Python işlemi durdurulurken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Python scriptini çalıştıran iç metot
        /// </summary>
        private async Task<IHttpActionResult> RunPythonScriptInternal(string argument)
        {
            try
            {
                // Eğer zaten bir işlem çalışıyorsa durdur
                if (pythonProcess != null && !pythonProcess.HasExited)
                {
                    pythonProcess.Kill();
                }

                // Python script yolunu belirle
                string yemeksepetiFolder = GetYemeksepetiFolder();
                string bearerFilePath = GetBearerFilePath();

                // Eğer bearer dosyası zaten varsa sil
                if (File.Exists(bearerFilePath))
                {
                    File.Delete(bearerFilePath);
                }

                // Python betiğini çalıştır
                string pythonScriptPath = Path.Combine(yemeksepetiFolder, "yemeksepeti.py");

                if (!File.Exists(pythonScriptPath))
                {
                    return BadRequest($"Python script dosyası bulunamadı: {pythonScriptPath}");
                }

                // FileSystemWatcher oluştur ve dosya değişikliklerini izle
                var fileWatcher = new FileSystemWatcher(yemeksepetiFolder)
                {
                    Filter = "current_bearer.txt",
                    EnableRaisingEvents = true
                };

                // Token alındığında işaretlemek için TaskCompletionSource
                var tokenTaskSource = new TaskCompletionSource<string>();

                // Dosya oluşturulduğunda veya değiştirildiğinde tetiklenecek event handler
                FileSystemEventHandler fileHandler = null;
                fileHandler = (s, e) =>
                {
                    try
                    {
                        // Dosyanın yazılması tamamlanana kadar kısa bir bekleme
                        System.Threading.Thread.Sleep(500);

                        // Dosyayı oku
                        string token = File.ReadAllText(bearerFilePath).Trim();
                        bearerToken = token; // Statik değişkene kaydet

                        // Event handler'ı kaldır
                        fileWatcher.Created -= fileHandler;
                        fileWatcher.Changed -= fileHandler;

                        // TaskCompletionSource'u tamamla
                        tokenTaskSource.TrySetResult(token);
                    }
                    catch (Exception ex)
                    {
                        tokenTaskSource.TrySetException(ex);
                    }
                };

                // Event handler'ları ekle
                fileWatcher.Created += fileHandler;
                fileWatcher.Changed += fileHandler;

                pythonProcess = new Process();
                pythonProcess.StartInfo.FileName = "python";
                pythonProcess.StartInfo.Arguments = $"{pythonScriptPath} {argument}";
                pythonProcess.StartInfo.UseShellExecute = false;
                pythonProcess.StartInfo.CreateNoWindow = true;
                pythonProcess.StartInfo.RedirectStandardOutput = true;
                pythonProcess.StartInfo.RedirectStandardError = true;
                pythonProcess.Start();

                // Çıktıları asenkron olarak oku
                string output = await pythonProcess.StandardOutput.ReadToEndAsync();
                string error = await pythonProcess.StandardError.ReadToEndAsync();

                if (!string.IsNullOrEmpty(error))
                {
                    fileWatcher.Dispose();
                    return BadRequest($"Python betiği çalıştırılırken hata oluştu: {error}");
                }

                // Token için maksimum bekleme süresi
                var tokenTask = await Task.WhenAny(
                    tokenTaskSource.Task,
                    Task.Delay(TimeSpan.FromSeconds(60)) // 60 saniye bekle
                );

                // FileSystemWatcher'ı temizle
                fileWatcher.EnableRaisingEvents = false;
                fileWatcher.Dispose();

                if (tokenTask == tokenTaskSource.Task)
                {
                    // Token başarıyla alındı
                    string token = await tokenTaskSource.Task;
                    return Ok(new { Success = true, Message = "Bearer token başarıyla alındı.", Token = token });
                }
                else
                {
                    // Zaman aşımı oluştu
                    if (File.Exists(bearerFilePath))
                    {
                        // Dosya var ama event tetiklenmedi, manuel oku
                        string token = File.ReadAllText(bearerFilePath).Trim();
                        bearerToken = token;
                        return Ok(new { Success = true, Message = "Bearer token başarıyla alındı (manuel okuma).", Token = token });
                    }
                    else
                    {
                        return Ok(new
                        {
                            Success = false,
                            Message = "Token alınamadı. Python betiği çalıştı ancak token dosyası oluşturulmadı. " +
                                      "Lütfen manuel olarak giriş yapın veya daha sonra tekrar deneyin."
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Hata: {ex.Message}");
            }
        }

        /// <summary>
        /// Yemeksepeti klasör yolunu alır
        /// </summary>
        private string GetYemeksepetiFolder()
        {
            // API'nin çalıştığı klasörü al
            string appPath = Application.StartupPath;

            // Yemeksepeti klasörü (API ile aynı dizinde olduğunu varsayıyoruz)
            string yemeksepetiFolder = appPath;

            return yemeksepetiFolder;
        }

        /// <summary>
        /// Bearer token dosyasının yolunu alır
        /// </summary>
        private string GetBearerFilePath()
        {
            string yemeksepetiFolder = GetYemeksepetiFolder();
            return Path.Combine(yemeksepetiFolder, "current_bearer.txt");
        }
    }
}
