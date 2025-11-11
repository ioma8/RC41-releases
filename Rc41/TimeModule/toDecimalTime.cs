using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TimeModule
{
    public partial class TimeModule
    {
        public double toDecimalTime(Number x)
        {
            int h, m, s, hn;
            double d;
            h = x.Int();
            if (h < 0) h = -h;
            m = x.Decimal(0, 2);
            s = x.Decimal(2, 2);
            hn = x.Decimal(4, 2);
            d = (h * 3600) + (m * 60) + s;
            d *= 100;
            d += hn;
            d /= 8640000;
            return d;
        }
    }
}
