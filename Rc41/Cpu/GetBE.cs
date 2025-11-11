using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public void GetBE(Number x, ref int b, ref int e)
        {
            int i;
            int p;
            int n;
            b = 0;
            e = 0;
            p = 0;
            if (x.esign == 0)
            {
                b = x.mantissa[p++];
                n = x.exponent[0] * 10 + x.exponent[1];
                while (n > 0)
                {
                    b *= 10;
                    if (p < 10) b += x.mantissa[p++];
                    n--;
                }
            }
            else
            {
                n = x.exponent[0] * 10 + x.exponent[1];
                while (n > 0)
                {
                    n--;
                    p--;
                }
                p++;
            }
            for (i = 0; i < 3; i++)
            {
                e *= 10;
                if (p >= 0 && p < 10) e += x.mantissa[p];
                p++;
            }
        }
    }
}
