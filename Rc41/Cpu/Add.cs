using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public Number Add(Number a, Number b)
        {
            DNumber da;
            DNumber db;
            DNumber dc;
            da = NumberToDNumber(a);
            db = NumberToDNumber(b);
            dc = D_Add(da, db);
            return DNumberToNumber(dc);
        }
    }
}
