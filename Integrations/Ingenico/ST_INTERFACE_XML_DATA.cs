using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_INTERFACE_XML_DATA
    {
        public byte RetryCounter;

        public byte IpRetryCount;

        public uint AckTimeOut;

        public uint CommTimeOut;

        public uint InterCharacterTimeOut;

        public string PortName;

        public int BaudRate;

        public int ByteSize;

        public int fParity;

        public int Parity;

        public int StopBit;

        public byte IsTcpConnection;

        public string IP;

        public int Port;
    }

}
