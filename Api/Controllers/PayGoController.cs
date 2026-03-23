using ArtiConnect.Api.Modals.Inpos;
using ArtiConnect.Api.Modals.PayGo;
using ArtiConnect.Integrations.Inpos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.IO;
using ArtiConnect.Hugin;
using PAYGO_OKC;
using static PCPOSOKC.DLL;
using static PCPOSOKC.Utils;
using static PAYGO_OKC.PAYGO_OKC;

namespace ArtiConnect.Api.Controllers
{
    [ApiLogger]
    [RoutePrefix("api/paygo")]
    public class PayGoController : ApiController
    {
        // PAYGO_OKC sınıfını kullanarak static olarak erişim   
        PAYGO_OKC.PAYGO_OKC _paygo = new PAYGO_OKC.PAYGO_OKC(false);
        private static bool _initialized = false;
        private static string _logFilePath;
        private static Dictionary<string, List<BatchItem>> _batchCollections = new Dictionary<string, List<BatchItem>>();

        // Constructor ile PAYGO_OKC sınıfını başlat
        public PayGoController()
        {
            try
            {
                if (!_initialized && true)
                {
                    // DLL'in varlığını kontrol et
                    string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PAYGO_PCPOSOKC.dll");
                    if (!File.Exists(dllPath))
                    {
                        throw new DllNotFoundException($"PAYGO_PCPOSOKC.dll bulunamadı: {dllPath}");
                    }

                    // Log dosyası için yol oluştur
                    _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log{DateTime.Now.ToString("yyyyMMdd")}.txt");

                    // PAYGO_OKC sınıfını başlat
                    _paygo = new PAYGO_OKC.PAYGO_OKC(false);
                    _paygo.paygo_SetDeviceInfo("NewLand", "EXTDEVICEDLL", "001009");
                    _paygo.paygo_SetLogStatus(true, "");
                    //_paygo.error
                    _initialized = true;
                }
            }
            catch (Exception ex)
            {
                // Hata logla
                System.Diagnostics.Debug.WriteLine($"PAYGO_OKC başlatma hatası: {ex.Message}");
                LogToFile($"PAYGO_OKC başlatma hatası: {ex.Message}");
            }
        }

        private void LogToFile(string message)
        {
            try
            {
                File.AppendAllText(_logFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
            }
            catch
            {
                // Log yazma hatası - sessizce devam et
            }
        }

        [HttpPost]
        [Route("connect")]
        public IHttpActionResult Connect(PayGoConnectRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Ip))
                    return BadRequest("Ip adresi boş olamaz.");
                else if (string.IsNullOrEmpty(request.Port))
                    return BadRequest("Port boş olamaz.");

                EchoRspParams echoRspPrms = new EchoRspParams();

                // TCP/IP bağlantısı aç ve Echo komutu gönder
                int result = _paygo.paygo_OpenTcpIpAndGMP3Echo_ToDevice("", request.Ip, request.Port, ref echoRspPrms);

                if (result == 1) // Başarılı sonuç
                {
                    return Ok(new
                    {
                        Success = true,
                        Message = "Bağlantı başarıyla kuruldu.",
                        EchoResponse = new
                        {
                            InternalErrNum = echoRspPrms.InternalErrNum,
                            DocumentStatus = echoRspPrms.TranStatus,
                            VersionInfo = echoRspPrms.groupDF6F.VersionInfo,
                            ExtDevIndex = echoRspPrms.groupDF6F.ExtDevIndex,
                            TermSerial = echoRspPrms.groupDF02.TermSerial
                        }
                    });
                }
                else if (result == 0)
                {
                    return Ok(new
                    {
                        Success = true,
                        Message = "Bağlantı başarıyla kuruldu."
                    });
                }
                else
                {
                    return BadRequest($"Bağlantı başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"Connect hatası: {ex.Message}");
                return BadRequest($"Bağlantı kurulurken hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("connect/serial")]
        public IHttpActionResult ConnectSerial(SerialConnectRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Port))
                    return BadRequest("Port boş olamaz.");

                EchoRspParams echoRspPrms = new EchoRspParams();

                // Seri port bağlantısı aç ve Echo komutu gönder
                int result = _paygo.paygo_OpenSerialPortAndGMP3Echo_ToDevice(request.Port, "", "", ref echoRspPrms);

                if (result == 0) // Başarılı sonuç
                {
                    return Ok(new
                    {
                        Success = true,
                        Message = "Seri port bağlantısı başarıyla kuruldu.",
                        EchoResponse = new
                        {
                            InternalErrNum = echoRspPrms.InternalErrNum,
                            DocumentStatus = echoRspPrms.TranStatus,
                            VersionInfo = echoRspPrms.groupDF6F.VersionInfo,
                            ExtDevIndex = echoRspPrms.groupDF6F.ExtDevIndex,
                            TermSerial = echoRspPrms.groupDF02.TermSerial
                        }
                    });
                }
                else
                {
                    return BadRequest($"Seri port bağlantısı başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"ConnectSerial hatası: {ex.Message}");
                return BadRequest($"Seri port bağlantısı kurulurken hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("echo")]
        public IHttpActionResult Echo()
        {
            try
            {
                EchoRspParams echoRspPrms = new EchoRspParams();
                int result = _paygo.paygo_GMP3Echo_ToDevice(ref echoRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "Echo",
                        InternalErrNum = echoRspPrms.InternalErrNum,
                        ErrRespCode = echoRspPrms.groupDF6F.ErrRespCode,
                        VersionInfo = echoRspPrms.groupDF6F.VersionInfo,
                        ExtDevIndex = echoRspPrms.groupDF6F.ExtDevIndex,
                        DocumentStatus = echoRspPrms.TranStatus,
                        TermSerial = echoRspPrms.groupDF02.TermSerial,
                        TranDate = echoRspPrms.groupDF02.TranDate,
                        TranTime = echoRspPrms.groupDF02.TranTime,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Echo komutu başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"Echo hatası: {ex.Message}");
                return BadRequest($"Echo komutu sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("ping")]
        public IHttpActionResult Ping()
        {
            try
            {
                PingRspParams pingRspPrms = new PingRspParams();
                int result = _paygo.paygo_Ping_ToDevice(ref pingRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "Ping",
                        InternalErrNum = pingRspPrms.InternalErrNum,
                        DocumentStatus = pingRspPrms.TranStatus,
                        AppVersion = pingRspPrms.AppVersion,
                        TranDate = pingRspPrms.TranDate,
                        TranTime = pingRspPrms.TranTime,
                        ZNum = pingRspPrms.ZNum,
                        ReceiptNum = pingRspPrms.ReceiptNum,
                        EJNum = pingRspPrms.EJNum,
                        Amount = pingRspPrms.Amount,
                        FiscalId = pingRspPrms.FiscalId,
                        CashierId = pingRspPrms.CashierId,
                        EcrMode = pingRspPrms.EcrMode,
                        EcrLocation = pingRspPrms.EcrLocation,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Ping komutu başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"Ping hatası: {ex.Message}");
                return BadRequest($"Ping komutu sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("cashier/login")]
        public IHttpActionResult CashierLogin(CashierLoginRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.CashierId))
                    return BadRequest("Kasiyer ID boş olamaz.");

                CommonRspParams changeEcrModeRspPrms = new CommonRspParams();
                _paygo.paygo_ChangeEcrMode_ToDevice("2", ref changeEcrModeRspPrms);

                CommonRspParams commonRspPrms = new CommonRspParams();
                int result = _paygo.paygo_CashierLogin_ToDevice(request.CashierId, request.CashierPwd ?? "", ref commonRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "CashierLogin",
                        InternalErrNum = commonRspPrms.InternalErrNum,
                        DocumentStatus = commonRspPrms.TranStatus,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Kasiyer girişi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"CashierLogin hatası: {ex.Message}");
                return BadRequest($"Kasiyer girişi sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("cashier/logout")]
        public IHttpActionResult CashierLogout()
        {
            try
            {
                CommonRspParams commonRspPrms = new CommonRspParams();
                int result = _paygo.paygo_CashierLogout_ToDevice(ref commonRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "CashierLogout",
                        InternalErrNum = commonRspPrms.InternalErrNum,
                        DocumentStatus = commonRspPrms.TranStatus,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Kasiyer çıkışı başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"CashierLogout hatası: {ex.Message}");
                return BadRequest($"Kasiyer çıkışı sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("document/open")]
        public IHttpActionResult OpenDocument(OpenDocumentRequest request)
        {
            try
            {
                OpenDocumentReqParams openDocumentReqPrms = new OpenDocumentReqParams();
                OpenDocumentRspParams openDocumentRspPrms = new OpenDocumentRspParams();

                // Request parametrelerini doldur
                openDocumentReqPrms.Amount = request.Amount ?? "";
                openDocumentReqPrms.TranDate = request.TranDate ?? DateTime.Now.ToString("yyMMdd");
                openDocumentReqPrms.TranTime = request.TranTime ?? DateTime.Now.ToString("HHmmss");
                openDocumentReqPrms.DocumentType = request.DocumentType ?? "";
                openDocumentReqPrms.Vkn = request.Vkn ?? "";
                openDocumentReqPrms.BillSerialNo = request.BillSerialNo ?? "";
                openDocumentReqPrms.DispatchNote_value = request.DispatchNote ?? "0";
                openDocumentReqPrms.OrderNo = request.OrderNo ?? "";
                openDocumentReqPrms.IsLaterOn_value = request.IsLaterOn ?? "0";
                openDocumentReqPrms.OwnerName = request.OwnerName ?? "";
                openDocumentReqPrms.MerchantNo = request.MerchantNo ?? "";
                openDocumentReqPrms.IsTakeComm_value = request.IsTakeComm ?? "0";
                openDocumentReqPrms.Plate = request.Plate ?? "";
                openDocumentReqPrms.Title = request.Title ?? "";
                openDocumentReqPrms.Commision = request.Commision ?? "";

                int result = _paygo.paygo_OpenDocument_ToDevice(openDocumentReqPrms, ref openDocumentRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "OpenDocument",
                        InternalErrNum = openDocumentRspPrms.InternalErrNum,
                        DocumentStatus = openDocumentRspPrms.TranStatus,
                        TranDate = openDocumentRspPrms.TranDate,
                        TranTime = openDocumentRspPrms.TranTime,
                        ZNum = openDocumentRspPrms.ZNum,
                        ReceiptNum = openDocumentRspPrms.ReceiptNum,
                        ReceiptUniqueNumber = openDocumentRspPrms.ReceiptUniqueNumber,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Belge açma işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"OpenDocument hatası: {ex.Message}");
                return BadRequest($"Belge açma sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("transaction")]
        public IHttpActionResult DoTransaction(DoTransactionRequest request)
        {
            try
            {
                DoTransReqParams doTransReqPrms = new DoTransReqParams();
                DoTranRspParams doTranRspPrms = new DoTranRspParams();

                doTransReqPrms.freeTextReqPrms = new FreeTextReqParams();

                // Request parametrelerini doldur
                doTransReqPrms.Amount = request.Amount ?? "";
                doTransReqPrms.ProcessType = request.ProcessType ?? "";
                doTransReqPrms.DepartmentId = request.DepartmentId ?? "";
                doTransReqPrms.PLUID = request.PLUID ?? "";
                doTransReqPrms.NonTaxId = request.NonTaxId ?? "";
                doTransReqPrms.Quantity = request.Quantity ?? "";
                doTransReqPrms.UnitPrice = request.UnitPrice ?? "";
                doTransReqPrms.Barcode = request.Barcode ?? "";
                doTransReqPrms.Rate = request.Rate ?? "";
                doTransReqPrms.ItemName = request.ItemName ?? "";
                doTransReqPrms.TranId = request.TranId ?? "";
                doTransReqPrms.CollectionId = request.CollectionId ?? "";

                if (request.FreeText != null)
                {
                    doTransReqPrms.freeTextReqPrms.FreeText_Format = request.FreeText.Format ?? "";
                    doTransReqPrms.freeTextReqPrms.FreeText_Duzen = request.FreeText.Duzen ?? "";
                    doTransReqPrms.freeTextReqPrms.FreeText_Msg = request.FreeText.Message ?? "";
                }

                int result = _paygo.paygo_DoTran_ToDevice(doTransReqPrms, ref doTranRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "DoTransaction",
                        InternalErrNum = doTranRspPrms.InternalErrNum,
                        DocumentStatus = doTranRspPrms.TranStatus,
                        ZNum = doTranRspPrms.ZNum,
                        ReceiptNum = doTranRspPrms.ReceiptNum,
                        SubTotalAmonut = doTranRspPrms.SubTotalAmonut,
                        SoldItems = doTranRspPrms.SoldItems,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"İşlem başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"DoTransaction hatası: {ex.Message}");
                return BadRequest($"İşlem sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("payment")]
        public IHttpActionResult DoPayment(DoPaymentRequest request)
        {
            try
            {
                DoPaymentReqParams doPaymentReqPrms = new DoPaymentReqParams();
                DoPaymentRspParams doPaymentRspPrms = new DoPaymentRspParams();

                // Request parametrelerini doldur
                doPaymentReqPrms.PaymentType = request.PaymentType ?? "";
                doPaymentReqPrms.Amount = request.Amount ?? "";
                doPaymentReqPrms.CurrencyIndex = request.CurrencyIndex ?? "";
                doPaymentReqPrms.ExcRate = request.ExcRate ?? "";
                doPaymentReqPrms.CurrencyFlag_value = request.CurrencyFlag ?? "0";

                int result = _paygo.paygo_DoPayment_ToDevice(doPaymentReqPrms, ref doPaymentRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "DoPayment",
                        InternalErrNum = doPaymentRspPrms.InternalErrNum,
                        DocumentStatus = doPaymentRspPrms.TranStatus,
                        ZNum = doPaymentRspPrms.ZNum,
                        ReceiptNum = doPaymentRspPrms.ReceiptNum,
                        Amount = doPaymentRspPrms.Amount,
                        AcquirerId = doPaymentRspPrms.AcquirerId,
                        RemainAmount = doPaymentRspPrms.RemainAmount,
                        PaymentType = doPaymentRspPrms.PaymentType,
                        PaymentName = doPaymentRspPrms.PaymentName,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Ödeme işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"DoPayment hatası: {ex.Message}");
                return BadRequest($"Ödeme işlemi sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("document/close")]
        public IHttpActionResult CloseDocument(CloseDocumentRequest request)
        {
            try
            {
                FreeTextReqParams freeTextReqPrms = new FreeTextReqParams();
                CloseDocumentRspParams closeDocumentRspPrms = new CloseDocumentRspParams();

                // Request parametrelerini doldur
                freeTextReqPrms.FreeText_Format = request.Format ?? "";
                freeTextReqPrms.FreeText_Duzen = request.Duzen ?? "";
                freeTextReqPrms.FreeText_Msg = request.Message ?? "";

                int result = _paygo.paygo_CloseDocument_ToDevice(freeTextReqPrms, ref closeDocumentRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "CloseDocument",
                        InternalErrNum = closeDocumentRspPrms.InternalErrNum,
                        DocumentStatus = closeDocumentRspPrms.TranStatus,
                        RemainAmount = closeDocumentRspPrms.RemainAmount,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Belge kapatma işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"CloseDocument hatası: {ex.Message}");
                return BadRequest($"Belge kapatma sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("receipt/print")]
        public IHttpActionResult PrintReceipt(PrintReceiptRequest request)
        {
            try
            {
                // Kasiyer girişi işlemi (eğer gerekiyorsa)
                if (request.CashierLogin != null)
                {
                    //CommonRspParams cashierLoginRspPrms = new CommonRspParams();
                    //int loginResult = _paygo.paygo_CashierLogin_ToDevice(
                    //    request.CashierLogin.CashierId ?? "",
                    //    request.CashierLogin.CashierPwd ?? "",
                    //    ref cashierLoginRspPrms);
                    //System.Threading.Thread.Sleep(1000);

                    //if (loginResult != 0)
                    //{
                    //    return BadRequest($"Kasiyer girişi başarısız. Hata kodu: {loginResult}");
                    //}
                }
                 
                CommonRspParams changeEcrModeRspPrms = new CommonRspParams();
                _paygo.paygo_ChangeEcrMode_ToDevice("3", ref changeEcrModeRspPrms);

                PrintReceiptReqParams printReceiptReqPrms = new PrintReceiptReqParams();
                PrintReceiptRspParams printReceiptRspPrms = new PrintReceiptRspParams();

                // Alt nesneleri oluştur
                printReceiptReqPrms.openDocumentReqPrms = new OpenDocumentReqParams();
                printReceiptReqPrms.doBatchTranReqPrms = new DoBatchTranReqParams();
                printReceiptReqPrms.doPaymentReqPrms = new DoPaymentReqParams();
                printReceiptReqPrms.freeTextReqPrms = new FreeTextReqParams();

                // Batch işlem öğelerini hazırla
                string strBatchTranItems = "";
                int batchItemCount = 0;

                if (request.BatchItems != null && request.BatchItems.Count > 0)
                {
                    batchItemCount = Math.Min(request.BatchItems.Count, 100); // Maksimum 100 öğe

                    for (int i = 0; i < batchItemCount; i++)
                    {
                        var item = request.BatchItems[i];

                        strBatchTranItems += $"No:{i + 1}\n";
                        strBatchTranItems += $"ProType:{item.ProcessType ?? ""}\n";
                        strBatchTranItems += $"DepId:{item.DepartmentId ?? ""}\n";
                        strBatchTranItems += $"PLUId:{item.PLUID ?? ""}\n";
                        strBatchTranItems += $"NONTAXId:{item.NonTaxId ?? ""}\n";
                        strBatchTranItems += $"CollId:{item.CollectionId ?? ""}\n";
                        strBatchTranItems += $"Quantity:{item.Quantity ?? ""}\n";
                        strBatchTranItems += $"UnitPrice:{item.UnitPrice ?? ""}\n";
                        strBatchTranItems += $"Barcode:{item.Barcode ?? ""}\n";
                        strBatchTranItems += $"Rate:{item.Rate ?? ""}\n";

                        // ItemName için UTF8 kodlama
                        strBatchTranItems += "ItemName:";
                        if (!string.IsNullOrEmpty(item.ItemName))
                        {
                            UTF8Encoding utf8 = new UTF8Encoding();
                            byte[] encodeBytes = utf8.GetBytes(item.ItemName);
                            strBatchTranItems += ByteArrayToHexString(encodeBytes, encodeBytes.Length);
                        }
                        strBatchTranItems += "\n";

                        strBatchTranItems += $"Amount:{item.Amount ?? ""}\n";
                        strBatchTranItems += $"TranId:{item.TranId ?? ""}\n";
                        strBatchTranItems += $"FreeText:{item.FreeText ?? ""}\n";
                        strBatchTranItems += "\r";
                    }

                    if (batchItemCount > 0)
                    {
                        strBatchTranItems += "\n\n";
                    }
                }

                printReceiptReqPrms.doBatchTranReqPrms.BatchItemCnt = batchItemCount;
                printReceiptReqPrms.doBatchTranReqPrms.strBatchTranItems = strBatchTranItems;

                // Belge açma parametrelerini doldur
                if (request.OpenDocument != null)
                {
                    printReceiptReqPrms.openDocumentReqPrms.Amount = request.OpenDocument.Amount ?? "";
                    printReceiptReqPrms.openDocumentReqPrms.TranDate = request.OpenDocument.TranDate ?? DateTime.Now.ToString("yyMMdd");
                    printReceiptReqPrms.openDocumentReqPrms.TranTime = request.OpenDocument.TranTime ?? DateTime.Now.ToString("HHmmss");
                    printReceiptReqPrms.openDocumentReqPrms.DocumentType = request.OpenDocument.DocumentType ?? "";
                    printReceiptReqPrms.openDocumentReqPrms.Vkn = request.OpenDocument.Vkn ?? "";
                    printReceiptReqPrms.openDocumentReqPrms.BillSerialNo = request.OpenDocument.BillSerialNo ?? "";
                    printReceiptReqPrms.openDocumentReqPrms.DispatchNote_value = request.OpenDocument.DispatchNote ?? "0";
                    printReceiptReqPrms.openDocumentReqPrms.OrderNo = request.OpenDocument.OrderNo ?? "";
                    printReceiptReqPrms.openDocumentReqPrms.IsLaterOn_value = request.OpenDocument.IsLaterOn ?? "0";
                    printReceiptReqPrms.openDocumentReqPrms.OwnerName = request.OpenDocument.OwnerName ?? "";
                    printReceiptReqPrms.openDocumentReqPrms.MerchantNo = request.OpenDocument.MerchantNo ?? "";
                    printReceiptReqPrms.openDocumentReqPrms.IsTakeComm_value = request.OpenDocument.IsTakeComm ?? "0";
                    printReceiptReqPrms.openDocumentReqPrms.Plate = request.OpenDocument.Plate ?? "";
                    printReceiptReqPrms.openDocumentReqPrms.Title = request.OpenDocument.Title ?? "";
                    printReceiptReqPrms.openDocumentReqPrms.Commision = request.OpenDocument.Commision ?? "";
                }

                // Ödeme parametrelerini doldur
                if (request.Payment != null)
                {
                    printReceiptReqPrms.doPaymentReqPrms.PaymentType = request.Payment.PaymentType ?? "";
                    printReceiptReqPrms.doPaymentReqPrms.Amount = request.Payment.Amount ?? "";
                    printReceiptReqPrms.doPaymentReqPrms.CurrencyIndex = request.Payment.CurrencyIndex ?? "";
                    printReceiptReqPrms.doPaymentReqPrms.ExcRate = request.Payment.ExcRate ?? "";
                    printReceiptReqPrms.doPaymentReqPrms.CurrencyFlag_value = request.Payment.CurrencyFlag ?? "0";
                }

                // Belge kapatma parametrelerini doldur
                if (request.CloseDocument != null)
                {
                    printReceiptReqPrms.freeTextReqPrms.FreeText_Format = request.CloseDocument.Format ?? "";
                    printReceiptReqPrms.freeTextReqPrms.FreeText_Duzen = request.CloseDocument.Duzen ?? "";
                    printReceiptReqPrms.freeTextReqPrms.FreeText_Msg = request.CloseDocument.Message ?? "";
                }

                // Fiş yazdırma işlemini gerçekleştir
                int result = _paygo.paygo_PrintReceipt_ToDevice(printReceiptReqPrms, ref printReceiptRspPrms);

                // Kasiyer çıkışı işlemi (eğer gerekiyorsa)
                //CommonRspParams cashierLogoutRspPrms = new CommonRspParams();
                //_paygo.paygo_CashierLogout_ToDevice(ref cashierLogoutRspPrms);


                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "PrintReceipt",
                        InternalErrNum = printReceiptRspPrms.InternalErrNum,
                        DocumentStatus = printReceiptRspPrms.TranStatus,
                        TranDate = printReceiptRspPrms.TranDate,
                        TranTime = printReceiptRspPrms.TranTime,
                        ZNum = printReceiptRspPrms.ZNum,
                        ReceiptNum = printReceiptRspPrms.ReceiptNum,
                        ReceiptUniqueNumber = printReceiptRspPrms.ReceiptUniqueNumber,
                        Amount = printReceiptRspPrms.Amount,
                        AcquirerId = printReceiptRspPrms.AcquirerId,
                        RemainAmount = printReceiptRspPrms.RemainAmount,
                        PaymentType = printReceiptRspPrms.PaymentType,
                        PaymentName = printReceiptRspPrms.PaymentName,
                        SoldItems = printReceiptRspPrms.SoldItems,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Fiş yazdırma işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"PrintReceipt hatası: {ex.Message}");
                return BadRequest($"Fiş yazdırma sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("batch/transaction")]
        public IHttpActionResult DoBatchTransaction(BatchTransactionRequest request)
        {
            try
            {
                if (request.BatchItems == null || request.BatchItems.Count == 0)
                {
                    return BadRequest("En az bir işlem öğesi gereklidir.");
                }

                string strBatchTranItems = "";
                int batchItemCount = Math.Min(request.BatchItems.Count, 100); // Maksimum 100 öğe

                for (int i = 0; i < batchItemCount; i++)
                {
                    var item = request.BatchItems[i];

                    strBatchTranItems += $"No:{i + 1}\n";
                    strBatchTranItems += $"ProType:{item.ProcessType ?? ""}\n";
                    strBatchTranItems += $"DepId:{item.DepartmentId ?? ""}\n";
                    strBatchTranItems += $"PLUId:{item.PLUID ?? ""}\n";
                    strBatchTranItems += $"NONTAXId:{item.NonTaxId ?? ""}\n";
                    strBatchTranItems += $"CollId:{item.CollectionId ?? ""}\n";
                    strBatchTranItems += $"Quantity:{item.Quantity ?? ""}\n";
                    strBatchTranItems += $"UnitPrice:{item.UnitPrice ?? ""}\n";
                    strBatchTranItems += $"Barcode:{item.Barcode ?? ""}\n";
                    strBatchTranItems += $"Rate:{item.Rate ?? ""}\n";

                    // ItemName için UTF8 kodlama
                    strBatchTranItems += "ItemName:";
                    if (!string.IsNullOrEmpty(item.ItemName))
                    {
                        UTF8Encoding utf8 = new UTF8Encoding();
                        byte[] encodeBytes = utf8.GetBytes(item.ItemName);
                        strBatchTranItems += ByteArrayToHexString(encodeBytes, encodeBytes.Length);
                    }
                    strBatchTranItems += "\n";

                    strBatchTranItems += $"Amount:{item.Amount ?? ""}\n";
                    strBatchTranItems += $"TranId:{item.TranId ?? ""}\n";
                    strBatchTranItems += $"FreeText:{item.FreeText ?? ""}\n";
                    strBatchTranItems += "\r";
                }

                if (batchItemCount > 0)
                {
                    strBatchTranItems += "\n\n";
                }

                DoBatchTranReqParams doBatchTranReqPrms = new DoBatchTranReqParams();
                BatchTranRspParams batchTranRspPrms = new BatchTranRspParams();

                doBatchTranReqPrms.BatchItemCnt = batchItemCount;
                doBatchTranReqPrms.strBatchTranItems = strBatchTranItems;

                int result = _paygo.paygo_DoBatchTran_ToDevice(doBatchTranReqPrms, ref batchTranRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "BatchTransaction",
                        InternalErrNum = batchTranRspPrms.InternalErrNum,
                        DocumentStatus = batchTranRspPrms.TranStatus,
                        ZNum = batchTranRspPrms.ZNum,
                        ReceiptNum = batchTranRspPrms.ReceiptNum,
                        SoldItems = batchTranRspPrms.SoldItems,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Toplu işlem başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"DoBatchTransaction hatası: {ex.Message}");
                return BadRequest($"Toplu işlem sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("report/print")]
        public IHttpActionResult PrintReport(PrintReportRequest request)
        {
            try
            {
                PrintReportReqParams printReportReqPrms = new PrintReportReqParams();
                PrintReportRspParams printReportRspPrms = new PrintReportRspParams();

                // Request parametrelerini doldur
                printReportReqPrms.AcquirerId = request.AcquirerId ?? "";
                printReportReqPrms.ZNum = request.ZNum ?? "";
                printReportReqPrms.ReceiptNum = request.ReceiptNum ?? "";
                printReportReqPrms.ReportType = request.ReportType ?? "";
                printReportReqPrms.StartDate = request.StartDate ?? "";
                printReportReqPrms.EndDate = request.EndDate ?? "";
                printReportReqPrms.StartTime = request.StartTime ?? "";
                printReportReqPrms.EndTime = request.EndTime ?? "";
                printReportReqPrms.StartZNo = request.StartZNo ?? "";
                printReportReqPrms.EndZNo = request.EndZNo ?? "";
                printReportReqPrms.StartRNo = request.StartRNo ?? "";
                printReportReqPrms.EndRNo = request.EndRNo ?? "";
                printReportReqPrms.StartPLUNo = request.StartPLUNo ?? "";
                printReportReqPrms.EndPLUNo = request.EndPLUNo ?? "";

                int result = _paygo.paygo_PrintReport_ToDevice(printReportReqPrms, ref printReportRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "PrintReport",
                        InternalErrNum = printReportRspPrms.InternalErrNum,
                        DocumentStatus = printReportRspPrms.TranStatus,
                        ReportSoftCopy = printReportRspPrms.ReportSoftCopy,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Rapor yazdırma işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"PrintReport hatası: {ex.Message}");
                return BadRequest($"Rapor yazdırma sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("receipt/inquiry")]
        public IHttpActionResult ReceiptInquiry(string zNum, string receiptNum)
        {
            try
            {
                if (string.IsNullOrEmpty(zNum) || string.IsNullOrEmpty(receiptNum))
                {
                    return BadRequest("Z numarası ve fiş numarası gereklidir.");
                }

                ReceiptInqRspParams receiptInqRspPrms = new ReceiptInqRspParams();
                int result = _paygo.paygo_ReceiptInq_ToDevice(zNum, receiptNum, ref receiptInqRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "ReceiptInquiry",
                        InternalErrNum = receiptInqRspPrms.InternalErrNum,
                        DocumentStatus = receiptInqRspPrms.TranStatus,
                        ReportSoftCopy = receiptInqRspPrms.ReportSoftCopy,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Fiş sorgulama işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"ReceiptInquiry hatası: {ex.Message}");
                return BadRequest($"Fiş sorgulama sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("daily/totals")]
        public IHttpActionResult GetDailyTotals()
        {
            try
            {
                GetDailyTotalsRspParams getDailyTotalsRspPrms = new GetDailyTotalsRspParams();
                int result = _paygo.paygo_GetDailyTotals_ToDevice(ref getDailyTotalsRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "GetDailyTotals",
                        InternalErrNum = getDailyTotalsRspPrms.InternalErrNum,
                        DocumentStatus = getDailyTotalsRspPrms.TranStatus,
                        DailyTotals = getDailyTotalsRspPrms.DailyTotals,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Günlük toplam alma işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"GetDailyTotals hatası: {ex.Message}");
                return BadRequest($"Günlük toplam alma sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("free/print")]
        public IHttpActionResult FreePrint(FreePrintRequest request)
        {
            try
            {
                FreeTextReqParams freeTextReqPrms = new FreeTextReqParams();
                CommonRspParams commonRspPrms = new CommonRspParams();

                // Request parametrelerini doldur
                freeTextReqPrms.FreeText_Format = request.Format ?? "";
                freeTextReqPrms.FreeText_Duzen = request.Duzen ?? "";
                freeTextReqPrms.FreeText_Msg = request.Message ?? "";

                int result = _paygo.paygo_FreePrint_ToDevice(freeTextReqPrms, ref commonRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "FreePrint",
                        InternalErrNum = commonRspPrms.InternalErrNum,
                        DocumentStatus = commonRspPrms.TranStatus,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Serbest yazdırma işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"FreePrint hatası: {ex.Message}");
                return BadRequest($"Serbest yazdırma sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("exchange/rates")]
        public IHttpActionResult GetExchangeRates()
        {
            try
            {
                GetExchangeRspParams getExchangeRspPrms = new GetExchangeRspParams();
                int result = _paygo.paygo_GetExchange_ToDevice(ref getExchangeRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "GetExchangeRates",
                        InternalErrNum = getExchangeRspPrms.InternalErrNum,
                        DocumentStatus = getExchangeRspPrms.TranStatus,
                        CurrTable = getExchangeRspPrms.CurrTable,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Döviz kurları alma işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"GetExchangeRates hatası: {ex.Message}");
                return BadRequest($"Döviz kurları alma sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("exchange/set")]
        public IHttpActionResult SetExchangeRate(SetExchangeRateRequest request)
        {
            try
            {
                SetExchangeReqParams setExchangeReqPrms = new SetExchangeReqParams();
                CommonRspParams commonRspPrms = new CommonRspParams();

                // Request parametrelerini doldur
                setExchangeReqPrms.CurrencyName = request.CurrencyName ?? "";
                setExchangeReqPrms.ExcRate = request.ExcRate ?? "";
                setExchangeReqPrms.CurrencyIndex = request.CurrencyIndex ?? "";

                int result = _paygo.paygo_SetExchange_ToDevice(setExchangeReqPrms, ref commonRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "SetExchangeRate",
                        InternalErrNum = commonRspPrms.InternalErrNum,
                        DocumentStatus = commonRspPrms.TranStatus,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Döviz kuru ayarlama işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"SetExchangeRate hatası: {ex.Message}");
                return BadRequest($"Döviz kuru ayarlama sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("vat/rates")]
        public IHttpActionResult GetVatRates()
        {
            try
            {
                GetVatRatesRspParams getVatRatesRspPrms = new GetVatRatesRspParams();
                int result = _paygo.paygo_GetVatRates_ToDevice(ref getVatRatesRspPrms);
                if (result == 0)
                {
                    // Declare the dictionary outside the condition
                    Dictionary<string, int> vatRates = null;

                    // Parse VAT rates if available
                    if (!string.IsNullOrEmpty(getVatRatesRspPrms.VatRates) && getVatRatesRspPrms.VatRates.Length >= 32)
                    {
                        vatRates = new Dictionary<string, int>();
                        vatRates.Add("VatRate1", Convert.ToInt32(getVatRatesRspPrms.VatRates.Substring(0, 4)));
                        vatRates.Add("VatRate2", Convert.ToInt32(getVatRatesRspPrms.VatRates.Substring(4, 4)));
                        vatRates.Add("VatRate3", Convert.ToInt32(getVatRatesRspPrms.VatRates.Substring(8, 4)));
                        vatRates.Add("VatRate4", Convert.ToInt32(getVatRatesRspPrms.VatRates.Substring(12, 4)));
                        vatRates.Add("VatRate5", Convert.ToInt32(getVatRatesRspPrms.VatRates.Substring(16, 4)));
                        vatRates.Add("VatRate6", Convert.ToInt32(getVatRatesRspPrms.VatRates.Substring(20, 4)));
                        vatRates.Add("VatRate7", Convert.ToInt32(getVatRatesRspPrms.VatRates.Substring(24, 4)));
                        vatRates.Add("VatRate8", Convert.ToInt32(getVatRatesRspPrms.VatRates.Substring(28, 4)));
                    }

                    // Create the response object with all properties
                    var response = new
                    {
                        Success = true,
                        CommandName = "GetVatRates",
                        InternalErrNum = getVatRatesRspPrms.InternalErrNum,
                        DocumentStatus = getVatRatesRspPrms.TranStatus,
                        VatRates = vatRates, // This will be null if no VAT rates were parsed
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"KDV oranları alma işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"GetVatRates hatası: {ex.Message}");
                return BadRequest($"KDV oranları alma sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("header/set")]
        public IHttpActionResult SetHeader(SetHeaderRequest request)
        {
            try
            {
                if (request.HeaderLines == null || request.HeaderLines.Count == 0)
                {
                    return BadRequest("En az bir başlık satırı gereklidir.");
                }

                string strHeaderText = "";

                foreach (var line in request.HeaderLines)
                {
                    if (!string.IsNullOrEmpty(line.Text))
                    {
                        strHeaderText += "\n";
                        strHeaderText += line.LineNumber.ToString();
                        strHeaderText += line.Text;
                    }
                }

                CommonRspParams commonRspPrms = new CommonRspParams();
                int result = _paygo.paygo_SetHeader_ToDevice(strHeaderText, ref commonRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "SetHeader",
                        InternalErrNum = commonRspPrms.InternalErrNum,
                        DocumentStatus = commonRspPrms.TranStatus,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Başlık ayarlama işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"SetHeader hatası: {ex.Message}");
                return BadRequest($"Başlık ayarlama sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("car/park")]
        public IHttpActionResult CarPark(CarParkRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Amount))
                    return BadRequest("Tutar boş olamaz.");
                if (string.IsNullOrEmpty(request.PaymentType))
                    return BadRequest("Ödeme tipi boş olamaz.");

                CarParkRspParams carParkRspPrms = new CarParkRspParams();
                int result = _paygo.paygo_CarPark_ToDevice(
                    request.Amount,
                    request.PaymentType,
                    request.PlateNo ?? "",
                    ref carParkRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "CarPark",
                        InternalErrNum = carParkRspPrms.InternalErrNum,
                        DocumentStatus = carParkRspPrms.TranStatus,
                        ZNum = carParkRspPrms.ZNum,
                        ReceiptNum = carParkRspPrms.ReceiptNum,
                        Amount = carParkRspPrms.Amount,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Otopark işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"CarPark hatası: {ex.Message}");
                return BadRequest($"Otopark işlemi sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("plu/set")]
        public IHttpActionResult SetPLU(SetPLURequest request)
        {
            try
            {
                SetPLUReqParams setPLUReqPrms = new SetPLUReqParams();
                CommonRspParams commonRspPrms = new CommonRspParams();

                // Request parametrelerini doldur
                setPLUReqPrms.Amount = request.Amount ?? "";
                setPLUReqPrms.PLUNo = request.PLUNo ?? "";
                setPLUReqPrms.Barcode = request.Barcode ?? "";
                setPLUReqPrms.ItemName = request.ItemName ?? "";
                setPLUReqPrms.GroupNo = request.GroupNo ?? "";
                setPLUReqPrms.StockControl = request.StockControl ?? "";
                setPLUReqPrms.StockPiece = request.StockPiece ?? "";
                setPLUReqPrms.PrintPLUSlip = request.PrintPLUSlip ? "1" : "0";

                int result = _paygo.paygo_SetPLU_ToDevice(setPLUReqPrms, ref commonRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "SetPLU",
                        InternalErrNum = commonRspPrms.InternalErrNum,
                        DocumentStatus = commonRspPrms.TranStatus,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"PLU ayarlama işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"SetPLU hatası: {ex.Message}");
                return BadRequest($"PLU ayarlama sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("plu/list")]
        public IHttpActionResult GetPLUList(string startPLUNo, string endPLUNo)
        {
            try
            {
                if (string.IsNullOrEmpty(startPLUNo) || string.IsNullOrEmpty(endPLUNo))
                {
                    return BadRequest("Başlangıç ve bitiş PLU numaraları gereklidir.");
                }

                GetDepListRspParams getDepListRspPrms = new GetDepListRspParams();
                int result = _paygo.paygo_GetPLUList_ToDevice(startPLUNo, endPLUNo, ref getDepListRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "GetPLUList",
                        InternalErrNum = getDepListRspPrms.InternalErrNum,
                        DocumentStatus = getDepListRspPrms.TranStatus,
                        DepartmentInfo = getDepListRspPrms.DepartmentInfo,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"PLU listesi alma işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"GetPLUList hatası: {ex.Message}");
                return BadRequest($"PLU listesi alma sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("department/set")]
        public IHttpActionResult SetDepartment(SetDepartmentRequest request)
        {
            try
            {
                SetDepartmentListReqParams setDepartmentListReqPrms = new SetDepartmentListReqParams();
                CommonRspParams commonRspPrms = new CommonRspParams();

                // Request parametrelerini doldur
                setDepartmentListReqPrms.Amount = request.Amount ?? "";
                setDepartmentListReqPrms.DepartmentId = request.DepartmentId ?? "";
                setDepartmentListReqPrms.VatGroup = request.VatGroup ?? "";
                setDepartmentListReqPrms.ItemName = request.ItemName ?? "";
                setDepartmentListReqPrms.DepLimitAmount = request.DepLimitAmount ?? "";

                int result = _paygo.paygo_SetDepList_ToDevice(setDepartmentListReqPrms, ref commonRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "SetDepartment",
                        InternalErrNum = commonRspPrms.InternalErrNum,
                        DocumentStatus = commonRspPrms.TranStatus,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Departman ayarlama işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"SetDepartment hatası: {ex.Message}");
                return BadRequest($"Departman ayarlama sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("department/list")]
        public IHttpActionResult GetDepartmentList()
        {
            try
            { 
                CommonRspParams changeEcrModeRspPrms = new CommonRspParams();
                _paygo.paygo_ChangeEcrMode_ToDevice("3", ref changeEcrModeRspPrms);

                GetDepListRspParams getDepListRspPrms = new GetDepListRspParams();
                int result = _paygo.paygo_GetDepList_ToDevice(ref getDepListRspPrms);
                 
                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "GetDepartmentList",
                        InternalErrNum = getDepListRspPrms.InternalErrNum,
                        DocumentStatus = getDepListRspPrms.TranStatus,
                        DepartmentInfo = getDepListRspPrms.DepartmentInfo,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Departman listesi alma işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"GetDepartmentList hatası: {ex.Message}");
                return BadRequest($"Departman listesi alma sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("group/set")]
        public IHttpActionResult SetGroup(SetGroupRequest request)
        {
            try
            {
                SetGroupReqParams setGroupReqPrms = new SetGroupReqParams();
                CommonRspParams commonRspPrms = new CommonRspParams();

                // Request parametrelerini doldur
                setGroupReqPrms.GroupNo = request.GroupNo ?? "";
                setGroupReqPrms.DepartmentId = request.DepartmentId ?? "";
                setGroupReqPrms.GroupName = request.GroupName ?? "";

                int result = _paygo.paygo_SetGroup_ToDevice(setGroupReqPrms, ref commonRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "SetGroup",
                        InternalErrNum = commonRspPrms.InternalErrNum,
                        DocumentStatus = commonRspPrms.TranStatus,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Grup ayarlama işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"SetGroup hatası: {ex.Message}");
                return BadRequest($"Grup ayarlama sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("power/off")]
        public IHttpActionResult PowerOff()
        {
            try
            {
                CommonRspParams commonRspPrms = new CommonRspParams();
                int result = _paygo.paygo_PowerOFF_ToDevice(ref commonRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "PowerOff",
                        InternalErrNum = commonRspPrms.InternalErrNum,
                        DocumentStatus = commonRspPrms.TranStatus,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Cihazı kapatma işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"PowerOff hatası: {ex.Message}");
                return BadRequest($"Cihazı kapatma sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("app/restart")]
        public IHttpActionResult RestartApp()
        {
            try
            {
                CommonRspParams commonRspPrms = new CommonRspParams();
                int result = _paygo.paygo_RestartApp_ToDevice(ref commonRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "RestartApp",
                        InternalErrNum = commonRspPrms.InternalErrNum,
                        DocumentStatus = commonRspPrms.TranStatus,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Uygulama yeniden başlatma işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"RestartApp hatası: {ex.Message}");
                return BadRequest($"Uygulama yeniden başlatma sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("bank/list")]
        public IHttpActionResult GetBankList()
        {
            try
            {
                GetBankListRspParams getBankListRspPrms = new GetBankListRspParams();
                int result = _paygo.paygo_GetBankList_ToDevice(ref getBankListRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "GetBankList",
                        InternalErrNum = getBankListRspPrms.InternalErrNum,
                        DocumentStatus = getBankListRspPrms.TranStatus,
                        SentBankData = getBankListRspPrms.SentBankData,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Banka listesi alma işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"GetBankList hatası: {ex.Message}");
                return BadRequest($"Banka listesi alma sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("ecr/mode")]
        public IHttpActionResult ChangeEcrMode(ChangeEcrModeRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.EcrMode))
                    return BadRequest("ECR modu boş olamaz.");

                CommonRspParams commonRspPrms = new CommonRspParams();
                int result = _paygo.paygo_ChangeEcrMode_ToDevice(request.EcrMode, ref commonRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "ChangeEcrMode",
                        InternalErrNum = commonRspPrms.InternalErrNum,
                        DocumentStatus = commonRspPrms.TranStatus,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"ECR modu değiştirme işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"ChangeEcrMode hatası: {ex.Message}");
                return BadRequest($"ECR modu değiştirme sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("document/total")]
        public IHttpActionResult GetDocumentTotal()
        {
            try
            {
                GetReceiptTotalRspParams getReceiptTotalRspPrms = new GetReceiptTotalRspParams();
                int result = _paygo.paygo_GetReceiptTotal_ToDevice(ref getReceiptTotalRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "GetDocumentTotal",
                        InternalErrNum = getReceiptTotalRspPrms.InternalErrNum,
                        DocumentStatus = getReceiptTotalRspPrms.TranStatus,
                        ReceiptNum = getReceiptTotalRspPrms.ReceiptNum,
                        Amount = getReceiptTotalRspPrms.Amount,
                        RemainAmount = getReceiptTotalRspPrms.RemainAmount,
                        ReportSoftCopy = getReceiptTotalRspPrms.ReportSoftCopy,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Belge toplamı alma işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"GetDocumentTotal hatası: {ex.Message}");
                return BadRequest($"Belge toplamı alma sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("drawer")]
        public IHttpActionResult Drawer(DrawerRequest request)
        {
            try
            {
                DrawerReqParams drawerReqPrms = new DrawerReqParams();
                DrawerRspParams drawerRspPrms = new DrawerRspParams();

                drawerReqPrms.freeTextReqPrms = new FreeTextReqParams();

                // Request parametrelerini doldur
                drawerReqPrms.Amount = request.Amount ?? "";
                drawerReqPrms.DrawerStatus = request.DrawerStatus ?? "01"; // 01: Save, 02: Take

                if (request.FreeText != null)
                {
                    drawerReqPrms.freeTextReqPrms.FreeText_Format = request.FreeText.Format ?? "";
                    drawerReqPrms.freeTextReqPrms.FreeText_Duzen = request.FreeText.Duzen ?? "";
                    drawerReqPrms.freeTextReqPrms.FreeText_Msg = request.FreeText.Message ?? "";
                }

                int result = _paygo.paygo_Drawer_ToDevice(drawerReqPrms, ref drawerRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "Drawer",
                        InternalErrNum = drawerRspPrms.InternalErrNum,
                        DocumentStatus = drawerRspPrms.TranStatus,
                        DrawerTotalAmount = drawerRspPrms.DrawerTotalAmount,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Çekmece işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"Drawer hatası: {ex.Message}");
                return BadRequest($"Çekmece işlemi sırasında hata oluştu: {ex.Message}");
            }
        } 

        [HttpPost]
        [Route("non-tax/item")]
        public IHttpActionResult NonTaxItem(NonTaxItemRequest request)
        {
            try
            {
                NonTaxItemReqParams nonTaxItemReqPrms = new NonTaxItemReqParams();
                CommonRspParams commonRspPrms = new CommonRspParams();

                // Request parametrelerini doldur
                nonTaxItemReqPrms.Amount = request.Amount ?? "";
                nonTaxItemReqPrms.NonTaxId = request.NonTaxId ?? "";
                nonTaxItemReqPrms.ItemName = request.ItemName ?? "";
                nonTaxItemReqPrms.Tckn_value = request.Tckn ?? "0";

                int result = _paygo.paygo_NonTaxItem_ToDevice(nonTaxItemReqPrms, ref commonRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "NonTaxItem",
                        InternalErrNum = commonRspPrms.InternalErrNum,
                        DocumentStatus = commonRspPrms.TranStatus,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Vergisiz ürün işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"NonTaxItem hatası: {ex.Message}");
                return BadRequest($"Vergisiz ürün işlemi sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("batch/freeprint")]
        public IHttpActionResult FreePrintBatch(FreePrintBatchRequest request)
        {
            try
            {
                if (request.BatchItems == null || request.BatchItems.Count == 0)
                {
                    return BadRequest("En az bir serbest yazdırma öğesi gereklidir.");
                }

                string strBatchTranItems = "";
                int batchItemCount = Math.Min(request.BatchItems.Count, 100); // Maksimum 100 öğe

                for (int i = 0; i < batchItemCount; i++)
                {
                    var item = request.BatchItems[i];

                    strBatchTranItems += $"No:{i + 1}\n";
                    strBatchTranItems += $"ProType:A1\n"; // Serbest yazdırma için A1
                    strBatchTranItems += $"DepId:\n";
                    strBatchTranItems += $"PLUId:\n";
                    strBatchTranItems += $"NONTAXId:\n";
                    strBatchTranItems += $"CollId:\n";
                    strBatchTranItems += $"Quantity:\n";
                    strBatchTranItems += $"UnitPrice:\n";
                    strBatchTranItems += $"Barcode:\n";
                    strBatchTranItems += $"Rate:\n";
                    strBatchTranItems += $"ItemName:\n";
                    strBatchTranItems += $"Amount:\n";
                    strBatchTranItems += $"TranId:\n";

                    // FreeText için format oluştur
                    string freeText = item.Format + item.Duzen + string.Format("{0:x2}", item.Message.Length);
                    if (!string.IsNullOrEmpty(item.Message))
                    {
                        UTF8Encoding utf8 = new UTF8Encoding();
                        byte[] encodeBytes = utf8.GetBytes(item.Message);
                        freeText += ByteArrayToHexString(encodeBytes, encodeBytes.Length);
                    }

                    strBatchTranItems += $"FreeText:{freeText}\n";
                    strBatchTranItems += "\r";
                }

                if (batchItemCount > 0)
                {
                    strBatchTranItems += "\n\n";
                }

                DoBatchTranReqParams doBatchTranReqPrms = new DoBatchTranReqParams();
                BatchTranRspParams batchTranRspPrms = new BatchTranRspParams();

                doBatchTranReqPrms.BatchItemCnt = batchItemCount;
                doBatchTranReqPrms.strBatchTranItems = strBatchTranItems;

                int result = _paygo.paygo_FreePrintDoBatchTran_ToDevice(doBatchTranReqPrms, ref batchTranRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "FreePrintBatch",
                        InternalErrNum = batchTranRspPrms.InternalErrNum,
                        DocumentStatus = batchTranRspPrms.TranStatus,
                        ZNum = batchTranRspPrms.ZNum,
                        ReceiptNum = batchTranRspPrms.ReceiptNum,
                        SoldItems = batchTranRspPrms.SoldItems,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Toplu serbest yazdırma işlemi başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"FreePrintBatch hatası: {ex.Message}");
                return BadRequest($"Toplu serbest yazdırma sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("pair")]
        public IHttpActionResult Pair()
        {
            try
            {
                EchoRspParams echoRspPrms = new EchoRspParams();
                int result = _paygo.paygo_GMP3Pair_ToDevice(ref echoRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "Pair",
                        InternalErrNum = echoRspPrms.InternalErrNum,
                        ErrRespCode = echoRspPrms.groupDF6F.ErrRespCode,
                        VersionInfo = echoRspPrms.groupDF6F.VersionInfo,
                        ExtDevIndex = echoRspPrms.groupDF6F.ExtDevIndex,
                        DocumentStatus = echoRspPrms.TranStatus,
                        TermSerial = echoRspPrms.groupDF02.TermSerial,
                        TranDate = echoRspPrms.groupDF02.TranDate,
                        TranTime = echoRspPrms.groupDF02.TranTime,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Eşleştirme komutu başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"Pair hatası: {ex.Message}");
                return BadRequest($"Eşleştirme komutu sırasında hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("disconnect")]
        public IHttpActionResult Disconnect()
        {
            try
            {
                _paygo.paygo_CloseComm();
                return Ok(new { Success = true, Message = "Bağlantı başarıyla kapatıldı." });
            }
            catch (Exception ex)
            {
                LogToFile($"Disconnect hatası: {ex.Message}");
                return BadRequest($"Bağlantı kapatılırken hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("batch/create")]
        public IHttpActionResult CreateBatch(CreateBatchRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.BatchId))
                    return BadRequest("Batch ID boş olamaz.");

                if (_batchCollections.ContainsKey(request.BatchId))
                    return BadRequest($"Bu ID ile zaten bir batch koleksiyonu mevcut: {request.BatchId}");

                _batchCollections[request.BatchId] = new List<BatchItem>();

                return Ok(new
                {
                    Success = true,
                    Message = $"Batch koleksiyonu oluşturuldu: {request.BatchId}",
                    BatchId = request.BatchId,
                    ItemCount = 0
                });
            }
            catch (Exception ex)
            {
                LogToFile($"CreateBatch hatası: {ex.Message}");
                return BadRequest($"Batch oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("batch/add")]
        public IHttpActionResult AddBatchItem(AddBatchItemRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.BatchId))
                    return BadRequest("Batch ID boş olamaz.");

                if (!_batchCollections.ContainsKey(request.BatchId))
                    return BadRequest($"Bu ID ile bir batch koleksiyonu bulunamadı: {request.BatchId}");

                if (request.Item == null)
                    return BadRequest("Batch öğesi boş olamaz.");

                // ProcessType kontrolü
                if (request.Item.ProcessType == "AA")
                    return BadRequest("All void (AA) batch tablosuna eklenemez.");

                // Özel kontroller - Form1.cs'deki BtnAddBatch_Click metodundan uyarlanmıştır
                if (request.Item.ProcessType != "A1")
                {
                    // A4 (Amount Discount) kontrolü
                    if (request.Item.ProcessType == "A4" && string.IsNullOrEmpty(request.Item.Amount))
                    {
                        return BadRequest("İndirim işleminde, tutar doldurulmalıdır!");
                    }

                    // A5/A6 (Discount/Increase) kontrolü
                    if ((request.Item.ProcessType == "A5" || request.Item.ProcessType == "A6") &&
                        string.IsNullOrEmpty(request.Item.Rate))
                    {
                        return BadRequest("İndirim ve artırım işleminde, artırım oranı doldurulmalıdır!");
                    }
                }

                // Maksimum öğe kontrolü
                if (_batchCollections[request.BatchId].Count >= 100)
                    return BadRequest("Bir batch koleksiyonu en fazla 100 öğe içerebilir.");

                // Öğeyi ekle
                _batchCollections[request.BatchId].Add(request.Item);

                return Ok(new
                {
                    Success = true,
                    Message = $"Öğe batch koleksiyonuna eklendi: {request.BatchId}",
                    BatchId = request.BatchId,
                    ItemCount = _batchCollections[request.BatchId].Count,
                    ItemIndex = _batchCollections[request.BatchId].Count - 1
                });
            }
            catch (Exception ex)
            {
                LogToFile($"AddBatchItem hatası: {ex.Message}");
                return BadRequest($"Batch öğesi eklenirken hata oluştu: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("batch/items")]
        public IHttpActionResult GetBatchItems(string batchId)
        {
            try
            {
                if (string.IsNullOrEmpty(batchId))
                    return BadRequest("Batch ID boş olamaz.");

                if (!_batchCollections.ContainsKey(batchId))
                    return BadRequest($"Bu ID ile bir batch koleksiyonu bulunamadı: {batchId}");

                return Ok(new
                {
                    Success = true,
                    BatchId = batchId,
                    ItemCount = _batchCollections[batchId].Count,
                    Items = _batchCollections[batchId]
                });
            }
            catch (Exception ex)
            {
                LogToFile($"GetBatchItems hatası: {ex.Message}");
                return BadRequest($"Batch öğeleri alınırken hata oluştu: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("batch/clear")]
        public IHttpActionResult ClearBatch(string batchId)
        {
            try
            {
                if (string.IsNullOrEmpty(batchId))
                    return BadRequest("Batch ID boş olamaz.");

                if (!_batchCollections.ContainsKey(batchId))
                    return BadRequest($"Bu ID ile bir batch koleksiyonu bulunamadı: {batchId}");

                _batchCollections[batchId].Clear();

                return Ok(new
                {
                    Success = true,
                    Message = $"Batch koleksiyonu temizlendi: {batchId}",
                    BatchId = batchId,
                    ItemCount = 0
                });
            }
            catch (Exception ex)
            {
                LogToFile($"ClearBatch hatası: {ex.Message}");
                return BadRequest($"Batch temizlenirken hata oluştu: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("batch/delete")]
        public IHttpActionResult DeleteBatch(string batchId)
        {
            try
            {
                if (string.IsNullOrEmpty(batchId))
                    return BadRequest("Batch ID boş olamaz.");

                if (!_batchCollections.ContainsKey(batchId))
                    return BadRequest($"Bu ID ile bir batch koleksiyonu bulunamadı: {batchId}");

                _batchCollections.Remove(batchId);

                return Ok(new
                {
                    Success = true,
                    Message = $"Batch koleksiyonu silindi: {batchId}"
                });
            }
            catch (Exception ex)
            {
                LogToFile($"DeleteBatch hatası: {ex.Message}");
                return BadRequest($"Batch silinirken hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("batch/execute")]
        public IHttpActionResult ExecuteBatch(ExecuteBatchRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.BatchId))
                    return BadRequest("Batch ID boş olamaz.");

                if (!_batchCollections.ContainsKey(request.BatchId))
                    return BadRequest($"Bu ID ile bir batch koleksiyonu bulunamadı: {request.BatchId}");

                if (_batchCollections[request.BatchId].Count == 0)
                    return BadRequest("Batch koleksiyonu boş, en az bir öğe gereklidir.");

                // Batch işlem öğelerini hazırla
                string strBatchTranItems = "";
                int batchItemCount = _batchCollections[request.BatchId].Count;

                for (int i = 0; i < batchItemCount; i++)
                {
                    var item = _batchCollections[request.BatchId][i];

                    strBatchTranItems += $"No:{i + 1}\n";
                    strBatchTranItems += $"ProType:{item.ProcessType ?? ""}\n";
                    strBatchTranItems += $"DepId:{item.DepartmentId ?? ""}\n";
                    strBatchTranItems += $"PLUId:{item.PLUID ?? ""}\n";
                    strBatchTranItems += $"NONTAXId:{item.NonTaxId ?? ""}\n";
                    strBatchTranItems += $"CollId:{item.CollectionId ?? ""}\n";
                    strBatchTranItems += $"Quantity:{item.Quantity ?? ""}\n";
                    strBatchTranItems += $"UnitPrice:{item.UnitPrice ?? ""}\n";
                    strBatchTranItems += $"Barcode:{item.Barcode ?? ""}\n";
                    strBatchTranItems += $"Rate:{item.Rate ?? ""}\n";

                    // ItemName için UTF8 kodlama
                    strBatchTranItems += "ItemName:";
                    if (!string.IsNullOrEmpty(item.ItemName))
                    {
                        UTF8Encoding utf8 = new UTF8Encoding();
                        byte[] encodeBytes = utf8.GetBytes(item.ItemName);
                        strBatchTranItems += ByteArrayToHexString(encodeBytes, encodeBytes.Length);
                    }
                    strBatchTranItems += "\n";

                    strBatchTranItems += $"Amount:{item.Amount ?? ""}\n";
                    strBatchTranItems += $"TranId:{item.TranId ?? ""}\n";
                    strBatchTranItems += $"FreeText:{item.FreeText ?? ""}\n";
                    strBatchTranItems += "\r";
                }

                if (batchItemCount > 0)
                {
                    strBatchTranItems += "\n\n";
                }

                DoBatchTranReqParams doBatchTranReqPrms = new DoBatchTranReqParams();
                BatchTranRspParams batchTranRspPrms = new BatchTranRspParams();

                doBatchTranReqPrms.BatchItemCnt = batchItemCount;
                doBatchTranReqPrms.strBatchTranItems = strBatchTranItems;

                int result = _paygo.paygo_DoBatchTran_ToDevice(doBatchTranReqPrms, ref batchTranRspPrms);

                if (result == 0)
                {
                    var response = new
                    {
                        Success = true,
                        CommandName = "BatchTransaction",
                        InternalErrNum = batchTranRspPrms.InternalErrNum,
                        DocumentStatus = batchTranRspPrms.TranStatus,
                        ZNum = batchTranRspPrms.ZNum,
                        ReceiptNum = batchTranRspPrms.ReceiptNum,
                        SoldItems = batchTranRspPrms.SoldItems,
                        ResponsedTag = _paygo.TranPrms.rspMem.ResponsedTag,
                        BatchId = request.BatchId,
                        ItemCount = batchItemCount
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest($"Toplu işlem başarısız. Hata kodu: {result}");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"ExecuteBatch hatası: {ex.Message}");
                return BadRequest($"Batch çalıştırılırken hata oluştu: {ex.Message}");
            }
        }

        // Yardımcı metotlar
        private string ByteArrayToHexString(byte[] bytes, int length)
        {
            StringBuilder sb = new StringBuilder(length * 2);
            for (int i = 0; i < length; i++)
            {
                sb.AppendFormat("{0:X2}", bytes[i]);
            }
            return sb.ToString();
        }

        // Türkçe karakter dönüşümü için yardımcı metot
        private string TrCharReplace(string str)
        {
            char[] nTr = { 'ð', 'þ', 'ý', 'Þ', 'Ý', 'Ð' };
            char[] Tr = { 'ğ', 'ş', 'ı', 'Ş', 'İ', 'Ğ' };
            for (int i = 0; i < nTr.Length; i++)
                str = str.Replace(nTr[i], Tr[i]);
            return str;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    // Bağlantıyı kapat
                    //_paygo.paygo_CloseComm();
                }
                catch (Exception ex)
                {
                    LogToFile($"Dispose hatası: {ex.Message}");
                }
            }
            base.Dispose(disposing);
        }
    }
}