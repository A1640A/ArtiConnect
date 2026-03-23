using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class SubeMasrafFilterModel
    {
        public int? SubeId { get; set; }
        public int? MasrafKategorisiId { get; set; }
        public int? OdemeYontemiId { get; set; }
        public DateTime? BaslangicTarih { get; set; }
        public DateTime? BitisTarih { get; set; }
        public float? MinTutar { get; set; }
        public float? MaxTutar { get; set; }
        public string SearchTerm { get; set; }
        public string SortBy { get; set; } = "tarih";
        public string SortDirection { get; set; } = "DESC";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}