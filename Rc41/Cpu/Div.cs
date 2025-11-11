using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number Div(Number a, Number b)
        {
            DNumber da;
            DNumber db;
            DNumber dc;
            if (IsZero(b))
            {
                Message("DATA ERROR");
                Error();
                return b;
            }
            da = NumberToDNumber(a);
            db = NumberToDNumber(b);
            dc = D_Div(da, db);
            return DNumberToNumber(dc);
        }
    }
}
