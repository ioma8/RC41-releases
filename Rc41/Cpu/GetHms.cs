using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        HMS GetHms(Number a)
        {
            int e;
            int p;
            HMS ret = new HMS();
            e = a.exponent[0] * 10 + a.exponent[1];
            if (a.esign != 0) e = -e;
            p = 0;
            ret.hours = 0;
            ret.minutes = 0;
            ret.seconds = 0;
            ret.fractional = 0;
            while (e >= 0)
            {
                if (p < 10) ret.hours = (ret.hours * 10) + a.mantissa[p++];
                else ret.hours *= 10;
                e--;
            }
            e++;
            while (e < 0) { p++; e++; }
            if (p < 10) ret.minutes = a.mantissa[p++] * 10;
            if (p < 10) ret.minutes += a.mantissa[p++];
            if (p < 10) ret.seconds = a.mantissa[p++] * 10;
            if (p < 10) ret.seconds += a.mantissa[p++];
            if (p < 10) ret.fractional = a.mantissa[p++] * 10;
            if (p < 10) ret.fractional += a.mantissa[p++];
            if (a.sign != 0)
            {
                ret.hours = -ret.hours;
                ret.minutes = -ret.minutes;
                ret.seconds = -ret.seconds;
                ret.fractional = -ret.fractional;
            }
            return ret;
        }
    }
}
