using ArtiConnect.Properties;
using DevExpress.Data.Svg;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    internal class SmartDllClient
    {
        private byte[] _dllVersion = new byte[24];

        private ST_TICKET _cticket;

        private Dictionary<uint, byte[]> TransactionUniqueIdList = new Dictionary<uint, byte[]>();

        private byte _isBackground;

        private static SmartDllClient _instance;

        public string DllVersionFriendlyName { get; set; }

        public List<EcrInterface> EcrInterfaces { get; set; }

        public EcrInterface CurrentInterface { get; set; }

        private ulong _currentInterfaceHandleValue
        {
            get
            {
                if (CurrentInterface != null && CurrentInterface.ActiveTransactionHandle != null)
                {
                    return CurrentInterface.ActiveTransactionHandle.Index;
                }
                return 0uL;
            }
        }

        public static SmartDllClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SmartDllClient(selfControlledInstantiation: true);
                }
                return _instance;
            }
        }

        public SmartDllClient()
        {
            throw new Exception(Resources.SingletonErrorText);
        }

        private SmartDllClient(bool selfControlledInstantiation)
        {
        }

        public string GetDllVersion()
        {
            string result = "";
            Array.Clear(_dllVersion, 0, _dllVersion.Length);
            uint num = GMPSmartDLL.GMP_GetDllVersion(_dllVersion);
            DllVersionFriendlyName = GMP_Tools.SetEncoding(_dllVersion);
            if (num != 0 || string.Compare(DllVersionFriendlyName, "1602030800") < 0)
            {
                return string.Format(Resources.UncompatibleDllText, "1602030800", DllVersionFriendlyName);
            }
            return result;
        }

        public List<EcrInterface> RetrieveAllEcrInterfaces()
        {
            if (EcrInterfaces != null)
            {
                EcrInterfaces.Clear();
                EcrInterfaces = null;
            }
            EcrInterfaces = new List<EcrInterface>();
            uint[] array = new uint[20];
            byte[] array2 = new byte[64];
            uint num = GMPSmartDLL.FP3_GetInterfaceHandleList(array, (uint)array.Length);
            for (uint num2 = 0u; num2 < num; num2++)
            {
                GMPSmartDLL.FP3_GetInterfaceID(array[num2], array2, (uint)array2.Length);
                string friendlyName = array[num2].ToString("X8") + "-" + GMP_Tools.SetEncoding(array2);
                EcrInterface ecrInterface = new EcrInterface();
                ecrInterface.FriendlyName = friendlyName;
                ecrInterface.Index = array[num2];
                ecrInterface.Id = array2;
                EcrInterfaces.Add(ecrInterface);
            }
            return EcrInterfaces;
        }

        public void Echo()
        {
            ST_ECHO pStEcho = new ST_ECHO();
            uint num = Json_GMPSmartDLL.FP3_Echo(CurrentInterface.Index, ref pStEcho, 10000);
            if (num != 0)
            {
                throw new SmartDllClientException(num);
            }
        }

        public List<string> StartPairingAndGetPairResults()
        {
            ST_GMP_PAIR pStPair = new ST_GMP_PAIR();
            pStPair.szExternalDeviceBrand = "INGENICO";
            pStPair.szExternalDeviceModel = "IWE280";
            pStPair.szExternalDeviceSerialNumber = "12344567";
            pStPair.szEcrSerialNumber = "JHWE20000079";
            pStPair.szProcOrderNumber = "000001";
            ST_GMP_PAIR_RESP pStPairResp = new ST_GMP_PAIR_RESP();
            uint num = Json_GMPSmartDLL.FP3_StartPairingInit(CurrentInterface.Index, ref pStPair, ref pStPairResp);
            if (num != 0)
            {
                throw new SmartDllClientException(num);
            }
            return new List<string>
        {
            pStPairResp.szEcrSerialNumber,
            "Ret Code 2 " + pStPairResp.ErrorCode.ToString("X2"),
            "Device Brand 16 " + pStPairResp.szEcrBrand,
            "Device Model 16 " + pStPairResp.szEcrModel,
            "Device Serial Num 16 " + pStPairResp.szEcrSerialNumber,
            "Version Num 8" + pStPairResp.szVersionNumber + " Version No"
        };
        }

        public List<uint> DoTlvDataOperations()
        {
            byte[] array = new byte[100];
            List<uint> list = new List<uint>();
            short pDataLen = 0;
            uint num = GMPSmartDLL.FP3_GetTlvData(CurrentInterface.Index, 14675206, array, (short)array.Length, ref pDataLen);
            string text = "0";
            for (int i = 0; i < pDataLen; i++)
            {
                text += array[i].ToString("X2");
            }
            Convert.ToInt32(text);
            if (num != 0)
            {
                list.Add(num);
            }
            num = 0u;
            byte[] array2 = new byte[1];
            short pDataLen2 = 0;
            num = GMPSmartDLL.FP3_GetTlvData(CurrentInterface.Index, 14675561, array2, (short)array2.Length, ref pDataLen2);
            if (num != 0)
            {
                list.Add(num);
            }
            byte[] array3 = new byte[100];
            short pDataLen3 = 0;
            num = GMPSmartDLL.FP3_GetTlvData(CurrentInterface.Index, 14675284, array3, (short)array3.Length, ref pDataLen3);
            if (num != 0)
            {
                list.Add(num);
            }
            byte[] array4 = new byte[100];
            array4[0] = 0;
            array4[1] = 0;
            array4[2] = 16;
            array4[3] = 0;
            array4[4] = 16;
            array4[5] = 0;
            short num2 = 6;
            num = GMPSmartDLL.FP3_SetTlvData(CurrentInterface.Index, 14675284u, array4, (ushort)num2);
            if (num != 0)
            {
                list.Add(num);
            }
            byte[] array5 = new byte[100];
            short pDataLen4 = 0;
            num = GMPSmartDLL.FP3_GetTlvData(CurrentInterface.Index, 14675284, array5, (short)array5.Length, ref pDataLen4);
            if (num != 0)
            {
                list.Add(num);
            }
            return list;
        }

        public ST_TAX_RATE[] SaveDepartmentsAndTaxes()
        {
            uint num = 0u;
            int pNumberOfTotalRecordsReceived = 0;
            ST_TAX_RATE[] pStTaxRate = new ST_TAX_RATE[8];
            ST_DEPARTMENT[] pStDepartments = new ST_DEPARTMENT[12];
            int pNumberOfTotalDepartments = 0;
            int pNumberOfTotalRecords = 0;
            for (int i = 0; i < pStDepartments.Length; i++)
            {
                pStDepartments[i] = new ST_DEPARTMENT();
            }
            num = Json_GMPSmartDLL.FP3_GetTaxRates(CurrentInterface.Index, ref pNumberOfTotalRecords, ref pNumberOfTotalRecordsReceived, ref pStTaxRate, 8);
            if (!CurrentInterface.TransactionTaxRateList.ContainsKey(CurrentInterface.Index))
            {
                CurrentInterface.TransactionTaxRateList.Add(CurrentInterface.Index, pStTaxRate);
            }
            if (num != 0)
            {
                throw new SmartDllClientException(num);
            }
            num = Json_GMPSmartDLL.FP3_GetDepartments(CurrentInterface.Index, ref pNumberOfTotalDepartments, ref pNumberOfTotalRecordsReceived, ref pStDepartments, 12);
            if (!CurrentInterface.TransactionDepartmentList.ContainsKey(CurrentInterface.Index))
            {
                CurrentInterface.TransactionDepartmentList.Add(CurrentInterface.Index, pStDepartments);
            }
            if (num != 0)
            {
                throw new SmartDllClientException(num);
            }
            for (int j = 0; j < pNumberOfTotalRecordsReceived; j++)
            {
                _ = 7;
            }
            if (num != 0)
            {
                throw new SmartDllClientException(num);
            }
            return pStTaxRate;
        }

        public uint InitAndRetrieveTransactionHandles()
        {
            byte[] array = new byte[24];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = byte.MaxValue;
            }
            return RestartAndRetrieveTrxHandles(array);
        }

        public uint RestartAndRetrieveTrxHandles(byte[] uniqueId, byte[] uniqueIdSign = null, byte[] userData = null, int timeout = 10000)
        {
            ulong hTrx = 0uL;
            int lengthOfUniqueIdSign = 0;
            int lengthOfUserData = 0;
            if (uniqueIdSign != null)
            {
                lengthOfUniqueIdSign = uniqueIdSign.Length;
            }
            if (userData != null)
            {
                lengthOfUserData = userData.Length;
            }
            uint result = GMPSmartDLL.FP3_Start(CurrentInterface.Index, ref hTrx, _isBackground, uniqueId, uniqueId.Length, uniqueIdSign, lengthOfUniqueIdSign, userData, lengthOfUserData, timeout);
            AddNewTransactionHandleToCurrentInterface(hTrx);
            return result;
        }

        public void AddNewTransactionHandleToCurrentInterface(ulong tranHandle)
        {
            TransactionHandle transactionHandle = new TransactionHandle();
            transactionHandle.Index = tranHandle;
            transactionHandle.FriendlyName = tranHandle.ToString("X2") + " FG";
            if (CurrentInterface.TransactionHandleList == null)
            {
                CurrentInterface.TransactionHandleList = new List<TransactionHandle>();
            }
            CurrentInterface.AddNewOrActivateTransactionHandle(transactionHandle);
        }

        public uint CloseTransactionHandles()
        {
            uint result = GMPSmartDLL.FP3_Close(CurrentInterface.Index, _currentInterfaceHandleValue, 10000);
            DeleteActiveTransactionHandle();
            return result;
        }

        public void ResetTransactionHandleListOfCurrentInterface()
        {
            CurrentInterface.TransactionHandleList.Clear();
        }

        public void DeleteActiveTransactionHandle()
        {
            TransactionHandle transactionHandle = CurrentInterface.TransactionHandleList.Find((TransactionHandle item) => item.Active);
            if (transactionHandle != null)
            {
                CurrentInterface.TransactionHandleList.Remove(transactionHandle);
            }
        }

        public void ResetAllInterfaces()
        {
            CurrentInterface.TransactionHandleList.Clear();
            EcrInterfaces.Clear();
            CurrentInterface = null;
        }

        public byte[] GetUniqueIdByInterface()
        {
            byte[] array = new byte[24];
            if (TransactionUniqueIdList.ContainsKey(CurrentInterface.Index))
            {
                Array.Copy(TransactionUniqueIdList[CurrentInterface.Index], array, 24);
            }
            return array;
        }

        public void ClearTransactionUniqueIdOnCurrentInterface()
        {
            if (TransactionUniqueIdList.ContainsKey(CurrentInterface.Index))
            {
                Array.Clear(TransactionUniqueIdList[CurrentInterface.Index], 0, 24);
            }
        }

        public string GetInterfaceInfo()
        {
            ST_INTERFACE_XML_DATA stXmlData = new ST_INTERFACE_XML_DATA();
            Json_GMPSmartDLL.FP3_GetInterfaceXmlDataByHandle(CurrentInterface.Index, ref stXmlData);
            byte[] array = new byte[64];
            string text = "Handle : " + CurrentInterface.Index.ToString("X8") + Environment.NewLine;
            GMPSmartDLL.FP3_GetInterfaceID(CurrentInterface.Index, array, (uint)array.Length);
            text = text + "ID : " + GMP_Tools.GetStringFromBytes(array) + Environment.NewLine;
            if (stXmlData.IsTcpConnection == 0)
            {
                text = text + "ConnectionState Type : Port" + Environment.NewLine;
                text = text + "Port Name :" + stXmlData.PortName + Environment.NewLine;
                text = text + "Baudrate : " + stXmlData.BaudRate + Environment.NewLine;
                text = text + "ByteSize : " + stXmlData.ByteSize + Environment.NewLine;
                text = text + "fParity : " + stXmlData.fParity + Environment.NewLine;
                text = text + "Parity : " + stXmlData.Parity + Environment.NewLine;
                text = text + "StopBit : " + stXmlData.StopBit + Environment.NewLine;
                text = text + "RetryCounter : " + stXmlData.RetryCounter + Environment.NewLine;
            }
            else
            {
                text = text + "ConnectionState Type : IP" + Environment.NewLine;
                text = text + "IP :" + stXmlData.IP + stXmlData.Port + Environment.NewLine;
                text = text + "IpRetryCount : " + stXmlData.IpRetryCount + Environment.NewLine;
            }
            text = text + "AckTimeOut : " + stXmlData.AckTimeOut + Environment.NewLine;
            text = text + "CommTimeOut : " + stXmlData.CommTimeOut + Environment.NewLine;
            return text + "InterCharacterTimeOut : " + stXmlData.InterCharacterTimeOut + Environment.NewLine;
        }

        public List<string> ShowTransactionDetails(ST_TICKET pstTicket, bool itemDetail)
        {
            if (pstTicket == null)
            {
                return null;
            }
            List<string> list = new List<string>();
            string text = "";
            for (int i = 0; i < 24; i++)
            {
                text += pstTicket.uniqueId[i].ToString("X2");
            }
            list.Add(string.Format("UNIQUE ID        : " + text));
            list.Add(string.Format("TICKET TYPE      : " + pstTicket.ticketType));
            list.Add(string.Format("Z NO             : " + pstTicket.ZNo));
            list.Add(string.Format("F NO             : " + pstTicket.FNo));
            list.Add(string.Format("EJNO             : " + pstTicket.EJNo));
            list.Add(string.Format("TRANSACTION FLAG : " + pstTicket.TransactionFlags.ToString().PadLeft(8, '0')));
            if ((pstTicket.TransactionFlags & 2u) != 0)
            {
                list.Add($"                : FLG_XTRANS_GMP3");
            }
            if ((pstTicket.TransactionFlags & 0x20000u) != 0)
            {
                list.Add($"                : FLG_XTRANS_TICKET_HEADER_PRINTED");
            }
            if ((pstTicket.TransactionFlags & 0x40000u) != 0)
            {
                list.Add($"                : FLG_XTRANS_TICKET_TOTALS_AND_PAYMENTS_PRINTED");
            }
            if ((pstTicket.TransactionFlags & 0x80000u) != 0)
            {
                list.Add($"                : FLG_XTRANS_TICKET_FOOTER_BEFORE_MF_PRINTED");
            }
            if ((pstTicket.TransactionFlags & 0x100000u) != 0)
            {
                list.Add($"                : FLG_XTRANS_TICKET_FOOTER_MF_PRINTED");
            }
            if ((pstTicket.TransactionFlags & 0x100u) != 0)
            {
                list.Add($"                : FLG_XTRANS_TAXFREE_PARAMETERS_SET");
            }
            if ((pstTicket.TransactionFlags & 0x8000u) != 0)
            {
                list.Add($"                : FLG_XTRANS_INVOICE_PARAMETERS_SET");
            }
            if ((pstTicket.TransactionFlags & 0x1000u) != 0)
            {
                list.Add($"                : FLG_XTRANS_FULL_RCPT_CANCEL");
            }
            if ((pstTicket.TransactionFlags & 0x2000u) != 0)
            {
                list.Add($"                : FLG_XTRANS_NONEY_COLLECTION_EXISTS");
            }
            if ((pstTicket.TransactionFlags & 0x4000u) != 0)
            {
                list.Add($"                : FLG_XTRANS_TAXLESS_ITEM_EXISTS");
            }
            if ((pstTicket.TransactionFlags & 0x200u) != 0)
            {
                list.Add($"                : FLG_XTRANS_TICKETTING_EXISTS");
            }
            list.Add(string.Format("OPTION FLAG      : " + pstTicket.OptionFlags.ToString().PadLeft(8, '0')));
            if (pstTicket.KatkiPayiAmount != 0)
            {
                list.Add($"MATRAHSZ        : {pstTicket.KatkiPayiAmount / 100u}.{pstTicket.KatkiPayiAmount % 100u}");
            }
            if (pstTicket.TotalReceiptDiscount != 0)
            {
                list.Add($"TOTAL DISCOUNT  : {pstTicket.TotalReceiptDiscount / 100u}.{pstTicket.TotalReceiptDiscount % 100u}");
            }
            if (pstTicket.TotalReceiptIncrement != 0)
            {
                list.Add($"TOTAL INCREAE   : {pstTicket.TotalReceiptIncrement / 100u}.{pstTicket.TotalReceiptIncrement % 100u}");
            }
            if (pstTicket.TotalReceiptItemCancel != 0)
            {
                list.Add($"TOTAL VOID      : {pstTicket.TotalReceiptItemCancel / 100u}.{pstTicket.TotalReceiptItemCancel % 100u}");
            }
            if (pstTicket.KasaAvansAmount != 0)
            {
                list.Add($"KASA AVANS      : {pstTicket.KasaAvansAmount / 100u}.{pstTicket.KasaAvansAmount % 100u}");
            }
            if (pstTicket.KasaPaymentAmount != 0)
            {
                list.Add($"KASA PAYMENT    : {pstTicket.KasaPaymentAmount / 100u}.{pstTicket.KasaPaymentAmount % 100u}");
            }
            if (pstTicket.CashBackAmount != 0)
            {
                list.Add($"CASHBACK        : {pstTicket.CashBackAmount / 100u}.{pstTicket.CashBackAmount % 100u}");
            }
            if (pstTicket.invoiceAmount != 0)
            {
                list.Add($"INVOICE         : {pstTicket.invoiceAmount / 100u}.{pstTicket.invoiceAmount % 100u}");
            }
            if (pstTicket.TaxFreeCalculated != 0)
            {
                list.Add($"TAXFREE CALCULA : {pstTicket.TaxFreeCalculated / 100u}.{pstTicket.TaxFreeCalculated % 100u}");
            }
            if (pstTicket.TaxFreeRefund != 0)
            {
                list.Add($"TAXFREE REFUND  : {pstTicket.TaxFreeRefund / 100u}.{pstTicket.TaxFreeRefund % 100u}");
            }
            if (pstTicket.TotalReceiptReversedPayment != 0)
            {
                list.Add($"REVERSE PAYMENTS: {FormatAmount(pstTicket.TotalReceiptReversedPayment, ECurrency.CURRENCY_NONE)} ");
            }
            if (pstTicket.TotalReceiptIncrement != 0)
            {
                list.Add($"INSTALLMENT COUNT   : {pstTicket.TotalReceiptIncrement / 100u}.{pstTicket.TotalReceiptIncrement % 100u}");
            }
            if (pstTicket.TotalReceiptPayment != 0)
            {
                list.Add($"PAYMENTS        : {FormatAmount(pstTicket.TotalReceiptPayment, ECurrency.CURRENCY_NONE)} ");
            }
            for (int j = 0; j < pstTicket.stPayment.Length; j++)
            {
                if (pstTicket.stPayment[j] == null)
                {
                    continue;
                }
                for (int k = 0; k < pstTicket.stPayment[j].stBankPayment.numberOfsubPayment; k++)
                {
                    if (pstTicket.stPayment[j].stBankPayment.stBankSubPaymentInfo[k].amount != 0)
                    {
                        list.Add($"BONUS NAME      : {pstTicket.stPayment[j].stBankPayment.stBankSubPaymentInfo[k].name} ");
                        list.Add($"BONUS TYPE      : {pstTicket.stPayment[j].stBankPayment.stBankSubPaymentInfo[k].type} ");
                        list.Add($"BONUS AMOUNT    : {pstTicket.stPayment[j].stBankPayment.stBankSubPaymentInfo[k].amount} ");
                    }
                }
                if (pstTicket.stPayment[j].stBankPayment.numberOfInstallments != 0)
                {
                    list.Add($"INSTALLMENT COUNT      : {pstTicket.stPayment[j].stBankPayment.numberOfInstallments} ");
                }
            }
            return list;
        }

        private string FormatAmount(uint amount, ECurrency currency)
        {
            string text = $"{amount / 100u}.{amount % 100u:00}";
            switch (currency)
            {
                case ECurrency.CURRENCY_DOLAR:
                    text += " $";
                    break;
                case ECurrency.CURRENCY_EU:
                    text += " €";
                    break;
                case ECurrency.CURRENCY_TL:
                    text += " TL";
                    break;
                default:
                    text += " ?";
                    break;
                case ECurrency.CURRENCY_NONE:
                    break;
            }
            return text;
        }

        public uint GetTicket()
        {
            return Json_GMPSmartDLL.FP3_GetTicket(CurrentInterface.Index, _currentInterfaceHandleValue, ref _cticket, 10000);
        }

        public ST_TICKET ReloadTransaction(int maxTryCount = 5)
        {
            ulong pFlagsActive = 0uL;
            uint num = 0u;
            ST_TICKET pstTicket = new ST_TICKET();
            for (int i = 0; i < maxTryCount; i++)
            {
                num = GMPSmartDLL.FP3_OptionFlags(CurrentInterface.Index, _currentInterfaceHandleValue, ref pFlagsActive, 7uL, 0uL, 10000);
                if (num != 61468)
                {
                    num = Json_GMPSmartDLL.FP3_GetTicket(CurrentInterface.Index, _currentInterfaceHandleValue, ref pstTicket, 10000);
                    if (num != 61468 && num == 0)
                    {
                        break;
                    }
                }
            }
            if (num != 0)
            {
                throw new SmartDllClientException(num);
            }
            return pstTicket;
        }

        public List<string> GetUniqueIdList()
        {
            List<string> list = new List<string>();
            ushort pTotalNumberOfItems = 0;
            ushort pNumberOfItemsInThis = 0;
            ST_UNIQUE_ID[] pStUniqueIdList = new ST_UNIQUE_ID[256];
            for (int i = 0; i < pStUniqueIdList.Length; i++)
            {
                pStUniqueIdList[i] = new ST_UNIQUE_ID();
            }
            ushort num = 0;
            do
            {
                uint num2 = Json_GMPSmartDLL.FP3_FunctionGetUniqueIdList(CurrentInterface.Index, ref pStUniqueIdList, (ushort)(256 - num), num, ref pTotalNumberOfItems, ref pNumberOfItemsInThis, 50000);
                if (num2 != 0)
                {
                    throw new SmartDllClientException(num2);
                }
                for (int j = 0; j < pNumberOfItemsInThis; j++)
                {
                    string text = "";
                    for (int k = 0; k < 24; k++)
                    {
                        text += pStUniqueIdList[j].uniqueId[k].ToString("X2");
                    }
                    list.Add(text);
                }
                num = (ushort)(num + pNumberOfItemsInThis);
            }
            while (num < pTotalNumberOfItems);
            return list;
        }

        public void Ping()
        {
            uint num = GMPSmartDLL.FP3_Ping(CurrentInterface.Index, 1100);
            if (num != 0)
            {
                throw new SmartDllClientException(num);
            }
        }

        public uint ItemSale(ref ST_ITEM stItem, ref ST_TICKET m_stTicket)
        {
            return Json_GMPSmartDLL.FP3_ItemSale(CurrentInterface.Index, _currentInterfaceHandleValue, ref stItem, ref m_stTicket, 10000);
        }

        public int PrepareItemSale(byte[] Buffer, int MaxSize, ref ST_ITEM pStItem)
        {
            return Json_GMPSmartDLL.prepare_ItemSale(Buffer, MaxSize, ref pStItem);
        }

        public int PrepareSetInvoice(byte[] Buffer, int MaxSize, ref ST_INVIOCE_INFO pStInvoiceInfo)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStInvoiceInfo));
            byte[] array = new byte[200000];
            int result = Json_GMPSmartDLL.Json_prepare_SetInvoice(Buffer, MaxSize, bytesFromString, array, array.Length);
            string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
            pStInvoiceInfo = JsonConvert.DeserializeObject<ST_INVIOCE_INFO>(stringFromBytes);
            return result;
        }

        public uint ProcessBatchCommands(uint hInt, ref ulong hTrx, ref ST_MULTIPLE_RETURN_CODE[] pReturnCodes, ref ushort IndexOfReturnCodes, byte[] SendBuffer, ushort SendBufferLen, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            return Json_GMPSmartDLL.FP3_MultipleCommand(hInt, ref hTrx, ref pReturnCodes, ref IndexOfReturnCodes, SendBuffer, SendBufferLen, ref pstTicket, TimeoutInMiliseconds);
        }

        public uint PressTicketHeader()
        {
            return GMPSmartDLL.FP3_TicketHeader(CurrentInterface.Index, _currentInterfaceHandleValue, TTicketType.TProcessSale, 10000);
        }

        public uint PressTicketHeaderInvoice()
        {
            return GMPSmartDLL.FP3_TicketHeader(CurrentInterface.Index, _currentInterfaceHandleValue, TTicketType.TInvoice, 10000);
        }

        internal uint ForwardSaleTicket(bool isInvoice = false, string c_tax_id = "")
        {
            byte[] userData = new byte[8] { 116, 101, 115, 116, 100, 97, 116, 97 };
            byte[] uniqueIdByInterface = GetUniqueIdByInterface();
            uint num = RestartAndRetrieveTrxHandles(uniqueIdByInterface, null, userData);
            TTicketType ticketType = TTicketType.TProcessSale;
            switch (num)
            {
                case 2080u:
                    return num;
                case 0u:
                    if (isInvoice)
                    {
                        ticketType = TTicketType.TInvoice;
                        ST_INVIOCE_INFO pStInvoiceInfo = new ST_INVIOCE_INFO();
                        pStInvoiceInfo.source = 0;
                        pStInvoiceInfo.amount = 0uL;
                        pStInvoiceInfo.currency = 949;
                        string str = "111111111111";
                        if (c_tax_id != "" && c_tax_id != null)
                        {
                            str = c_tax_id;
                        }
                        pStInvoiceInfo.no = new byte[25];
                        ConvertAscToBcdArray(" ", ref pStInvoiceInfo.no, pStInvoiceInfo.no.Length);
                        pStInvoiceInfo.tck_no = new byte[12];
                        ConvertAscToBcdArray(str, ref pStInvoiceInfo.tck_no, pStInvoiceInfo.tck_no.Length);
                        pStInvoiceInfo.vk_no = new byte[12];
                        ConvertAscToBcdArray(str, ref pStInvoiceInfo.vk_no, pStInvoiceInfo.vk_no.Length);
                        pStInvoiceInfo.date = new byte[3];
                        string s = DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Year.ToString().Substring(2, 2).PadLeft(2, '0');
                        ConvertStringToHexArray(s, ref pStInvoiceInfo.date, 3);
                        Array.Reverse(pStInvoiceInfo.date);
                        ST_TICKET pstTicket = new ST_TICKET();
                        num = Json_GMPSmartDLL.FP3_SetInvoice(CurrentInterface.Index, _currentInterfaceHandleValue, ref pStInvoiceInfo, ref pstTicket, 10000);
                    }
                    break;
            }
            if (num == 0)
            {
                num = GMPSmartDLL.FP3_TicketHeader(CurrentInterface.Index, _currentInterfaceHandleValue, ticketType, 10000);
            }
            if (num == 0)
            {
                ulong pFlagsActive = 0uL;
                num = GMPSmartDLL.FP3_OptionFlags(CurrentInterface.Index, _currentInterfaceHandleValue, ref pFlagsActive, 7uL, 0uL, 10000);
            }
            return num;
        }

        public uint RetriveTicket(ref ST_TICKET m_stTicket, decimal receiptAmount, decimal totalReceiptAmountPaid)
        {
            while (Json_GMPSmartDLL.FP3_GetTicket(CurrentInterface.Index, _currentInterfaceHandleValue, ref m_stTicket, 10000) != 0)
            {
            }
            if ((decimal)m_stTicket.TotalReceiptAmount == receiptAmount * 100m && (decimal)m_stTicket.TotalReceiptPayment == totalReceiptAmountPaid * 100m)
            {
                return 0u;
            }
            return 61443u;
        }

        public uint Payment(ref ST_PAYMENT_REQUEST paymentRequest, ref ST_TICKET stTicket, uint tryCount = 0u)
        {
            uint num = 0u;
            num = Json_GMPSmartDLL.FP3_Payment(CurrentInterface.Index, _currentInterfaceHandleValue, ref paymentRequest, ref stTicket, 120000);
            _cticket = stTicket;
            if (num == 61443)
            {
                try
                {
                    ST_TICKET sT_TICKET = ReloadTransaction();
                    if (sT_TICKET == null || sT_TICKET.stPayment == null || sT_TICKET.stPayment.Length == 0)
                    {
                        return num;
                    }
                    ST_PAYMENT sT_PAYMENT = sT_TICKET.stPayment.TakeWhile((ST_PAYMENT item) => item != null).LastOrDefault();
                    if (sT_PAYMENT == null || sT_PAYMENT.payAmount == 0)
                    {
                        return num;
                    }
                    if (sT_TICKET == null)
                    {
                        return num;
                    }
                    if (sT_PAYMENT.payAmount == paymentRequest.payAmount)
                    {
                        Json_GMPSmartDLL.MergeItemStruct(stTicket, sT_TICKET);
                        return 0u;
                    }
                    return num;
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(SmartDllClientException))
                    {
                        return (ex as SmartDllClientException).DllErrorCode;
                    }
                    return 10000u;
                }
            }
            return num;
        }

        public void ConvertAscToBcdArray(string str, ref byte[] arr, int arrLen)
        {
            arrLen = str.Length;
            Array.Copy(Encoding.Default.GetBytes(str), 0, arr, 0, str.Length);
        }

        public void ConvertStringToHexArray(string s, ref byte[] Out_byteArr, int byteArrLen)
        {
            byte[] array = new byte[s.Length / 2];
            for (int i = 0; i < array.Length; i++)
            {
                string value = s.Substring(i * 2, 2);
                array[i] = Convert.ToByte(value, 16);
            }
            byteArrLen = array.Length;
            Array.Copy(array, 0, Out_byteArr, 0, array.Length);
        }
    }

}
