using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_DEPARTMENT
    {
        public string szDeptName;

        public byte u8TaxIndex;

        public ECurrency iCurrencyType;

        public EItemUnitTypes iUnitType;

        public ulong u64Limit;

        public ulong u64Price;

        public byte bLuchVoucher;

        public ST_DEPARTMENT()
        {
            szDeptName = "";
        }
    }

}
