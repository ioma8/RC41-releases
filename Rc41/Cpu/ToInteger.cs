using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public int ToInteger(Number a)
        {
            int e;
            int r;
            int p;
            if (a.esign != 0) return 0;
            e = (a.exponent[0] * 10) + a.exponent[1];
            r = a.mantissa[0];
            p = 1;
            while (e > 0)
            {
                r *= 10;
                if (p < 10) r += a.mantissa[p++];
                e--;
            }
            return r;
        }
    }
}
