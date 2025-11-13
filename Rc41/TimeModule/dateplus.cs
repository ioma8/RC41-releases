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
        public void dateplus()
        {
            int m, d, y;
            int days;
            Number x;
            Number ret;
            int p;
            x = cpu.RecallNumber(Cpu.R_Y);
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
            DateTime dt = new DateTime(y, m, d);
            x = cpu.RecallNumber(Cpu.R_X);
            d = x.Int();
            dt = dt.AddDays(d);


            p = 0;
            ret = new Number(0);
            if (cpu.FlagSet(31))
            {
                if (dt.Day > 9)
                {
                    ret.mantissa[p++] = (byte)(dt.Day / 10);
                    ret.mantissa[p++] = (byte)(dt.Day % 10);
                    ret.exponent[1] = 1;
                }
                else
                {
                    ret.mantissa[p++] = (byte)dt.Day;

                }
                ret.mantissa[p++] = (byte)(dt.Month / 10);
                ret.mantissa[p++] = (byte)(dt.Month % 10);
            }
            else
            {
                if (dt.Month > 9)
                {
                    ret.mantissa[p++] = 1;
                    ret.mantissa[p++] = (byte)(dt.Month - 10);
                    ret.exponent[1] = 1;
                }
                else
                {
                    ret.mantissa[p++] = (byte)dt.Month;

                }
                ret.mantissa[p++] = (byte)(dt.Day / 10);
                ret.mantissa[p++] = (byte)(dt.Day % 10);
            }
            y = dt.Year;
            ret.mantissa[p++] = (byte)(y / 1000);
            y = y % 1000;
            ret.mantissa[p++] = (byte)(y / 100);
            y = y % 100;
            ret.mantissa[p++] = (byte)(y / 10);
            ret.mantissa[p++] = (byte)(y % 10);
            cpu.ram[Cpu.LIFT] = (byte)'D';
            cpu.StoreNumber(ret, Cpu.R_X);
            x = cpu.RecallNumber(Cpu.R_Z);
            cpu.StoreNumber(x, Cpu.R_Y);
            x = cpu.RecallNumber(Cpu.R_T);
            cpu.StoreNumber(x, Cpu.R_Z);
        }
    }
}
