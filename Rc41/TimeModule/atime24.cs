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
        public void atime24()
        {
            int h, h2, m, s, hn;
            int d;
            Number x;
            string a;
            x = cpu.RecallNumber(Cpu.R_X);
            if (x.sign != 0 && x.sign != 9)
            {
                cpu.Message("ALPHA DATA");
                cpu.Error();
                return;
            }
            h = x.Int();
            if (h < 0) h = -h;
            m = x.Decimal(0, 2);
            s = x.Decimal(2, 2);
            hn = x.Decimal(4, 2);
            h2 = (h > 12) ? h - 12 : h;
            if (x.sign == 9 && h < 12) h = 12 + h;
            d = (cpu.FlagSet(36)) ? 8 : 0;
            d += (cpu.FlagSet(37)) ? 4 : 0;
            d += (cpu.FlagSet(38)) ? 2 : 0;
            d += (cpu.FlagSet(39)) ? 1 : 0;
            a = cpu.GetAlpha();
            a += (char)(h / 10 + '0');
            a += (char)(h % 10 + '0');
            if (d > 0) a += ':';
            if (d > 0)
            {
                a += (char)(m / 10 + '0');
                a += (char)(m % 10 + '0');
            }
            if (d > 2) a += ':';
            if (d > 2)
            {
                a += (char)(s / 10 + '0');
                a += (char)(s % 10 + '0');
            }
            if (d > 4) a += '.';
            if (d > 4)
            {
                a += (char)(hn / 10 + '0');
                a += (char)(hn % 10 + '0');
            }
            if (a.Length > 24) a = a.Substring(a.Length - 24);
            cpu.SetAlpha(a);
        }
    }
}
