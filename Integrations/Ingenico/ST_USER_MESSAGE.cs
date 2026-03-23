using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_USER_MESSAGE
    {
        public uint flag;

        public ushort len;

        public string message;

        public ST_USER_MESSAGE()
        {
            message = "";
        }
    }

}
