using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TimeModule
{
    public partial class TimeModule
    {
        public string timeToAscii(double t)
        {
            int d, h, m, s, hn;
            d = (int)t;
            t -= d;
            t *= 86400;
            hn = (int)Math.Truncate((t - Math.Truncate(t)) * 100);
            h = (int)(t / 3600);
            t -= (h * 3600);
            m = (int)(t / 60);
            t -= (m * 60);
            s = (int)t;
            h += (d * 24);
            if (h >= 100) h = h % 100;
            return $"{h:d02}:{m:d02}:{s:d02}.{hn:d02}";
        }

        public string timeToAscii(Number x)
        {
            int h, m, s, hn;
            h = x.Int();
            if (h < 0) h = -h;
            m = x.Decimal(0, 2);
            s = x.Decimal(2, 2);
            hn = x.Decimal(4, 2);
            return $"{h:d02}:{m:d02}:{s:d02}.{hn:d02}";
        }
    }
}
