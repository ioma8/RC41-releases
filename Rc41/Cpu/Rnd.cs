using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        void Rnd()
        {
            int i;
            int s;
            int decimals;
            int mode;
            int e;
            Number a;
            decimals = 0;
            if (FlagSet(36)) decimals |= 8;
            if (FlagSet(37)) decimals |= 4;
            if (FlagSet(38)) decimals |= 2;
            if (FlagSet(39)) decimals |= 1;
            mode = 0;
            if (FlagSet(40)) mode |= 2;
            if (FlagSet(41)) mode |= 1;
            a = RecallNumber(R_X);
            if (mode == 0)
            {
                for (i = decimals; i < 10; i++)
                    if (i + 1 < 10) a.mantissa[i + 1] = 0;
            }
            if (mode == 1)
            {
                e = (a.exponent[0] * 10) + a.exponent[1];
                s = e % 3;
                for (i = decimals + s; i < 10; i++)
                    if (i + 1 < 10) a.mantissa[i + 1] = 0;
            }
            if (mode == 2)
            {
                e = (a.exponent[0] * 10) + a.exponent[1];
                for (i = decimals + e; i < 10; i++)
                    if (i + 1 < 10) a.mantissa[i + 1] = 0;
            }
            StoreNumber(a, R_X);
        }
    }
}
