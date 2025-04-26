using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class SerialPortRequest
    {
        public string PortName { get; set; } = "COM1";
        public int BaudRate { get; set; } = 9600;
        public int Parity { get; set; } = 0; // None
        public int DataBits { get; set; } = 8;
        public int StopBits { get; set; } = 1; // One
        public int Handshake { get; set; } = 0; // None
        public int ReadTimeout { get; set; } = 500;
        public int WriteTimeout { get; set; } = 500;
    }
}
