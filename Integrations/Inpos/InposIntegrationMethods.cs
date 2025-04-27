using DevExpress.XtraSplashScreen;
using Inpos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtiConnect.Integrations.Inpos
{
    public class InposIntegrationMethods
    {
        private InposSaleReceipt receipt;
        private InposEcrMultipleSaleItems saleItems = new InposEcrMultipleSaleItems(0);
        private UInt32 Timeout = 10000;
        private bool Closing = false;
        private string SerialNo;
        private string ListenIp = "0  .0  .0  .0";
        private string ListenPort = "59000";

        public InposIntegrationMethods(string _SerialNo)
        {
            SerialNo = _SerialNo;

            SetActiveDevice();
        }

        public class ActionResult
        {
            public bool Status { get; set; }
            public string Message { get; set; }
            public bool Success { get; internal set; }

            public static ActionResult ReturnSuccess(string Message = "")
            {
                return new ActionResult()
                {
                    Status = true,
                    Message = string.IsNullOrEmpty(Message) ? "Işlem Tamamlandı" : Message
                };
            }

            public static ActionResult ReturnError(string ErrorMessage)
            {
                return new ActionResult()
                {
                    Status = false,
                    Message = ErrorMessage
                };
            }
        }

        public static string GetInposExtVersion()
        {
            return InposExt.Version();
        }

        public static void CloseAll()
        {
            InposExt.CloseAll();
        }

        public static ActionResult GetErrorDetailString()
        {
            InposExtErrorDetail errorDetail = InposExt.ErrorDetail();

            if (errorDetail == InposExtErrorDetail.InposNoErrorDetail)
                return ActionResult.ReturnSuccess();

            return ActionResult.ReturnError("Hata Detayı: " + ((Int32)errorDetail).ToString() + " " + "(" + errorDetail.ToString() + ")");
        }

        public ActionResult SetActiveDevice()
        {
            InposExtError error = InposExt.SetActiveDevice(SerialNo);

            if (error == InposExtError.InposNoError)
            {
                String sn = "";
                InposExt.ActiveDevice(out sn);
                return ActionResult.ReturnSuccess("Etkin cihaz değiştirildi: " + sn);
            }
            else
            {
                return ActionResult.ReturnError("Etkin cihaz değiştirilemedi. "
                          + "Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString());
            }
        }

        public ActionResult Initialize()
        {
            InposExt.Close();
            InposExt.CloseAll();

            Application.DoEvents();

            Closing = false;

            while (!Closing)
            {
                InposExtError error = InposExt.Initialize(1, SerialNo.ToUpper(), ListenIp.Replace(" ", ""), (UInt16)decimal.Parse(ListenPort), 10000);
                if (error != InposExtError.InposNoError)
                {
                    if (error != InposExtError.InposConnectionError)
                    {
                        Closing = true;
                        return ActionResult.ReturnError("Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                                  + GetErrorDetailString());
                    }
                    else
                    {
                        Closing = true;
                    }
                }
                else
                {
                    break;
                }

                Application.DoEvents();
            }

            if (!Closing)
            {
                UInt64 limit = 0;
                Int32 zdt = 0, edt = 0;

                InposExtError error = InposExt.SaleLimit(Timeout, ref limit);
                if (error == InposExtError.InposNoError)
                {
                    error = InposExt.LastZDateTime(Timeout, ref zdt);
                    if (error == InposExtError.InposNoError)
                    {
                        error = InposExt.EcrDateTime(Timeout, ref edt);
                    }
                }

                if (error != InposExtError.InposNoError)
                {
                    return ActionResult.ReturnSuccess("Bağlı.");
                }
                else
                {
                    String sn = "";
                    InposExt.ActiveDevice(out sn);

                    return ActionResult.ReturnSuccess("Bağlı."
                                + " Satış limiti: " + String.Format("{0:0.00}", limit / 100.0).ToString() + " TL"
                                + " Son Z: " + InposExt.FromUnixTime(zdt).ToString("dd.MM.yyyy HH:mm:ss")
                                + " Yazarkasa saati: " + InposExt.FromUnixTime(edt).ToString("dd.MM.yyyy HH:mm:ss")
                                + " Etkin Cihaz: " + sn);
                }
            }

            Closing = true;
            return ActionResult.ReturnError($"Hata{Environment.NewLine}{GetErrorDetailString().Message}");
        }

        public ActionResult GetYazarKasaState()
        {
            InposEcrState state;
            InposExtError error = InposExt.EcrState(Timeout, out state);

            if (error == InposExtError.InposNoError)
                return ActionResult.ReturnSuccess("Yazarkasa durumu: " + ((Int32)state).ToString() + " " + "(" + state.ToString() + ")");
            else
                return ActionResult.ReturnError("Yazarkasa durumu sorgulanamadı. "
                          + "Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString());
        }

        public ActionResult Close()
        {
            bool closeActiveDevice = Closing;

            Closing = true;

            if (closeActiveDevice)
                InposExt.Close();

            return ActionResult.ReturnSuccess("Bağlı değil.");
        }

        public ActionResult StartSale()
        {
            receipt.receiptNo = 0;
            receipt.zNo = 0;

            InposExtError error = InposExt.StartSale(Timeout);

            if (error == InposExtError.InposNoError)
                return ActionResult.ReturnSuccess("Satış başladı.");
            else
                return ActionResult.ReturnError("Satış başlatılamadı. "
                          + "Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString());
        }


        public ActionResult GetSaleState()
        {
            InposEcrSaleState saleState;
            InposEcrSaleTotals totals = new InposEcrSaleTotals();

            InposExtError error = InposExt.SaleState(Timeout, out saleState, ref totals, ref receipt);

            if (error == InposExtError.InposNoError)
            {
                // Construct the message to return
                string message = "";
                //message += "\nToplam: " + totals.totalAmount.ToString() + " KDV: " + totals.totalVat.ToString() + " Kalem: " + totals.itemCount;
                //message += "\nTahsilat Tutarı: " + totals.amountToPay.ToString() + " Nakit: " + totals.cashPaymentAmount.ToString() + " Kredi K: " + totals.creditCardPaymentAmount.ToString();
                message += "Fiş No: " + receipt.receiptNo.ToString() + " | " + " Z No: " + receipt.zNo.ToString();


                return ActionResult.ReturnSuccess(message);
            }
            else
            {
                return ActionResult.ReturnError("Satış durumu sorgulanamadı. "
                          + "Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString());
            }
        }


        public ActionResult CancelSale()
        {
            saleItems.count = 0;

            InposExtError error = InposExt.CancelSale(Timeout);

            if (error == InposExtError.InposNoError)
                return ActionResult.ReturnSuccess("Satış iptal edildi.");
            else
                return ActionResult.ReturnError("Satış iptal edilemedi. "
                          + "Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString());
        }

        public ActionResult AddSaleItem(string ItemName, UInt64 UnitPrice, UInt32 Multiplier, Int32 DiscountRate, UInt64 DiscountAmount, byte Section, Unit Unit)
        {
            InposExtError error = InposExtError.InposNoError;
            InposEcrSaleTotals totals = new InposEcrSaleTotals();

            int addedItemCount = 0;

            if (saleItems.count == 0)
            {
                InposEcrSaleItem item;

                item.name = ItemName;
                item.unitPrice = (UInt64)UnitPrice;
                item.multiplier = (UInt32)Multiplier;
                item.discountRate = (Int32)DiscountRate;
                item.discountAmount = (UInt64)DiscountAmount;
                item.section = (byte)Section;
                item.unit = Unit;

                error = InposExt.AddSaleItem(Timeout, ref item, ref totals);

                if (error == InposExtError.InposNoError)
                    addedItemCount = 1;
            }
            else
            {
                error = InposExt.AddMultipleSaleItems(Timeout, ref saleItems, ref totals);

                if (error == InposExtError.InposNoError)
                    addedItemCount = saleItems.count;
            }

            saleItems.count = 0;

            if (error == InposExtError.InposNoError)
            {
                string message = "";
                message += "Satış kalem(ler)i eklendi.";
                message += Environment.NewLine;
                message += "Toplam: " + totals.totalAmount.ToString() + " KDV: " + totals.totalVat.ToString() + " Tahsilat Tutarı: " + totals.amountToPay.ToString() + " Kalem: " + totals.itemCount;
                message += Environment.NewLine;
                message += addedItemCount.ToString() + " kalem eklendi.";

                return ActionResult.ReturnSuccess(message);
            }
            else
            {
                return ActionResult.ReturnError("Satış kalem(ler)i eklenemedi. "
                          + "Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                + GetErrorDetailString());
            }
        }

        public ActionResult EndSale(PaymentType PaymentType, decimal ReceiptNo, decimal ZNo, bool IsExt)
        {
            string message = "";

            InposSaleType type = InposSaleType.SaleWithReceiptType;

            InposExt.SaleType(Timeout, ref type);
            InposExtError error = InposExt.EndSale(PaymentType);

            if (error == InposExtError.InposNoError)
                message += "Satış sonlandırma komutu gönderildi.";
            else
                message += "Satış sonlandırma komutu gönderilemedi. "
                          + "Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString();

            if (error == InposExtError.InposNoError)
            {
                if (type != InposSaleType.SaleWithReceiptType)
                    return ActionResult.ReturnSuccess(message);

                InposSaleReceipt r = new InposSaleReceipt((UInt32)ReceiptNo, (UInt32)ZNo);

                message += "Fiş bekleniyor... ";
                Application.DoEvents();

                if (IsExt)
                {
                    InposEcrSaleTotalsExt totals = new InposEcrSaleTotalsExt();

                    error = InposExt.WaitReceiptData(5 * 60 * 1000, ref receipt, ref totals);

                    if (error == InposExtError.InposNoError)
                    {
                        DumpReceipt(totals);
                    }
                }
                else
                {
                    InposEcrSaleTotals totals = new InposEcrSaleTotals();

                    error = InposExt.WaitReceiptData(5 * 60 * 1000, ref receipt, ref totals);

                    if (error == InposExtError.InposNoError)
                    {
                        DumpReceipt(totals);
                    }
                }


                if (error != InposExtError.InposNoError)
                    return ActionResult.ReturnError("Hata: " + ((Int32)error).ToString());
            }

            return ActionResult.ReturnError(message);
        }

        public ActionResult DeleteLastItem()
        {
            InposEcrSaleTotals totals = new InposEcrSaleTotals();

            InposExtError error = InposExt.DeleteLastSaleItem(Timeout, ref totals);

            if (error == InposExtError.InposNoError)
            {
                string message = "";
                message += "Satış kalemi silindi.";
                message += Environment.NewLine + "Toplam: " + totals.totalAmount.ToString() + " KDV: " + totals.totalVat.ToString() +
                             " Tahsilat Tutarı: " + totals.amountToPay.ToString() + " Kalem: " + totals.itemCount;

                return ActionResult.ReturnSuccess(message);
            }
            else
            {
                return ActionResult.ReturnError("Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString());
            }
        }

        public static ActionResult DumpReceipt(InposEcrSaleTotalsExt totals)
        {
            string message = "";
            message += "Kalem: " + totals.itemCount.ToString() + " Toplam: " + totals.totalAmount.ToString() + " KDV: " + totals.totalVat.ToString() + " Tahs.Tutarı: " + totals.amountToPay.ToString();
            message += Environment.NewLine + "Nakit: " + totals.cashPaymentAmount.ToString();
            message += Environment.NewLine + "Banka[" + totals.creditCardPayment.totalCount.ToString() + "]: " + totals.creditCardPayment.totalAmount;
            if (totals.creditCardPayment.totalCount > 0)
            {
                message += " -> ";

                for (UInt32 i = 0; i < totals.creditCardPayment.totalCount; i++)
                    message += "{" + totals.creditCardPayment.acquires[i].id.ToString() + " - " + totals.creditCardPayment.acquires[i].amount.ToString() + "} ";
            }

            message += Environment.NewLine + "Yemek[" + totals.mealCardPayment.totalCount.ToString() + "]: " + totals.mealCardPayment.totalAmount;
            if (totals.mealCardPayment.totalCount > 0)
            {
                message += " -> ";

                for (UInt32 i = 0; i < totals.mealCardPayment.totalCount; i++)
                    message += "{" + totals.mealCardPayment.acquires[i].id.ToString() + " - " + totals.creditCardPayment.acquires[i].amount.ToString() + "} ";
            }

            return ActionResult.ReturnSuccess(message);
        }

        private static ActionResult DumpReceipt(InposEcrSaleTotals totals)
        {
            string message = "";

            message += "Toplam: " + totals.totalAmount.ToString() + " KDV: " + totals.totalVat.ToString() + " Tahsilat Tutarı: " + totals.amountToPay.ToString();
            message += Environment.NewLine + "Nakit: " + totals.cashPaymentAmount.ToString() + " Kredi K: " + totals.creditCardPaymentAmount + " Kalem: " + totals.itemCount.ToString();

            return ActionResult.ReturnSuccess(message);
        }

        public ActionResult GetReceiptData(decimal ReceiptNo, decimal ZNo, bool IsExt)
        {
            InposSaleReceipt r = new InposSaleReceipt((UInt32)ReceiptNo, (UInt32)ZNo);

            InposExtError error = InposExtError.InposNoError;

            if (IsExt)
            {
                InposEcrSaleTotalsExt totals = new InposEcrSaleTotalsExt();

                error = InposExt.ReceiptData(Timeout, ref r, ref totals);

                if (error == InposExtError.InposNoError)
                {
                    return DumpReceipt(totals);
                }
            }
            else
            {
                InposEcrSaleTotals totals = new InposEcrSaleTotals();

                error = InposExt.ReceiptData(Timeout, ref r, ref totals);

                if (error == InposExtError.InposNoError)
                {
                    return DumpReceipt(totals);
                }
            }

            if (error != InposExtError.InposNoError)
            {
                return ActionResult.ReturnError("Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString());
            }

            return ActionResult.ReturnError("Hata");
        }

        public ActionResult CheckPaper()
        {
            InposExtError error = InposExt.CheckPrinterPaper(Timeout);

            if (error == InposExtError.InposNoError)
                return ActionResult.ReturnSuccess("Yazıcıda kağıt var.");
            else
                return ActionResult.ReturnError("Yazıcı kağıdı kontrolü başarısız. "
                          + "Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                + GetErrorDetailString());
        }

        public ActionResult AddPayment(PaymentType PaymentType, decimal PaymentAmount, Acquirer? acquirer = null)
        {
            try
            {
                if (PaymentAmount <= 0)
                {
                    return ActionResult.ReturnError("Geçersiz ödeme tutarı");
                }

                int errorCount = 0;
                int valueSaleState = 0;
                int valueEcrState = 0;
                InposExtError error;
                InposEcrSaleState saleState;
                InposEcrState ecrState;

                PaymentAmount = PaymentAmount * 100;

                InposSaleReceipt r = new InposSaleReceipt(0, 0);
                InposEcrSaleTotalsExt first = new InposEcrSaleTotalsExt();

                error = InposExt.ReceiptData(Timeout, ref r, ref first);

                if (acquirer.HasValue)
                {
                    error = InposExt.AddPayment(PaymentType, (UInt64)PaymentAmount, acquirer.Value);
                }
                else
                {
                    error = InposExt.AddPayment(PaymentType, (UInt64)PaymentAmount);
                }

                Thread.Sleep(200);
                valueEcrState = InternalCheckEcrStatus(valueEcrState, ref errorCount);
                ecrState = (InposEcrState)valueEcrState;
                valueSaleState = InternalCheckSaleStatus(valueSaleState, ref errorCount);

                while (ecrState == InposEcrState.InposEcrNotUsable)
                {
                    valueEcrState = InternalCheckEcrStatus(valueEcrState, ref errorCount);
                    ecrState = (InposEcrState)valueEcrState;
                    Thread.Sleep(500);
                }
                if (PaymentType == PaymentType.CreditCardPayment)
                {
                    int maxWaitAttempts = 30;
                    int currentAttempt = 0;

                    while (currentAttempt < maxWaitAttempts)
                    {
                        valueSaleState = InternalCheckSaleStatus(valueSaleState, ref errorCount);
                        saleState = (InposEcrSaleState)(valueSaleState);

                        if (saleState == InposEcrSaleState.InposSaleDataFinalized ||
                            saleState == InposEcrSaleState.InposSaleIdle)
                        {
                            break;
                        }

                        if (saleState == InposEcrSaleState.InposSaleWaitingForTransactionCompleted)
                        {
                            Thread.Sleep(1000);
                            currentAttempt++;
                            continue;
                        }

                        return ActionResult.ReturnError($"Kredi kartı ödeme işlemi beklenmeyen bir durumda. Durum: {saleState}");
                    }

                    if (currentAttempt >= maxWaitAttempts)
                    {
                        return ActionResult.ReturnError("Kredi kartı ödeme işlemi zaman aşımına uğradı");
                    }
                }

                return ActionResult.ReturnSuccess("Ödeme bilgisi gönderildi.");
            }
            catch (Exception ex)
            {
                return ActionResult.ReturnError($"Ödeme işleminde beklenmedik hata: {ex.Message}");
            }
        }

        private int InternalCheckEcrStatus(int valueEcrState, ref int errorCount)
        {
            bool isConnected = false;
            InposEcrState ecrState;
            InposExtError errorEcr;

            errorEcr = InposExt.EcrState(Timeout, out ecrState);
            while (errorEcr == InposExtError.InposConnectionError && errorCount < 10)
            {
                isConnected = ReconnectDevice(isConnected, ref errorCount);
            }

            if (ecrState == InposEcrState.InposEcrInitialization)
            {
                isConnected = ReconnectDevice(isConnected, ref errorCount);
                if (isConnected)
                {
                    errorEcr = InposExt.EcrState(Timeout, out ecrState);
                }
            }

            valueEcrState = (int)ecrState;
            return valueEcrState;
        }

        private int InternalCheckSaleStatus(int valueSaleState, ref int errorCount)
        {
            InposEcrSaleState saleState;
            bool isConnected = false;
            InposExtError errorSale;
            InposEcrSaleTotals totalsDummy = new InposEcrSaleTotals();

            errorSale = InposExt.SaleState(Timeout, out saleState, ref totalsDummy, ref receipt);

            while (errorSale == InposExtError.InposConnectionError && errorCount < 10)
            {
                isConnected = ReconnectDevice(isConnected, ref errorCount);
                if (isConnected)
                {
                    break;
                }
            }

            valueSaleState = (int)saleState;
            return valueSaleState;
        }

        private bool ReconnectDevice(bool isConnected, ref int errorCount)
        {
            Closing = false;

            while (!Closing)
            {
                InposExtError error = InposExt.Initialize(1, SerialNo.ToUpper(), ListenIp.Replace(" ", ""), (UInt16)decimal.Parse(ListenPort), 10000);

                if (error != InposExtError.InposNoError)
                {
                    if (error == InposExtError.InposConnectionError)
                    {
                        errorCount++;
                        Closing = true;
                        return isConnected;
                    }
                }
                else
                {
                    isConnected = true;
                    break;
                }
            }

            return isConnected;
        }


        public ActionResult GetSectionData(decimal Section)
        {
            InposEcrSaleItem item = new InposEcrSaleItem();
            item.section = (byte)Section;

            InposExtError error = InposExt.SectionData(Timeout, ref item);

            if (error == InposExtError.InposNoError)
                return ActionResult.ReturnSuccess(item.multiplier.ToString());
            else
                return ActionResult.ReturnError("Kısım bilgisi alınamadı. "
                          + "Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString());
        }

        public ActionResult GetSectionName(decimal Section)
        {
            InposEcrSaleItem item = new InposEcrSaleItem();
            item.section = (byte)Section;

            InposExtError error = InposExt.SectionData(Timeout, ref item);

            if (error == InposExtError.InposNoError)
                return ActionResult.ReturnSuccess(item.name);
            else
                return ActionResult.ReturnError("Kısım bilgisi alınamadı. "
                          + "Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString());
        }

        public static ActionResult XReport()
        {
            InposExtError error = InposExt.XReport();

            if (error == InposExtError.InposNoError)
                return ActionResult.ReturnSuccess("X raporu komutu gönderildi.");
            else
                return ActionResult.ReturnError("X raporu komutu gönderme başarısız. "
                          + "Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString());
        }

        public static ActionResult ZReport()
        {
            InposExtError error = InposExt.ZReport();

            if (error == InposExtError.InposNoError)
                return ActionResult.ReturnSuccess("Z raporu komutu gönderildi.");
            else
                return ActionResult.ReturnError("Z raporu komutu gönderme başarısız. "
                          + "Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString());
        }

        public ActionResult GetCurrentZ()
        {
            UInt32 zNo = 0;
            InposExtError error = InposExt.CurrentZ(Timeout, out zNo);

            if (error == InposExtError.InposNoError)
                return ActionResult.ReturnSuccess(zNo.ToString());
            else
                return ActionResult.ReturnError("Hata:" + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString());
        }

        public ActionResult Login()
        {
            InposExtError error = InposExt.Login(Timeout);

            if (error == InposExtError.InposNoError)
                return ActionResult.ReturnSuccess("Kasiyer hesabına girildi.");
            else
                return ActionResult.ReturnError("Kasiyer hesabına girilemedi. "
                          + "Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString());
        }

        public ActionResult Logout()
        {
            InposExtError error = InposExt.Logout(Timeout);

            if (error == InposExtError.InposNoError)
                return ActionResult.ReturnSuccess("Kasiyer hesabına çıkıldı.");
            else
                return ActionResult.ReturnError("Kasiyer hesabına çıkılamadı. "
                          + "Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString());
        }

        public ActionResult DeleteItem(decimal PaymentAmount)
        {
            InposEcrSaleTotals totals = new InposEcrSaleTotals();

            InposExtError error = InposExt.DeleteSaleItem(Timeout, (UInt32)PaymentAmount, ref totals);

            if (error == InposExtError.InposNoError)
            {
                string message = "";
                message += "Satış kalemi silindi.";
                message += Environment.NewLine + "Toplam: " + totals.totalAmount.ToString() + " KDV: " + totals.totalVat.ToString() +
                             " Tahsilat Tutarı: " + totals.amountToPay.ToString() + " Kalem: " + totals.itemCount;

                return ActionResult.ReturnSuccess(message);
            }
            else
            {
                return ActionResult.ReturnError("Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString());
            }
        }

        public ActionResult SetSaleType(InposSaleType type)
        {
            InposExtError error = InposExt.SetSaleType(Timeout, type);

            if (error == InposExtError.InposNoError)
                return ActionResult.ReturnSuccess("Satış Tipi:" + type.ToString());
            else
                return ActionResult.ReturnError("Satış tipi değiştirilemedi. "
                          + "Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString());
        }

        public static ActionResult BlockKeys()
        {
            InposExtError error = InposExt.BlockEcrKeys();
            if (error == InposExtError.InposNoError)
                return ActionResult.ReturnSuccess("");
            else
                return ActionResult.ReturnError("Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString());
        }

        public static ActionResult UnblockKeys()
        {
            InposExtError error = InposExt.UnblockEcrKeys();

            if (error == InposExtError.InposNoError)
                return ActionResult.ReturnSuccess("");
            else
                return ActionResult.ReturnError("Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString());
        }

        public ActionResult GetKeyBlockStatus()
        {
            Int32 status = 0;

            InposExtError error = InposExt.EcrKeyBlockingStatus(Timeout, ref status);

            if (error == InposExtError.InposNoError)
                return ActionResult.ReturnSuccess((status == 0 ? "Kilit Yok" : "Kilitli"));
            else
                return ActionResult.ReturnError("Hata: " + ((Int32)error).ToString() + " " + "(" + error.ToString() + ")" + "\n"
                          + GetErrorDetailString());
        }

        internal ActionResult DumpReceipt()
        {
            throw new NotImplementedException();
        }

        internal static uint GetTimeout()
        {
            throw new NotImplementedException();
        }

        internal static void DumpReceiptExt(InposEcrSaleTotalsExt totals)
        {
            throw new NotImplementedException();
        }

        internal static void DumpReceiptStandard(InposEcrSaleTotals totals)
        {
            throw new NotImplementedException();
        }

        internal ActionResult EndSale(string paymentType, object value1, object value2, bool v)
        {
            throw new NotImplementedException();
        }
    }
}
