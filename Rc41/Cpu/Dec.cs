using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number Dec(Number a)
        {
            int i;
            int n;
            string tmp;
            if (a.esign != 0)
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
            for (i = 0; i < 10; i++)
                if (a.mantissa[i] > 7)
                {
                    Message("DATA ERROR");
                    Error();
                    return a;
                }
            n = a.mantissa[0];
            for (i = 1; i <= a.exponent[1]; i++)
            {
                n *= 8;
                n += a.mantissa[i];
            }
            tmp = $"{n:d}";
            a = AtoN(tmp);
            return a;
        }
    }
}
