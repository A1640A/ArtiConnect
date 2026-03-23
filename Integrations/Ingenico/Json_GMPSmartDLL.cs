using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    internal class Json_GMPSmartDLL
    {
        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_CreateInterface(ref uint phInt, byte[] szID, byte IsDefault, byte[] szJsonXmlData);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_GetInterfaceXmlDataByID(byte[] szID, byte[] szInterfaceXmlData, int JsonMaxLen);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_GetInterfaceXmlDataByHandle(uint hInt, byte[] szInterfaceXmlData, int JsonMaxLen);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_UpdateInterfaceXmlDataByID(byte[] szID, byte[] szInterfaceXmlData);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_UpdateInterfaceXmlDataByHandle(uint hInt, byte[] szInterfaceXmlData);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_GetGlobalXmlData(byte[] szGlobalXmlData, int JsonMaxLen);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_UpdateGlobalXmlData(byte[] szGlobalXmlData);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_GetTaxRates(uint hInt, ref int pNumberOfTotalRecords, ref int pNumberOfTotalRecordsReceived, byte[] pJsonTaxRate, byte[] szJsonTaxRate_Out, int JsonTaxRateLen_Out, int NumberOfRecordsRequested);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_GetDepartments(uint hInt, ref int pNumberOfTotalDepartments, ref int pNumberOfTotalDepartmentsReceived, byte[] pJsonDepartments, byte[] szJsonDepartments_Out, int JsonDepartmentsLen_Out, int NumberOfDepartmentRequested);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_GetTaxRates_Ex(uint hInt, byte offsetOfTaxRates, ref int pNumberOfTotalRecords, ref int pNumberOfTotalRecordsReceived, byte[] pJsonTaxRate, byte[] szJsonTaxRate_Out, int JsonTaxRateLen_Out, int NumberOfRecordsRequested);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_GetDepartments_Ex(uint hInt, byte offsetOfDepartments, ref int pNumberOfTotalDepartments, ref int pNumberOfTotalDepartmentsReceived, byte[] pJsonDepartments, byte[] szJsonDepartments_Out, int JsonDepartmentsLen_Out, int NumberOfDepartmentRequested);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_GetCurrencyProfile(uint hInt, byte[] szJsonExchangeProfileTable_In, byte[] szJsonExchangeProfileTable_Out, int szJsonExchangeProfileTable_Out_Length);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_SetCurrencyProfile(uint hInt, byte[] supervisorPassword, byte profileIndex, byte ProfileProcessType, byte[] szJsonExchangeProfileTable_In);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_SetCurrencyProfileIndex(uint hInt, ulong hTrx, byte index, byte[] pstTicket, int Timeout);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_SetDepartments(uint hInt, byte[] pJsonDepartments, byte[] pJsonDepartments_Out, int pJsonDepartmentsLen_Out, byte NumberOfDepartmentRequested, byte[] szSupervisorPassword);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_KasaAvans(uint hInt, ulong hTrx, int Amount, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_CustomerAvans(uint hInt, ulong hTrx, int Amount, byte[] szJsonTicket_Out, int JsonTicketLen_Out, byte[] szCustomerName, byte[] szTckn, byte[] szVkn, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_CariHesap(uint hInt, ulong hTrx, int Amount, byte[] szJsonTicket_Out, int JsonTicketLen_Out, byte[] szCustomerName, byte[] szTckn, byte[] szVkn, byte[] szBelgeNo, byte[] szBelgeDate, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_KasaPayment(uint hInt, ulong hTrx, int Amount, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_GetTicket(uint hInt, ulong hTrx, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_ItemSale(uint hInt, ulong hTrx, byte[] szJsonItem, byte[] szJsonItem_Out, int JsonItemLen_Out, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_Payment(uint hInt, ulong hTrx, byte[] stPaymentRequest, byte[] Out_stPaymentRequest, int Out_stPaymentRequestLen, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_FunctionVasPaymentRefund(uint hInt, byte[] stPaymentRequest, byte[] Out_stPaymentRequest, int Out_stPaymentRequestLen, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_PrintUserMessage(uint hInt, ulong hTrx, byte[] szJsonUserMessage, byte[] szJsonUserMessage_Out, int JsonUserMessageLen_Out, ushort NumberOfMessage, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_PrintUserMessage_Ex(uint hInt, ulong hTrx, byte[] szJsonUserMessage, byte[] szJsonUserMessage_Out, int JsonUserMessageLen_Out, ushort NumberOfMessage, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_Plus(uint hInt, ulong hTrx, int Amount, byte[] szText, byte[] szJsonTicket_Out, int JsonTicketLen_Out, ushort IndexOfItem, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_LoyaltyDiscount(uint hInt, ulong hTrx, byte isRate, int Amount, byte Rate, byte[] szLoyaltyCustomerId, byte[] szText, ushort indexOfItem, ref int pchangedAmount, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int timeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_Minus(uint hInt, ulong hTrx, int Amount, byte[] szText, byte[] szJsonTicket_Out, int JsonTicketLen_Out, ushort IndexOfItem, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_Inc(uint hInt, ulong hTrx, byte Rate, byte[] szText, byte[] szJsonTicket_Out, int JsonTicketLen_Out, ushort IndexOfItem, ref int pChangedAmount, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_Dec(uint hInt, ulong hTrx, byte Rate, byte[] szText, byte[] szJsonTicket_Out, int JsonTicketLen_Out, ushort IndexOfItem, ref int pChangedAmount, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_VoidAll(uint hInt, ulong hTrx, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TmeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_Pretotal(uint hInt, ulong hTrx, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_Matrahsiz(uint hInt, ulong hTrx, byte[] TckNo, ushort SubtypeOfTaxException, int MatrahsizAmount, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_VoidPayment(uint hInt, ulong hTrx, ushort Index, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_VoidItem(uint hInt, ulong hTrx, ushort Index, ulong ItemCount, byte ItemCountPrecision, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_FunctionGetUniqueIdList(uint hInt, byte[] szUniqueIdList, byte[] szUniqueIdList_Out, int UniqueIdListLen_Out, ushort MaxNumberOfitems, ushort IndexOfitemsToStart, ref ushort pTotalNumberOfItems, ref ushort pNumberOfItemsInThis, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_FunctionReadCard(uint hInt, int CardReaderTypes, byte[] szCardInfo, byte[] szCardInfo_Out, int CardInfoLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_GetCashierTable(uint hInt, ref int pNumberOfTotalRecords, ref int pNumberOfTotalRecordsReceived, byte[] szCashier, byte[] szCashier_Out, int CashierLen_Out, int NumberOfRecordsRequested, ref short pActiveCashier);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_Echo(uint hInt, byte[] szEcho_Out, int EchoLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_GetPaymentApplicationInfo(uint hInt, ref byte pNumberOfTotalRecords, ref byte pNumberOfTotalRecordsReceived, byte[] szExchange, byte[] szExchange_Out, int ExchangeLen_Out, byte NumberOfRecordsRequested);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_SetOnlineInvoice(uint hInt, ulong hTrx, byte[] szJsonInvoiceInfo, byte[] szTicket_Out, int TicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_SetTaxFreeInfo(uint hInt, ulong hTrx, byte[] szJsonTaxFreeInfo, byte[] szTicket_Out, int TicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_SetInvoice(uint hInt, ulong hTrx, byte[] szJsonInvoiceInfo, byte[] szJsonInvoiceInfo_Out, int JsonInvoiceInfoLen_Out, byte[] szTicket_Out, int TicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_SendSMMData(uint hInt, ulong hTrx, byte[] szJsonSMMData, byte[] szTicket_Out, int TicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_SendGiderPusulasi(uint hInt, ulong hTrx, byte[] szJsonGiderPusulasi, byte[] szTicket_Out, int TicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_SetTaxFree(uint hInt, ulong hTrx, byte[] szJsonTaxFreeInfo, byte[] szTicket_Out, int TicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_StartPairingInit(uint hInt, byte[] szPairing, byte[] szPairingResp, int PairingRespLen);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Json_prepare_ItemSale(byte[] Buffer, int MaxSize, byte[] szJsonItem, byte[] szJsonItem_Out, int JsonItemLen_Out);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Json_prepare_Payment(byte[] Buffer, int MaxSize, byte[] szJsonPaymentRequest, byte[] szJsonPaymentRequest_Out, int JsonPaymentRequestLen_Out);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_FunctionReports(uint hInt, int FunctionFlags, byte[] szJsonFunctionParameters, byte[] szJsonFunctionParameters_Out, int JsonFunctionParametersLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_FunctionReadZReport(uint hInt, byte[] szJsonFunctionParameters, byte[] szJsonZReport_Out, int JsonZReportLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_FunctionReadDM_Report(uint hInt, byte[] szJsonFunctionParameters, byte[] szJsonDM_Report_Out, int JsonDM_ReportLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_FunctionPaymentCheck(uint hInt, byte[] szJsonCheckResponse_In, byte[] szJsonCheckResponse_Out, int szJsonCheckResponseLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Json_prepare_SetInvoice(byte[] Buffer, int MaxSize, byte[] szJsonInvoiceInfo, byte[] szJsonInvoiceInfo_Out, int JsonInvoiceInfoLen_Out);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Json_prepare_SendSMMData(byte[] Buffer, int MaxSize, byte[] szJsonSMMData);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Json_prepare_SendGiderPusulasi(byte[] Buffer, int MaxSize, byte[] szJsonSMMData);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Json_prepare_SetOnlineInvoice(byte[] Buffer, int MaxSize, byte[] szJsonInvoiceInfo);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Json_prepare_SetTaxFreeInfo(byte[] Buffer, int MaxSize, byte[] szJsonTaxFreeInfo, byte[] szJsonTaxFreeInfo_Out, int JsonTaxFreeInfoLen_Out);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Json_prepare_SetTaxFreeInfo(byte[] Buffer, int MaxSize, byte[] szJsonTaxFreeInfo);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Json_prepare_PrintUserMessage(byte[] Buffer, int MaxSize, byte[] szJsonUserMessage, byte[] szJsonUserMessage_Out, int JsonUserMessageLen_Out, ushort NumberOfMessage);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Json_prepare_PrintUserMessage_Ex(byte[] Buffer, int MaxSize, byte[] szJsonUserMessage, byte[] szJsonUserMessage_Out, int JsonUserMessageLen_Out, ushort NumberOfMessage);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_parse_FiscalPrinter(byte[] szJsonReturnCodes_Out, int JsonReturnCodestLen_Out, ref ushort pNumberOfreturnCodes, uint RecvMsgId, byte[] RecvFullBuffer, ushort RecvFullLen, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int MaxNumberOfReturnCode, int MaxReturnCodeDataLen);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_parse_GetEcr(byte[] szJsonReturnCodes_Out, int JsonReturnCodesLen_Out, ref int pNumberOfReturnCodes, uint RecvMsgId, byte[] RecvFullBuffer, ushort RecvFullLen, int MaxNumberOfReturnCode, int MaxReturnCodeDataLen);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Json_prepare_ReversePayment(byte[] Buffer, int MaxSize, byte[] szJsonPaymentRequest, byte[] szJsonPaymentRequest_Out, int JsonPaymentRequestLen_Out, ushort NumberOfPaymentRequests);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Json_prepare_Date(byte[] Buffer, int MaxSize, uint Tag_Id, byte[] Title, byte[] Text, byte[] Mask, byte[] szJsonValue, byte[] szJsonValue_Out, int JsonValueLen_Out, int timeout);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Json_prepare_Condition(byte[] Buffer, int MaxSize, byte[] szJsonCondition, byte[] szJsonCondition_Out, int JsonConditionLen_Out);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_ReversePayment(uint hInt, ulong hTrx, byte[] szJsonPaymentRequest, byte[] szJsonPaymentRequest_Out, int JsonPaymentRequestLen_Out, short NumberOfPaymentRequests, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_JumpToECR(uint hInt, ulong hTrx, ulong JumpFlags, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_MultipleCommand(uint hInt, ref ulong hTrx, byte[] stJsonReturnCodes_Out, int JsonReturnCodesLen_Out, ref ushort pIndexOfReturnCodes, byte[] SendBuffer, ushort SendBufferLen, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_SetTaxFree(uint hInt, ulong hTrx, int Flag, byte[] szName, byte[] szSurname, byte[] szIdentificationNo, byte[] szCity, byte[] szCountry, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_SetParkingTicket(uint hInt, ulong hTrx, byte[] szCarIdentification, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_SetTaxFreeRefundAmount(uint hInt, ulong hTrx, int RefundAmount, ushort RefundAmountCurrency, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_LoyaltyCustomerQuery(uint hInt, ulong hTrx, byte[] szJsonLoyaltyServiceInfo, byte[] szJsonLoyaltyServiceInfo_Out, int JsonLoyaltyServiceInfoLen_Out, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_FunctionChangeTicketHeader(uint hInt, byte[] szSupervisorPassword, ref ushort pNumberOfSpaceTotal, ref ushort pNumberOfSpaceUsed, byte[] szJsonTicketHeader, byte[] szJsonTicketHeader_Out, int JsonTicketHeaderLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_GetTicketHeader(uint hInt, ushort IndexOfHeader, byte[] szJsonTicketHeader, byte[] szJsonTicketHeader_Out, int JsonTicketHeaderLen_Out, ref ushort pNumberOfSpaceTotal, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_GetOnlineInvoiceInfo(uint hInt, byte[] szOnlineInvoiceId, int OnlineInvoiceIdLen, byte[] szJsonOnlineInvoiceInfo_Out, int JsonOnlineInvoiceInfoLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_Database_QueryColomnCaptions(uint hInt, byte[] szJsonDatabaseResult_Out, int JsonDatabaseResultLen_Out);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_Database_QueryReadLine(uint hInt, ushort NumberOfLinesRequested, ushort NumberOfColomnsRequested, byte[] szJsonDatabaseResult_Out, int JsonDatabaseResultLen_Out);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_Database_FreeStructure(uint hInt, byte[] szJsonDatabaseResult, byte[] szJsonDatabaseResult_Out, int JsonDatabaseResultLen_Out);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_Database_Execute(uint hInt, byte[] szSqlWord, byte[] szJsonDatabaseResult_Out, int JsonDatabaseResultLen_Out);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_GetPLU(uint hInt, byte[] szBarcode, byte[] szJsonPluRecord, byte[] szJsonPluRecord_Out, int JsonPluRecordLen_Out, byte[] szJsonPluGroupRecord, byte[] szJsonPluGroupRecord_Out, int szJsonPluGroupRecordLen_Out, int MaxNumberOfGroupRecords, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_GetVasApplicationInfo(uint hInt, ref byte pNumberOfTotalRecords, ref byte pNumberOfTotalRecordsReceived, byte[] szJsonPaymentApplicationInfo_Out, int JsonPaymentApplicationInfoLen_Out, ushort vasType);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_GetVasLoyaltyServiceInfo(uint hInt, ref byte pNumberOfTotalRecords, ref byte pNumberOfTotalRecordsReceived, byte[] szJsonVasApplicationInfo_Out, int JsonVasApplicationInfoLen_Out, ushort VasAppId);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_FunctionEkuSeek(uint hInt, byte[] szJsonEKUAppInfo, byte[] szJsonEKUAppInfo_Out, int JsonEKUAppInfoLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_FileSystem_DirListFiles(uint hInt, byte[] szDirName, byte[] szJsonStFile, byte[] szJsonStFile_Out, int JsonStFileLen_Out, short MaxNumberOfFiles, ref short pNumberOfFiles);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_FunctionEkuReadHeader(uint hInt, short Index, byte[] szJsonEkuHeader, byte[] szJsonEkuHeader_Out, int JsonEkuHeaderLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_FunctionEkuReadData(uint hInt, ref ushort pEkuDataBufferReceivedLen, byte[] szJsonEKUAppInfo, byte[] szJsonEKUAppInfo_Out, int JsonEKUAppInfoLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_FunctionEkuReadInfo(uint hInt, ushort EkuAccessFunction, byte[] szJsonEkuModuleInfo, byte[] szJsonEkuModuleInfo_Out, int JsonEkuModuleInfoLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_FunctionModuleReadInfo(uint hInt, int AccessFunction, byte[] szJsonModuleInfo, byte[] szJsonModuleInfo_Out, int JsonModuleInfoLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_FunctionBankingRefund(uint hInt, byte[] szJsonPaymentRequest, byte[] szJsonPaymentRequest_Out, int JsonPaymentRequestLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_FunctionBankingRefundExt(uint hInt, byte[] szJsonPaymentRequest, byte[] szJsonPaymentRequest_Out, int JsonPaymentRequestLen_Out, byte[] szJsonPaymentResponse_Out, int szJsonPaymentResponseLen, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_FunctionBankingBatch(uint hInt, ushort BkmId, ref ushort pNumberOfBankResponse, byte[] szJsonMultipleBankResponse_Out, int JsonMultipleBankResponseLen_Out, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_SetIniParameters(string szJsonIniParameter);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_GetIniParameters(byte[] szJsonIniParameter_Out, int szJsonIniParameterLen_Out);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_FunctionGetHandleList(uint hInt, byte[] szJsonHandleList_Out, int JsonHandleListLen_Out, byte StatusFilter, ushort StartIndexOfHandle, ushort HandleListSize, ref ushort TotalNumberOfHandlesInEcr, ref ushort ReceivedNumberOfHandleInList, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Json_FP3_FunctionTransactionInquiry(uint hInt, byte[] szJsonTransInquiry, byte[] szJsonTransInquiry_Out, int JsonTransInquiryLen_Out, int TimeoutInMiliseconds);

        public static uint FP3_CreateInterface(ref uint phInt, string szID, byte IsDefault, ST_INTERFACE_XML_DATA pstXmlData)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pstXmlData));
            byte[] bytesFromString2 = GMP_Tools.GetBytesFromString(szID);
            return Json_FP3_CreateInterface(ref phInt, bytesFromString2, IsDefault, bytesFromString);
        }

        public static uint FP3_GetInterfaceXmlDataByID(string szID, ref ST_INTERFACE_XML_DATA pStInterfaceXmlData)
        {
            byte[] array = new byte[50000];
            uint num = Json_FP3_GetInterfaceXmlDataByID(GMP_Tools.GetBytesFromString(szID), array, array.Length);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStInterfaceXmlData = JsonConvert.DeserializeObject<ST_INTERFACE_XML_DATA>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_GetInterfaceXmlDataByHandle(uint hInt, ref ST_INTERFACE_XML_DATA stXmlData)
        {
            byte[] array = new byte[50000];
            uint num = Json_FP3_GetInterfaceXmlDataByHandle(hInt, array, array.Length);
            if (num == 0)
            {
                string value = GMP_Tools.SetEncoding(array);
                stXmlData = JsonConvert.DeserializeObject<ST_INTERFACE_XML_DATA>(value);
            }
            return num;
        }

        public static uint FP3_UpdateInterfaceXmlDataByID(string szID, ref ST_INTERFACE_XML_DATA pstInterfaceXmlData)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pstInterfaceXmlData));
            return Json_FP3_UpdateInterfaceXmlDataByID(GMP_Tools.GetBytesFromString(szID), bytesFromString);
        }

        public static uint FP3_UpdateInterfaceXmlDataByHandle(uint hInt, ref ST_INTERFACE_XML_DATA pstInterfaceXmlData)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pstInterfaceXmlData));
            return Json_FP3_UpdateInterfaceXmlDataByHandle(hInt, bytesFromString);
        }

        public static uint FP3_GetGlobalXmlData(ref ST_GLOBAL_XML_DATA StGlobalXmlData)
        {
            GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(StGlobalXmlData));
            byte[] array = new byte[50000];
            uint num = Json_FP3_GetGlobalXmlData(array, array.Length);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                StGlobalXmlData = JsonConvert.DeserializeObject<ST_GLOBAL_XML_DATA>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_UpdateGlobalXmlData(ref ST_GLOBAL_XML_DATA StGlobalXmlData)
        {
            return Json_FP3_UpdateGlobalXmlData(GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(StGlobalXmlData)));
        }

        public static void MergeItemStruct(ST_TICKET StTicketDest, ST_TICKET StTicketSrc)
        {
            StTicketDest.szTicketDate = StTicketSrc.szTicketDate;
            StTicketDest.szTicketTime = StTicketSrc.szTicketTime;
            StTicketDest.SourceVasAppID = StTicketSrc.SourceVasAppID;
            StTicketDest.PaymentVasAppID = StTicketSrc.PaymentVasAppID;
            StTicketDest.BankVasAppID = StTicketSrc.BankVasAppID;
            StTicketDest.CashBackAmount = StTicketSrc.CashBackAmount;
            StTicketDest.EJNo = StTicketSrc.EJNo;
            StTicketDest.FNo = StTicketSrc.FNo;
            StTicketDest.invoiceAmount = StTicketSrc.invoiceAmount;
            StTicketDest.invoiceAmountCurrency = StTicketSrc.invoiceAmountCurrency;
            StTicketDest.invoiceDate = StTicketSrc.invoiceDate;
            StTicketDest.invoiceNo = StTicketSrc.invoiceNo;
            StTicketDest.invoiceType = StTicketSrc.invoiceType;
            StTicketDest.KasaAvansAmount = StTicketSrc.KasaAvansAmount;
            StTicketDest.KasaPaymentAmount = StTicketSrc.KasaPaymentAmount;
            StTicketDest.KatkiPayiAmount = StTicketSrc.KatkiPayiAmount;
            StTicketDest.numberOfItemsInThis = StTicketSrc.numberOfItemsInThis;
            StTicketDest.numberOfPaymentsInThis = StTicketSrc.numberOfPaymentsInThis;
            StTicketDest.numberOfPrinterLinesInThis = StTicketSrc.numberOfPrinterLinesInThis;
            StTicketDest.OptionFlags = StTicketSrc.OptionFlags;
            StTicketDest.TaxFreeCalculated = StTicketSrc.TaxFreeCalculated;
            StTicketDest.TaxFreeRefund = StTicketSrc.TaxFreeRefund;
            StTicketDest.TckNo = StTicketSrc.TckNo;
            StTicketDest.ticketType = StTicketSrc.ticketType;
            StTicketDest.totalNumberOfItems = StTicketSrc.totalNumberOfItems;
            StTicketDest.totalNumberOfPayments = StTicketSrc.totalNumberOfPayments;
            StTicketDest.totalNumberOfPrinterLines = StTicketSrc.totalNumberOfPrinterLines;
            StTicketDest.numberOfLoyaltyInThis = StTicketSrc.numberOfLoyaltyInThis;
            StTicketDest.TotalReceiptAmount = StTicketSrc.TotalReceiptAmount;
            StTicketDest.TotalReceiptDiscount = StTicketSrc.TotalReceiptDiscount;
            StTicketDest.TotalReceiptIncrement = StTicketSrc.TotalReceiptIncrement;
            StTicketDest.TotalReceiptItemCancel = StTicketSrc.TotalReceiptItemCancel;
            StTicketDest.TotalReceiptPayment = StTicketSrc.TotalReceiptPayment;
            StTicketDest.TotalReceiptReversedPayment = StTicketSrc.TotalReceiptReversedPayment;
            StTicketDest.TotalReceiptTax = StTicketSrc.TotalReceiptTax;
            StTicketDest.TransactionFlags = StTicketSrc.TransactionFlags;
            StTicketDest.uniqueId = StTicketSrc.uniqueId;
            StTicketDest.ZNo = StTicketSrc.ZNo;
            StTicketDest.UserData = StTicketSrc.UserData;
            StTicketDest.LastPaymentErrorCode = StTicketSrc.LastPaymentErrorCode;
            StTicketDest.LastPaymentErrorMsg = StTicketSrc.LastPaymentErrorMsg;
            StTicketDest.BankPaymentUniqueId = StTicketSrc.BankPaymentUniqueId;
            StTicketDest.CurrencyProfileIndex = StTicketSrc.CurrencyProfileIndex;
            StTicketDest.stTaxDetails = new ST_VATDetail[StTicketSrc.stTaxDetails.Length];
            for (int i = 0; i < StTicketDest.stTaxDetails.Length; i++)
            {
                StTicketDest.stTaxDetails[i] = new ST_VATDetail();
            }
            StTicketDest.stPrinterCopy = new ST_printerDataForOneLine[StTicketSrc.totalNumberOfPrinterLines];
            for (int j = 0; j < StTicketDest.stPrinterCopy.Length; j++)
            {
                StTicketDest.stPrinterCopy[j] = new ST_printerDataForOneLine();
            }
            for (int k = 0; k < StTicketSrc.stTaxDetails.Length; k++)
            {
                if (StTicketSrc.stTaxDetails != null && StTicketSrc.stTaxDetails[k] != null)
                {
                    StTicketDest.stTaxDetails[k] = StTicketSrc.stTaxDetails[k];
                }
            }
            for (int l = 0; l < StTicketSrc.totalNumberOfItems; l++)
            {
                if (StTicketSrc.SaleInfo != null && StTicketSrc.SaleInfo[l] != null)
                {
                    StTicketDest.SaleInfo[l] = StTicketSrc.SaleInfo[l];
                }
            }
            for (int m = 0; m < StTicketSrc.totalNumberOfPayments; m++)
            {
                if (StTicketSrc.stPayment != null && StTicketSrc.stPayment[m] != null)
                {
                    StTicketDest.stPayment[m] = StTicketSrc.stPayment[m];
                }
            }
            for (int n = 0; n < StTicketSrc.numberOfLoyaltyInThis; n++)
            {
                if (StTicketSrc.stLoyaltyService != null && StTicketSrc.stLoyaltyService[n] != null)
                {
                    StTicketDest.stLoyaltyService[n] = StTicketSrc.stLoyaltyService[n];
                }
            }
            for (int num = 0; num < StTicketSrc.totalNumberOfPrinterLines; num++)
            {
                if (StTicketSrc.stPrinterCopy != null && StTicketSrc.stPrinterCopy[num] != null)
                {
                    StTicketDest.stPrinterCopy[num] = StTicketSrc.stPrinterCopy[num];
                }
            }
        }

        private static void MergeTaxRateStruct(ST_TAX_RATE[] StTaxRateDest, ST_TAX_RATE[] StTaxRateSrc, int index, int size)
        {
            int num = 0;
            for (int i = index; i < index + size; i++)
            {
                StTaxRateDest[i].taxRate = StTaxRateSrc[num++].taxRate;
            }
        }

        private static void MergeDepartmentStruct(ST_DEPARTMENT[] StDepartmentDest, ST_DEPARTMENT[] StDepartmentSrc, int index, int size)
        {
            int num = 0;
            for (int i = index; i < index + size; i++)
            {
                StDepartmentDest[i].iCurrencyType = StDepartmentSrc[num].iCurrencyType;
                StDepartmentDest[i].iUnitType = StDepartmentSrc[num].iUnitType;
                StDepartmentDest[i].szDeptName = StDepartmentSrc[num].szDeptName;
                StDepartmentDest[i].u64Limit = StDepartmentSrc[num].u64Limit;
                StDepartmentDest[i].u64Price = StDepartmentSrc[num].u64Price;
                StDepartmentDest[i].u8TaxIndex = StDepartmentSrc[num].u8TaxIndex;
                num++;
            }
        }

        public static uint FP3_GetTaxRates(uint hInt, ref int pNumberOfTotalRecords, ref int pNumberOfTotalRecordsReceived, ref ST_TAX_RATE[] pStTaxRate, int NumberOfRecordsRequested)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStTaxRate));
            byte[] array = new byte[50000];
            uint num = Json_FP3_GetTaxRates(hInt, ref pNumberOfTotalRecords, ref pNumberOfTotalRecordsReceived, bytesFromString, array, array.Length, NumberOfRecordsRequested);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStTaxRate = JsonConvert.DeserializeObject<ST_TAX_RATE[]>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_GetDepartments(uint hInt, ref int pNumberOfTotalDepartments, ref int pNumberOfTotalDepartmentsReceived, ref ST_DEPARTMENT[] pStDepartments, int NumberOfDepartmentRequested)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStDepartments));
            byte[] array = new byte[50000];
            uint num = Json_FP3_GetDepartments(hInt, ref pNumberOfTotalDepartments, ref pNumberOfTotalDepartmentsReceived, bytesFromString, array, array.Length, NumberOfDepartmentRequested);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStDepartments = JsonConvert.DeserializeObject<ST_DEPARTMENT[]>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_GetTaxRates_Ex(uint hInt, byte indexOfTaxRates, ref int pNumberOfTotalRecords, ref int pNumberOfTotalRecordsReceived, ref ST_TAX_RATE[] pStTaxRateOrg, int NumberOfRecordsRequested)
        {
            ST_TAX_RATE[] value = new ST_TAX_RATE[NumberOfRecordsRequested];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(value));
            byte[] array = new byte[50000];
            uint num = Json_FP3_GetTaxRates_Ex(hInt, indexOfTaxRates, ref pNumberOfTotalRecords, ref pNumberOfTotalRecordsReceived, bytesFromString, array, array.Length, NumberOfRecordsRequested);
            if (num == 0)
            {
                MergeTaxRateStruct(StTaxRateSrc: JsonConvert.DeserializeObject<ST_TAX_RATE[]>(GMP_Tools.GetStringFromBytes(array)), StTaxRateDest: pStTaxRateOrg, index: indexOfTaxRates, size: pNumberOfTotalRecordsReceived);
            }
            return num;
        }

        public static uint FP3_GetDepartments_Ex(uint hInt, byte indexOfDepartments, ref int pNumberOfTotalDepartments, ref int pNumberOfTotalDepartmentsReceived, ref ST_DEPARTMENT[] pStDepartmentsOrg, int NumberOfDepartmentRequested)
        {
            ST_DEPARTMENT[] value = new ST_DEPARTMENT[NumberOfDepartmentRequested];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(value));
            byte[] array = new byte[50000];
            uint num = Json_FP3_GetDepartments_Ex(hInt, indexOfDepartments, ref pNumberOfTotalDepartments, ref pNumberOfTotalDepartmentsReceived, bytesFromString, array, array.Length, NumberOfDepartmentRequested);
            if (num == 0)
            {
                MergeDepartmentStruct(StDepartmentSrc: JsonConvert.DeserializeObject<ST_DEPARTMENT[]>(GMP_Tools.GetStringFromBytes(array)), StDepartmentDest: pStDepartmentsOrg, index: indexOfDepartments, size: pNumberOfTotalDepartmentsReceived);
            }
            return num;
        }

        public static uint FP3_GetCurrencyProfile(uint hInt, ref ST_EXCHANGE_PROFILE[] pStExchangeProfile)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStExchangeProfile));
            byte[] array = new byte[50000];
            uint num = Json_FP3_GetCurrencyProfile(hInt, bytesFromString, array, array.Length);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStExchangeProfile = JsonConvert.DeserializeObject<List<ST_EXCHANGE_PROFILE>>(stringFromBytes).ToArray();
            }
            return num;
        }

        public static uint FP3_SetCurrencyProfile(uint hInt, string supervisorPassword, byte profileIndex, byte ProfileProcessType, ST_EXCHANGE_PROFILE[] pStExchangeProfile)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStExchangeProfile));
            _ = new byte[50000];
            return Json_FP3_SetCurrencyProfile(hInt, GMP_Tools.GetBytesFromString(supervisorPassword), profileIndex, ProfileProcessType, bytesFromString);
        }

        public static uint FP3_SetCurrencyProfileIndex(uint hInt, ulong hTrx, byte index, ST_TICKET stTicket, int TimeoutInMiliseconds)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(stTicket));
            return Json_FP3_SetCurrencyProfileIndex(hInt, hTrx, index, bytesFromString, TimeoutInMiliseconds);
        }

        public static uint FP3_SetDepartments(uint hInt, ref ST_DEPARTMENT[] pStDepartments, byte NumberOfDepartmentRequested, string szSupervisorPassword)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStDepartments));
            byte[] bytesFromString2 = GMP_Tools.GetBytesFromString(szSupervisorPassword);
            byte[] array = new byte[50000];
            uint num = Json_FP3_SetDepartments(hInt, bytesFromString, array, array.Length, NumberOfDepartmentRequested, bytesFromString2);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStDepartments = JsonConvert.DeserializeObject<ST_DEPARTMENT[]>(stringFromBytes);
            }
            return num;
        }

        public static uint Json_FP3_CustomerAvans(uint hInt, ulong hTrx, int Amount, ref ST_TICKET pstTicket, string szCustomerName, string szTckn, string szVkn, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szCustomerName);
            byte[] bytesFromString2 = GMP_Tools.GetBytesFromString(szTckn);
            byte[] bytesFromString3 = GMP_Tools.GetBytesFromString(szVkn);
            uint num = Json_FP3_CustomerAvans(hInt, hTrx, Amount, array, array.Length, bytesFromString, bytesFromString2, bytesFromString3, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint Json_FP3_CariHesap(uint hInt, ulong hTrx, int Amount, ref ST_TICKET pstTicket, string szCustomerName, string szTckn, string szVkn, string szBelgeNo, string szBelgeDate, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szCustomerName);
            byte[] bytesFromString2 = GMP_Tools.GetBytesFromString(szTckn);
            byte[] bytesFromString3 = GMP_Tools.GetBytesFromString(szVkn);
            byte[] bytesFromString4 = GMP_Tools.GetBytesFromString(szBelgeNo);
            byte[] bytesFromString5 = GMP_Tools.GetBytesFromString(szBelgeDate);
            uint num = Json_FP3_CariHesap(hInt, hTrx, Amount, array, array.Length, bytesFromString, bytesFromString2, bytesFromString3, bytesFromString4, bytesFromString5, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint SetIniParameter(ST_INI_PARAM pStIniParameter)
        {
            return Json_SetIniParameters(JsonConvert.SerializeObject(pStIniParameter));
        }

        public static uint GetIniParameters(ref ST_INI_PARAM pStIniParameter)
        {
            byte[] array = new byte[200000];
            uint num = Json_GetIniParameters(array, array.Length);
            if (num == 0)
            {
                string value = GMP_Tools.SetEncoding(array);
                pStIniParameter = JsonConvert.DeserializeObject<ST_INI_PARAM>(value);
            }
            return num;
        }

        public static uint FP3_FunctionGetHandleList(uint hInt, ref ST_HANDLE_LIST[] stHandleList, byte StatusFilter, ushort StartIndexOfHandle, ushort HandleListSize, ref ushort TotalNumberOfHandlesInEcr, ref ushort ReceivedNumberOfHandleInList, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            uint num = Json_FP3_FunctionGetHandleList(hInt, array, array.Length, StatusFilter, StartIndexOfHandle, HandleListSize, ref TotalNumberOfHandlesInEcr, ref ReceivedNumberOfHandleInList, TimeoutInMiliseconds);
            if (num == 0)
            {
                string value = GMP_Tools.SetEncoding(array);
                stHandleList = JsonConvert.DeserializeObject<ST_HANDLE_LIST[]>(value);
            }
            return num;
        }

        public static uint FP3_KasaAvans(uint hInt, ulong hTrx, int Amount, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            uint num = Json_FP3_KasaAvans(hInt, hTrx, Amount, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint FP3_KasaPayment(uint hInt, ulong hTrx, int Amount, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[50000];
            uint num = Json_FP3_KasaPayment(hInt, hTrx, Amount, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint FP3_ItemSale(uint hInt, ulong hTrx, ref ST_ITEM StItem, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(StItem));
            byte[] array2 = new byte[200000];
            uint num = Json_FP3_ItemSale(hInt, hTrx, bytesFromString, array2, array2.Length, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array2);
                StItem = JsonConvert.DeserializeObject<ST_ITEM>(stringFromBytes);
                stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint FP3_Payment(uint hInt, ulong hTrx, ref ST_PAYMENT_REQUEST pStPaymentRequest, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStPaymentRequest));
            byte[] array2 = new byte[200000];
            uint num = Json_FP3_Payment(hInt, hTrx, bytesFromString, array2, array2.Length, array, array.Length, TimeoutInMiliseconds);
            if (num != 0)
            {
                return num;
            }
            string stringFromBytes = GMP_Tools.GetStringFromBytes(array2);
            pStPaymentRequest = JsonConvert.DeserializeObject<ST_PAYMENT_REQUEST>(stringFromBytes);
            stringFromBytes = GMP_Tools.GetStringFromBytes(array);
            ST_TICKET sT_TICKET = new ST_TICKET();
            sT_TICKET = JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes);
            MergeItemStruct(pstTicket, sT_TICKET);
            return num;
        }

        public static uint FP3_FunctionVasPaymentRefund(uint hInt, ref ST_PAYMENT_REQUEST pStPaymentRequest, int TimeoutInMiliseconds)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStPaymentRequest));
            byte[] array = new byte[200000];
            uint num = Json_FP3_FunctionVasPaymentRefund(hInt, bytesFromString, array, array.Length, TimeoutInMiliseconds);
            if (num != 0)
            {
                return num;
            }
            string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
            pStPaymentRequest = JsonConvert.DeserializeObject<ST_PAYMENT_REQUEST>(stringFromBytes);
            return num;
        }

        public static uint FP3_PrintUserMessage(uint hInt, ulong hTrx, ref ST_USER_MESSAGE[] pStUser, ushort NumberOfMessage, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStUser));
            byte[] array2 = new byte[200000];
            uint num = Json_FP3_PrintUserMessage(hInt, hTrx, bytesFromString, array2, array2.Length, NumberOfMessage, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array2);
                pStUser = JsonConvert.DeserializeObject<ST_USER_MESSAGE[]>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_PrintUserMessage_Ex(uint hInt, ulong hTrx, ref ST_USER_MESSAGE[] pStUser, ushort NumberOfMessage, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStUser));
            byte[] array2 = new byte[200000];
            uint num = Json_FP3_PrintUserMessage_Ex(hInt, hTrx, bytesFromString, array2, array2.Length, NumberOfMessage, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array2);
                pStUser = JsonConvert.DeserializeObject<ST_USER_MESSAGE[]>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_GetTicket(uint hInt, ulong hTrx, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            uint num = Json_FP3_GetTicket(hInt, hTrx, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint FP3_Plus(uint hInt, ulong hTrx, int Amount, string szText, ref ST_TICKET pstTicket, ushort IndexOfItem, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szText);
            uint num = Json_FP3_Plus(hInt, hTrx, Amount, bytesFromString, array, array.Length, IndexOfItem, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint FP3_LoyaltyDiscount(uint hInt, ulong hTrx, byte isRate, int Amount, byte Rate, string szLoyaltyCustomerId, string szText, ushort indexOfItem, ref int pchangedAmount, ref ST_TICKET pstTicket, int timeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szText);
            byte[] bytesFromString2 = GMP_Tools.GetBytesFromString(szLoyaltyCustomerId);
            uint num = Json_FP3_LoyaltyDiscount(hInt, hTrx, isRate, Amount, Rate, bytesFromString2, bytesFromString, indexOfItem, ref pchangedAmount, array, array.Length, timeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint FP3_Minus(uint hInt, ulong hTrx, int Amount, string szText, ref ST_TICKET pstTicket, ushort IndexOfItem, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szText);
            uint num = Json_FP3_Minus(hInt, hTrx, Amount, bytesFromString, array, array.Length, IndexOfItem, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint FP3_Dec(uint hInt, ulong hTrx, byte Rate, string szText, ref ST_TICKET pstTicket, ushort IndexOfItem, ref int pChangedAmount, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szText);
            uint num = Json_FP3_Dec(hInt, hTrx, Rate, bytesFromString, array, array.Length, IndexOfItem, ref pChangedAmount, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint FP3_Inc(uint hInt, ulong hTrx, byte Rate, string szText, ref ST_TICKET pstTicket, ushort IndexOfItem, ref int pChangedAmount, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szText);
            uint num = Json_FP3_Inc(hInt, hTrx, Rate, bytesFromString, array, array.Length, IndexOfItem, ref pChangedAmount, TimeoutInMiliseconds);
            if (num == 0)
            {
                string value = GMP_Tools.SetEncoding(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(value), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint FP3_VoidAll(uint hInt, ulong hTrx, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            uint num = Json_FP3_VoidAll(hInt, hTrx, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint FP3_Pretotal(uint hInt, ulong hTrx, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            uint num = Json_FP3_Pretotal(hInt, hTrx, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint FP3_Matrahsiz(uint hInt, ulong hTrx, string szTckNo, ushort SubtypeOfTaxException, int MatrahsizAmount, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szTckNo);
            uint num = Json_FP3_Matrahsiz(hInt, hTrx, bytesFromString, SubtypeOfTaxException, MatrahsizAmount, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint FP3_VoidPayment(uint hInt, ulong hTrx, ushort Index, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            uint num = Json_FP3_VoidPayment(hInt, hTrx, Index, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint FP3_VoidItem(uint hInt, ulong hTrx, ushort Index, ulong ItemCount, byte ItemCountPrecision, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            uint num = Json_FP3_VoidItem(hInt, hTrx, Index, ItemCount, ItemCountPrecision, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint FP3_FunctionGetUniqueIdList(uint hInt, ref ST_UNIQUE_ID[] pStUniqueIdList, ushort MaxNumberOfitems, ushort IndexOfitemsToStart, ref ushort pTotalNumberOfItems, ref ushort pNumberOfItemsInThis, int TimeoutInMiliseconds)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStUniqueIdList));
            byte[] array = new byte[50000];
            uint num = Json_FP3_FunctionGetUniqueIdList(hInt, bytesFromString, array, array.Length, MaxNumberOfitems, IndexOfitemsToStart, ref pTotalNumberOfItems, ref pNumberOfItemsInThis, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStUniqueIdList = JsonConvert.DeserializeObject<ST_UNIQUE_ID[]>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_FunctionReadCard(uint hInt, int CardReaderTypes, ref ST_CARD_INFO pStCardInfo, int TimeoutInMiliseconds)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStCardInfo));
            byte[] array = new byte[50000];
            uint num = Json_FP3_FunctionReadCard(hInt, CardReaderTypes, bytesFromString, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStCardInfo = JsonConvert.DeserializeObject<ST_CARD_INFO>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_GetCashierTable(uint hInt, ref int pNumberOfTotalRecords, ref int pNumberOfTotalRecordsReceived, ref ST_CASHIER[] pStCashier, int NumberOfRecordsRequested, ref short pActiveCashier)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStCashier));
            byte[] array = new byte[200000];
            uint num = Json_FP3_GetCashierTable(hInt, ref pNumberOfTotalRecords, ref pNumberOfTotalRecordsReceived, bytesFromString, array, array.Length, NumberOfRecordsRequested, ref pActiveCashier);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStCashier = JsonConvert.DeserializeObject<ST_CASHIER[]>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_Echo(uint hInt, ref ST_ECHO pStEcho, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            uint num = Json_FP3_Echo(hInt, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStEcho = JsonConvert.DeserializeObject<ST_ECHO>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_StartPairingInit(uint hInt, ref ST_GMP_PAIR pStPair, ref ST_GMP_PAIR_RESP pStPairResp, int TimeoutInMiliseconds = 10000)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStPair));
            byte[] array = new byte[200000];
            uint num = Json_FP3_StartPairingInit(hInt, bytesFromString, array, array.Length);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStPairResp = JsonConvert.DeserializeObject<ST_GMP_PAIR_RESP>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_GetPaymentApplicationInfo(uint hInt, ref byte pNumberOfTotalRecords, ref byte pNumberOfTotalRecordsReceived, ref ST_PAYMENT_APPLICATION_INFO[] pStAppInfo, byte NumberOfRecordsRequested)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStAppInfo));
            byte[] array = new byte[50000];
            uint num = Json_FP3_GetPaymentApplicationInfo(hInt, ref pNumberOfTotalRecords, ref pNumberOfTotalRecordsReceived, bytesFromString, array, array.Length, NumberOfRecordsRequested);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStAppInfo = JsonConvert.DeserializeObject<ST_PAYMENT_APPLICATION_INFO[]>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_SetInvoice(uint hInt, ulong hTrx, ref ST_INVIOCE_INFO pStInvoiceInfo, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStInvoiceInfo));
            byte[] array2 = new byte[200000];
            uint num = Json_FP3_SetInvoice(hInt, hTrx, bytesFromString, array2, array2.Length, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array2);
                pStInvoiceInfo = JsonConvert.DeserializeObject<ST_INVIOCE_INFO>(stringFromBytes);
                stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pstTicket = JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_SetOnlineInvoice(uint hInt, ulong hTrx, ref ST_ONLINE_INVIOCE_INFO pStOnlineInvoiceInfo, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStOnlineInvoiceInfo));
            uint result = Json_FP3_SetOnlineInvoice(hInt, hTrx, bytesFromString, array, array.Length, TimeoutInMiliseconds);
            string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
            pstTicket = JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes);
            return result;
        }

        public static uint FP3_SetTaxFreeInfo(uint hInt, ulong hTrx, ref ST_TAXFREE_INFO pStTaxFreeInfo, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStTaxFreeInfo));
            uint result = Json_FP3_SetTaxFreeInfo(hInt, hTrx, bytesFromString, array, array.Length, TimeoutInMiliseconds);
            string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
            pstTicket = JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes);
            return result;
        }

        public static int prepare_ItemSale(byte[] Buffer, int MaxSize, ref ST_ITEM pStItem)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStItem));
            byte[] array = new byte[200000];
            int num = Json_prepare_ItemSale(Buffer, MaxSize, bytesFromString, array, array.Length);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStItem = JsonConvert.DeserializeObject<ST_ITEM>(stringFromBytes);
            }
            return num;
        }

        public static int prepare_Payment(byte[] Buffer, int MaxSize, ref ST_PAYMENT_REQUEST pStPaymentRequest)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStPaymentRequest));
            byte[] array = new byte[200000];
            int result = Json_prepare_Payment(Buffer, MaxSize, bytesFromString, array, array.Length);
            string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
            pStPaymentRequest = JsonConvert.DeserializeObject<ST_PAYMENT_REQUEST>(stringFromBytes);
            return result;
        }

        public static uint FP3_FunctionReports(uint hInt, int functionFlags, ref ST_FUNCTION_PARAMETERS pStFunctionParameters, int TimeoutInMiliseconds)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStFunctionParameters));
            byte[] array = new byte[50000];
            return Json_FP3_FunctionReports(hInt, functionFlags, bytesFromString, array, array.Length, TimeoutInMiliseconds);
        }

        public static uint FP3_FunctionReadZReport(uint hInt, ref ST_FUNCTION_PARAMETERS pStFunctionParameters, ref ST_Z_REPORT pStZReport, int TimeoutInMiliseconds)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStFunctionParameters));
            byte[] array = new byte[200000];
            uint num = Json_FP3_FunctionReadZReport(hInt, bytesFromString, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStZReport = JsonConvert.DeserializeObject<ST_Z_REPORT>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_FunctionReadDM_Report(uint hInt, ref ST_FUNCTION_PARAMETERS pStFunctionParameters, ref ST_DM_REPORT pstDM_Report, int TimeoutInMiliseconds)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStFunctionParameters));
            byte[] array = new byte[200000];
            uint num = Json_FP3_FunctionReadDM_Report(hInt, bytesFromString, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pstDM_Report = JsonConvert.DeserializeObject<ST_DM_REPORT>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_FunctionPaymentCheck(uint hInt, char[] uniqueId, ref ST_PAYMENT_RESPONSE paymentCheckResponse, int TimeoutInMiliseconds)
        {
            JsonConvert.SerializeObject(paymentCheckResponse);
            byte[] bytes = Encoding.UTF8.GetBytes(uniqueId);
            byte[] array = new byte[200000];
            uint num = Json_FP3_FunctionPaymentCheck(hInt, bytes, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                paymentCheckResponse = JsonConvert.DeserializeObject<ST_PAYMENT_RESPONSE>(stringFromBytes);
            }
            return num;
        }

        public static int prepare_SetInvoice(byte[] Buffer, int MaxSize, ref ST_INVIOCE_INFO pStInvoiceInfo)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStInvoiceInfo));
            byte[] array = new byte[200000];
            int result = Json_prepare_SetInvoice(Buffer, MaxSize, bytesFromString, array, array.Length);
            string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
            pStInvoiceInfo = JsonConvert.DeserializeObject<ST_INVIOCE_INFO>(stringFromBytes);
            return result;
        }

        public static int prepare_SetOnlineInvoice(byte[] Buffer, int MaxSize, ref ST_ONLINE_INVIOCE_INFO pStInvoiceInfo)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStInvoiceInfo));
            return Json_prepare_SetOnlineInvoice(Buffer, MaxSize, bytesFromString);
        }

        public static int prepare_SetTaxFreeInfo(byte[] Buffer, int MaxSize, ref ST_TAXFREE_INFO pStTaxFreeInfo)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStTaxFreeInfo));
            return Json_prepare_SetTaxFreeInfo(Buffer, MaxSize, bytesFromString);
        }

        public static uint parse_FiscalPrinter(ref ST_MULTIPLE_RETURN_CODE[] pStReturnCodes, ref ushort pNumberOfreturnCodes, uint RecvMsgId, byte[] RecvFullBuffer, ushort RecvFullLen, ref ST_TICKET pstTicket, int MaxNumberOfReturnCode, int MaxReturnCodeDataLen)
        {
            byte[] array = new byte[200000];
            byte[] array2 = new byte[200000];
            uint num = Json_parse_FiscalPrinter(array2, array2.Length, ref pNumberOfreturnCodes, RecvMsgId, RecvFullBuffer, RecvFullLen, array, array.Length, MaxNumberOfReturnCode, MaxReturnCodeDataLen);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array2);
                pStReturnCodes = JsonConvert.DeserializeObject<ST_MULTIPLE_RETURN_CODE[]>(stringFromBytes);
                stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pstTicket = JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes);
            }
            return num;
        }

        public static uint parse_GetEcr(ref ST_MULTIPLE_RETURN_CODE[] pStReturnCodes, ref int pNumberOfreturnCodes, uint RecvMsgId, byte[] RecvFullBuffer, ushort RecvFullLen, int MaxNumberOfReturnCode, int MaxReturnCodeDataLen)
        {
            JsonConvert.SerializeObject(pStReturnCodes);
            byte[] array = new byte[200000];
            uint num = Json_parse_GetEcr(array, array.Length, ref pNumberOfreturnCodes, RecvMsgId, RecvFullBuffer, RecvFullLen, MaxNumberOfReturnCode, MaxReturnCodeDataLen);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStReturnCodes = JsonConvert.DeserializeObject<ST_MULTIPLE_RETURN_CODE[]>(stringFromBytes);
            }
            return num;
        }

        public static int prepare_ReversePayment(byte[] Buffer, int MaxSize, ref ST_PAYMENT_REQUEST pStPaymentRequest, ushort NumberOfPaymentRequests)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStPaymentRequest));
            byte[] array = new byte[200000];
            int num = Json_prepare_ReversePayment(Buffer, MaxSize, bytesFromString, array, array.Length, NumberOfPaymentRequests);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStPaymentRequest = JsonConvert.DeserializeObject<ST_PAYMENT_REQUEST>(stringFromBytes);
            }
            return num;
        }

        public static int prepare_Date(byte[] Buffer, int MaxSize, uint TagId, byte[] Title, byte[] Text, byte[] Mask, ref ST_DATE_TIME pStValue, int TimeoutInMiliseconds)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStValue));
            byte[] array = new byte[200000];
            int num = Json_prepare_Date(Buffer, MaxSize, TagId, Title, Text, Mask, bytesFromString, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStValue = JsonConvert.DeserializeObject<ST_DATE_TIME>(stringFromBytes);
            }
            return num;
        }

        public static int prepare_Condition(byte[] Buffer, int MaxSize, ref ST_CONDITIONAL_IF pStCondition)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStCondition));
            byte[] array = new byte[200000];
            int num = Json_prepare_Condition(Buffer, MaxSize, bytesFromString, array, array.Length);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStCondition = JsonConvert.DeserializeObject<ST_CONDITIONAL_IF>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_ReversePayment(uint hInt, ulong hTrx, ref ST_PAYMENT_REQUEST pStPaymentRequest, short NumberOfPaymentRequests, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStPaymentRequest));
            byte[] array2 = new byte[200000];
            uint num = Json_FP3_ReversePayment(hInt, hTrx, bytesFromString, array2, array2.Length, NumberOfPaymentRequests, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array2);
                pStPaymentRequest = JsonConvert.DeserializeObject<ST_PAYMENT_REQUEST>(stringFromBytes);
                stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pstTicket = JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes);
            }
            return num;
        }

        public static int prepare_PrintUserMessage(byte[] Buffer, int MaxSize, ref ST_USER_MESSAGE[] pStUserMessage, ushort NumberOfMessage)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStUserMessage));
            byte[] array = new byte[200000];
            int num = Json_prepare_PrintUserMessage(Buffer, MaxSize, bytesFromString, array, array.Length, NumberOfMessage);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStUserMessage = JsonConvert.DeserializeObject<ST_USER_MESSAGE[]>(stringFromBytes);
            }
            return num;
        }

        public static int prepare_PrintUserMessage_Ex(byte[] Buffer, int MaxSize, ref ST_USER_MESSAGE[] pStUserMessage, ushort NumberOfMessage)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStUserMessage));
            byte[] array = new byte[200000];
            int num = Json_prepare_PrintUserMessage_Ex(Buffer, MaxSize, bytesFromString, array, array.Length, NumberOfMessage);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStUserMessage = JsonConvert.DeserializeObject<ST_USER_MESSAGE[]>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_JumpToECR(uint hInt, ulong hTrx, ulong JumpFlags, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            uint num = Json_FP3_JumpToECR(hInt, hTrx, JumpFlags, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint FP3_MultipleCommand(uint hInt, ref ulong hTrx, ref ST_MULTIPLE_RETURN_CODE[] pReturnCodes, ref ushort IndexOfReturnCodes, byte[] SendBuffer, ushort SendBufferLen, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] array2 = new byte[200000];
            uint num = Json_FP3_MultipleCommand(hInt, ref hTrx, array2, array2.Length, ref IndexOfReturnCodes, SendBuffer, SendBufferLen, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array2);
                pReturnCodes = JsonConvert.DeserializeObject<ST_MULTIPLE_RETURN_CODE[]>(stringFromBytes);
                stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pstTicket = JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_SetTaxFree(uint hInt, ulong hTrx, int szFlag, string szName, string szSurname, string szIdentificationNo, string szCity, string szCountry, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szName);
            byte[] bytesFromString2 = GMP_Tools.GetBytesFromString(szSurname);
            byte[] bytesFromString3 = GMP_Tools.GetBytesFromString(szIdentificationNo);
            byte[] bytesFromString4 = GMP_Tools.GetBytesFromString(szCity);
            byte[] bytesFromString5 = GMP_Tools.GetBytesFromString(szCountry);
            uint num = Json_FP3_SetTaxFree(hInt, hTrx, szFlag, bytesFromString, bytesFromString2, bytesFromString3, bytesFromString4, bytesFromString5, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint FP3_SetParkingTicket(uint hInt, ulong hTrx, string szCarIdentification, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szCarIdentification);
            uint num = Json_FP3_SetParkingTicket(hInt, hTrx, bytesFromString, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint FP3_SetTaxFreeRefundAmount(uint hInt, ulong hTrx, int RefundAmount, ushort RefundAmountCurrency, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            uint num = Json_FP3_SetTaxFreeRefundAmount(hInt, hTrx, RefundAmount, RefundAmountCurrency, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint FP3_LoyaltyCustomerQuery(uint hInt, ulong hTrx, ref ST_LOYALTY_SERVICE_REQ pstLoyaltyServiceReq, ref ST_TICKET pstTicket, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pstLoyaltyServiceReq));
            byte[] array2 = new byte[200000];
            uint num = Json_FP3_LoyaltyCustomerQuery(hInt, hTrx, bytesFromString, array2, array2.Length, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array2);
                pstLoyaltyServiceReq = JsonConvert.DeserializeObject<ST_LOYALTY_SERVICE_REQ>(stringFromBytes);
                stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                ST_TICKET sT_TICKET = new ST_TICKET();
                MergeItemStruct(StTicketSrc: JsonConvert.DeserializeObject<ST_TICKET>(stringFromBytes), StTicketDest: pstTicket);
            }
            return num;
        }

        public static uint FP3_FunctionChangeTicketHeader(uint hInt, string szSupervisorPassword, ref ushort pNumberOfSpaceTotal, ref ushort pNumberOfSpaceUsed, ref ST_TICKET_HEADER pStTicketHeader, int TimeoutInMiliseconds)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStTicketHeader));
            byte[] array = new byte[200000];
            byte[] bytesFromString2 = GMP_Tools.GetBytesFromString(szSupervisorPassword);
            uint num = Json_FP3_FunctionChangeTicketHeader(hInt, bytesFromString2, ref pNumberOfSpaceTotal, ref pNumberOfSpaceUsed, bytesFromString, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStTicketHeader = JsonConvert.DeserializeObject<ST_TICKET_HEADER>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_GetTicketHeader(uint hInt, ushort IndexOfHeader, ref ST_TICKET_HEADER pStTicketHeader, ref ushort pNumberOfSpaceTotal, int TimeoutInMiliseconds)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStTicketHeader));
            byte[] array = new byte[200000];
            uint num = Json_FP3_GetTicketHeader(hInt, IndexOfHeader, bytesFromString, array, array.Length, ref pNumberOfSpaceTotal, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStTicketHeader = JsonConvert.DeserializeObject<ST_TICKET_HEADER>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_GetOnlineInvoiceInfo(uint hInt, byte[] szOnlineInvoiceInfo, int OnlineInvoiceIdLen, ref ST_ONLINE_INVIOCE_INFO pStOnlineInvoiceInfo, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            uint num = Json_FP3_GetOnlineInvoiceInfo(hInt, szOnlineInvoiceInfo, OnlineInvoiceIdLen, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStOnlineInvoiceInfo = JsonConvert.DeserializeObject<ST_ONLINE_INVIOCE_INFO>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_Database_QueryColomnCaptions(uint hInt, ref ST_DATABASE_RESULT pStDatabaseResult)
        {
            byte[] array = new byte[200000];
            uint num = Json_FP3_Database_QueryColomnCaptions(hInt, array, array.Length);
            if (num == 0 || num == 101 || num == 100)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStDatabaseResult = JsonConvert.DeserializeObject<ST_DATABASE_RESULT>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_Database_QueryReadLine(uint hInt, ushort NumberOfLinesRequested, ushort NumberOfColomnsRequested, ref ST_DATABASE_RESULT pStDatabaseResult)
        {
            byte[] array = new byte[200000];
            uint num = Json_FP3_Database_QueryReadLine(hInt, NumberOfLinesRequested, NumberOfColomnsRequested, array, array.Length);
            if (num == 0 || num == 101 || num == 100)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStDatabaseResult = JsonConvert.DeserializeObject<ST_DATABASE_RESULT>(stringFromBytes);
            }
            return num;
        }

        public static void FP3_Database_FreeStructure(uint hInt, ref ST_DATABASE_RESULT pStDatabaseResult)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(pStDatabaseResult));
            byte[] array = new byte[200000];
            Json_FP3_Database_FreeStructure(hInt, bytesFromString, array, array.Length);
            string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
            pStDatabaseResult = JsonConvert.DeserializeObject<ST_DATABASE_RESULT>(stringFromBytes);
        }

        public static uint FP3_Database_Execute(uint hInt, string szSqlWord, ref ST_DATABASE_RESULT pStDatabaseResult)
        {
            byte[] array = new byte[200000];
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szSqlWord);
            uint num = Json_FP3_Database_Execute(hInt, bytesFromString, array, array.Length);
            if (num == 0 || num == 101 || num == 100)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                pStDatabaseResult = JsonConvert.DeserializeObject<ST_DATABASE_RESULT>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_GetPLU(uint hInt, string szBarcode, ref ST_PLU_RECORD StPluRecord, ref ST_PLU_GROUP_RECORD[] StPluGroupRecord, int MaxNumberOfGroupRecords, int TimeoutInMiliseconds)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(StPluRecord));
            byte[] array = new byte[200000];
            byte[] bytesFromString2 = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(StPluGroupRecord));
            byte[] array2 = new byte[200000];
            byte[] bytesFromString3 = GMP_Tools.GetBytesFromString(szBarcode);
            uint num = Json_FP3_GetPLU(hInt, bytesFromString3, bytesFromString, array, array.Length, bytesFromString2, array2, array2.Length, MaxNumberOfGroupRecords, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                StPluRecord = JsonConvert.DeserializeObject<ST_PLU_RECORD>(stringFromBytes);
                stringFromBytes = GMP_Tools.GetStringFromBytes(array2);
                StPluGroupRecord = JsonConvert.DeserializeObject<ST_PLU_GROUP_RECORD[]>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_GetVasApplicationInfo(uint hInt, ref byte pNumberOfTotalRecords, ref byte pNumberOfTotalRecordsReceived, ref ST_PAYMENT_APPLICATION_INFO[] StPaymentAppInfo, ushort vasType)
        {
            byte[] array = new byte[200000];
            uint num = Json_FP3_GetVasApplicationInfo(hInt, ref pNumberOfTotalRecords, ref pNumberOfTotalRecordsReceived, array, array.Length, vasType);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                StPaymentAppInfo = JsonConvert.DeserializeObject<ST_PAYMENT_APPLICATION_INFO[]>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_GetVasLoyaltyServiceInfo(uint hInt, ref byte pNumberOfTotalRecords, ref byte pNumberOfTotalRecordsReceived, ref ST_LOYALTY_SERVICE_INFO[] StLoyaltyAppInfo, ushort VasAppId)
        {
            byte[] array = new byte[200000];
            uint num = Json_FP3_GetVasLoyaltyServiceInfo(hInt, ref pNumberOfTotalRecords, ref pNumberOfTotalRecordsReceived, array, array.Length, VasAppId);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                StLoyaltyAppInfo = JsonConvert.DeserializeObject<ST_LOYALTY_SERVICE_INFO[]>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_FunctionEkuSeek(uint hInt, ref ST_EKU_APPINF StEKUAppInfo, int TimeoutInMiliseconds)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(StEKUAppInfo));
            byte[] array = new byte[200000];
            uint num = Json_FP3_FunctionEkuSeek(hInt, bytesFromString, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                StEKUAppInfo = JsonConvert.DeserializeObject<ST_EKU_APPINF>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_FileSystem_DirListFiles(uint hInt, string szDirName, ref ST_FILE[] StFile, short MaxNumberOfFiles, ref short pNumberOfFiles)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(StFile));
            byte[] array = new byte[200000];
            byte[] bytesFromString2 = GMP_Tools.GetBytesFromString(szDirName);
            uint num = Json_FP3_FileSystem_DirListFiles(hInt, bytesFromString2, bytesFromString, array, array.Length, MaxNumberOfFiles, ref pNumberOfFiles);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                StFile = JsonConvert.DeserializeObject<ST_FILE[]>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_FunctionEkuReadHeader(uint hInt, short Index, ref ST_EKU_HEADER StEkuHeader, int TimeoutInMiliseconds)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(StEkuHeader));
            byte[] array = new byte[200000];
            uint num = Json_FP3_FunctionEkuReadHeader(hInt, Index, bytesFromString, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                StEkuHeader = JsonConvert.DeserializeObject<ST_EKU_HEADER>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_FunctionEkuReadData(uint hInt, ref ushort pEkuDataBufferReceivedLen, ref ST_EKU_APPINF StEKUAppInfo, int TimeoutInMiliseconds)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(StEKUAppInfo));
            byte[] array = new byte[200000];
            uint num = Json_FP3_FunctionEkuReadData(hInt, ref pEkuDataBufferReceivedLen, bytesFromString, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                StEKUAppInfo = JsonConvert.DeserializeObject<ST_EKU_APPINF>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_FunctionEkuReadInfo(uint hInt, ushort EkuAccessFunction, ref ST_EKU_MODULE_INFO StEkuModuleInfo, int TimeoutInMiliseconds)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(StEkuModuleInfo));
            byte[] array = new byte[200000];
            uint num = Json_FP3_FunctionEkuReadInfo(hInt, EkuAccessFunction, bytesFromString, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                StEkuModuleInfo = JsonConvert.DeserializeObject<ST_EKU_MODULE_INFO>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_FunctionModuleReadInfo(uint hInt, int AccessFunction, ref ST_MODULE_USAGE_INFO StModuleUsageInfo, int TimeoutInMiliseconds)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(StModuleUsageInfo));
            byte[] array = new byte[200000];
            uint num = Json_FP3_FunctionModuleReadInfo(hInt, AccessFunction, bytesFromString, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                StModuleUsageInfo = JsonConvert.DeserializeObject<ST_MODULE_USAGE_INFO>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_FunctionBankingRefund(uint hInt, ref ST_PAYMENT_REQUEST StReversePayment, int TimeoutInMiliseconds)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(StReversePayment));
            byte[] array = new byte[200000];
            uint num = Json_FP3_FunctionBankingRefund(hInt, bytesFromString, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                StReversePayment = JsonConvert.DeserializeObject<ST_PAYMENT_REQUEST>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_FunctionBankingRefundExt(uint hInt, ref ST_PAYMENT_REQUEST StReversePayment, ref ST_PAYMENT_RESPONSE stReverseResponse, int TimeoutInMiliseconds)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(StReversePayment));
            byte[] array = new byte[200000];
            byte[] array2 = new byte[200000];
            uint num = Json_FP3_FunctionBankingRefundExt(hInt, bytesFromString, array, array.Length, array2, array2.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                StReversePayment = JsonConvert.DeserializeObject<ST_PAYMENT_REQUEST>(stringFromBytes);
            }
            string stringFromBytes2 = GMP_Tools.GetStringFromBytes(array2);
            stReverseResponse = JsonConvert.DeserializeObject<ST_PAYMENT_RESPONSE>(stringFromBytes2);
            return num;
        }

        public static uint FP3_FunctionBankingBatch(uint hInt, ushort BkmId, ref ushort pNumberOfBankResponse, ref ST_MULTIPLE_BANK_RESPONSE[] StMultipleBankResponse, int TimeoutInMiliseconds)
        {
            byte[] array = new byte[200000];
            uint num = Json_FP3_FunctionBankingBatch(hInt, BkmId, ref pNumberOfBankResponse, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                StMultipleBankResponse = JsonConvert.DeserializeObject<ST_MULTIPLE_BANK_RESPONSE[]>(stringFromBytes);
            }
            return num;
        }

        public static uint FP3_FunctionTransactionInquiry(uint hInt, ref ST_TRANS_INQUIRY stTransInquiry, int TimeoutInMiliseconds)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(JsonConvert.SerializeObject(stTransInquiry));
            byte[] array = new byte[50000];
            uint num = Json_FP3_FunctionTransactionInquiry(hInt, bytesFromString, array, array.Length, TimeoutInMiliseconds);
            if (num == 0)
            {
                string stringFromBytes = GMP_Tools.GetStringFromBytes(array);
                stTransInquiry = JsonConvert.DeserializeObject<ST_TRANS_INQUIRY>(stringFromBytes);
            }
            return num;
        }
    }

}
