using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class MasrafRaporRequestModel
    {
        [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
        public DateTime BaslangicTarih { get; set; }

        [Required(ErrorMessage = "Bitiş tarihi zorunludur.")]
        public DateTime BitisTarih { get; set; }

        [Required(ErrorMessage = "Rapor türü zorunludur.")]
        [RegularExpression("^(sube|kategori|odemeyontemi|aylik)$", ErrorMessage = "Geçersiz rapor türü. Kabul edilen değerler: sube, kategori, odemeyontemi, aylik")]
        public string RaporTuru { get; set; }
    }
}