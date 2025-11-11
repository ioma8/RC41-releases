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
        public void setDate()
        {
            int m, d, y;
            double o, n;
            int days;
            Number x;
            DateTime now = DateTime.FromOADate(DateTime.Now.ToOADate() + offset);
            x = cpu.RecallNumber(Cpu.R_X);
            if (x.sign !=0 && x.sign != 9)
            {
                cpu.Message("ALPHA DATA");
                cpu.Error();
                return;
            }
            if (x.sign != 0)
            {
                cpu.Message("DATA ERROR");
                cpu.Error();
                return;
            }
            if (cpu.FlagSet(31))
            {
                d = x.Int();
                m = x.Decimal(0, 2);
                y = x.Decimal(2, 4);
            }
            else
            {
                m = x.Int();
                d = x.Decimal(0, 2);
                y = x.Decimal(2, 4);
            }
            if (y < 1900 || y > 2199 || m < 1 || m > 12 || d < 1)
            {
                cpu.Message("DATA ERROR");
                cpu.Error();
                return;
            }
            days = (m == 1 || m == 3 || m == 5 || m == 7 || m == 8 || m == 10 || m == 12) ? 31 : 30;
            if (m == 2) days = DateTime.DaysInMonth(y, m);
            if (d > days)
            {
                cpu.Message("DATA ERROR");
                cpu.Error();
                return;
            }
            DateTime dt = new DateTime(y, m, d, now.Hour, now.Minute, now.Second);
            o = DateTime.Now.ToOADate();
            n = dt.ToOADate();
            offset = n - o;
        }
    }
}
