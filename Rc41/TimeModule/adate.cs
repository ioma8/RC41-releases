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
        public void adate()
        {
            int m, d, y;
            int dg;
            double o, n;
            int days;
            Number x;
            x = cpu.RecallNumber(Cpu.R_X);
            if (x.sign != 0 && x.sign != 9)
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
            dg = (cpu.FlagSet(36)) ? 8 : 0;
            dg += (cpu.FlagSet(37)) ? 4 : 0;
            dg += (cpu.FlagSet(38)) ? 2 : 0;
            dg += (cpu.FlagSet(39)) ? 1 : 0;
            string a = cpu.GetAlpha();
            if (cpu.FlagSet(31))
            {
                if (d > 9) a += (char)(d / 10 + '0');
                a += (char)(d % 10 + '0');
                if (dg > 0)
                {
                    a += '/';
                    a += (char)(m / 10 + '0');
                    a += (char)(m % 10 + '0');
                }
                if (dg > 2) a += '/';
                if (dg > 4)
                {
                    a += (char)(y / 1000 + '0');
                    y = y % 1000;
                    a += (char)(y / 100 + '0');
                    y = y % 100;
                    a += (char)(y / 10 + '0');
                    a += (char)(y % 10 + '0');
                }
                else if (dg > 2)
                {
                    y = y % 1000;
                    y = y % 100;
                    a += (char)(y / 10 + '0');
                    a += (char)(y % 10 + '0');
                }
            }
            else
            {
                if (m > 9) a += (char)(m / 10 + '0');
                a += (char)(m % 10 + '0');
                if (dg > 0)
                {
                    a += '/';
                    a += (char)(d / 10 + '0');
                    a += (char)(d % 10 + '0');
                }
                if (dg > 2) a += '/';
                if (dg > 4)
                {
                    a += (char)(y / 1000 + '0');
                    y = y % 1000;
                    a += (char)(y / 100 + '0');
                    y = y % 100;
                    a += (char)(y / 10 + '0');
                    a += (char)(y % 10 + '0');
                }
                else if (dg > 2)
                {
                    y = y % 1000;
                    y = y % 100;
                    a += (char)(y / 10 + '0');
                    a += (char)(y % 10 + '0');
                }
            }

            if (a.Length > 24) a = a.Substring(a.Length - 24);
            cpu.SetAlpha(a);

        }
    }
}
