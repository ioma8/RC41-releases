using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number HmsPlus(Number a, Number b)
        {
            string buffer;
            HMS ha;
            HMS hb;
            ha = GetHms(a);
            hb = GetHms(b);
            ha.fractional += hb.fractional;
            if (ha.fractional >= 100) { ha.fractional -= 100; ha.seconds++; }
            if (ha.fractional < 0) { ha.fractional += 100; ha.seconds--; }
            ha.seconds += hb.seconds;
            if (ha.seconds >= 60) { ha.seconds -= 60; ha.minutes++; }
            if (ha.seconds < 0) { ha.seconds += 60; ha.minutes--; }
            ha.minutes += hb.minutes;
            if (ha.minutes >= 60) { ha.minutes -= 60; ha.hours++; }
            if (ha.minutes < 0) { ha.minutes += 60; ha.hours--; }
            ha.hours += hb.hours;
            buffer = $"{ha.hours:d}.{ha.minutes:d2}{ha.seconds:d2}{ha.fractional:d2}";
            a = AtoN(buffer);
            return a;
        }
    }
}
