using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    internal class Defines
    {
        public const string m_deptIndCol = "Dept Index";

        public const string m_taxIndCol = "Tax Index";

        public const string m_nameCol = "Name";

        public const string m_unitCol = "Unit Type";

        public const string m_amountCol = "Amount";

        public const string m_currencyCol = "Currency";

        public const string m_limitAmountCol = "Limit Amount";

        public const string m_lunchCardCol = "Lunch Card";

        public const int TRAN_STATUS_FREE = 1;

        public const int TRAN_STATUS_RESERVED = 2;

        public const int TRAN_STATUS_SAVED = 3;

        public const int TRAN_STATUS_VOIDED = 4;

        public const int TRAN_STATUS_COMPLETED = 5;

        public const int LOYALTY_CUSTOMER_ID_TYPE_MOBILE_TEL = 1;

        public const int LOYALTY_CUSTOMER_ID_TYPE_MUSTERI_NO = 2;

        public const int LOYALTY_CUSTOMER_ID_TYPE_DIGER = 3;

        public const int TIMEOUT_DEFAULT = 10000;

        public const int TIMEOUT_CARD_TRANSACTIONS = 100000;

        public const int TIMEOUT_ECHO = 10000;

        public const int TIMEOUT_PRINT_MF = 20000;

        public const int TIMEOUT_DATABASE_EXECUTE = 20000;

        public const int MAX_UNIQUE_ID = 256;

        public const string DLL_VERSION_MIN = "1602030800";

        public const uint BANK_TRAN_FLAG_DO_NOT_ASK_FOR_MISSING_LOYALTY_POINT = 67108864u;

        public const uint BANK_TRAN_FLAG_ALL_INPUT_FROM_EXTERNAL_SYSTEM = 134217728u;

        public const uint BANK_TRAN_FLAG_ASK_FOR_MISSING_REFUND_INPUTS = 268435456u;

        public const uint BANK_TRAN_FLAG_LOYALTY_POINT_NOT_SUPPORTED_FOR_TRANS = 536870912u;

        public const uint BANK_TRAN_FLAG_ONLINE_FORCED_TRANSACTION = 1073741824u;

        public const uint BANK_TRAN_FLAG_MANUAL_PAN_ENTRY_NOT_ALLOWED = 2147483648u;

        public const uint BANK_TRAN_FLAG_AUTHORISATION_FOR_INVOICE_PAYMENT = 33554432u;

        public const uint BANK_TRAN_FLAG_SALE_WITHOUT_CAMPAIGN = 16777216u;

        public const int EKU_SEEK_MODE_ALL_TYPE = 255;

        public const int STANDART_BUFFER = 50000;

        public const int GMP_TICKET_BUFFER = 200000;

        public const int GMP_EXT_DEVICE_FILEDIR_BITMAP = 14675448;

        public const int GMP_EXT_DEVICE_TAG_Z_NO = 14675206;

        public const int GMP_EXT_DEVICE_TAG_TRAN_DB_NAME = 14675541;

        public const int GMP_EXT_DEVICE_FIS_LIMIT = 14675284;

        public const int GMP_TAG_GROUP_OKC_DOVIZ_TABLOSU = 57209;

        public const int GMP3_OPTION_RETURN_AFTER_SINGLE_PAYMENT = 2;

        public const int GMP3_OPTION_RETURN_AFTER_COMPLETE_PAYMENT = 4;

        public const int GMP3_OPTION_DONT_ALLOW_NEW_ITEM = 8;

        public const int GMP3_OPTION_DONT_ALLOW_VOID_ITEM = 16;

        public const int GMP3_OPTION_DONT_ALLOW_VOID_PAYMENT = 32;

        public const int GMP3_OPTION_CONTINUE_IN_OFFLINE_MODE = 64;

        public const int GMP3_OPTION_DONT_SEND_TRANSACTION_RESULT = 128;

        public const int GMP3_OPTION_DONT_ALLOW_NEW_PAYMENT_CASH_TL = 131072;

        public const int GMP3_OPTION_DONT_ALLOW_NEW_PAYMENT_CASH_EXCHANGE = 262144;

        public const int GMP3_OPTION_DONT_ALLOW_NEW_PAYMENT_BANKCARD = 524288;

        public const int GMP3_OPTION_DONT_ALLOW_NEW_PAYMENT_YEMEKCEKI = 1048576;

        public const int GMP3_OPTION_DONT_ALLOW_NEW_PAYMENT_MOBILE = 2097152;

        public const int GMP3_OPTION_DONT_ALLOW_NEW_PAYMENT_HEDIYECEKI = 4194304;

        public const int GMP3_OPTION_DONT_ALLOW_NEW_PAYMENT_IKRAM = 8388608;

        public const int GMP3_OPTION_DONT_ALLOW_NEW_PAYMENT_ODEMESIZ = 16777216;

        public const int GMP3_OPTION_DONT_ALLOW_NEW_PAYMENT_KAPORA = 33554432;

        public const int GMP3_OPTION_DONT_ALLOW_NEW_PAYMENT_PUAN = 67108864;

        public const int GMP3_OPTION_DONT_ALLOW_NEW_PAYMENT = 134086656;

        public const int PAYMENT_OTHER_ALL = 32760;

        public const int PAYMENT_OTHER_REVERSE = 260046848;

        public const int PAYMENT_VAS_ALL = 260079608;

        public const int PAYMENT_BANK_ALL = 6291460;

        public const int MAX_TAXRATE_COUNT = 8;

        public const int MAX_DEPARTMENT_COUNT = 12;

        public const int MAX_EXCHANGE_COUNT = 6;

        public const int MAX_CASHIER_COUNT = 4;

        public const int MAX_CINEMA_DEPARTMENT_COUNT = 8;

        public const int GMP3_FISCAL_PRINTER_MODE_REQ = 16747145;

        public const int GMP3_FISCAL_PRINTER_MODE_REQ_E = 16747401;

        public const int GMP3_FISCAL_PRINTER_MODE_RES = 16748169;

        public const int GMP3_FISCAL_PRINTER_MODE_RES_E = 16748425;

        public const int GMP3_EXT_DEVICE_GET_DATA_REQ = 16747136;

        public const int GMP3_EXT_DEVICE_GET_DATA_REQ_E = 16747392;

        public const int GMP3_EXT_DEVICE_GET_DATA_RES = 16748160;

        public const int GMP3_EXT_DEVICE_GET_DATA_RES_E = 16748416;

        public const int GMP_EXT_DEVICE_FLIGHT_MODE = 14675561;

        public const int GMP_EXT_DEVICE_TICKET_TIMEOUT = 14675562;

        public const int GMP_EXT_DEVICE_COMM_STATUS = 14675563;

        public const int GMP_EXT_DEVICE_COMM_SCENARIO = 14675564;

        public const int GMP_EXT_DEVICE_STAND_BY_TIME = 14675565;

        public const int GMP_EXT_DEVICE_FISCAL_USAGE_INFO = 14675573;

        public const int GMP_EXT_DEVICE_EKU_USAGE_INFO = 14675574;

        public const byte GMP3_STATE_BIT_FLIGHT_MODE = 1;

        public const byte GMP3_STATE_BIT_GPRS_CONNECTED = 2;

        public const byte GMP3_STATE_BIT_ETHERNET_CONNECTED = 4;

        public const byte MAX_LOYALITY_TRANS_NUMBER = 8;

        public const int GMP3_OPTION_ECHO_PRINTER = 1;

        public const int GMP3_OPTION_ECHO_PAYMENT_DETAILS = 2;

        public const int GMP3_OPTION_ECHO_ITEM_DETAILS = 4;

        public const int GMP3_OPTION_NO_RECEIPT_LIMIT_CONTROL_FOR_ITEMS = 8;

        public const int GMP3_OPTION_DONOT_CONTROL_PAYMENTS_FOR_RECEIPT_CANCEL = 16;

        public const int GMP3_OPTION_GET_CONFIRMATION_FOR_PAYMENT_CANCEL = 32;

        public const int GMP3_OPTION_ECHO_LOYALTY_DETAILS = 64;

        public const int GMP3_OPTION_DONT_PRINT_MERCHANT_SLIPS = 512;

        public const int TRAN_RESULT_OK = 0;

        public const int TRAN_RESULT_NOT_ALLOWED = 1;

        public const int TRAN_RESULT_TIMEOUT = 2;

        public const int TRAN_RESULT_USER_ABORT = 4;

        public const int TRAN_RESULT_EKU_PROBLEM = 8;

        public const int TRAN_RESULT_CONTINUE = 16;

        public const int TRAN_RESULT_NO_PAPER = 32;

        public const int APP_ERR_GMP3_INVALID_HANDLE = 2317;

        public const int APP_ERR_ALREADY_DONE = 2080;

        public const int APP_ERR_PAYMENT_NOT_SUCCESSFUL_AND_NO_MORE_ERROR_CODE = 2085;

        public const int APP_ERR_PAYMENT_NOT_SUCCESSFUL_AND_MORE_ERROR_CODE = 2086;

        public const int DLL_RETCODE_RECV_BUSY = 61468;

        public const int APP_ERR_TICKET_HEADER_ALREADY_PRINTED = 2078;

        public const int APP_ERR_TICKET_HEADER_NOT_PRINTED = 2077;

        public const int APP_ERR_FIS_LIMITI_ASILAMAZ = 2067;

        public const int DLL_RETCODE_TIMEOUT = 61443;

        public const int APP_ERR_FILE_EOF = 2226;

        public const int SQLITE_OK = 0;

        public const int SQLITE_ROW = 100;

        public const int SQLITE_DONE = 101;

        public const int SQLITE_INTEGER = 1;

        public const int SQLITE_FLOAT = 2;

        public const int SQLITE_TEXT = 3;

        public const int SQLITE_BLOB = 4;

        public const int SQLITE_NULL = 5;

        public const int PS_24 = 0;

        public const int PS_12 = 1;

        public const int PS_32 = 2;

        public const int PS_48 = 4;

        public const int PS_BOLD = 8;

        public const int PS_CENTER = 16;

        public const int PS_RIGHT = 32;

        public const int PS_INVERTED = 64;

        public const int PS_UNIQUE_ID = 128;

        public const int PS_BARCODE = 256;

        public const int PS_ECR_TICKET_HEADER = 512;

        public const int PS_GRAPHIC = 1024;

        public const int PS_QRCODE = 2048;

        public const int PS_16 = 4096;

        public const int PS_38 = 8192;

        public const int PS_MULT2 = 1;

        public const int PS_MULT4 = 2;

        public const int PS_MULT8 = 4;

        public const int PS_ECR_TICKET_ITEM = 65536;

        public const int PS_ECR_TICKET_COPY = 131072;

        public const int PS_ECR_USER_MSG_BEFORE_HEADER = 262144;

        public const int PS_ECR_USER_MSG_AFTER_TOTALS = 524288;

        public const int PS_ECR_USER_MSG_BEFORE_MF = 1048576;

        public const int PS_ECR_USER_MSG_AFTER_MF = 2097152;

        public const int PS_NO_PAPER_CHECK = 4194304;

        public const int PS_FEED_LINE = 8388608;

        public const int ITEM_TYPE_FREE = 0;

        public const int ITEM_TYPE_DEPARTMENT = 1;

        public const int ITEM_TYPE_PLU = 2;

        public const int ITEM_TYPE_TICKET = 3;

        public const int ITEM_TYPE_MONEYCOLLECTION = 9;

        public const int DLL_RETCODE_FAIL = 61451;

        public const int PAYMENT_SUBTYPE_PROCESS_ON_POS = 0;

        public const int PAYMENT_SUBTYPE_SALE = 1;

        public const int PAYMENT_SUBTYPE_INSTALMENT_SALE = 2;

        public const int PAYMENT_SUBTYPE_LOYALTY_PUAN = 3;

        public const int PAYMENT_SUBTYPE_ADVANCE_REFUND = 4;

        public const int PAYMENT_SUBTYPE_INSTALLMENT_REFUND = 5;

        public const int PAYMENT_SUBTYPE_REFERENCED_REFUND = 6;

        public const int PAYMENT_SUBTYPE_REFERENCED_REFUND_WITH_CARD = 7;

        public const int PAYMENT_SUBTYPE_REFERENCED_REFUND_WITHOUT_CARD = 8;

        public const int APP_ERR_FISCAL_INVALID_ENTRY = 2009;

        public const int FLAG_SETSCENARIO_ETHERNET = 1;

        public const int FLAG_SETSCENARIO_GPRS = 2;

        public const int FLAG_SETSCENARIO_ETHERNET_GPRS = 3;
    }

}
