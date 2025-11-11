using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number Sqrt(Number a)
        {
            int i;
            bool flag;
            DNumber x;
            DNumber e;
            DNumber last;
            DNumber t;
            if (a.sign != 0)
            {
                Message("DATA ERROR");
                Error();
                return a;
            }
            x = NumberToDNumber(a);
            e = D_Div(x, D_TWO);

            flag = true;
            while (flag)
            {
                last = e;
                t = D_Div(x, e);
                t = D_Add(t, e);
                e = D_Mul(t, D_HALF);

                flag = false;
                for (i = 0; i < 15; i++)
                    if (last.mantissa[i] != e.mantissa[i]) flag = true;
            }
            return DNumberToNumber(e);
        }
    }
}
