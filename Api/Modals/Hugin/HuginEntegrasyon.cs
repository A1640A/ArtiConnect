using DevExpress.PivotGrid.OLAP.Mdx;
using Hugin.Common;
using Hugin.ExDevice;
using Hugin.GMPCommon;
using Hugin.POS.CompactPrinter.FP300;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArtiConnect.Api.Modals.Hugin.Enums;

namespace ArtiConnect.Api.Modals.Hugin
{
    public class UrunBilgisi
    {
        public string UrunAdi { get; set; }
        public int KdvOrani { get; set; }
        public int DepartmanNo { get; set; }
        public decimal Miktar { get; set; }
        public decimal Fiyat { get; set; }
    }

    public class HuginEntegrasyon : IBridge
    {
        public string fiscalId = "";
        public static Encoding DefaultEncoding = Encoding.GetEncoding(1254);
        public static string[] credits = new string[ProgramConfig.MAX_CREDIT_COUNT];
        public static FCurrency[] currencies = new FCurrency[ProgramConfig.MAX_FCURRENCY_COUNT];
        public static string[] foodCards = new string[ProgramConfig.MAX_CREDIT_COUNT];
        public Dictionary<int, int> kdvOraniDepartmanNoDictionary;

        public FormLanguage language = FormLanguage.TR;
        public static ICompactPrinter printer = null;

        public ICompactPrinter Printer
        {
            get
            {
                return printer;
            }
        }

        public FormLanguage Language
        {
            get { return language; }
        }

        public static string[] Credits
        {
            get { return credits; }
        }
        public static string[] FoodCards
        {
            get { return foodCards; }
        }

        public static void SetCredit(int id, string creditName)
        {
            credits[id] = creditName;
        }

        public static void SetFoodCard(int id, string foodCardName)
        {
            credits[id] = foodCardName;
        }

        public static FCurrency[] Currencies
        {
            get { return currencies; }
        }

        public static void SetCurrency(int id, FCurrency currency)
        {
            if (id < currencies.Length)
            {
                currencies[id] = currency;
            }
        }

        public string FiscalId
        {
            get { return fiscalId; }
        }

        public void SetFiscalId(string strId)
        {
            int id = int.Parse(strId.Substring(2));

            if (id == 0 || id > 99999999)
            {
                throw new Exception("Geçersiz mali numara.");
            }
            fiscalId = strId;

            if (printer != null)
                printer.FiscalRegisterNo = fiscalId;
        }

        public delegate void LogDelegate(String log);
        public void Log(string log)
        {
            if (!Directory.Exists("HuginLog"))
                Directory.CreateDirectory("HuginLog");

            string fileName = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
            string filePath = Path.Combine("HuginLog", fileName);

            string timeStampedLog = $"[{DateTime.Now:HH:mm:ss}] {log}";

            try
            {
                File.AppendAllText(filePath, timeStampedLog + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Dosya yazma hatası: {ex.Message}");
            }
        }

        public delegate void LogDelegate2();
        public void Log()
        {
            if (printer != null)
            {
                // Log klasörünün kontrolü ve oluşturulması
                if (!Directory.Exists("HuginLog"))
                    Directory.CreateDirectory("HuginLog");

                // Tarih ve saat ile dosya adı oluşturma
                string fileName = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
                string filePath = Path.Combine("HuginLog", fileName);

                string lastlog = printer.GetLastLog();
                StringBuilder logBuilder = new StringBuilder();

                logBuilder.AppendLine("***************************************************");

                if (!String.IsNullOrEmpty(lastlog))
                {
                    if (!lastlog.Contains("|"))
                    {
                        Log(lastlog);
                        return;
                    }

                    string[] parsedLog = lastlog.Split('|');

                    if (parsedLog.Length == 5)
                    {
                        string command = parsedLog[0];
                        string sequnce = parsedLog[1];
                        string state = parsedLog[2];
                        string errorCode = parsedLog[3];
                        string errorMsg = parsedLog[4];

                        if (command != "NULL")
                        {
                            string commandText;
                            if (sequnce.Length == 1)
                                commandText = String.Format("{0} {1}:", sequnce, FormMessage.COMMAND.PadRight(12, ' '));
                            else if (sequnce.Length == 2)
                                commandText = String.Format("{0} {1}:", sequnce, FormMessage.COMMAND.PadRight(11, ' '));
                            else
                                commandText = String.Format("{0} {1}:", sequnce, FormMessage.COMMAND.PadRight(10, ' '));

                            logBuilder.AppendLine($"{commandText}{command}");
                            logBuilder.AppendLine($"  {FormMessage.FPU_STATE.PadRight(12, ' ')}:{state}");
                        }

                        string responseStatus = "SUCCESS";
                        if (errorCode != "0")
                        {
                            responseStatus = state == FormMessage.NEED_SERVICE && errorCode != "3" ? "WARNING" : "ERROR";
                        }

                        logBuilder.AppendLine($"  {FormMessage.RESPONSE.PadRight(12, ' ')}:{errorMsg} [{responseStatus}]");
                    }
                }

                try
                {
                    // Zaman damgası ekleme
                    string timeStampedLog = $"[{DateTime.Now:HH:mm:ss}]\n{logBuilder}";
                    File.WriteAllText(filePath, timeStampedLog);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Dosya yazma hatası: {ex.Message}");
                }
            }
        }

        public static IConnection conn;
        public IConnection Connection
        {
            get
            {
                return conn;
            }
            set
            {
                conn = value;
            }
        }

        public static int seqNumber = 1;
        public static int SequenceNumber
        {
            get { return seqNumber; }
            set { seqNumber = value; }
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public string GetMBId()
        {
            System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            System.Management.ManagementObjectCollection moc = mos.Get();
            string motherBoard = "";
            foreach (System.Management.ManagementObject mo in moc)
            {
                motherBoard = (string)mo["SerialNumber"];
            }

            return motherBoard;
        }

        public static bool isMatchedBefore = false;
        public void MatchExDevice(string FiscalId, string Port)
        {
            SetFiscalId(FiscalId);

            // DeviceInfo sınıfı gerekli bilgiler ile doldurulur
            DeviceInfo serverInfo = new DeviceInfo();
            serverInfo.IP = System.Net.IPAddress.Parse(GetIPAddress());
            serverInfo.IPProtocol = IPProtocol.IPV4;

            serverInfo.Brand = "HUGIN";

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["Brand"]))
            {
                serverInfo.Brand = ConfigurationManager.AppSettings["Brand"];
            }

            serverInfo.Model = "HUGIN COMPACT";
            serverInfo.Port = Convert.ToInt32(Port);
            serverInfo.TerminalNo = FiscalId.PadLeft(8, '0');
            serverInfo.Version = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).LastWriteTime.ToShortDateString();
            try
            {
                // Motherboard serisi alınır
                serverInfo.SerialNum = CreateMD5(GetMBId()).Substring(0, 8);
            }
            catch
            {
                // Seri alınırken sıkıntı yaşanırsa default bir değer verilebilir
                serverInfo.SerialNum = "ABCD1234";
            }


            if (conn.IsOpen)
            {
                if (isMatchedBefore)
                {
                    // Eğer önceden eşleme yapıldıysa sadece connection objesinin kütüphaneye set edlmesi yeterli olacaktır.
                    printer.SetCommObject(conn.ToObject());
                    return;
                }
                try
                {
                    printer = new CompactPrinter();

                    // Eşleme öncesi ÖKC sicil numarası kütüphane üzerinde ilgili alana set edilir.
                    printer.FiscalRegisterNo = fiscalId;

                    try
                    {
                        // Loglama yapılacak dizin ve log seviyesi istenirse set edilir. Opsiyonel seçeneklerdir. 
                        //Set edilmemesi durumunda default değerler kullanılır.
                        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogDirectory"]))
                        {
                            printer.LogDirectory = System.Configuration.ConfigurationManager.AppSettings["LogDirectory"];
                        }

                        printer.LogerLevel = int.Parse(System.Configuration.ConfigurationManager.AppSettings["LogLevel"]);
                    }
                    catch { }

                    // Eşleme başlatılır. Başarılı ise true, başarısız ise false döner.
                    if (!printer.Connect(conn.ToObject(), serverInfo))
                    {
                        throw new OperationCanceledException(FormMessage.UNABLE_TO_MATCH);
                    }

                    // ÖKC üzerinde desteklenen bağlantı kapasitesi kontrol edilir, oluşturulan connection ile farklı ise düzenleme yapılır.
                    // Check supported printer size and set if it is different
                    if (printer.PrinterBufferSize != conn.BufferSize)
                    {
                        conn.BufferSize = printer.PrinterBufferSize;
                    }
                    printer.SetCommObject(conn.ToObject());
                    isMatchedBefore = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                CPResponse.Bridge = this;
            }
        }

        public string GetIPAddress()
        {
            System.Net.IPHostEntry host;
            string localIP = "?";
            host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (System.Net.IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }

        public class Departman
        {
            public int No { get; set; }
            public string KisimAdi { get; set; }
            public int KdvGrubuNo { get; set; }
        }

        public List<Departman> GetDepartman()
        {
            var result = new List<Departman>();

            for (int i = 0; i < ProgramConfig.MAX_DEPARTMENT_COUNT; i++)
            {
                try
                {
                    CPResponse response = new CPResponse(this.Printer.GetDepartment(i + 1));

                    if (response.ErrorCode != 0)
                    {
                        return null;
                    }

                    var departman = new Departman();
                    departman.No = i + 1;

                    string paramVal = response.GetNextParam();
                    if (!String.IsNullOrEmpty(paramVal))
                    {
                        departman.KisimAdi = paramVal;
                    }

                    paramVal = response.GetNextParam();
                    if (!String.IsNullOrEmpty(paramVal))
                    {
                        departman.KdvGrubuNo = int.TryParse(paramVal, out int kdvNo) ? kdvNo : 0;
                    }

                    result.Add(departman);
                }
                catch (TimeoutException)
                {
                    this.Log(FormMessage.TIMEOUT_ERROR);
                }
                catch
                {
                    this.Log(FormMessage.OPERATION_FAILS);
                }
            }

            return result;
        }

        public class KdvGrubu
        {
            public int No { get; set; }
            public int KdvOrani { get; set; }
        }

        public List<KdvGrubu> GetKdvGruplari()
        {
            var result = new List<KdvGrubu>();

            for (int i = 0; i < ProgramConfig.MAX_VAT_RATE_COUNT; i++)
            {
                try
                {
                    CPResponse response = new CPResponse(this.Printer.GetVATRate(i));

                    if (response.ErrorCode == 0)
                    {
                        var vat = response.GetNextParam();

                        result.Add(new KdvGrubu
                        {
                            No = i + 1,
                            KdvOrani = int.TryParse(vat, out int kdvOrani) ? kdvOrani : -1
                        });
                    }
                }
                catch (TimeoutException)
                {
                    this.Log(FormMessage.TIMEOUT_ERROR);
                }
                catch
                {
                    this.Log(FormMessage.OPERATION_FAILS);
                }
            }

            return result;
        }

        public Dictionary<int, int> GetKdvOraniDepartmanNoDictionary()
        {
            var result = new Dictionary<int, int>();

            var departmanList = GetDepartman();
            var kdvGrubuList = GetKdvGruplari();

            foreach (var kdvGrubu in kdvGrubuList.Where(x => x.KdvOrani >= 0))
            {
                var departman = departmanList.FirstOrDefault(x => x.KdvGrubuNo == kdvGrubu.No);
                if (departman != null)
                    result.Add(kdvGrubu.KdvOrani, departman.No);
            }

            return result;
        }

        public void start()
        {
            try
            {
                CPResponse response = new CPResponse(this.Printer.PrintDocumentHeader());
                if (response.ErrorCode == 0)
                {
                    this.Log(FormMessage.DOCUMENT_ID.PadRight(12, ' ') + ":" + response.GetNextParam());
                }
            }
            catch (System.Exception ex)
            {
                this.Log(FormMessage.OPERATION_FAILS + ": " + ex.Message);
            }
        }

        public string FixTurkishUpperCase(string text)
        {
            // stack current culture
            System.Globalization.CultureInfo currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            // Set Turkey culture
            System.Globalization.CultureInfo turkey = new System.Globalization.CultureInfo("tr-TR");
            System.Threading.Thread.CurrentThread.CurrentCulture = turkey;

            string cultured = text.ToUpper();

            // Pop old culture
            System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;

            return cultured;
        }

        public void addSatis(UrunBilgisi satis)
        {
            int pluNo = new Random().Next(0, 99999);
            Decimal quantity = satis.Miktar;
            Decimal price = satis.Fiyat;
            var department = kdvOraniDepartmanNoDictionary[satis.KdvOrani];

            try
            {
                CPResponse response = new CPResponse(this.Printer.PrintItem(pluNo, quantity, price, FixTurkishUpperCase(satis.UrunAdi), "", department, 0));


                if (response.ErrorCode == 0)
                {
                    this.Log(String.Format(FormMessage.SUBTOTAL.PadRight(12, ' ') + ":{0}", response.GetNextParam()));
                }
                else if (response.ErrorCode == 114)
                    addSatis(satis);
            }
            catch (Exception ex)
            {
                this.Log(FormMessage.OPERATION_FAILS + ": " + ex.Message);
            }
        }

        public void addCashPayment(decimal amount)
        {
            try
            {
                CPResponse response = new CPResponse(this.Printer.PrintPayment(0, -1, amount));

                if (response.ErrorCode == 0)
                {
                    this.Log(String.Format(FormMessage.SUBTOTAL.PadRight(12, ' ') + ":{0}", response.GetNextParam()));

                    this.Log(String.Format(FormMessage.PAID_TOTAL.PadRight(12, ' ') + ":{0}", response.GetNextParam()));

                }

            }
            catch (System.Exception ex)
            {
                this.Log(FormMessage.OPERATION_FAILS + ": " + ex.Message);
            }
        }

        public void addCreditPayment(decimal amount)
        {
            try
            {
                CPResponse response = new CPResponse(this.Printer.GetEFTAuthorisation(amount, 1, ""));

                if (response.ErrorCode == 0)
                {
                    string totalAmount = response.GetNextParam();
                    string provisionCode = response.GetNextParam();
                    string paidAmount = response.GetNextParam();
                    string installmentCount = response.GetNextParam();
                    string acquirerId = response.GetNextParam();
                    string bin = response.GetNextParam();
                    string issuerId = response.GetNextParam();
                    string subOprtType = response.GetNextParam();
                    string batch = response.GetNextParam();
                    string stan = response.GetNextParam();
                    string totalPaidAmount = response.GetNextParam();

                    this.Log(String.Format("İşlem Tutarı   :{0}", paidAmount));
                    this.Log(String.Format("Ödeme Toplamı  :{0}", totalPaidAmount));
                    this.Log(String.Format("Belge Tutarı   :{0}", totalAmount));
                    this.Log(String.Format("Taksit sayısı  :{0}", installmentCount));
                    this.Log(String.Format("Provizyon kodu :{0}", provisionCode));
                    this.Log(String.Format("ACQUIRER ID    :{0}", acquirerId));
                    this.Log(String.Format("BIN            :{0}", bin));
                    this.Log(String.Format("ISSUERER ID    :{0}", issuerId));
                    if (!String.IsNullOrEmpty(batch))
                        this.Log(String.Format("BATCH NO       :{0}", batch));
                    if (!String.IsNullOrEmpty(stan))
                        this.Log(String.Format("STAN NO        :{0}", stan));

                    if (subOprtType == null)
                    {
                        subOprtType = SubOperationType.SATIS.ToString();
                    }
                    else
                    {
                        subOprtType = Enum.GetName(typeof(SubOperationType), int.Parse(subOprtType));
                    }
                    this.Log(String.Format("Alt İşlem Tipi :{0}", subOprtType));

                }

            }
            catch (System.Exception ex)
            {
                this.Log(FormMessage.OPERATION_FAILS + ": " + ex.Message);
            }
        }

        public void endSale()
        {
            try
            {
                CPResponse response = new CPResponse(this.Printer.CloseReceipt(false));

                if (response.ErrorCode == 0)
                {
                    this.Log(FormMessage.DOCUMENT_ID.PadRight(12, ' ') + ":" + response.GetNextParam());
                }
            }
            catch (Exception ex)
            {
                this.Log(FormMessage.OPERATION_FAILS + ": " + ex.Message);
            }
        }

        public void addDiscount(decimal amount)
        {
            try
            {
                CPResponse response = new CPResponse(this.Printer.PrintAdjustment((int)AdjustmentType.PercentFee, amount, 0));

                if (response.ErrorCode == 0)
                {
                    this.Log(String.Format(FormMessage.SUBTOTAL.PadRight(12, ' ') + ":{0:#0.00}", response.GetNextParam()));
                }
            }
            catch (System.Exception ex)
            {
                this.Log(FormMessage.OPERATION_FAILS + ": " + ex.Message);
            }
        }
    }
}
