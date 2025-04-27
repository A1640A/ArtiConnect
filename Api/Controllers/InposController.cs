using ArtiConnect.Api.Modals.Inpos;
using ArtiConnect.DataAccess;
using ArtiConnect.Entities;
using ArtiConnect.Integrations;
using ArtiConnect.Integrations.Inpos;
using Inpos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using static ArtiConnect.Integrations.Inpos.InposIntegrationMethods;

namespace ArtiConnect.Api.Controllers
{
    [ApiLogger]
    [RoutePrefix("api/inpos")]
    public class InposController : ApiController
    {
        private AppDbContext db = new AppDbContext();
        private static InposIntegrationMethods _entegrasyon;
        private static object _lockObject = new object();
        private static string _currentSerialNumber;

        /// <summary>
        /// Yazarkasa ile bağlantı kurar
        /// </summary>
        [HttpPost]
        [Route("connect")]
        public IHttpActionResult Connect(InposConnectRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.SerialNumber))
                {
                    return BadRequest("Seri numarası boş olamaz.");
                }

                _currentSerialNumber = request.SerialNumber;
                _entegrasyon = new InposIntegrationMethods(request.SerialNumber);
                var result = _entegrasyon.Initialize(); 

                if (result.Status)
                {
                    return Ok(new { Success = true, Message = result.Message });
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            { 
                return BadRequest($"Bağlantı kurulurken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Yazarkasa bağlantısını sonlandırır
        /// </summary>
        [HttpPost]
        [Route("disconnect")]
        public IHttpActionResult Disconnect()
        {
            try
            {
                if (_entegrasyon == null)
                {
                    return BadRequest("Aktif bir bağlantı bulunmuyor.");
                }

                var result = _entegrasyon.Close();
                _entegrasyon = null; 

                if (result.Status)
                {
                    return Ok(new { Success = true, Message = result.Message });
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            { 
                return BadRequest($"Bağlantı sonlandırılırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Yazarkasa durumunu sorgular
        /// </summary>
        [HttpGet]
        [Route("status")]
        public IHttpActionResult GetStatus()
        {
            try
            {
                if (_entegrasyon == null)
                {
                    return Ok(new { Connected = false, Message = "Bağlantı kurulmadı." });
                }

                var result = _entegrasyon.GetYazarKasaState(); 

                return Ok(new
                {
                    Connected = true,
                    Status = result.Status,
                    Message = result.Message,
                    SerialNumber = _currentSerialNumber
                });
            }
            catch (Exception ex)
            { 
                return BadRequest($"Durum sorgulanırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Satış başlatır
        /// </summary>
        [HttpPost]
        [Route("startSale")]
        public IHttpActionResult StartSale()
        {
            try
            {
                CheckConnection();

                var result = _entegrasyon.StartSale(); 

                if (result.Status)
                {
                    return Ok(new { Success = true, Message = result.Message });
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            { 
                return BadRequest($"Satış başlatılırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Satış durumunu sorgular
        /// </summary>
        [HttpGet]
        [Route("saleStatus")]
        public IHttpActionResult GetSaleStatus()
        {
            try
            {
                CheckConnection();

                var result = _entegrasyon.GetSaleState(); 

                if (result.Status)
                {
                    return Ok(new { Success = true, Message = result.Message });
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            { 
                return BadRequest($"Satış durumu sorgulanırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Satışı iptal eder
        /// </summary>
        [HttpPost]
        [Route("cancelSale")]
        public IHttpActionResult CancelSale()
        {
            try
            {
                CheckConnection();

                var result = _entegrasyon.CancelSale(); 

                if (result.Status)
                {
                    return Ok(new { Success = true, Message = result.Message });
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            { 
                return BadRequest($"Satış iptal edilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Satışa ürün ekler
        /// </summary>
        [HttpPost]
        [Route("addSaleItem")]
        public IHttpActionResult AddSaleItem(AddSaleItemRequest request)
        {
            try
            {
                CheckConnection();

                var result = _entegrasyon.AddSaleItem(
                    request.ItemName,
                    request.UnitPrice,
                    request.Multiplier,
                    request.DiscountRate,
                    request.DiscountAmount,
                    request.Section,
                    request.Unit
                ); 

                if (result.Status)
                {
                    return Ok(new { Success = true, Message = result.Message });
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            { 
                return BadRequest($"Satış kalemi eklenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Son satır kalemini siler
        /// </summary>
        [HttpPost]
        [Route("deleteLastItem")]
        public IHttpActionResult DeleteLastItem()
        {
            try
            {
                CheckConnection();

                var result = _entegrasyon.DeleteLastItem(); 

                if (result.Status)
                {
                    return Ok(new { Success = true, Message = result.Message });
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            { 
                return BadRequest($"Son satır silinirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Ödeme ekler
        /// </summary>
        [HttpPost]
        [Route("addPayment")]
        public IHttpActionResult AddPayment(AddPaymentRequest request)
        {
            try
            {
                CheckConnection();

                PaymentType paymentType;
                if (!Enum.TryParse(request.PaymentType, out paymentType))
                {
                    return BadRequest($"Geçersiz ödeme tipi: {request.PaymentType}");
                }

                ActionResult result;
                if (request.AcquirerId.HasValue)
                {
                    Acquirer acquirer = (Acquirer)request.AcquirerId.Value;
                    result = _entegrasyon.AddPayment(paymentType, request.PaymentAmount, acquirer);
                }
                else
                {
                    result = _entegrasyon.AddPayment(paymentType, request.PaymentAmount);
                } 

                if (result.Status)
                {
                    return Ok(new { Success = true, Message = result.Message });
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            { 
                return BadRequest($"Ödeme eklenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Z raporu alır
        /// </summary>
        [HttpPost]
        [Route("zReport")]
        public IHttpActionResult GetZReport()
        {
            try
            {
                CheckConnection();

                var result = InposIntegrationMethods.ZReport(); 

                if (result.Status)
                {
                    return Ok(new { Success = true, Message = result.Message });
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            { 
                return BadRequest($"Z raporu alınırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// X raporu alır
        /// </summary>
        [HttpPost]
        [Route("xReport")]
        public IHttpActionResult GetXReport()
        {
            try
            {
                CheckConnection();

                var result = InposIntegrationMethods.XReport(); 

                if (result.Status)
                {
                    return Ok(new { Success = true, Message = result.Message });
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            { 
                return BadRequest($"X raporu alınırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Kasiyer girişi yapar
        /// </summary>
        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login()
        {
            try
            {
                CheckConnection();

                var result = _entegrasyon.Login(); 

                if (result.Status)
                {
                    return Ok(new { Success = true, Message = result.Message });
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            { 
                return BadRequest($"Kasiyer girişi yapılırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Kasiyer çıkışı yapar
        /// </summary>
        [HttpPost]
        [Route("logout")]
        public IHttpActionResult Logout()
        {
            try
            {
                CheckConnection();

                var result = _entegrasyon.Logout(); 

                if (result.Status)
                {
                    return Ok(new { Success = true, Message = result.Message });
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            { 
                return BadRequest($"Kasiyer çıkışı yapılırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Cihazı kilitler
        /// </summary>
        [HttpPost]
        [Route("blockKeys")]
        public IHttpActionResult BlockKeys()
        {
            try
            {
                CheckConnection();

                var result = InposIntegrationMethods.BlockKeys(); 

                if (result.Status)
                {
                    return Ok(new { Success = true, Message = result.Message });
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            { 
                return BadRequest($"Cihaz kilitlenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Cihaz kilidini açar
        /// </summary>
        [HttpPost]
        [Route("unblockKeys")]
        public IHttpActionResult UnblockKeys()
        {
            try
            {
                CheckConnection();

                var result = InposIntegrationMethods.UnblockKeys(); 

                if (result.Status)
                {
                    return Ok(new { Success = true, Message = result.Message });
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            { 
                return BadRequest($"Cihaz kilidi açılırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Cihaz kilit durumunu sorgular
        /// </summary>
        [HttpGet]
        [Route("keyBlockStatus")]
        public IHttpActionResult GetKeyBlockStatus()
        {
            try
            {
                CheckConnection();

                var result = _entegrasyon.GetKeyBlockStatus(); 

                if (result.Status)
                {
                    return Ok(new { Success = true, Message = result.Message });
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            { 
                return BadRequest($"Cihaz kilit durumu sorgulanırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Kısım bilgilerini getirir
        /// </summary>
        [HttpGet]
        [Route("sections")]
        public IHttpActionResult GetSections()
        {
            try
            {
                CheckConnection();

                var sections = new List<SectionInfo>();

                for (int i = 1; i <= 8; i++)
                {
                    var vatRateResult = _entegrasyon.GetSectionData(i);
                    var nameResult = _entegrasyon.GetSectionName(i);

                    if (vatRateResult.Status && nameResult.Status)
                    {
                        float vatRate = float.Parse(vatRateResult.Message) / 100;

                        sections.Add(new SectionInfo
                        {
                            SectionId = i,
                            VatRate = vatRate,
                            Name = nameResult.Message
                        });
                    }
                } 

                return Ok(new { Success = true, Sections = sections });
            }
            catch (Exception ex)
            { 
                return BadRequest($"Kısım bilgileri alınırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirli bir kısımın bilgilerini getirir
        /// </summary>
        [HttpGet]
        [Route("section/{id}")]
        public IHttpActionResult GetSection(int id)
        {
            try
            {
                CheckConnection();

                if (id < 1 || id > 8)
                {
                    return BadRequest("Geçersiz kısım numarası. 1-8 arasında bir değer olmalıdır.");
                }

                var vatRateResult = _entegrasyon.GetSectionData(id);
                var nameResult = _entegrasyon.GetSectionName(id);

                if (!vatRateResult.Status || !nameResult.Status)
                {
                    return BadRequest("Kısım bilgisi alınamadı: " +
                        (!vatRateResult.Status ? vatRateResult.Message : nameResult.Message));
                }

                float vatRate = float.Parse(vatRateResult.Message) / 100; 

                return Ok(new
                {
                    Success = true,
                    Section = new SectionInfo
                    {
                        SectionId = id,
                        VatRate = vatRate,
                        Name = nameResult.Message
                    }
                });
            }
            catch (Exception ex)
            { 
                return BadRequest($"Kısım bilgisi alınırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Güncel Z numarasını getirir
        /// </summary>
        [HttpGet]
        [Route("currentZ")]
        public IHttpActionResult GetCurrentZ()
        {
            try
            {
                CheckConnection();

                var result = _entegrasyon.GetCurrentZ(); 

                if (result.Status)
                {
                    return Ok(new { Success = true, ZNumber = result.Message });
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            { 
                return BadRequest($"Güncel Z numarası alınırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Yazıcı kağıt durumunu kontrol eder
        /// </summary>
        [HttpGet]
        [Route("checkPaper")]
        public IHttpActionResult CheckPaper()
        {
            try
            {
                CheckConnection();

                var result = _entegrasyon.CheckPaper(); 

                if (result.Status)
                {
                    return Ok(new { Success = true, Message = result.Message });
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            { 
                return BadRequest($"Yazıcı kağıdı kontrol edilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Fiş bilgilerini getirir
        /// </summary>
        [HttpGet]
        [Route("receiptData")]
        public IHttpActionResult GetReceiptData(int receiptNo, int zNo, bool isExt = false)
        {
            try
            {
                CheckConnection();

                var result = _entegrasyon.GetReceiptData(receiptNo, zNo, isExt); 

                if (result.Status)
                {
                    return Ok(new { Success = true, Message = result.Message });
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            { 
                return BadRequest($"Fiş bilgisi alınırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Satış tipini ayarlar
        /// </summary>
        [HttpPost]
        [Route("setSaleType")]
        public IHttpActionResult SetSaleType(SetSaleTypeRequest request)
        {
            try
            {
                CheckConnection();

                InposSaleType saleType;
                if (!Enum.TryParse(request.SaleType, out saleType))
                {
                    return BadRequest($"Geçersiz satış tipi: {request.SaleType}");
                }

                var result = _entegrasyon.SetSaleType(saleType); 

                if (result.Status)
                {
                    return Ok(new { Success = true, Message = result.Message });
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            { 
                return BadRequest($"Satış tipi ayarlanırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Inpos DLL versiyonunu getirir
        /// </summary>
        [HttpGet]
        [Route("version")]
        public IHttpActionResult GetVersion()
        {
            try
            {
                string version = InposIntegrationMethods.GetInposExtVersion(); 

                return Ok(new { Success = true, Version = version });
            }
            catch (Exception ex)
            { 
                return BadRequest($"Versiyon bilgisi alınırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Bağlantı kontrolü yapar, bağlantı yoksa hata fırlatır
        /// </summary>
        private void CheckConnection()
        {
            if (_entegrasyon == null)
            {
                throw new Exception("Yazarkasa ile bağlantı kurulmadı. Önce bağlantı kurulmalıdır.");
            }
        } 
    } 
}