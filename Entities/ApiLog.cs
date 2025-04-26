using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Entities
{
    public class ApiLog
    {
        [Key]
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public string RequestData { get; set; }
        public string ResponseData { get; set; }
        public int StatusCode { get; set; }
    }
}
