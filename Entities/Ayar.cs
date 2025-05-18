using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Entities
{
    public class Ayar
    {
        [Key]
        public int Id { get; set; }
        public string Port { get; set; }
        public string RemoteDbServerName { get; set; }
        public string RemoteDbUserName { get; set; }
        public string RemoteDbPassword { get; set; }
        public string RemoteDbDatabaseName { get; set; }

        public string YemekSepetiCurrentToken { get; set; }
    }
}
