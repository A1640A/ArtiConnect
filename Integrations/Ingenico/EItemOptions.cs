using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public enum EItemOptions
    {
        ITEM_OPTION_TAX_EXCEPTION_TAXLESS = 4096,
        ITEM_TAX_EXCEPTION_VAT_INCLUDED_TO_PRICE = 4096,
        ITEM_TAX_EXCEPTION_VAT_NOT_INCLUDED_TO_PRICE = 32768
    } 
}
