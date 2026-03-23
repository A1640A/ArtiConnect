using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Hugin
{
    public class Utils
    {

        public Utils()
        {

        }

        public static string ByteArrayToHexString(byte[] bytes, int len)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < len; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        public static byte[] HexStringToByteArrayFast(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException("HexString must have an even number of characters.");
            }

            int byteLength = hexString.Length / 2;
            byte[] byteArray = new byte[byteLength];

            for (int i = 0; i < byteLength; i++)
            {
                byteArray[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return byteArray;
        }

        public static byte[] HexStringToByteArrayFastest(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException("HexString must have an even number of characters.");
            }

            int byteLength = hexString.Length / 2;
            byte[] byteArray = new byte[byteLength];
            char c;
            int value;

            for (int i = 0; i < byteLength; i++)
            {
                c = hexString[i * 2];
                value = (c >= '0' && c <= '9' ? c - '0' : (c >= 'a' && c <= 'f' ? c - 'a' + 10 : (c >= 'A' && c <= 'F' ? c - 'A' + 10 : -1)));

                if (value == -1)
                {
                    throw new FormatException("Invalid hex character");
                }

                byteArray[i] = (byte)(value << 4);

                c = hexString[i * 2 + 1];
                value = (c >= '0' && c <= '9' ? c - '0' : (c >= 'a' && c <= 'f' ? c - 'a' + 10 : (c >= 'A' && c <= 'F' ? c - 'A' + 10 : -1)));
                if (value == -1)
                {
                    throw new FormatException("Invalid hex character");
                }

                byteArray[i] |= (byte)value;
            }

            return byteArray;
        }
        public static string ByteArrayToString(byte[] ba, int length)
        {
            byte[] tmpBuff = new byte[length];
            Buffer.BlockCopy(ba, 0, tmpBuff, 0, length);
            return System.Text.Encoding.Default.GetString(tmpBuff);

        }
        public static string ByteArrayToString(byte[] ba, int index, int length)
        {

            byte[] tmpBuff = new byte[length];
            Buffer.BlockCopy(ba, index, tmpBuff, 0, length);
            return System.Text.Encoding.Default.GetString(tmpBuff);
        }
        public static string ByteArrayToString(byte[] byteArray)
        {
            return System.Text.Encoding.Default.GetString(byteArray);
        }

        public static string StringToISO_8859_9Str(string myString)
        {
            byte[] mybyte = Encoding.Default.GetBytes(myString);
            return Encoding.UTF8.GetString(mybyte);
        }
        public static string StringToDefaultStr(string myString)
        {
            byte[] mybyte = Encoding.Default.GetBytes(myString);
            return System.Text.Encoding.Default.GetString(mybyte);
        }
        public static string ByteArrToISO_8859_9Str(byte[] value, int index, int len)
        {
            return Encoding.UTF8.GetString(value, index, value.Length);
        }
        public static string ByteToHex(byte[] comByte)
        {
            string returnStr = "";
            if (comByte != null)
            {
                for (int i = 0; i < comByte.Length; i++)
                {
                    returnStr += comByte[i].ToString("X2");
                }
            }
            return returnStr;
        }
        public static ushort CalculateCRC16(byte[] Buffer, ushort Len, ushort offset_arg)
        {
            if (Buffer == null || Buffer.Length == 0) return 0;
            if (offset_arg < 0) return 0;
            if (Len == 0) Len = (ushort)(Buffer.Length - (ushort)offset_arg);
            int length = offset_arg + Len;
            if (length > Buffer.Length) return 0;
            ushort crc = 0;// Initial value
            for (int i = offset_arg; i < length; i++)
            {
                crc ^= Buffer[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 1) > 0)
                        crc = (ushort)((crc >> 1) ^ 0xA001);// 0xA001 = reverse 0x8005
                    else
                        crc = (ushort)(crc >> 1);
                }
            }
            byte[] ret = BitConverter.GetBytes(crc);
            Array.Reverse(ret);
            return 0;
        }

        public static byte CommUtil_CalcLRC(byte[] byMessage, int usLength, byte offset_arg)
        {
            if (byMessage == null || byMessage.Length == 0) return 0;
            if (offset_arg < 0) return 0;
            if (usLength == 0) usLength = byMessage.Length - offset_arg;
            int length = offset_arg + usLength;
            if (length > byMessage.Length) return 0;
            byte lrc = 0;
            for (int i = offset_arg; i < length; i++)
            {
                lrc += byMessage[i];
            }
            lrc = (byte)((lrc ^ 0xff) + 1);
            return lrc;
        }
        public static byte[] GetByteArray(string value)
        {
            if (value != null && value != "")
                return System.Text.Encoding.Default.GetBytes(value);
            else
                return null;
        }
        public static byte[] GetByteArrayISO_8859_9(string value)
        {
            if (value != null && value != "")
            {
                //System.Text.Encoding ISO8859 = System.Text.Encoding.GetEncoding("iso8859-9");
                //System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("gb2312");
                //byte[] gb = GB2312.GetBytes(str);
                //return ISO8859.GetString(gb);

                Encoding iso = Encoding.GetEncoding("ISO_8859_9");
                Encoding utf8 = Encoding.UTF8;
                byte[] utfBytes = utf8.GetBytes(value);
                byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
                string msg = iso.GetString(isoBytes);

                return isoBytes;
            }
            else
                return null;
        }
        public static string GetByteString(byte[] value)
        {
            return System.Text.Encoding.Default.GetString(value);
        }
        public static byte[] HexStr2ByteArr(string msg)
        {
            if (msg != null && msg != "")
            {
                msg = msg.Replace(" ", "");
                if ((msg.Length % 2) != 0)
                {
                    msg += " ";
                }
                byte[] returnBytes = new byte[msg.Length / 2];
                for (int i = 0; i < returnBytes.Length; i++)
                {
                    returnBytes[i] = Convert.ToByte(msg.Substring(i * 2, 2), 16);
                }
                return returnBytes;
            }
            else
                return null;
        }

        public static bool IpParse(string ipAddress)
        {
            bool bret = false;
            System.Net.IPAddress ip;

            if (ipAddress != null && ipAddress != "")
            {
                if (System.Net.IPAddress.TryParse(ipAddress, out ip))
                {
                    bret = true;
                }
                else
                {
                    bret = false;
                }
            }
            return bret;
        }
        public static bool isValidPort(int port)
        {
            bool bret = true;
            int isPort;
            string txtPort = port.ToString();

            if (txtPort == "")
            {
                bret = false;
            }
            else if (!int.TryParse(txtPort, out isPort))
            {
                bret = false;
            }
            else if ((port < 1024) || (port > 65335))
            {
                bret = true;
            }
            else
            {
                bret = false;
            }
            return bret;
        }
        public static string GetErrExplain(int ErrNo)
        {

            string returnStr = "";
            switch (ErrNo)
            {
                case 0x10:
                    returnStr = "Message integrity verification error";//GrupDF6F ErrRespCode
                    break;
                case 0x11:
                    returnStr = "Invalid message";//GrupDF6F ErrRespCode
                    break;
                case 0x12:
                    returnStr = "Message length verification error";//GrupDF6F ErrRespCode
                    break;
                case 0x13:
                    returnStr = "LRC verification error";//GrupDF6F ErrRespCode
                    break;
                case 0x20:
                    returnStr = "Unknown Terminal Serial No";//GrupDF6F ErrRespCode
                    break;
                case 0x30:
                    returnStr = "Unknown Tag";//GrupDF6F ErrRespCode
                    break;
                case 0x31:
                    returnStr = "Tag length error";//GrupDF6F ErrRespCode
                    break;
                case 0x32:
                    returnStr = "Tag data reading error";//GrupDF6F ErrRespCode
                    break;
                case 0x90:
                    returnStr = "Invalid operation";//GrupDF6F ErrRespCode
                    break;
                case 0:
                    returnStr = "Successful";
                    break;
                case 1:
                    returnStr = "Unsuccessful";
                    break;
                case 2:
                    returnStr = "There is no paper.";
                    break;
                case 3:
                    //returnStr = "Missing Parameter";
                    returnStr = "The mode is wrong, please change to admin mode.";
                    break;
                case 4:
                    //returnStr = "Length Error";
                    returnStr = "The mode is wrong, please change to sale mode.";
                    break;
                case 5:
                    //returnStr = "Data Error";
                    returnStr = "Data Error,Please do echo firstly";
                    break;
                case 6:
                    //returnStr = "Wrong Parameter";
                    returnStr = "There is still remain amount.";
                    break;
                case 7:
                    returnStr = "Transaciton alredy voided";
                    //returnStr = "File Error";
                    break;
                case 8:
                    returnStr = "E/C is already done, you can use Product Void";
                    //returnStr = "DB Error";
                    break;
                case 9:
                    returnStr = "Please close document firstly.";
                    //returnStr = "General Error";
                    break;
                case 10:
                    returnStr = "Cashier not logged in";
                    break;
                case 11:
                    returnStr = "Should be Taken to Sales Position";
                    break;
                case 12:
                    returnStr = "Should be Taken to Manager Position";
                    break;
                case 13://RMP3 ERR OVER KEY COUNTER
                    returnStr = "Key counter is out or expired, please re-pair.";
                    break;
                case 61:
                    returnStr = "Unsuccessful";
                    break;
                case 64:
                    returnStr = "User Interrupt";
                    break;
                case 65:
                    returnStr = "Send Error";
                    break;
                case 67:
                    returnStr = "Time Out";
                    break;
                case 68:
                    returnStr = "Encryption Error";
                    break;
                case 69:
                    returnStr = "Decryption Error";
                    break;
                case 71:
                    returnStr = "Certificate Reeading Error";
                    break;
                case 72:
                    returnStr = "Pairind Request Message Could not Received";
                    break;
                case 73:
                    returnStr = "Pairing Request Message Parsing Error";
                    break;
                case 75:
                    returnStr = "Pairing response Could not send";
                    break;
                case 76:
                    returnStr = "Pairing request Message Could not received";
                    break;
                case 77:
                    returnStr = "Ending Request Message Parsing Error";
                    break;
                case 82:
                    returnStr = "Ending Request Message HMAC Calculation Error";
                    break;
                case 83:
                    returnStr = "Store Key Error";
                    break;
                case 84:
                    returnStr = "Ending Response Sending Error";
                    break;
                case 85:
                    returnStr = "Key Response error Could not received";
                    break;
                case 200:
                    returnStr = "Wrong Input";
                    break;
                case 201:
                    returnStr = "Invalid Transaction";
                    break;
                case 202:
                    returnStr = "Transaction is not completed";
                    break;
                case 203:
                    returnStr = "Incorrect Password";
                    break;
                case 204:
                    returnStr = "Wrong Parameter";
                    break;
                case 205:
                    returnStr = "Root Certificate Error";
                    break;
                case 206:
                    returnStr = "ECR Settings Should be set";
                    break;
                case 207:
                    returnStr = "Z Report Should be printed";
                    break;
                case 208:
                    returnStr = "Cashier Log-in Error";
                    break;
                case 209:
                    returnStr = "The device is intervened";
                    break;
                case 210:
                    returnStr = "Fiscal Memory Full";
                    break;
                case 211:
                    returnStr = "EJ Integrity Error";
                    break;
                case 212:
                    returnStr = "Daily Memory Full";
                    break;
                case 213:
                    returnStr = "EJ Error";
                    break;
                case 214:
                    returnStr = "Fiscal memory communication error";
                    break;
                case 215:
                    returnStr = "TSM parameter integrity error";
                    break;
                case 216:
                    returnStr = "EJ Full";
                    break;
                case 217:
                    returnStr = "ECR date / time error";
                    break;
                case 218:
                    returnStr = "Daily receipt limit is exceeded";
                    break;
                case 219:
                    returnStr = "Cashier log-in should be made";
                    break;
                case 220:
                    returnStr = "New transaction cannot be initiated";
                    break;
                case 221:
                    returnStr = "Undefined section";
                    break;
                case 222:
                    returnStr = "Faulty amount";
                    break;
                case 223:
                    returnStr = "Payment should be completed";
                    break;
                case 224:
                    returnStr = "Department sales limit is exceeded";
                    break;
                case 225:
                    returnStr = "Receipt amount limit is exceeded";
                    break;
                case 226:
                    returnStr = "Invoice printer is not connected";
                    break;
                case 227:
                    returnStr = "Undefined PLU";
                    break;
                case 228:
                    returnStr = "Department ID faulty";
                    break;
                case 229:
                    returnStr = "PLU Stock insufficient";
                    break;
                case 230:
                    returnStr = "Wrong discount rate";
                    break;
                case 231:
                    returnStr = "Discount cannot be zero";
                    break;
                case 232:
                    returnStr = "Wrong discount amount";
                    break;
                case 233:
                    returnStr = "Wrong increase rate";
                    break;
                case 234:
                    returnStr = "Increase cannot be zero";
                    break;
                case 235:
                    returnStr = "Wrong increase amount";
                    break;
                case 236:
                    returnStr = "Record / File couldn’t be found";
                    break;
                case 237:
                    returnStr = "Memory error";
                    break;
                case 238:
                    returnStr = "File reading error";
                    break;
                case 239:
                    returnStr = "Fiscal memory integrity error";
                    break;
                case 240:
                    returnStr = "Daily Z limit is exceeded";
                    break;
                case 241:
                    returnStr = "Report couldn’t be printed";
                    break;
                case 242:
                    returnStr = "Bank isn’t loaded";
                    break;
                case 243:
                    returnStr = "Cash register’s cash is insufficient";
                    break;
                case 244:
                    returnStr = "Bank transaction unsuccessful";
                    break;
                case 245:
                    returnStr = "NTP update error";
                    break;
                case 246:
                    returnStr = "Software couldn’t be updated";
                    break;
                case 247:
                    returnStr = "Settings cannot be changed";
                    break;
                case 248:
                    returnStr = "Partial payment limit is exceeded";
                    break;
                case 249:
                    returnStr = "Cash register is not fiscal";
                    break;
                case 250:
                    returnStr = "GMP3 key usage period expired";
                    break;
                case 251:
                    returnStr = "Pairing should be done";
                    break;
                case 252:
                    returnStr = "Terminated EJ";
                    break;
                case 253:
                    returnStr = "Receipt Cancelled";
                    break;
                case 254:
                    returnStr = "Barcode has been defined";
                    break;
                case 255:
                    returnStr = "Undefined Group";
                    break;
                case 256:
                    returnStr = "VAT is not set";
                    break;
                case 257:
                    returnStr = "Bank - Communication could not achieved";
                    break;
                case 258:
                    returnStr = "Bank - response could not received";
                    break;
                case 259:
                    returnStr = "Bank - Daily close must be done";
                    break;
                case 260:
                    returnStr = "Bank - Daily close is wrong";
                    break;
                case 261:
                    returnStr = "Bank - Terminal keys should be loaded";
                    break;
                case 262:
                    returnStr = "Bank - Terminal intallation should be done";
                    break;
                case 263:
                    returnStr = "Bank - Terminal keys Exchange must be done";
                    break;
                case 264:
                    returnStr = "Bank - Terminal parameters should be loaded/updated";
                    break;
                case 265:
                    returnStr = "Bank - Transaction could not be found in the batch";
                    break;
                case 266:
                    returnStr = "Bank - Transaction could not saved to the batch";
                    break;
                case 267:
                    returnStr = "Bank- Transaction exists in batch but can not be slip";
                    break;
                case 268:
                    returnStr = "Bank-Transaction is not exist in Batch";
                    break;
                case 269:
                    returnStr = "Bank- User Interrupt";
                    break;
                case 270:
                    returnStr = "Bank- Usser Time Out";
                    break;
                case 271:
                    returnStr = "Bank- BKM ID Could not found";
                    break;
                case 272:
                    returnStr = "Wrong Payment type";
                    break;

                default:
                    returnStr = "unknown error";
                    break;
            }

            return returnStr;
        }

    }
}

