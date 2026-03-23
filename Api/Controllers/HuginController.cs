using ArtiConnect.Api.Modals.Hugin;
using ArtiConnect.DataAccess;
using ArtiConnect.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ArtiConnect.Api.Controllers
{
    [ApiLogger]
    [RoutePrefix("api/hugin")]
    public class HuginController : ApiController
    {
        private AppDbContext db = new AppDbContext();
        private static HuginEntegrasyon _entegrasyon;
        private static object _lockObject = new object();
        private static string _currentFiscalId;
        private static string _currentPort;

        /// <summary>
        /// Yazarkasa ile bağlantı kurar
        /// </summary>
        [HttpPost]
        [Route("connect")]
        public IHttpActionResult Connect(HuginConnectRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.FiscalId))
                {
                    return BadRequest("Fiscal ID boş olamaz.");
                }

                if (string.IsNullOrEmpty(request.Port))
                {
                    return BadRequest("Port bilgisi boş olamaz.");
                }

                _entegrasyon = new HuginEntegrasyon();
                _entegrasyon.Connection = new TCPConnection(request.IpAddress, request.Port.ToInt());

                _currentFiscalId = request.FiscalId;
                _currentPort = request.Port; 

                try
                {
                    _entegrasyon.Connection.Open();
                    _entegrasyon.MatchExDevice(request.FiscalId, request.Port);
                    _entegrasyon.SetFiscalId(request.FiscalId);

                    _entegrasyon.kdvOraniDepartmanNoDictionary = _entegrasyon.GetKdvOraniDepartmanNoDictionary();
                    return Ok(new { Success = true, Message = "Bağlantı başarıyla kuruldu." });
                }
                catch (Exception ex)
                {
                    _entegrasyon.Connection.Close();
                    return BadRequest($"Cihaz eşleştirme hatası: {ex.Message}");
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

                if (_entegrasyon.Connection != null && _entegrasyon.Connection.IsOpen)
                {
                    _entegrasyon.Connection.Close();
                }

                _entegrasyon = null;
                return Ok(new { Success = true, Message = "Bağlantı başarıyla sonlandırıldı." });
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

                bool isConnected = _entegrasyon.Connection != null && _entegrasyon.Connection.IsOpen;

                return Ok(new
                {
                    Connected = isConnected,
                    FiscalId = _currentFiscalId,
                    Port = _currentPort,
                    Message = isConnected ? "Bağlantı aktif." : "Bağlantı kapalı."
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
                _entegrasyon.start();
                return Ok(new { Success = true, Message = "Satış başarıyla başlatıldı." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Satış başlatılırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Satışa ürün ekler
        /// </summary>
        [HttpPost]
        [Route("addSaleItem")]
        public IHttpActionResult AddSaleItem(HuginAddSaleItemRequest request)
        {
            try
            {
                CheckConnection();

                var urunBilgisi = new UrunBilgisi
                {
                    UrunAdi = request.UrunAdi,
                    KdvOrani = request.KdvOrani,
                    DepartmanNo = request.DepartmanNo,
                    Miktar = request.Miktar,
                    Fiyat = request.Fiyat
                };

                _entegrasyon.addSatis(urunBilgisi);
                return Ok(new { Success = true, Message = "Ürün başarıyla eklendi." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Ürün eklenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Nakit ödeme ekler
        /// </summary>
        [HttpPost]
        [Route("addCashPayment")]
        public IHttpActionResult AddCashPayment(AddCashPaymentRequest request)
        {
            try
            {
                CheckConnection();
                _entegrasyon.addCashPayment(request.Amount);
                return Ok(new { Success = true, Message = "Nakit ödeme başarıyla eklendi." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Nakit ödeme eklenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Kredi kartı ödemesi ekler
        /// </summary>
        [HttpPost]
        [Route("addCreditPayment")]
        public IHttpActionResult AddCreditPayment(AddCreditPaymentRequest request)
        {
            try
            {
                CheckConnection();
                _entegrasyon.addCreditPayment(request.Amount);
                return Ok(new { Success = true, Message = "Kredi kartı ödemesi başarıyla eklendi." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Kredi kartı ödemesi eklenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// İndirim ekler
        /// </summary>
        [HttpPost]
        [Route("addDiscount")]
        public IHttpActionResult AddDiscount(AddDiscountRequest request)
        {
            try
            {
                CheckConnection();
                _entegrasyon.addDiscount(request.Amount);
                return Ok(new { Success = true, Message = "İndirim başarıyla eklendi." });
            }
            catch (Exception ex)
            {
                return BadRequest($"İndirim eklenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Satışı tamamlar
        /// </summary>
        [HttpPost]
        [Route("endSale")]
        public IHttpActionResult EndSale()
        {
            try
            {
                CheckConnection();
                _entegrasyon.endSale();
                return Ok(new { Success = true, Message = "Satış başarıyla tamamlandı." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Satış tamamlanırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Departman bilgilerini getirir
        /// </summary>
        [HttpGet]
        [Route("departments")]
        public IHttpActionResult GetDepartments()
        {
            try
            {
                CheckConnection();
                var departments = _entegrasyon.GetDepartman();
                if (departments == null)
                {
                    return BadRequest("Departman bilgileri alınamadı.");
                }
                return Ok(new { Success = true, Departments = departments });
            }
            catch (Exception ex)
            {
                return BadRequest($"Departman bilgileri alınırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// KDV gruplarını getirir
        /// </summary>
        [HttpGet]
        [Route("vatRates")]
        public IHttpActionResult GetVatRates()
        {
            try
            {
                CheckConnection();
                var vatRates = _entegrasyon.GetKdvGruplari();
                return Ok(new { Success = true, VatRates = vatRates });
            }
            catch (Exception ex)
            {
                return BadRequest($"KDV oranları alınırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// KDV oranı ve departman no eşleştirmelerini getirir
        /// </summary>
        [HttpGet]
        [Route("vatDepartmentMapping")]
        public IHttpActionResult GetVatDepartmentMapping()
        {
            try
            {
                CheckConnection();
                var mapping = _entegrasyon.GetKdvOraniDepartmanNoDictionary();
                return Ok(new { Success = true, Mapping = mapping });
            }
            catch (Exception ex)
            {
                return BadRequest($"KDV-Departman eşleştirmeleri alınırken hata oluştu: {ex.Message}");
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

            if (_entegrasyon.Connection == null || !_entegrasyon.Connection.IsOpen)
            {
                throw new Exception("Yazarkasa bağlantısı kapalı. Lütfen tekrar bağlantı kurun.");
            }
        }
    }

    #region Request Models

    public class HuginConnectRequest
    {
        public string FiscalId { get; set; }
        public string IpAddress { get; set; }
        public string Port { get; set; }
    }

    public class HuginAddSaleItemRequest
    {
        public string UrunAdi { get; set; }
        public int KdvOrani { get; set; }
        public int DepartmanNo { get; set; }
        public ulong Miktar { get; set; }
        public ulong Fiyat { get; set; }
        public ulong IskontoTUtari { get; set; }

    }

    public class AddCashPaymentRequest
    {
        public decimal Amount { get; set; }
    }

    public class AddCreditPaymentRequest
    {
        public decimal Amount { get; set; }
    }

    public class AddDiscountRequest
    {
        public decimal Amount { get; set; }
    }

    #endregion
}