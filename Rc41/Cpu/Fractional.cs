using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number Fractional(Number a)
        {
            int i;
            int e;
            if (a.esign != 0) return a;
            e = (a.exponent[0] * 10) + a.exponent[1];
            while (e >= 0)
            {
                for (i = 0; i < 9; i++) a.mantissa[i] = a.mantissa[i + 1];
                e--;
            }
            while (a.mantissa[0] == 0 && IsZero(a) == false)
            {
                for (i = 0; i < 9; i++) a.mantissa[i] = a.mantissa[i + 1];
                e--;
            }
            if (IsZero(a)) return ZERO;
            a.esign = 9;
            e = -e;
            a.exponent[0] = (byte)(e / 10);
            a.exponent[1] = (byte)(e % 10);
            return a;
        }
    }
}
