using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number Fact(Number a)
        {
            int i;
            int n;
            double r;
            string tmp;
            if (a.esign != 0 || a.sign != 0)
            {
                Message("DATA ERROR");
                Error();
                return a;
            }
            if (a.exponent[0] != 0)
            {
                Message("DATA ERROR");
                Error();
                return a;
            }
            for (i = a.exponent[1] + 1; i < 10; i++)
                if (a.mantissa[i] != 0)
                {
                    Message("DATA ERROR");
                    Error();
                    return a;
                }
            n = a.mantissa[0];
            for (i = 1; i <= a.exponent[1]; i++)
            {
                n *= 10;
                n += a.mantissa[i];
            }
            if (n > 69)
            {
                Message("OUT OF RANGE");
                Error();
                return a;
            }
            r = 1;
            for (i = 1; i <= n; i++)
            {
                r *= i;
            }
            tmp = $"{r:e10}";
            a = AtoN(tmp);
            return a;
        }
    }
}
