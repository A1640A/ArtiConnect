using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class SubeStokSayimKartiRequestModel
    {
        [Required]
        public string SubeAdi { get; set; }

        [Required]
        public int StokHareketKartiId { get; set; }
    }
}
