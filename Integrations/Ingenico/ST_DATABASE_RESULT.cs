using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_DATABASE_RESULT
    {
        public int numberOfLines;

        public ST_DATABASE_LINE[] pstCaptionArray;

        public ST_DATABASE_LINE[] pstLineArray;

        public ST_DATABASE_RESULT()
        {
            pstCaptionArray = new ST_DATABASE_LINE[50];
            pstLineArray = new ST_DATABASE_LINE[50];
        }
    }

}
