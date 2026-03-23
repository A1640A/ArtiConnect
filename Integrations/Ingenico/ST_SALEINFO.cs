using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_SALEINFO
    {
        public byte ItemType;

        public ulong ItemPrice;

        public ulong IncAmount;

        public ulong DecAmount;

        public uint OrigialItemAmount;

        public ushort OriginalItemAmountCurrency;

        public ushort ItemVatRate;

        public ushort ItemCurrencyType;

        public byte ItemVatIndex;

        public byte ItemCountPrecision;

        public int ItemCount;

        public byte ItemUnitType;

        public byte DeptIndex;

        public uint Flag;

        public string Name;

        public string Barcode;

        public ST_SALEINFO()
        {
            Name = "";
            Barcode = "";
        }
    }

}
