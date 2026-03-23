using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_DATABASE_LINE
    {
        public int indexOfLine;

        public int numberOfColomns;

        public ST_DATABASE_COLOMN[] pstColomnArray;

        public ST_DATABASE_LINE()
        {
            pstColomnArray = new ST_DATABASE_COLOMN[50];
        }
    }

}
