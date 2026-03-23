using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_INI_PARAM
    {
        public byte IsCheckStructVersion;

        public byte RetryCounter;

        public byte IpRetryCount;

        public uint AckTimeOut;

        public uint CommTimeOut;

        public uint InterCharacterTimeOut;

        public string PortName;

        public uint BaudRate;

        public byte ByteSize;

        public byte fParity;

        public byte Parity;

        public byte StopBit;

        public byte IsTcpConnection;

        public string IP;

        public uint Port;

        public string LogPath;

        public byte LogPrintToFileOpen;

        public byte LogPrintToConsoleOpen;

        public byte LogGeneralOpen;

        public byte LogFunctionOpen;

        public byte LogSecurityOpen;

        public byte LogPrivateSecurityOpen;

        public byte LogCommOpen;

        public byte LogExtDevOpen;

        public byte LogJsonOpen;

        public byte LogJsonDataOpen;

        public byte LogGmp3TagsOpen;

        public byte LogPrintSerialNumOpen;

        public byte LogPrintDateOpen;

        public byte LogPrintTimeOpen;

        public byte LogPrintTypeOpen;

        public byte LogPrintVersionOpen;

        public byte LogPrintSourceFileOpen;

        public byte LogPrintSourceLineOpen;

        public ST_INI_PARAM()
        {
            PortName = "";
            IP = "";
            LogPath = "";
        }
    }

}
