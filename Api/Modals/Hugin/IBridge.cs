using Hugin.ExDevice;
using Hugin.POS.CompactPrinter.FP300;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.Hugin
{
    public interface IBridge
    {
        IConnection Connection { get; }
        void Log(String log);
        void Log();
        ICompactPrinter Printer { get; }
    }
}
