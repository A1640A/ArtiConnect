using ArtiConnect.Api.Modals.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class StokKartiFilterModel
    {
        public StokTuru? StokTuru { get; set; }
        public int? StokKartiKategoriId { get; set; }
        public int? DepoId { get; set; }
        public string SearchTerm { get; set; }

        // Sorting
        public string SortBy { get; set; } = "Id";
        public string SortDirection { get; set; } = "ASC";

        // Paging
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}