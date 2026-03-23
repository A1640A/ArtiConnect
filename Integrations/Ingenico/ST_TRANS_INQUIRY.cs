using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_TRANS_INQUIRY
    {
        public ushort BankBkmId;

        public string szTerminalId;

        public uint Batch;

        public uint Stan;

        public byte TransactionType;

        public ushort ECROptions;

        public uint ECROptions2;

        public ushort MessageResponseCode;

        public ushort AuthorisedBank;

        public string szResponseCode;

        public string szTransactionDateTime;

        public ushort TransactionInformationFlags;

        public ushort ApplicationInformationFlags;

        public string szPAN;

        public uint AuthorisedAmount;

        public string szAuthorisationNumber;

        public string szBankHostResponseCode;

        public string szAdditionalResponseDescriptionForDisplay;

        public string szBankApplicationSpecificInternalErrorDescription;

        public ushort BankSpecificErrorCode;

        public string szPOSApplicationBankVersion;

        public string szPOSApplicationInternalVersion;

        public ST_TRANS_INQUIRY()
        {
            szTerminalId = "";
            szResponseCode = "";
            szTransactionDateTime = "";
            szPAN = "";
            szAuthorisationNumber = "";
            szBankHostResponseCode = "";
            szAdditionalResponseDescriptionForDisplay = "";
            szBankApplicationSpecificInternalErrorDescription = "";
            szPOSApplicationBankVersion = "";
            szPOSApplicationInternalVersion = "";
        }
    }
}
