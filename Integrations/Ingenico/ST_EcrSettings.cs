using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public struct ST_EcrSettings
    {
        public byte InvoiceSettings;

        public byte Z_Settings;

        public ushort Z_Time_In_Minute;

        public byte Copy_Button_Secured;

        public ushort Backlight_Timeout;

        public ushort Backlight_Level;

        public ushort Keylock_Timeout;
    }

}
