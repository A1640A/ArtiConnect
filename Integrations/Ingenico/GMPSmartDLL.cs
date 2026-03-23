using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    internal class GMPSmartDLL
    {
        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint GenerateUniqueID(byte[] szPath);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_RemoveInterfaceByID(string InterfaceId);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint SetXmlFilePath(string szPath);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint GetXmlFilePath(byte[] szPath);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_FunctionCreateUniqueId(uint hInt, byte[] UniqueId, int timeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_GetInterfaceHandleList(uint[] phInt, uint Max);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_GetInterfaceID(uint hInt, byte[] szID, uint Max);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_GetInterfaceHandleByID(ref uint phInt, byte[] szID);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_FunctionCashierLogin")]
        public static extern uint FP3_FunctionCashierLogin_WE(uint hInt, int CashierIndex, byte[] szCashierPassword);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_FunctionAddCashier")]
        public static extern uint FP3_FunctionAddCashier_WE(uint hInt, ushort CashierIndex, byte[] szCashierName, byte[] szCashierPassword, byte[] szSupervisorPassword);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "SetIniFilePath")]
        public static extern void SetIniFilePath_WE(byte[] szPath);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_Database_Open")]
        public static extern uint FP3_Database_Open_WE(uint hInt, byte[] szName);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_FileSystem_DirChange")]
        public static extern uint FP3_FileSystem_DirChange_WE(uint hInt, byte[] szDirName);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_FileSystem_FileRead")]
        public static extern uint FP3_FileSystem_FileRead_WE(uint hInt, byte[] szFileName, int Offset, int Whence, byte[] Buffer, int MaxLen, ref short pReadLen);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_SetPluDatabaseInfo")]
        public static extern uint FP3_SetPluDatabaseInfo_WE(uint hInt, byte[] szPluDbName, short szPluDbType);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "Database_Query")]
        public static extern uint Database_Query_WE(byte[] szSqlWord, ref ushort pNumberOfColomns);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_Database_Query")]
        public static extern uint FP3_Database_Query_WE(uint hInt, byte[] szSqlWord, ref ushort pNumberOfColomns);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "prepare_SetTaxFree")]
        public static extern int prepare_SetTaxFree_WE(byte[] Buffer, int MaxSize, int Flag, byte[] szName, byte[] szSurname, byte[] szIdentificationNo, byte[] szCity, byte[] szCountry);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_KasaAvans(byte[] Buffer, int MaxSize, int Amount, byte[] pCustomerName, byte[] pTckn, byte[] pVkn);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern byte[] GetTagName(uint Tag);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void GetErrorMessage(uint ErrorCode, byte[] Buffer);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void GetErrorTurkishDescription(uint ErrorCode, byte[] Buffer);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int FP3_DisplayPaymentSummary(uint hInt, ulong hTrx, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int FP3_FunctionCashierLogout(uint hInt);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern ulong FiscalPrinter_GetHandle();

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void FiscalPrinter_ResetHandle();

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_TicketHeader(uint hInt, ulong hTrx, TTicketType TicketType, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_PrintTotalsAndPayments(uint hInt, ulong hTrx, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_PrintBeforeMF(uint hInt, ulong hTrx, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_PrintMF(uint hInt, ulong hTrx, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_Ping(uint hInt, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_Start(uint hInt, ref ulong hTrx, byte IsBackground, byte[] pUniqueId, int LengthOfUniqueId, byte[] pUniqueIdSign, int LengthOfUniqueIdSign, byte[] pUserData, int LengthOfUserData, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_Close(uint hInt, ulong hTrx, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_FunctionEcrParameter(uint hInt, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_FunctionBankingParameter(uint hInt, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_FunctionOpenDrawer(uint hInt);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_GetCurrentFiscalCounters(uint hInt, ref ushort pZNo, ref ushort pFNo, ref ushort pEKUNo);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint Database_GetHandle();

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_Database_GetHandle(uint hInt);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_Database_Close(uint hInt);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_Database_QueryReset(uint hInt);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_Database_QueryFinish(uint hInt);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_FunctionEkuPing(uint hInt, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_GetTlvData(uint hInt, int Tag, byte[] pData, short MaxBufferLen, ref short pDataLen);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_FileSystem_FileRemove(uint hInt, byte[] pFileName);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_FileSystem_FileRename(uint hInt, byte[] pFileNameOld, byte[] pFileNameNew);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_FileSystem_FileWrite(uint hInt, byte[] pFileName, int Offset, int Whence, byte[] Buffer, short Len);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint GMP_GetDllVersion(byte[] pVersion);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_GetPluDatabaseInfo(uint hInt, byte[] pPluDbName, ref short pPluDbType, ref uint pPluDbSize, ref uint pPluDbGrupsLastModified, ref uint pPluDbItemsLastModified);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern ushort gmpReadTLVlen_HL(ref ushort pLen, byte[] pPtr, ushort PtrLen);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "gmpReadTLVtag_HL")]
        public static extern ushort gmpReadTLVtag(ref uint pTag, byte[] pPtr, ushort PtrLen);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern ushort gmpTlvSetLen(byte[] pMsg, ushort TlvLen);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern ushort gmpSetTLV_HL(byte[] pMsg, int pMsgLen, uint Tag, byte[] pdata, ushort dataLen);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern ushort gmpSetTLVbcd(byte[] pMsg, uint Tag, uint Data, ushort BcdLen);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int gmpSearchTLV(uint Tag, int Ocurience, byte[] RecvBuffer, ushort RecvBufferLen, byte[] pData, ushort DataMaxLen);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int gmpSearchTLVbcd_8(uint Tag, int Ocurience, byte[] RecvBuffer, ushort RecvBufferLen, byte[] pData, byte BcdLen);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int gmpSearchTLVbcd_16(uint Tag, int Ocurience, byte[] RecvBuffer, ushort RecvBufferLen, byte[] pData, byte BcdLen);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int gmpSearchTLVbcd_32(uint Tag, int Ocurience, byte[] RecvBuffer, ushort RecvBufferLen, byte[] pData, byte BcdLen);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int gmpSearchTLVbcd_64(uint Tag, int Ocurience, byte[] RecvBuffer, ushort RecvBufferLen, byte[] pData, byte BcdLen);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_Start(byte[] Buffer, int MaxSize, byte[] pUniqueId, int LengthOfUniqueId, byte[] pUniqueIdSign, int LengthOfUniqueIdSign, byte[] pUserData, int LengthOfUserData);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_TicketHeader(byte[] Buffer, int MaxSize, TTicketType TicketType);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_Close(byte[] Buffer, int MaxSize);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_OptionFlags(byte[] Buffer, int MaxSize, ulong FlagsToBeSet, ulong FlagsToBeClear);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_PrintBeforeMF(byte[] Buffer, int MaxSize);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_PrintMF(byte[] Buffer, int MaxSize);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_SetParkingTicket(byte[] Buffer, int MaxSize, byte[] pCarIdentification);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_PrintTotalsAndPayments(byte[] Buffer, int MaxSize);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Json_prepare_PrintUserMessage(byte[] Buffer, int MaxSize, byte[] szJsonUserMessage, byte[] szJsonUserMessage_Out, int JsonUserMessageLen_Out, ushort NumberOfMessage);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Json_prepare_PrintUserMessage_Ex(byte[] Buffer, int MaxSize, byte[] szJsonUserMessage, byte[] szJsonUserMessage_Out, int JsonUserMessageLen_Out, ushort NumberOfMessage);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_VoidItem(byte[] Buffer, int MaxSize, ushort Index, long ItemCount, byte ItemCountPrecision);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_Matrahsiz(byte[] Buffer, int MaxSize, byte[] pTckNo, long MatrahsizAmount);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_Pretotal(byte[] Buffer, int MaxSize);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_DisplayPaymentSummary(byte[] Buffer, int MaxSize);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_Plus(byte[] Buffer, int MaxSize, int Amount, ushort IndexOfItem);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_Minus(byte[] Buffer, int MaxSize, int Amount, ushort IndexOfItem);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_Inc(byte[] Buffer, int MaxSize, byte Rate, ushort IndexOfItem);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_Dec(byte[] Buffer, int MaxSize, byte Rate, ushort IndexOfItem);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_VoidPayment(byte[] Buffer, int MaxSize, ushort Index);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_VoidAll(byte[] Buffer, int MaxSize);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_JumpToECR(byte[] Buffer, int MaxSize, ulong JumpFlags);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_SetTaxFreeRefundAmount(byte[] Buffer, int MaxSize, int RefundAmount, ushort RefundAmountCurrency);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_Text(byte[] Buffer, int MaxSize, uint TagId, byte[] pTitle, byte[] pText, byte[] pMask, byte[] pValue, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_Amount(byte[] Buffer, int MaxSize, uint TagId, byte[] pTitle, byte[] pText, byte[] pMask, byte[] pValue, int MaxLenOfValue, byte[] pSymbol, byte Align, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_Password(byte[] Buffer, int MaxSize, uint TagId, byte[] pTitle, byte[] pText, byte[] pMask, byte[] pValue, ushort MaxLenOfValue, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_MsgBox(byte[] Buffer, int MaxSize, uint TagId, byte[] pTitle, byte[] pText, byte Icon, byte Button, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_SetuApplicationFunction(uint hInt, byte[] pUApplicationName, uint UApplicationId, byte[] pFunctionName, uint FunctionId, uint FunctionVersion, uint FunctionFlags, byte[] pCommandBuffer, uint CommandLen, string szSupervisorPassword);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_GetDialogInput_Text(uint hInt, ref uint pGL_Dialog_retcode, uint TagId, byte[] pTitle, byte[] pText, byte[] pMask, byte[] pValue, int MaxLenOfValue, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_GetDialogInput_Date(uint hInt, ref uint pGL_Dialog_retcode, uint TagId, byte[] pTitle, byte[] pText, byte[] pMask, ref ST_DATE_TIME pValue, int MaxLenOfValue, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_GetDialogInput_Amount(uint hInt, ref uint pGL_Dialog_retcode, uint TagId, byte[] pTitle, byte[] pText, byte[] pMask, byte[] pValue, int MaxLenOfValue, byte[] pSymbol, byte Align, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_GetDialogInput_MsgBox(uint hInt, ref uint pGL_Dialog_retcode, uint TagId, byte[] pTitle, byte[] pText, byte Icon, byte Button, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_GetDialogInput_Password(uint hInt, ref uint pGL_Dialog_retcode, uint TagId, byte[] pTitle, byte[] pText, byte[] pMask, byte[] pValue, int MaxLenOfValue, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_OptionFlags(uint hInt, ulong hTrx, ref ulong pFlagsActive, ulong FlagsToBeSet, ulong FlagsToBeClear, int TimeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_SetTlvData(uint hInt, uint Tag, byte[] pData, ushort Len);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_SendFrontStationPrint(uint hInt, ulong hTrx, byte[] pSendBuffer, short SendLen, byte[] pReceiveBuffer, ref ushort ReceiveLen, int timeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_SendFrontStationPrintEx(uint hInt, ulong hTrx, byte[] pSendBuffer, short SendLen, byte[] pReceiveBuffer, ref ushort ReceiveLen, int ReceiveTimeOut, int PrinterTimeOut);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FiscalPrinter_SendFrontStationPrint(byte[] pSendBuffer, short SendLen, byte[] pReceiveBuffer, ref ushort ReceiveLen, int timeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_IsGmpPairingDone(uint hInt);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_FunctionLoadBackGroundHandleToFront(uint hInt, ulong hTrx, int timeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_GetMerchantSlip(uint hInt, ulong hTrx, int odemeIndex, uint fontSize, byte[] slip, ref int slipLen, int timeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_GetUserData(uint hInt, ulong hTrx, byte[] userData, ref int dataLen, int timeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_SendUserData(uint hInt, ulong hTrx, byte[] userData, int dataLen, int timeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_SetDrawerState(uint hInt, int state, int timeoutInMiliseconds);

        [DllImport("GmpSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int prepare_SendUserData(byte[] buffer, int MaxSize, byte[] userData, int userDataLen);

        [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sqlite3_open(string szFilename, out IntPtr pDb);

        [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sqlite3_close(IntPtr pDb);

        [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sqlite3_prepare_v2(IntPtr pDb, string szSql, int nByte, out IntPtr ppStmpt, IntPtr pzTail);

        [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sqlite3_step(IntPtr stmHandle);

        [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sqlite3_finalize(IntPtr stmHandle);

        [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern string sqlite3_errmsg(IntPtr db);

        [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sqlite3_column_count(IntPtr stmHandle);

        [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr sqlite3_column_origin_name(IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sqlite3_column_type(IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sqlite3_column_int(IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr sqlite3_column_text(IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double sqlite3_column_double(IntPtr stmHandle, int iCol);

        public static uint FP3_FunctionCashierLogin(uint hInt, int CashierIndex, string szCashierPassword)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szCashierPassword);
            return FP3_FunctionCashierLogin_WE(hInt, CashierIndex, bytesFromString);
        }

        public static uint FP3_FunctionAddCashier(uint hInt, ushort CashierIndex, string szCashierName, string szCashierPassword, string szSupervisorPassword)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szCashierName);
            byte[] bytesFromString2 = GMP_Tools.GetBytesFromString(szCashierPassword);
            byte[] bytesFromString3 = GMP_Tools.GetBytesFromString(szSupervisorPassword);
            return FP3_FunctionAddCashier_WE(hInt, CashierIndex, bytesFromString, bytesFromString2, bytesFromString3);
        }

        public static void SetIniFilePath(string szPath)
        {
            SetIniFilePath_WE(GMP_Tools.GetBytesFromString(szPath));
        }

        public static uint FP3_Database_Open(uint hInt, string szPath)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szPath);
            return FP3_Database_Open_WE(hInt, bytesFromString);
        }

        public static uint FP3_FileSystem_DirChange(uint hInt, string szPath)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szPath);
            return FP3_FileSystem_DirChange_WE(hInt, bytesFromString);
        }

        public static uint FP3_FileSystem_FileRead(uint hInt, string szFileName, int Offset, int Whence, byte[] Buffer, int MaxLen, ref short pReadLen)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szFileName);
            return FP3_FileSystem_FileRead_WE(hInt, bytesFromString, Offset, Whence, Buffer, MaxLen, ref pReadLen);
        }

        public static uint FP3_SetPluDatabaseInfo(uint hInt, string szPluDbName, short szPluDbType)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szPluDbName);
            return FP3_SetPluDatabaseInfo_WE(hInt, bytesFromString, szPluDbType);
        }

        public static uint Database_Query(string szSqlWord, ref ushort pNumberOfColomns)
        {
            return Database_Query_WE(GMP_Tools.GetBytesFromString(szSqlWord), ref pNumberOfColomns);
        }

        public static uint FP3_Database_Query(uint hInt, string szSqlWord, ref ushort pNumberOfColomns)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szSqlWord);
            return FP3_Database_Query_WE(hInt, bytesFromString, ref pNumberOfColomns);
        }

        public static int prepare_SetTaxFree(byte[] Buffer, int MaxSize, int Flag, string szName, string szSurname, string szIdentificationNo, string szCity, string szCountry)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szName);
            byte[] bytesFromString2 = GMP_Tools.GetBytesFromString(szSurname);
            byte[] bytesFromString3 = GMP_Tools.GetBytesFromString(szIdentificationNo);
            byte[] bytesFromString4 = GMP_Tools.GetBytesFromString(szCity);
            byte[] bytesFromString5 = GMP_Tools.GetBytesFromString(szCountry);
            return prepare_SetTaxFree_WE(Buffer, MaxSize, Flag, bytesFromString, bytesFromString2, bytesFromString3, bytesFromString4, bytesFromString5);
        }

        public static int prepare_KasaAvans(byte[] Buffer, int MaxSize, int Amount, string szCustomerName, string szTckn, string szVkn)
        {
            byte[] bytesFromString = GMP_Tools.GetBytesFromString(szCustomerName);
            byte[] bytesFromString2 = GMP_Tools.GetBytesFromString(szTckn);
            byte[] bytesFromString3 = GMP_Tools.GetBytesFromString(szVkn);
            return prepare_KasaAvans(Buffer, MaxSize, Amount, bytesFromString, bytesFromString2, bytesFromString3);
        }
    }

}
