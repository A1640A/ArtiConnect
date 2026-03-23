using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Hugin
{

    public enum COMMTYPE
    {
        RS232 = 1,
        TCPIP = 2
    }
    //[StructLayout(LayoutKind.Sequential)]
    public struct CommMedia
    {
        public int iSelectedMedia;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string strSerialPort;
        public int iServerPort;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string strServerAddr;

    }
    enum EM_TranStatus
    {
        Tran_Doc_Closed = 1, //means document is closed
        Tran_Doc_Voided = 2, //means document is voided
        Tran_Doc_Opened = 3, //means document is opened
        Tran_Doc_Wait_Close = 4, //means do payment is done, ecr waits for close document cmd
        Tran_Doc_Lost = 5, //means document could not be found
        Tran_Doc_Meal_Card = 6, //means document is Meal-Card
        Tran_Doc_Car_Park = 7, //means document is Car-Park
        Tran_Doc_Advance_IC_CC = 8, //means document is opened as Advance or InvoiceCollec or CurrencyCollec
        Tran_Doc_Payment_Remain_Amount = 9, //means document Remain Amount never pay.
    };
    public enum Tags
    {
        NONE = 0,
        msgREQ_GMP3Echo = 1,
        msgRSP_GMP3Echo = 2,
        msgREQ_GMP3Pair = 3,
        msgRSP_GMP3Pair = 4,
        msgREQ_GMP3Init = 5,
        msgRSP_GMP3Init = 6,
        msgREQ_GMP3KeyReq = 7,
        msgRSP_GMP3KeyReq = 8,
        msgREQ_GMP3Close = 9,
        msgRSP_GMP3Close = 10,
        msgREQ_Ping = 11,
        msgRSP_Ping = 12,
        msgREQ_SetHeader = 13,
        msgRSP_SetHeader = 14,
        msgREQ_OpenDocument = 15,
        msgRSP_OpenDocument = 16,
        msgREQ_CloseDocument = 17,
        msgRSP_CloseDocument = 18,
        msgREQ_DoTran = 19,
        msgRSP_DoTran = 20,
        msgREQ_GetReceiptTot = 21,
        msgRSP_GetReceiptTot = 22,
        msgREQ_DoPayment = 23,
        msgRSP_DoPayment = 24,
        msgREQ_InfoInquiry = 25,
        msgRSP_InfoInquiry = 26,
        msgREQ_SavePayment = 27,
        msgRSP_SavePayment = 28,
        msgREQ_FreePrint = 29,
        msgRSP_FreePrint = 30,
        msgREQ_PrintReport = 31,
        msgRSP_PrintReport = 32,
        msgREQ_GetExchange = 33,
        msgRSP_GetExchange = 34,
        msgREQ_SetExchange = 35,
        msgRSP_SetExchange = 36,
        msgREQ_GetVatRates = 37,
        msgRSP_GetVatRates = 38,
        msgREQ_ReceiptInq = 39,
        msgRSP_ReceiptInq = 40,
        msgREQ_CashierLogin = 41,
        msgRSP_CashierLogin = 42,
        msgREQ_ChangeEcrMode = 43,
        msgRSP_ChangeEcrMode = 44,
        msgREQ_EcrConfig = 45,
        msgRSP_EcrConfig = 46,
        msgREQ_RestartApp = 47,
        msgRSP_RestartApp = 48,
        msgREQ_SetPaperStatus = 49,
        msgRSP_SetPaperStatus = 50,
        msgREQ_OpenDrawer = 51,
        msgRSP_OpenDrawer = 52,
        msgREQ_GetDrawerStat = 53,
        msgRSP_GetDrawerStat = 54,
        msgREQ_SetPLU = 55,
        msgRSP_SetPLU = 56,
        msgREQ_BatchTran = 57,
        msgRSP_BatchTran = 58,
        msgREQ_GetBankList = 59,
        msgRSP_GetBankList = 60,
        msgREQ_GetDepList = 61,
        msgRSP_GetDepList = 62,
        msgREQ_SetDepList = 63,
        msgRSP_SetDepList = 64,
        msgREQ_GetPLUList = 65,
        msgRSP_GetPLUList = 66,
        msgREQ_SetGroup = 67,
        msgRSP_SetGroup = 68,
        msgREQ_PowerOFF = 69,
        msgRSP_PowerOFF = 70,
        msgREQ_GetUniqueId = 71,
        msgRSP_GetUniqueId = 72,
        msgREQ_AccountEnd = 73,
        msgRSP_AccountEnd = 74,
        msgREQ_AccountInfo = 75,
        msgRSP_AccountInfo = 76,

        msgREQ_CardInfo = 77,
        msgRSP_CardInfo = 78,
        msgREQ_GetDailyTotals = 79,
        msgRSP_GetDailyTotals = 80,
        msgREQ_CarPark = 81,
        msgRSP_CarPark = 82,
        msgREQ_CashierLogout = 83,
        msgRSP_CashierLogout = 84,
        msgREQ_GetMealAppList = 85,
        msgRSP_GetMealAppList = 86,
        msgREQ_Drawer = 87,
        msgRSP_Drawer = 88,
        msgREQ_HeartBeat = 89,
        msgRSP_HeartBeat = 90,
        msgREQ_NonTaxItem = 91,
        msgRSP_NonTaxItem = 92,
        msgREQ_MultiCmd_Tran = 151,
        msgRSP_MultiCmd_Tran = 152
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct GrupDF02
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string TermSerial;   //DF8204 12
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string TraceNo;      //DF8208 3
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string TranDate;     //DF8209 3
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string TranTime;     //DF820A 3
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GrupDF41
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string ExtDevVendorName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string ExtDevModel;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string ExtDevSerial;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string EcrVendorName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string EcrModel;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string EcrSerial;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string EcrRandomNumReq;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string EcrRandomNumRsp;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
        public string EncryptedMsg;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string EncryptedUnique;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GrupDF6F
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string keyInvalidationCnt;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 513)]
        public string CryptgrmA;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 513)]
        public string CryptgrmB;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string status;                   //0xDFEF04 BCD 1
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string ErrRespCode;             //0xDFEF06 BCD 2
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 513)]
        public string VendorPrivateField;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string ExtDevIndex;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 513)]
        public string OkcSign;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string VersionInfo;             //0xDFEF0B ASCII 11
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 513)]
        public string DHPrime;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string DHGenerator;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
        public string Kcv;                    //0xDFEF0E BYTE 32
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
        public string HMACVerifyVal;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
        public string OkcCert;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PluListTable
    {
        public string PluId;
        public string Name;
        public string Barcode;
        public string PriceAmount;
        //public string StockControl;
        //public string Stock;
        //public string GroupId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DepartmentListTable
    {
        public string DepartmentId;
        public string Amount;
        //public string Tax;
    }

    public struct BatchTranItem
    {
        public string ProcessType;
        public string DepartId;
        public string PLUId;
        public string NONTAXId;
        public string CollId;
        public string Quantity;
        public string UnitPrice;
        public string Barcode;
        public string Rate;
        public string ItemName;
        public string Amount;
        public string TranId;
        public string FreeText;
    }
    //[StructLayout(LayoutKind.Sequential)]
    public struct Members
    {
        public Tags ResponsedTag;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string SentBankData;                    //[in] DFEE36   BCD 512    
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string StartRNo;                    //[in] DFF125   BCD 6                           
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string EndRNo;                      //[in] DFF126   BCD 6                              
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DispatchNote;               //[in] DFF12F                              
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string ReceiptUniqueNumber;         //[out] DFF13D    
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4096)]
        public string DepartmentInfo;              //[out] DFF153                      
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7 * 1024)]
        public string ReportSoftCopy;             //[out] DFF169    ASCII 4096                                                  
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string PlateNO;                     //[in] DFF176   ASCII 20              
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Commision;                   //[in] DFF133 BCD  12  
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 104)]
        public string Title;                       //[in] DFF131 ASCII 50     
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string Plate;                       //[in] DFF130 ASCII 16           
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BillSerialNo;                //[in] DFF12E ASCII 20
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string EcrMode;                     //[in] DFF12D  BYTE 2
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string CashierPwd;                  //[in] DFF12C  ASCII 4   
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string CashierId;                   //[in-out] DFF12B     BCD 2  
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string TranStatus;                    //[out] DFF129   BCD 6  
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string EndPLUNo;                    //[in] DFF128   BCD 6  
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string StartPLUNo;                  //[in] DFF127   BCD 6    
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string PLUFlag;                  //[in] DFF17C   BCD 8   
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string EndZNo;                      //[in] DFF124   BCD 4 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string StartZNo;                    //[in] DFF123   BCD 4   
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string EndTime;                     //[in] DFF122   BCD 6   
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string StartTime;                   //[in] DFF121   BCD 6 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string UnitPrice;                   //[in] DFF159 BCD 12
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string PrintPLUSlip;                //[in] DFF148   BYTE 1
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string StockPiece;                  //[in] DFF141     BYTE 6 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string StockControl;                //[in] DFF140     BYTE 1  
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string GroupName;                   //[in] DFF161    ASCII 20
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string GroupNo;                     //[in] DFF13F     BYTE 3  
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string PLUNo;                       //[in] DFF13E     BYTE 3  
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string DrawerStatus;                      //[in] DFF13C     BYTE 1 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string DrawerTotalAmount;                 //[out] DFF13B 	BCD 	6   
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string SubTotalAmonut;                 //[out] DFF149 	BCD 	6   
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string EndDate;                     //[in] DFF120  BCD  6   
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string Vkn;                         //[in] DFF11D  ASCII 10
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string AppVersion;                    //[out] DFEE40 	ASCII 	1 - 20 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string InternalErrNum;              //[out] DFEE29 	BCD 	2 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string EJNum;                     //[out] DFEE5C 	BCD 	2 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string ReceiptNum;                    //[in-out] DFEE5B 	BCD 	3 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string ZNum;                      //[in-out] DFEE5A 	BCD 	2  xxx
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string TranTime;                  //[in-out] DFEE12 	BCD 	3 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string TranDate;                    //[in-out] DFEE11 	BCD 	3 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string Amount;                        //[in-out] DFEE0E 	BCD 	6 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string ProcessType;                   //[in] DFEE02 	 	1 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string AcquirerId;                    //[in] DFEE06 	BCD 	2 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string DepLimitAmount;              //[in] DFF155         
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string StartDate;                   //[in] DFF11F  BCD 6 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string DocumentType;                //[in] DFF103      BYTE 2
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string Tckn;                //[in] DFF11C      BYTE 12
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string VatRates;                    //[out] DFF119        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string TranId;                      //[in] DFF117 BYTE 8
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string FreeText;                    //[in] DFF116   ASCII 1--2048
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string PaymentType;                 //[in] DFF114  BYTE 2
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string ExcRate;                     //[in] DFF113    
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string CurrencyIndex;                     //[in] DFF11B   
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string CurrencyFlag;                     //[in] DFF11E   
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string CurrencyName;                     //[in] DFF106  
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string Rate;                        //[in] DFF112      BYTE 6
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 352)]
        public string HeaderText;                  //[in] DFF102      ASCII 42  8*44  
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string DepartmentId;                //[in] DFF10F  BYTE 2
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string NonTaxId;                //[in] DFF110  BYTE 4
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string PLUID;                    //[in] DFF111  BYTE 4
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string VatGroup;                    //[in] DFF134           BYTE 2
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string ItemName;                    //[in] DFF10A ASCII 64
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string Quantity;                    //[in] DFF108  BYTE 8
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
        public string Barcode;                     //[in] DFF107  ASCII  13
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string FiscalId;                    //[out] DFF105 ASCII 12
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string ReportType;                  //[in] DFF104    BYTE 6               
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string CollectionId;                 //[in] DFF138 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string OrderNo;               //[in] DFF177    
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string IsLaterOn;               //[in] DFF178                        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string OwnerName;               //[in] DFF179                       
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string MerchantNo;               //[in] DFF17A                        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string IsTakeComm;               //[in] DFF17B                     
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string RemainAmount;               //[out] DFF17D                   
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string EcrLocation;               //[out] DFF171  
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string CurrTable;          //[out] DFF11A     
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4096)]
        public string DailyTotals;        //[out] DFF163      
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5120)]
        public string SoldItems;        //[out] DFF17E    
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string MealAppList;        //[out] DFF137        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string BatchItemsCount;        //[in] DFF135  ASCII  8
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22 * 1024)]
        public string BatchItemsDetail;        //[in] DFF136 ASCII 26*1024     

        public GrupDF02 groupDF02;//[out]
        public GrupDF41 groupDF41;//[out]
        public GrupDF6F groupDF6F;//[out]

    }
    public enum DATA_LEN
    {
        MAX_BATCHITEM = 100,
        MAX_DISCOUNT_RATE = 10000,
        MAX_LENGTH = 20480,
        MAX_PLU_NUM = 100000
    }

    public enum ECR_RPRT_TYPS
    {
        X = 1048576,
        X_PLU_SALE_PP = 1048592,
        X_PLU_PRG_PP = 1048608,
        Z = 2097152,
        Z_PLU_SALE_PP = 2097168,
        Z_LAST_COPY = 2097184,
        Z_SOFT_LAST_COPY = 2097200,
        EJ_DETAIL = 3211264,
        EJ_Z_DETAIL = 3276800,
        EJ_R_DETAIL_DT = 3342337,
        EJ_R_DETAIL_ZR = 3342338,
        EJ_R_SNGL_CPY_DT = 3411969,
        EJ_R_SNGL_CPY_ZR = 3411970,
        EJ_R_SNGL_CPY_DTR = 3411971,
        EJ_R_SNGL_CPY_LASTSALE = 3411974,
        EJ_R_PRDC_CPY_DTDT = 3416065,
        EJ_R_PRDC_CPY_ZR = 3416066,
        EJ_R_PRDC_CPY_ZT = 3416068,
        FM_Z_DTL_DD = 4194561,
        FM_Z_DTL_ZZ = 4194565,
        FM_Z_SMRY_DD = 4194817,
        FM_Z_SMRY_ZZ = 4194821,
        FM_SALE_Z = 4195077,
        BANK_EOD = 5242880,
        AUDIT_REPORT = 5242881,
        DEVICE_PARAMS = 6291456,
        BANK_CUSTOMER_SLIP_COPY = 7340032,
        BANK_MERCHANT_SLIP_COPY = 7340033,
        BANK_BOTH_SLIP_COPY = 7340034,
        BANK_EOD_SLIP_COPY = 7340035,
        BANK_ERROR_SLIP_COPY = 7340036
    }

    public enum FIELD_WIDTH
    {
        BYTE = 2,
        Z = 4,
        R = 6,
        PLU = 6,
        AMOUNT = 12
    }
    public enum ECR_ERRORS
    {
        ENCRYPTION_ERR = -20,
        INVALID_CERT = -19,
        ECRVERSION_ERROR = -18,
        CERT_CHAIN_VERIFY_ERR = -17,
        EXPIRED_CERT = -16,
        ERR_NAK_RECEIVED = -15,
        CRYPTA_VERIFY_ERR = -14,
        VERIFY_ERR = -13,
        EXC_APPEND = -12,
        EXC_PARSE = -11,
        ACCESS_DENIED = -10,
        DLL_REGISTR_ERR = -9,
        FRAME_TOUT = -8,
        ACK_TOUT = -7,
        COMM_PORT_NOT_OPENED = -6,
        SYSTEM_ERR = -5,
        TOUT = -4,
        ERR_LRC = -3,
        ERR_FRAME = -2,
        ERR_NOK = -1,
        ERR_OK = 0
    }


    public class DLL
    {
        [DllImport("PAYGO_PCPOSOKC.dll", EntryPoint = "CloseComm", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void CloseComm();


        [DllImport("PAYGO_PCPOSOKC.dll", EntryPoint = "OpenComm", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OpenComm(COMMTYPE commType, CommMedia cm, int nWaitTimeOut = 60);//nWaitTimeOut Indicates the interval for sending heartbeat packets.The unit is seconds.

        [DllImport("PAYGO_PCPOSOKC.dll", EntryPoint = "OpenCommEx", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OpenCommEx(COMMTYPE commType, string strSerialPort, string strServerAddr, int iServerPort, int nWaitTimeOut = 60);//nWaitTimeOut Indicates the interval for sending heartbeat packets. The unit is seconds.


        [DllImport("PAYGO_PCPOSOKC.dll", EntryPoint = "GetEncryptedMsgStatus", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]

        public static extern bool GetEncryptedMsgStatus();

        [DllImport("PAYGO_PCPOSOKC.dll", EntryPoint = "GetLogStatus", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetLogStatus();
         
        [DllImport("PAYGO_PCPOSOKC.dll", EntryPoint = "SetDeviceInfo", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetDeviceInfo(string strExtDevVendor, string strExtDevModel, string strVersion);

        [DllImport("PAYGO_PCPOSOKC.dll", EntryPoint = "SetEncryptedMsgStatus", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetEncryptedMsgStatus(bool isEnc);
        [DllImport("PAYGO_PCPOSOKC.dll", EntryPoint = "SetLogStatus", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetLogStatus(bool EnableStat, string strLogPath = "");

        [DllImport("PAYGO_PCPOSOKC.dll", EntryPoint = "SendCmdToDevice", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SendCmdToDevice(Tags Tag, ref Members reqMembers, ref Members respMembers);

        [DllImport("PAYGO_PCPOSOKC.dll", EntryPoint = "SendCmdToDeviceCustomTimeout", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SendCmdToDeviceCustomTimeout(Tags Tag, ref Members reqMembers, ref Members respMembers, int nWaitTime = 30000);//nWaitTime is the timeout period for receiving instructions, in milliseconds.

        [DllImport("PAYGO_PCPOSOKC.dll", EntryPoint = "isValidCommunicator", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool isValidCommunicator();

        [DllImport("PAYGO_PCPOSOKC.dll", EntryPoint = "SetCommunicateConfig", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetCommunicateConfig(COMMTYPE commType, string strSerialPort, string strServerAddr, int iServerPort);

        [DllImport("PAYGO_PCPOSOKC.dll", EntryPoint = "GetCommunicateConfig", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetCommunicateConfig(ref int commType, IntPtr strSerialPort, IntPtr strServerAddr, ref int iServerPort);

        public enum ECR_ANSWER
        {
            EOT = 4,
            ACK = 6,
            NACK = 21
        }

        public static ushort[] CrcTable;

    }





}