using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number Hr(Number a)
        {
            HMS h;
            string tmp;
            double d;
            h = GetHms(a);
            d = h.minutes * 60 + h.seconds + ((double)h.fractional / 100.0);
            d /= 3600.0;
            d += h.hours;
            tmp = $"{d:e12}";
            a = AtoN(tmp);
            return a;
        }
    }
}
