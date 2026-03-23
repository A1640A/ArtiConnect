using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class Z_sectorData
    {
        public Z_cinema[] cinema;

        public Z_sectorData()
        {
            cinema = new Z_cinema[8];
        }
    }

}
