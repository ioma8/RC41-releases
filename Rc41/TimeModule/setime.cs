using Rc41.T_Cpu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TimeModule
{
    public partial class TimeModule
    {
        public void setime()
        {
            int h, m, s, hn;
            double o, n;
            Number x;
            DateTime now = DateTime.FromOADate(DateTime.Now.ToOADate() + offset);

            x = cpu.RecallNumber(Cpu.R_X);
            if (x.sign != 0 && x.sign != 9)
            {
                cpu.Message("ALPHA DATA");
                cpu.Error();
                return;
            }
            h = x.Int();
            m = x.Decimal(0, 2);
            s = x.Decimal(2, 2);
            hn = x.Decimal(4, 2);
            if (x.sign == 9) h = 12 + h;
            if (h < 0 || h > 23 || m < 0 || m > 59 || s < 0 || s > 59 || hn < 0 || hn > 99)
            {
                cpu.Message("DATA ERROR");
                cpu.Error();
                return;
            }
            DateTime dt = new DateTime(now.Year, now.Month, now.Day, h, m, s);
            o = DateTime.Now.ToOADate();
            n = dt.ToOADate();
            offset = n - o;

        }
    }
}
