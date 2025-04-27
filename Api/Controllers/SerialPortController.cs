using ArtiConnect.Api.Modals;
using ArtiConnect.DataAccess;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ArtiConnect.Api.Controllers
{
    [ApiLogger]
    [RoutePrefix("api/serialPort")]
    public class SerialPortController : BaseApiController
    {
        private AppDbContext db = new AppDbContext();
        private static SerialPort _serialPort;
        private static List<string> _receivedData = new List<string>();
        private static object _lockObject = new object();

        /// <summary>
        /// Sistemdeki tüm COM portlarını listeler
        /// </summary>
        [HttpGet]
        [Route("list")]
        public IHttpActionResult GetSerialPorts()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                return Ok(new { Ports = ports });
            }
            catch (Exception ex)
            {
                return BadRequest($"COM portları listelenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirtilen COM portunu açar
        /// </summary>
        [HttpPost]
        [Route("open")]
        public IHttpActionResult OpenSerialPort(SerialPortRequest request)
        {
            try
            {
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    return BadRequest("Zaten açık bir COM portu var. Önce mevcut portu kapatın.");
                }

                _serialPort = new SerialPort
                {
                    PortName = request.PortName,
                    BaudRate = request.BaudRate,
                    Parity = (Parity)request.Parity,
                    DataBits = request.DataBits,
                    StopBits = (StopBits)request.StopBits,
                    Handshake = (Handshake)request.Handshake,
                    ReadTimeout = request.ReadTimeout,
                    WriteTimeout = request.WriteTimeout
                };

                // Veri alındığında çağrılacak event handler
                _serialPort.DataReceived += SerialPort_DataReceived;

                // Portu aç
                _serialPort.Open(); 

                return Ok(new { Success = true, Message = $"{request.PortName} portu başarıyla açıldı." });
            }
            catch (Exception ex)
            {
                return BadRequest($"COM portu açılırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Açık olan COM portunu kapatır
        /// </summary>
        [HttpPost]
        [Route("close")]
        public IHttpActionResult CloseSerialPort()
        {
            try
            {
                if (_serialPort == null)
                {
                    return BadRequest("Açık bir COM portu bulunmuyor.");
                }

                if (_serialPort.IsOpen)
                {
                    _serialPort.DataReceived -= SerialPort_DataReceived;
                    _serialPort.Close();
                    _serialPort.Dispose();
                    _serialPort = null;

                    return Ok(new { Success = true, Message = "COM portu başarıyla kapatıldı." });
                }
                else
                {
                    return BadRequest("COM portu zaten kapalı.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"COM portu kapatılırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// COM portuna veri gönderir
        /// </summary>
        [HttpPost]
        [Route("send")]
        public IHttpActionResult SendData(SendDataRequest request)
        {
            try
            {
                if (_serialPort == null || !_serialPort.IsOpen)
                {
                    return BadRequest("Açık bir COM portu bulunmuyor.");
                }

                byte[] data;

                if (request.IsHex)
                {
                    // Hex string'i byte array'e çevir
                    data = HexStringToByteArray(request.Data);
                }
                else
                {
                    // Normal string'i byte array'e çevir
                    data = Encoding.ASCII.GetBytes(request.Data);
                }

                // Veriyi gönder
                _serialPort.Write(data, 0, data.Length);

                return Ok(new { Success = true, Message = "Veri başarıyla gönderildi." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Veri gönderilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// COM portundan alınan verileri okur
        /// </summary>
        [HttpGet]
        [Route("read")]
        public IHttpActionResult ReadData(bool clearBuffer = false)
        {
            try
            {
                if (_serialPort == null || !_serialPort.IsOpen)
                {
                    return BadRequest("Açık bir COM portu bulunmuyor.");
                }

                List<string> data;
                lock (_lockObject)
                {
                    data = new List<string>(_receivedData);

                    if (clearBuffer)
                    {
                        _receivedData.Clear();
                    }
                }

                return Ok(new { Data = data });
            }
            catch (Exception ex)
            {
                return BadRequest($"Veri okunurken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// COM port durumunu kontrol eder
        /// </summary>
        [HttpGet]
        [Route("status")]
        public IHttpActionResult GetStatus()
        {
            try
            {
                if (_serialPort == null)
                {
                    return Ok(new { IsOpen = false });
                }

                return Ok(new
                {
                    IsOpen = _serialPort.IsOpen,
                    PortName = _serialPort.PortName,
                    BaudRate = _serialPort.BaudRate,
                    Parity = _serialPort.Parity,
                    DataBits = _serialPort.DataBits,
                    StopBits = _serialPort.StopBits,
                    Handshake = _serialPort.Handshake,
                    BytesToRead = _serialPort.BytesToRead,
                    BytesToWrite = _serialPort.BytesToWrite
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"COM port durumu kontrol edilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Veri alındığında çağrılan event handler
        /// </summary>
        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                string data = sp.ReadExisting();

                lock (_lockObject)
                {
                    _receivedData.Add(data);

                    // Buffer'ı belirli bir boyutta tut
                    if (_receivedData.Count > 1000)
                    {
                        _receivedData.RemoveAt(0);
                    }
                }
            }
            catch (Exception)
            {
                // Event handler'da hata yakalamak önemlidir
                // Ancak burada log kaydı yapmak zor olabilir
            }
        }

        /// <summary>
        /// Hex string'i byte array'e çevirir
        /// </summary>
        private byte[] HexStringToByteArray(string hex)
        {
            // Boşlukları temizle
            hex = hex.Replace(" ", "").Replace("-", "");

            // Tek sayıda karakter varsa başına 0 ekle
            if (hex.Length % 2 != 0)
            {
                hex = "0" + hex;
            }

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }  
    }
}
