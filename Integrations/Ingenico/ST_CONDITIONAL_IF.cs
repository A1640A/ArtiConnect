using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public struct ST_CONDITIONAL_IF
    {
        public EConditionIds id;

        public EConditionTest eTestFormule;

        public ulong ui64TestValue;

        public ushort IfTrue_GOTO;

        public ushort IfFalse_GOTO;
    }

}
