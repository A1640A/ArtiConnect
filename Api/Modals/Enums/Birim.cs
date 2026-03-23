using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals.Enums
{
    public enum Birim
    {
        [Description("Ad")]
        Adet,
        [Description("Kg")]
        Kilogram,
        [Description("Gr")]
        Gram,
        [Description("Lt")]
        Litre,
        [Description("Ml")]
        Mililitre,
        [Description("Cl")]
        Cl,
        [Description("Kutu")]
        Kutu,
        [Description("Koli")]
        Koli,
        [Description("Kasa")]
        Kasa,
        [Description("Paket")]
        Paket
    }
}