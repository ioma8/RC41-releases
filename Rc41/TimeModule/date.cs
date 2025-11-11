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
        public void date()
        {
            int p;
            int y;
            Number ret = new Number();
            DateTime now  = DateTime.FromOADate(DateTime.Now.ToOADate() + offset);
            p = 0;
            if (cpu.FlagSet(31))
            {
                if (now.Day > 9)
                {
                    ret.mantissa[p++] = (byte)(now.Day / 10);
                    ret.mantissa[p++] = (byte)(now.Day % 10);
                    ret.exponent[1] = 1;
                }
                else
                {
                    ret.mantissa[p++] = (byte)now.Day;

                }
                ret.mantissa[p++] = (byte)(now.Month / 10);
                ret.mantissa[p++] = (byte)(now.Month % 10);
            }
            else
            {
                if (now.Month > 9)
                {
                    ret.mantissa[p++] = 1;
                    ret.mantissa[p++] = (byte)(now.Month - 10);
                    ret.exponent[1] = 1;
                }
                else
                {
                    ret.mantissa[p++] = (byte)now.Month;

                }
                ret.mantissa[p++] = (byte)(now.Day / 10);
                ret.mantissa[p++] = (byte)(now.Day % 10);
            }
            y = now.Year;
            ret.mantissa[p++] = (byte)(y / 1000);
            y = y % 1000;
            ret.mantissa[p++] = (byte)(y / 100);
            y = y % 100;
            ret.mantissa[p++] = (byte)(y / 10);
            ret.mantissa[p++] = (byte)(y % 10);
            cpu.StoreNumber(ret, Cpu.R_X);
        }
    }
}
