using ArtiConnect.Integrations.Ingenico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.UI.WebControls;
using Serilog;
using System.Windows.Forms;

namespace ArtiConnect.Api.Controllers
{
    [RoutePrefix("api/ingenico")]
    public class IngenicoController : ApiController
    {
        private static readonly object _lockObject = new object();
        private static string _deviceId = string.Empty;
        private static SmartDllClient _dllClient;
        private static bool _isInitialized = false;
        private static bool _isInvoice = false;
        private static ListView _batchCommands = new ListView();
        private static List<string> _userMessages = new List<string> { "Entegrator: ARTIPOS" };

        private static ListView BatchCommands
        {
            get
            {
                if (_batchCommands == null)
                {
                    _batchCommands = new ListView();
                    _batchCommands.View = System.Windows.Forms.View.Details;
                    _batchCommands.Columns.Add("No", 30);
                    _batchCommands.Columns.Add("Command", 100);
                    _batchCommands.Columns.Add("Data", 300);
                }
                return _batchCommands;
            }
        }

        public IngenicoController()
        {
            _batchCommands = new ListView();
            _batchCommands.View = System.Windows.Forms.View.Details;
            _batchCommands.Columns.Add("No", 30);
            _batchCommands.Columns.Add("Command", 100);
            _batchCommands.Columns.Add("Data", 300);
        }

        /// <summary>
        /// Yazarkasa ile bağlantı kurar ve eşleştirir
        /// </summary>
        [HttpPost]
        [Route("connect")]
        public IHttpActionResult Connect()
        {
            try
            {
                lock (_lockObject)
                {
                    if (_isInitialized)
                    {
                        return Ok(new { Success = true, Message = "Cihaz zaten bağlı ve eşleşmiş durumda.", DeviceId = _deviceId });
                    }

                    // SmartDllClient instance'ını al
                    _dllClient = SmartDllClient.Instance;

                    // DLL versiyonunu kontrol et
                    string dllVersion = _dllClient.GetDllVersion();
                    if (!string.IsNullOrWhiteSpace(dllVersion))
                    {
                        return BadRequest($"DLL versiyonu uyumsuz: {dllVersion}");
                    }

                    // Tüm ECR arayüzlerini al
                    List<EcrInterface> interfaces = _dllClient.RetrieveAllEcrInterfaces();
                    if (interfaces == null || interfaces.Count == 0)
                    {
                        return BadRequest("Hiçbir ECR arayüzü bulunamadı.");
                    }

                    // İlk arayüzü seç
                    _dllClient.CurrentInterface = interfaces[0];

                    try
                    {
                        // Echo testi yap
                        _dllClient.Echo();
                    }
                    catch (SmartDllClientException ex)
                    {
                        return BadRequest($"Echo testi başarısız: {ex.Message}");
                    }

                    // Eşleştirme işlemini başlat
                    List<string> pairResults;
                    try
                    {
                        pairResults = _dllClient.StartPairingAndGetPairResults();
                    }
                    catch (SmartDllClientException ex)
                    {
                        return BadRequest($"Eşleştirme başarısız: {ex.Message}");
                    }

                    if (pairResults == null || pairResults.Count < 2)
                    {
                        return BadRequest("Eşleştirme sonuçları alınamadı.");
                    }

                    _deviceId = pairResults[0];

                    // TLV veri işlemlerini yap
                    List<uint> tlvResults;
                    try
                    {
                        tlvResults = _dllClient.DoTlvDataOperations();
                    }
                    catch (Exception ex)
                    {
                        if (ex.GetType() == typeof(SmartDllClientException))
                        {
                            return BadRequest($"TLV veri işlemi başarısız: {ex.Message}");
                        }
                        return BadRequest($"TLV veri işlemi sırasında beklenmeyen hata: {ex.Message}");
                    }

                    // Departman ve vergi oranlarını kaydet
                    ST_TAX_RATE[] taxRates;
                    try
                    {
                        taxRates = _dllClient.SaveDepartmentsAndTaxes();
                    }
                    catch (Exception ex)
                    {
                        if (ex.GetType() == typeof(SmartDllClientException))
                        {
                            return BadRequest($"Departman ve vergi oranları kaydedilemedi: {ex.Message}");
                        }
                        return BadRequest($"Departman ve vergi oranları kaydedilirken beklenmeyen hata: {ex.Message}");
                    }

                    // İşlem handle'larını başlat
                    uint initResult = _dllClient.InitAndRetrieveTransactionHandles();
                    if (initResult == 2080)
                    {
                        // Tamamlanmamış fiş var, kullanıcıya bilgi ver
                        return Ok(new
                        {
                            Success = true,
                            Message = "Cihaz bağlantısı kuruldu ancak tamamlanmamış fiş bulundu.",
                            DeviceId = _deviceId,
                            IncompleteTicket = true
                        });
                    }
                    else if (initResult != 0)
                    {
                        // Hata durumu
                        return BadRequest($"İşlem handle'ları başlatılamadı. Hata kodu: {initResult}");
                    }

                    _isInitialized = true;
                    return Ok(new
                    {
                        Success = true,
                        Message = "Cihaz başarıyla bağlandı ve eşleştirildi.",
                        DeviceId = _deviceId,
                        TaxRates = taxRates.Select(t => new { Rate = t.taxRate / 100.0 }).ToList()
                    });
                }
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogLevel.Error, $"Bağlantı kurulurken hata: {ex.Message}\r\nStackTrace: {ex.StackTrace}", "Connect", _deviceId, ex, "Ingenico");
                return BadRequest($"Bağlantı kurulurken beklenmeyen hata: {ex.Message}");
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
                lock (_lockObject)
                {
                    if (!_isInitialized)
                    {
                        return BadRequest("Cihaz bağlı değil.");
                    }

                    try
                    {
                        // Aktif işlemleri temizle
                        VoidAll();
                    }
                    catch (Exception ex)
                    {
                        Logging.WriteLog(LogLevel.Warning, $"VoidAll sırasında hata: {ex.Message}", "Disconnect", _deviceId, ex, "Ingenico");
                    }

                    try
                    {
                        // İşlem handle'larını kapat
                        uint closeResult = _dllClient.CloseTransactionHandles();
                        if (closeResult != 0)
                        {
                            Logging.WriteLog(LogLevel.Warning, $"CloseTransactionHandles hata kodu: {closeResult}", "Disconnect", _deviceId, null, "Ingenico");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.WriteLog(LogLevel.Warning, $"CloseTransactionHandles sırasında hata: {ex.Message}", "Disconnect", _deviceId, ex, "Ingenico");
                    }

                    // Tüm arayüzleri sıfırla
                    _dllClient.ResetAllInterfaces();
                    _isInitialized = false;
                    _deviceId = string.Empty;

                    return Ok(new { Success = true, Message = "Cihaz bağlantısı başarıyla sonlandırıldı." });
                }
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogLevel.Error, $"Bağlantı sonlandırılırken hata: {ex.Message}\r\nStackTrace: {ex.StackTrace}", "Disconnect", _deviceId, ex, "Ingenico");
                return BadRequest($"Bağlantı sonlandırılırken beklenmeyen hata: {ex.Message}");
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
                if (!_isInitialized)
                {
                    return Ok(new { Connected = false, Message = "Cihaz bağlı değil." });
                }

                try
                {
                    // Ping testi yap
                    _dllClient.Ping();
                    return Ok(new
                    {
                        Connected = true,
                        Message = "Cihaz bağlı ve çalışıyor.",
                        DeviceId = _deviceId
                    });
                }
                catch (SmartDllClientException ex)
                {
                    _isInitialized = false;
                    return Ok(new
                    {
                        Connected = false,
                        Message = $"Cihaz yanıt vermiyor. Hata: {ex.Message}",
                        ErrorCode = ex.DllErrorCode
                    });
                }
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogLevel.Error, $"Durum sorgulanırken hata: {ex.Message}\r\nStackTrace: {ex.StackTrace}", "GetStatus", _deviceId, ex, "Ingenico");
                return BadRequest($"Durum sorgulanırken beklenmeyen hata: {ex.Message}");
            }
        }

        /// <summary>
        /// Nakit ödemeli fiş oluşturur
        /// </summary>
        [HttpPost]
        [Route("createCashReceipt")]
        public IHttpActionResult CreateCashReceipt(CreateCashReceiptRequest request)
        {
            try
            {
                CheckConnection();

                // Mevcut işlemleri temizle
                CleanupPreviousTransactions();

                // Ürünleri ekle
                foreach (var item in request.Items)
                {
                    ST_ITEM product = new ST_ITEM
                    {
                        countPrecition = 3,
                        type = 1,
                        subType = 0,
                        deptIndex = (byte)item.TaxRateIndex, // KDV oranına göre departman indeksi
                        amount = (uint)(item.UnitPrice * 100), // Kuruş cinsinden fiyat
                        currency = 949, // TL para birimi kodu
                        count = (uint)(item.Quantity * 1000), // Adet (1000 = 1.000)
                        unitType = 0,
                        pluPriceIndex = 0,
                        name = item.Name,
                        barcode = item.Barcode ?? ""
                    };

                    PrepareItemSale(product);
                }

                // Toplam tutarı kuruş cinsine çevir
                int amount = (int)(request.TotalAmount * 100);

                // Nakit ödeme işlemini gerçekleştir
                bool paymentResult = ProcessCashPayment(amount);

                if (!paymentResult)
                {
                    return BadRequest("Fiş oluşturma işlemi tamamlanamadı.");
                }

                return Ok(new { Success = true, Message = "Nakit ödemeli fiş başarıyla oluşturuldu." });
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogLevel.Error, $"Nakit fiş oluşturulurken hata: {ex.Message}\r\nStackTrace: {ex.StackTrace}", "CreateCashReceipt", _deviceId, ex, "Ingenico");
                return BadRequest($"Nakit fiş oluşturulurken beklenmeyen hata: {ex.Message}");
            }
        }

        /// <summary>
        /// Parçalı ödeme (nakit + kredi kartı) ile fiş oluşturur
        /// </summary>
        [HttpPost]
        [Route("createPartialPaymentReceipt")]
        public IHttpActionResult CreatePartialPaymentReceipt(CreatePartialPaymentRequest request)
        {
            try
            {
                CheckConnection();

                // Mevcut işlemleri temizle
                CleanupPreviousTransactions();

                // Ürünleri ekle
                foreach (var item in request.Items)
                {
                    ST_ITEM product = new ST_ITEM
                    {
                        countPrecition = 3,
                        type = 1,
                        subType = 0,
                        deptIndex = (byte)item.TaxRateIndex,
                        amount = (uint)(item.UnitPrice * 100),
                        currency = 949,
                        count = (uint)(item.Quantity * 1000),
                        unitType = 0,
                        pluPriceIndex = 0,
                        name = item.Name,
                        barcode = item.Barcode ?? ""
                    };

                    PrepareItemSale(product);
                }

                // Nakit tutarını kuruş cinsine çevir
                int cashAmount = (int)(request.CashAmount * 100);

                // Kredi kartı tutarını kuruş cinsine çevir
                int creditCardAmount = (int)(request.CreditCardAmount * 100);

                // Parçalı ödeme işlemini gerçekleştir
                bool paymentResult = ProcessPartialPayment(cashAmount, creditCardAmount);

                if (!paymentResult)
                {
                    return BadRequest("Fiş oluşturma işlemi tamamlanamadı.");
                }

                return Ok(new { Success = true, Message = "Parçalı ödemeli fiş başarıyla oluşturuldu." });
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogLevel.Error, $"Parçalı ödemeli fiş oluşturulurken hata: {ex.Message}\r\nStackTrace: {ex.StackTrace}", "CreatePartialPaymentReceipt", _deviceId, ex, "Ingenico");
                return BadRequest($"Parçalı ödemeli fiş oluşturulurken beklenmeyen hata: {ex.Message}");
            }
        }

        /// <summary>
        /// Mevcut fişi iptal eder
        /// </summary>
        [HttpPost]
        [Route("voidReceipt")]
        public IHttpActionResult VoidReceipt()
        {
            try
            {
                CheckConnection();

                try
                {
                    // Fiş iptal işlemini gerçekleştir
                    VoidAll();

                    // TransactionUniqueId'yi temizle
                    _dllClient.ClearTransactionUniqueIdOnCurrentInterface();

                    // Yeni bir işlem başlat
                    uint initResult = _dllClient.InitAndRetrieveTransactionHandles();
                    if (initResult != 0 && initResult != 2080)
                    {
                        HandleErrorCode(initResult);
                        return BadRequest($"Fiş iptal sonrası yeni işlem başlatılamadı. Hata kodu: {initResult}");
                    }

                    return Ok(new { Success = true, Message = "Fiş başarıyla iptal edildi." });
                }
                catch (Exception ex)
                {
                    Logging.WriteLog(LogLevel.Error, $"Fiş iptal işlemi sırasında hata: {ex.Message}", "VoidReceipt", _deviceId, ex, "Ingenico");
                    return BadRequest($"Fiş iptal işlemi sırasında hata: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogLevel.Error, $"Fiş iptal işlemi sırasında beklenmeyen hata: {ex.Message}\r\nStackTrace: {ex.StackTrace}", "VoidReceipt", _deviceId, ex, "Ingenico");
                return BadRequest($"Fiş iptal işlemi sırasında beklenmeyen hata: {ex.Message}");
            }
        }

        /// <summary>
        /// Yazarkasadan vergi oranlarını getirir
        /// </summary>
        [HttpGet]
        [Route("taxRates")]
        public IHttpActionResult GetTaxRates()
        {
            try
            {
                CheckConnection();

                // Vergi oranlarını al
                int pNumberOfTotalRecords = 0;
                int pNumberOfTotalRecordsReceived = 0;
                ST_TAX_RATE[] taxRates = new ST_TAX_RATE[8];
                uint result = Json_GMPSmartDLL.FP3_GetTaxRates(_dllClient.CurrentInterface.Index, ref pNumberOfTotalRecords, ref pNumberOfTotalRecordsReceived, ref taxRates, 8);

                if (result != 0)
                {
                    HandleErrorCode(result);
                    return BadRequest($"Vergi oranları alınırken hata oluştu. Hata kodu: {result}");
                }

                return Ok(new
                {
                    Success = true,
                    TaxRates = taxRates.Take(pNumberOfTotalRecordsReceived).Select((t, i) => new
                    {
                        Index = i,
                        Rate = t.taxRate / 100.0
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogLevel.Error, $"Vergi oranları alınırken beklenmeyen hata: {ex.Message}\r\nStackTrace: {ex.StackTrace}", "GetTaxRates", _deviceId, ex, "Ingenico");
                return BadRequest($"Vergi oranları alınırken beklenmeyen hata: {ex.Message}");
            }
        }

        /// <summary>
        /// Yazarkasadan departman bilgilerini getirir
        /// </summary>
        [HttpGet]
        [Route("departments")]
        public IHttpActionResult GetDepartments()
        {
            try
            {
                CheckConnection();

                // Departman bilgilerini al
                int pNumberOfTotalDepartments = 0;
                int pNumberOfTotalDepartmentsReceived = 0;
                ST_DEPARTMENT[] departments = new ST_DEPARTMENT[12];
                uint result = Json_GMPSmartDLL.FP3_GetDepartments(_dllClient.CurrentInterface.Index, ref pNumberOfTotalDepartments, ref pNumberOfTotalDepartmentsReceived, ref departments, 12);

                if (result != 0)
                {
                    HandleErrorCode(result);
                    return BadRequest($"Departman bilgileri alınırken hata oluştu. Hata kodu: {result}");
                }

                return Ok(new
                {
                    Success = true,
                    Departments = departments.Take(pNumberOfTotalDepartmentsReceived).Select((d, i) => new
                    {
                        Index = i,
                        Name = d.szDeptName,
                        TaxIndex = d.u8TaxIndex,
                        Price = d.u64Price / 100.0,
                        Limit = d.u64Limit / 100.0,
                        CurrencyType = d.iCurrencyType,
                        UnitType = d.iUnitType
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogLevel.Error, $"Departman bilgileri alınırken beklenmeyen hata: {ex.Message}\r\nStackTrace: {ex.StackTrace}", "GetDepartments", _deviceId, ex, "Ingenico");
                return BadRequest($"Departman bilgileri alınırken beklenmeyen hata: {ex.Message}");
            }
        }

        /// <summary>
        /// Yazarkasa DLL versiyonunu getirir
        /// </summary>
        [HttpGet]
        [Route("version")]
        public IHttpActionResult GetVersion()
        {
            try
            {
                string version = _dllClient?.DllVersionFriendlyName ?? "DLL yüklenmedi";
                return Ok(new { Success = true, Version = version });
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogLevel.Error, $"Versiyon bilgisi alınırken beklenmeyen hata: {ex.Message}\r\nStackTrace: {ex.StackTrace}", "GetVersion", _deviceId, ex, "Ingenico");
                return BadRequest($"Versiyon bilgisi alınırken beklenmeyen hata: {ex.Message}");
            }
        }

        #region Helper Methods

        /// <summary>
        /// Bağlantı kontrolü yapar
        /// </summary>
        private void CheckConnection()
        {
            if (!_isInitialized || _dllClient == null || _dllClient.CurrentInterface == null)
            {
                throw new Exception("Yazarkasa ile bağlantı kurulmadı. Önce bağlantı kurulmalıdır.");
            }
        }

        /// <summary>
        /// Önceki işlemleri temizler
        /// </summary>
        private void CleanupPreviousTransactions()
        {
            try
            {
                // Önce tüm işlemleri iptal et
                VoidAll();
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogLevel.Warning, $"VoidAll sırasında hata: {ex.Message}", "CleanupPreviousTransactions", _deviceId, ex, "Ingenico");
            }

            // BatchCommands listesini temizle
            BatchCommands.Items.Clear();

            // Yeni bir işlem başlatmak için
            try
            {
                // Unique ID al
                byte[] uniqueId = _dllClient.GetUniqueIdByInterface();

                // Yeni işlem başlat
                uint result = _dllClient.RestartAndRetrieveTrxHandles(uniqueId, null);

                // Eğer tamamlanmamış fiş varsa
                if (result == 2080)
                {
                    result = ReloadTransaction1();
                }

                // Hata varsa işle
                if (result != 0)
                {
                    HandleErrorCode(result);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogLevel.Error, $"Yeni işlem başlatılırken hata: {ex.Message}", "CleanupPreviousTransactions", _deviceId, ex, "Ingenico");
                throw;
            }
        }

        /// <summary>
        /// Ürün satışı hazırlar
        /// </summary>
        private void PrepareItemSale(ST_ITEM stItem)
        {
            int bufferSize = 0;
            byte[] buffer = new byte[1024];
            bufferSize = _dllClient.PrepareItemSale(buffer, buffer.Length, ref stItem);
            AddIntoCommandBatch("prepare_ItemSale", 16747145, buffer, bufferSize);
        }

        /// <summary>
        /// Komut listesine komut ekler
        /// </summary>
        private void AddIntoCommandBatch(string commandName, int commandType, byte[] buffer, int bufferLen)
        {
            byte[] array = new byte[bufferLen + 6];
            byte[] array2 = new byte[4];
            int Out_byteArrLen = 0;
            GMP_Tools.StringToByteArray(commandType.ToString("X2"), array2, ref Out_byteArrLen);
            Buffer.BlockCopy(array2, 0, array, 0, 4);

            byte[] array3 = new byte[2];
            int Out_byteArrLen2 = 0;
            string text = bufferLen.ToString("X2");
            if (text.Length % 2 == 1)
            {
                text = "0" + text;
            }
            GMP_Tools.StringToByteArray(text, array3, ref Out_byteArrLen2);
            Buffer.BlockCopy(array3, 0, array, 4, 2);
            Buffer.BlockCopy(buffer, 0, array, 6, bufferLen);

            ListViewItem listViewItem = new ListViewItem((BatchCommands.Items.Count + 1).ToString());
            listViewItem.SubItems.Add(commandName);
            listViewItem.SubItems.Add(GMP_Tools.ByteArrayToString(array, bufferLen + 6));
            BatchCommands.Items.Add(listViewItem);
        }

        /// <summary>
        /// Ödeme işlemini hazırlar
        /// </summary>
        private void PreparePayment(ST_PAYMENT_REQUEST[] stPaymentRequest)
        {
            byte[] array = new byte[1024];
            int num = 0;
            num = Json_GMPSmartDLL.prepare_Payment(array, array.Length, ref stPaymentRequest[0]);
            AddIntoCommandBatch("prepare_Payment", 16747145, array, num);
        }

        /// <summary>
        /// Toplam ve ödemeleri yazdırmayı hazırlar
        /// </summary>
        private void PreparePrintTotalsAndPayments()
        {
            byte[] array = new byte[1024];
            int num = 0;
            num = GMPSmartDLL.prepare_PrintTotalsAndPayments(array, array.Length);
            AddIntoCommandBatch("prepare_PrintTotalsAndPayments", 16747145, array, num);
        }

        /// <summary>
        /// Mali fiş öncesi alt bilgiyi yazdırmayı hazırlar
        /// </summary>
        private void PreparePrintBeforeMF()
        {
            byte[] array = new byte[1024];
            int num = 0;
            num = GMPSmartDLL.prepare_PrintBeforeMF(array, array.Length);
            AddIntoCommandBatch("prepare_PrintBeforeMF", 16747145, array, num);
        }

        /// <summary>
        /// Kullanıcı mesajını hazırlar
        /// </summary>
        private void PrepareUserMessage()
        {
            ST_USER_MESSAGE[] pStUserMessage = new ST_USER_MESSAGE[1024];
            for (int i = 0; i < pStUserMessage.Length; i++)
            {
                pStUserMessage[i] = new ST_USER_MESSAGE();
            }

            int num = 0;
            num = _userMessages.Count;

            for (int j = 0; j < num; j++)
            {
                string text = _userMessages[j];
                pStUserMessage[j].len = (byte)text.Length;
                pStUserMessage[j].message = text;
                pStUserMessage[j].flag = 8208u;
            }

            byte[] array = new byte[1024];
            int num2 = 0;
            num2 = Json_GMPSmartDLL.prepare_PrintUserMessage(array, array.Length, ref pStUserMessage, (ushort)num);
            AddIntoCommandBatch("prepare_PrintUserMessage", 16747145, array, num2);
        }

        /// <summary>
        /// Mali fişi yazdırmayı hazırlar
        /// </summary>
        private void PreparePrintMF()
        {
            byte[] array = new byte[1024];
            int num = 0;
            num = GMPSmartDLL.prepare_PrintMF(array, array.Length);
            AddIntoCommandBatch("prepare_PrintMF", 16747145, array, num);
        }

        /// <summary>
        /// Batch komutlarını işler
        /// </summary>
        private (ST_MULTIPLE_RETURN_CODE[], ST_TICKET) ProcessBatchCommands(bool isSale)
        {
            int num = 10;
            uint num2 = 0u;
            ST_TICKET pstTicket = new ST_TICKET();
            ST_MULTIPLE_RETURN_CODE[] pReturnCodes;

            while (true)
            {
                if (isSale && !_isInvoice)
                {
                    num2 = _dllClient.PressTicketHeader();
                }

                pReturnCodes = new ST_MULTIPLE_RETURN_CODE[1024];

                if (num2 == 2077)
                {
                    VoidAll();
                    _dllClient.ClearTransactionUniqueIdOnCurrentInterface();
                    _dllClient.CloseTransactionHandles();
                    continue;
                }

                if (num2 == 61445)
                {
                    VoidAll();
                    continue;
                }

                if (num2 != 61468)
                {
                    break;
                }

                HandleErrorCode(num2);
                _dllClient.ClearTransactionUniqueIdOnCurrentInterface();
                _dllClient.CloseTransactionHandles();
                byte[] uniqueIdByInterface = _dllClient.GetUniqueIdByInterface();
                _dllClient.RestartAndRetrieveTrxHandles(uniqueIdByInterface, null);
            }

            if (num2 == 0)
            {
                goto IL_00c2;
            }

            VoidAll();
            goto IL_0173;

        IL_0173:
            if (num2 == 61443)
            {
                BatchCommands.Clear();
                pReturnCodes[0] = new ST_MULTIPLE_RETURN_CODE
                {
                    retcode = 61443u
                };
                return (pReturnCodes, pstTicket);
            }

            if (pReturnCodes.Length == 1024)
            {
                if (num != 0)
                {
                    num--;
                    goto IL_00c2;
                }

                BatchCommands.Clear();
                pReturnCodes[0] = new ST_MULTIPLE_RETURN_CODE
                {
                    retcode = 61440u
                };
                return (pReturnCodes, pstTicket);
            }

            BatchCommands.Clear();
            return (pReturnCodes, pstTicket);

        IL_00c2:
            num2 = 0u;
            byte[] array = new byte[16384];
            ushort num3 = 0;
            uint pTag = 0u;
            byte[] array2 = new byte[16384];
            ushort pLen = 0;
            ushort IndexOfReturnCodes = 512;

            num3 = (ushort)GetBatchCommand(array);

            if (num3 != 0)
            {
                ushort num4 = 0;
                ulong hTrx = _dllClient.CurrentInterface.ActiveTransactionHandle.Index;

                while (num4 < num3 && num2 == 0)
                {
                    num4 = (ushort)(num4 + GMPSmartDLL.gmpReadTLVtag(ref pTag, array, num4));
                    num4 = (ushort)(num4 + GMPSmartDLL.gmpReadTLVlen_HL(ref pLen, array, num4));
                    Buffer.BlockCopy(array, num4, array2, 0, pLen);
                    num4 = (ushort)(num4 + pLen);
                    num2 = _dllClient.ProcessBatchCommands(_dllClient.CurrentInterface.Index, ref hTrx, ref pReturnCodes, ref IndexOfReturnCodes, array2, pLen, ref pstTicket, 100000);
                }
            }

            goto IL_0173;
        }

        /// <summary>
        /// Batch komutlarını alır
        /// </summary>
        private int GetBatchCommand(byte[] sendBuffer)
        {
            int count = BatchCommands.Items.Count;
            uint num = 0u;
            int num2 = 0;
            byte[] array = new byte[16384];
            ushort num3 = 0;

            for (int i = 0; i < count; i++)
            {
                byte[] array2 = new byte[1024];
                int Out_byteArrLen = 0;
                ushort num4 = 0;
                string s = BatchCommands.Items[i].SubItems[2].Text;
                GMP_Tools.StringToByteArray(s, array2, ref Out_byteArrLen);

                if (Out_byteArrLen != 0)
                {
                    string text = "";
                    for (int j = Out_byteArrLen - 4; j < Out_byteArrLen; j++)
                    {
                        text += array2[j].ToString("X2");
                    }

                    uint num5 = uint.Parse(text, System.Globalization.NumberStyles.HexNumber);
                    text = "";

                    for (int k = Out_byteArrLen - 6; k < Out_byteArrLen - 4; k++)
                    {
                        text += array2[k].ToString("X2");
                    }

                    num4 = ushort.Parse(text, System.Globalization.NumberStyles.HexNumber);

                    if (num == 0)
                    {
                        num = num5;
                    }

                    if (num != num5)
                    {
                        num2 += GMPSmartDLL.gmpSetTLV_HL(sendBuffer, num2, num, array, num3);
                        num3 = 0;
                        num = num5;
                    }

                    GMP_Tools.StringToByteArray_Rev(s, array2, ref Out_byteArrLen);
                    Buffer.BlockCopy(array2, 6, array, num3, num4);
                    num3 = (ushort)(num3 + num4);
                }
            }

            if (num3 != 0)
            {
                num2 += GMPSmartDLL.gmpSetTLV_HL(sendBuffer, num2, num, array, num3);
                num3 = 0;
            }

            return num2;
        }

        /// <summary>
        /// Nakit ödeme işlemini gerçekleştirir
        /// </summary>
        private bool ProcessCashPayment(int totalAmount)
        {
            try
            {
                // Nakit ödeme için ödeme isteği oluştur
                ST_PAYMENT_REQUEST[] paymentRequests = new ST_PAYMENT_REQUEST[1]
                {
                    new ST_PAYMENT_REQUEST
                    {
                        typeOfPayment = 1, // 1: Nakit ödeme
                        subtypeOfPayment = 0,
                        payAmount = (uint)totalAmount,
                        payAmountCurrencyCode = 949, // TL para birimi kodu
                        numberOfinstallments = 0
                    }
                };

                // Ödeme işlemini hazırla
                PreparePayment(paymentRequests);

                // Toplam ve ödemeleri yazdır
                PreparePrintTotalsAndPayments();

                // Fiş alt bilgisini yazdır
                PreparePrintBeforeMF();

                // Kullanıcı mesajını hazırla
                PrepareUserMessage();

                // Mali fişi yazdır
                PreparePrintMF();

                // Komutları işle
                var (returnCodes, ticket) = ProcessBatchCommands(true);

                // Hata kontrolü
                if (returnCodes.Length > 0)
                {
                    foreach (var code in returnCodes)
                    {
                        if (code != null && code.retcode != 0)
                        {
                            HandleErrorCode(code.retcode);
                            if (code.retcode == 2317)
                            {
                                // 2317 hatası için özel işlem zaten HandleErrorCode içinde yapılıyor
                                return false;
                            }
                        }
                    }
                }

                // İşlem tamamlandıktan sonra ekranı temizle ve oturumu kapat
                try
                {
                    // TransactionUniqueId'yi temizle
                    _dllClient.ClearTransactionUniqueIdOnCurrentInterface();

                    // İşlem handle'larını kapat
                    uint closeResult = _dllClient.CloseTransactionHandles();
                    if (closeResult != 0)
                    {
                        Logging.WriteLog(LogLevel.Warning, $"CloseTransactionHandles hata kodu: {closeResult}", "ProcessCashPayment", _deviceId, null, "Ingenico");
                    }

                    // Yeni bir işlem başlat (bu ekranı sıfırlar)
                    byte[] uniqueId = _dllClient.GetUniqueIdByInterface();
                    uint result = _dllClient.RestartAndRetrieveTrxHandles(uniqueId, null);
                    if (result != 0 && result != 2080)
                    {
                        HandleErrorCode(result);
                        Logging.WriteLog(LogLevel.Warning, $"Ekran temizleme işlemi sırasında hata: {result}", "ProcessCashPayment", _deviceId, null, "Ingenico");
                    }
                }
                catch (Exception ex)
                {
                    Logging.WriteLog(LogLevel.Warning, $"Ekran temizleme işlemi sırasında hata: {ex.Message}", "ProcessCashPayment", _deviceId, ex, "Ingenico");
                    // Bu hata işlemin başarısını etkilemeyecek, sadece log'a yazıyoruz
                }

                return true;
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogLevel.Error, $"Nakit ödeme işlemi sırasında hata: {ex.Message}", "ProcessCashPayment", _deviceId, ex, "Ingenico");
                return false;
            }
        }

        /// <summary>
        /// Parçalı ödeme işlemini gerçekleştirir
        /// </summary>
        private bool ProcessPartialPayment(int cashAmount, int creditCardAmount)
        {
            try
            {
                // Batch komutlarını temizle
                //BatchCommands.Items.Clear();

                // 1. Adım: Nakit ödeme
                ST_PAYMENT_REQUEST[] cashPayment = new ST_PAYMENT_REQUEST[1]
                {
                    new ST_PAYMENT_REQUEST
                    {
                        typeOfPayment = 1, // 1: Nakit ödeme
                        subtypeOfPayment = 0,
                        payAmount = (uint)cashAmount,
                        payAmountCurrencyCode = 949, // TL para birimi kodu
                        numberOfinstallments = 0
                    }
                };

                // Nakit ödemeyi hazırla - PreparePayment metodunu kullan
                if (cashAmount > 0)
                    PreparePayment(cashPayment);

                // 2. Adım: Kredi kartı ödemesi
                ST_PAYMENT_REQUEST[] creditCardPayment = new ST_PAYMENT_REQUEST[1]
                {
                    new ST_PAYMENT_REQUEST
                    {
                        typeOfPayment = 4, // 4: Kredi kartı ödemesi
                        subtypeOfPayment = 0,
                        payAmount = (uint)creditCardAmount,
                        payAmountCurrencyCode = 949, // TL para birimi kodu
                        numberOfinstallments = 0, // Tek çekim
                        BankPaymentUniqueId = GenerateUniqueId() // Benzersiz ID ekliyoruz
                    }
                };

                // Kredi kartı ödemesini hazırla - PreparePayment metodunu kullan
                if (creditCardAmount > 0)
                    PreparePayment(creditCardPayment);

                // Toplam ve ödemeleri yazdır
                PreparePrintTotalsAndPayments();

                // Fiş alt bilgisini yazdır
                PreparePrintBeforeMF();

                // Kullanıcı mesajını hazırla
                PrepareUserMessage();

                // Mali fişi yazdır
                PreparePrintMF();

                // Tüm komutları işle
                var (returnCodes, ticket) = ProcessBatchCommands(true);

                // Hata kontrolü
                if (returnCodes.Length > 0)
                {
                    foreach (var code in returnCodes)
                    {
                        if (code != null && code.retcode != 0)
                        {
                            HandleErrorCode(code.retcode);
                            if (code.retcode == 2317)
                            {
                                // 2317 hatası için özel işlem
                                return false;
                            }
                        }
                    }
                }

                // İşlem tamamlandıktan sonra ekranı temizle ve oturumu kapat
                try
                {
                    // TransactionUniqueId'yi temizle
                    _dllClient.ClearTransactionUniqueIdOnCurrentInterface();

                    // İşlem handle'larını kapat
                    uint closeResult = _dllClient.CloseTransactionHandles();
                    if (closeResult != 0)
                    {
                        Logging.WriteLog(LogLevel.Warning, $"CloseTransactionHandles hata kodu: {closeResult}", "ProcessPartialPayment", _deviceId, null, "Ingenico");
                    }

                    // Yeni bir işlem başlat (bu ekranı sıfırlar)
                    byte[] uniqueId = _dllClient.GetUniqueIdByInterface();
                    uint result = _dllClient.RestartAndRetrieveTrxHandles(uniqueId, null);
                    if (result != 0 && result != 2080)
                    {
                        HandleErrorCode(result);
                        Logging.WriteLog(LogLevel.Warning, $"Ekran temizleme işlemi sırasında hata: {result}", "ProcessPartialPayment", _deviceId, null, "Ingenico");
                    }
                }
                catch (Exception ex)
                {
                    Logging.WriteLog(LogLevel.Warning, $"Ekran temizleme işlemi sırasında hata: {ex.Message}", "ProcessPartialPayment", _deviceId, ex, "Ingenico");
                    // Bu hata işlemin başarısını etkilemeyecek, sadece log'a yazıyoruz
                }

                return true;
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogLevel.Error, $"Parçalı ödeme işlemi sırasında hata: {ex.Message}", "ProcessPartialPayment", _deviceId, ex, "Ingenico");
                return false;
            }
        }

        // Benzersiz ID oluşturmak için yardımcı metod
        private string GenerateUniqueId()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// Fişi iptal eder
        /// </summary>
        private void VoidAll()
        {
            ST_TICKET ticket = new ST_TICKET();
            uint errorCode = 0u;
            try
            {
                VoidAndCloseAllAndGetTicketInfo(ref ticket);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(SmartDllClientException))
                {
                    HandleErrorCode(errorCode);
                }
                throw;
            }
        }

        /// <summary>
        /// Fişi iptal eder ve fiş bilgilerini alır
        /// </summary>
        private void VoidAndCloseAllAndGetTicketInfo(ref ST_TICKET ticket)
        {
            uint num = Json_GMPSmartDLL.FP3_VoidAll(_dllClient.CurrentInterface.Index, _dllClient.CurrentInterface.ActiveTransactionHandle.Index, ref ticket, 10000);
            if (num != 0)
            {
                throw new SmartDllClientException(num);
            }

            _dllClient.CloseTransactionHandles();
            _dllClient.ClearTransactionUniqueIdOnCurrentInterface();
        }

        /// <summary>
        /// Tamamlanmamış işlemi yeniden yükler
        /// </summary>
        private uint ReloadTransaction1()
        {
            UpdateTrxHandles();
            ST_TICKET pstTicket;
            try
            {
                pstTicket = _dllClient.ReloadTransaction();
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(SmartDllClientException))
                {
                    return (ex as SmartDllClientException).DllErrorCode;
                }
                return 10000u;
            }

            return 0u;
        }

        /// <summary>
        /// İşlem handle'larını günceller
        /// </summary>
        private void UpdateTrxHandles()
        {
            uint num = 0u;
            ushort num2 = 20;
            ushort TotalNumberOfHandlesInEcr = 0;
            ushort ReceivedNumberOfHandleInList = 0;
            ST_HANDLE_LIST[] stHandleList = new ST_HANDLE_LIST[num2];

            num = Json_GMPSmartDLL.FP3_FunctionGetHandleList(_dllClient.CurrentInterface.Index, ref stHandleList, 1, 0, num2, ref TotalNumberOfHandlesInEcr, ref ReceivedNumberOfHandleInList, 10000);

            if (num == 0)
            {
                _dllClient.CurrentInterface.TransactionHandleList.Clear();
                for (int i = 0; i < TotalNumberOfHandlesInEcr; i++)
                {
                    _dllClient.AddNewTransactionHandleToCurrentInterface(stHandleList[i].Handle);
                }
            }
            else
            {
                HandleErrorCode(num);
            }
        }

        /// <summary>
        /// Hata kodlarını işler
        /// </summary>
        private void HandleErrorCode(uint errorCode)
        {
            try
            {
                if (errorCode == 2341)
                {
                    byte[] uniqueIdByInterface = _dllClient.GetUniqueIdByInterface();
                    uint num = _dllClient.RestartAndRetrieveTrxHandles(uniqueIdByInterface, null);
                    if (num == 2080)
                    {
                        num = ReloadTransaction1();
                    }
                }

                if (errorCode == 61468)
                {
                    Thread.Sleep(4000);
                }

                if (errorCode == 61443)
                {
                    Thread.Sleep(3000);
                }

                if (errorCode == 61472)
                {
                    Thread.Sleep(3000);
                }

                if (errorCode == 2317)
                {
                    byte[] uniqueIdByInterface2 = _dllClient.GetUniqueIdByInterface();
                    uint num2 = _dllClient.RestartAndRetrieveTrxHandles(uniqueIdByInterface2, null);
                    if (num2 == 2080)
                    {
                        num2 = ReloadTransaction1();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogLevel.Error, $"Hata kodu işlenirken beklenmeyen hata: {ex.Message}", "HandleErrorCode", _deviceId, ex, "Ingenico");
            }
        }

        #endregion
    }

    #region Request Models

    /// <summary>
    /// Nakit fiş oluşturma isteği
    /// </summary>
    public class CreateCashReceiptRequest
    {
        /// <summary>
        /// Fiş kalemleri
        /// </summary>
        public List<ReceiptItem> Items { get; set; }

        /// <summary>
        /// Toplam tutar
        /// </summary>
        public decimal TotalAmount { get; set; }
    }

    /// <summary>
    /// Parçalı ödeme isteği
    /// </summary>
    public class CreatePartialPaymentRequest
    {
        /// <summary>
        /// Fiş kalemleri
        /// </summary>
        public List<ReceiptItem> Items { get; set; }

        /// <summary>
        /// Nakit ödeme tutarı
        /// </summary>
        public decimal CashAmount { get; set; }

        /// <summary>
        /// Kredi kartı ödeme tutarı
        /// </summary>
        public decimal CreditCardAmount { get; set; }
    }

    /// <summary>
    /// Fiş kalemi
    /// </summary>
    public class ReceiptItem
    {
        /// <summary>
        /// Ürün adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Birim fiyat
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Vergi oranı indeksi (0-7 arası)
        /// </summary>
        public int TaxRateIndex { get; set; }

        /// <summary>
        /// Barkod (opsiyonel)
        /// </summary>
        public string Barcode { get; set; }
    }

    /// <summary>
    /// Satış tipi ayarlama isteği
    /// </summary>
    public class SetSaleTypeRequest
    {
        /// <summary>
        /// Satış tipi (Normal, Invoice, Return)
        /// </summary>
        public string SaleType { get; set; }
    }

    #endregion
}