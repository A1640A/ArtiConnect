using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class SubeUrunFilterModel
    {
        public int? SubeId { get; set; }
        public int? SubeUrunGrubuId { get; set; }
        public int? UrunBirimiId { get; set; }
        public bool? Favori { get; set; }
        public bool? PosEkranindaGoster { get; set; }
        public bool? ElTerminalindeGoster { get; set; }
        public string SearchTerm { get; set; }
        public string SortBy { get; set; } = "Id";
        public string SortDirection { get; set; } = "ASC";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}