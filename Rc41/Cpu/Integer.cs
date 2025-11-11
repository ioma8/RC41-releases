using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number Integer(Number a)
        {
            int i;
            int e;
            if (a.esign != 0) return ZERO;
            e = (a.exponent[0] * 10) + a.exponent[1];
            if (e < 10)
            {
                for (i = 1 + e; i < 10; i++) a.mantissa[i] = 0;
            }
            return a;
        }
    }
}
