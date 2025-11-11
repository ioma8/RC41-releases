using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number Normalize(Number n)
        {
            int i;
            int e;
            if (IsZero(n)) return n;
            e = (n.exponent[0] * 10) + n.exponent[1];
            if (n.esign != 0) e = -e;
            while (n.mantissa[0] == 0)
            {
                for (i = 0; i < 9; i++) n.mantissa[i] = n.mantissa[i + 1];
                n.mantissa[9] = 0;
                e--;
            }
            if (e < 0)
            {
                n.esign = 9;
                e = -e;
            }
            else n.esign = 0;
            n.exponent[0] = (byte)(e / 10);
            n.exponent[1] = (byte)(e % 10);
            return n;
        }
    }
}
