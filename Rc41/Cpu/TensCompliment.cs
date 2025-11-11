using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number TensCompliment(Number a)
        {
            int i;
            int c;
            c = 1;
            for (i = 9; i >= 0; i--)
            {
                a.mantissa[i] = (byte)((9 - a.mantissa[i]) + c);
                if (a.mantissa[i] > 9)
                {
                    a.mantissa[i] -= 10;
                    c = 1;
                }
                else c = 0;
            }
            return a;
        }
    }
}
