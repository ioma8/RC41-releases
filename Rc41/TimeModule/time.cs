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
        public void time()
        {
            int p;
            int h;
            Number ret = new Number();
            DateTime now = DateTime.FromOADate(DateTime.Now.ToOADate() + offset);
            p = 0;
            if (now.Hour > 9)
            {
                ret.mantissa[p++] = (byte)(now.Hour / 10);
                ret.mantissa[p++] = (byte)(now.Hour % 10);
                ret.exponent[1] = 1;
            }
            else
            {
                ret.mantissa[p++] = (byte)now.Hour;

            }
            ret.mantissa[p++] = (byte)(now.Minute / 10);
            ret.mantissa[p++] = (byte)(now.Minute % 10);
            ret.mantissa[p++] = (byte)(now.Second / 10);
            ret.mantissa[p++] = (byte)(now.Second % 10);
            h = now.Millisecond / 10;
            ret.mantissa[p++] = (byte)(h / 10);
            ret.mantissa[p++] = (byte)(h % 10);
            cpu.StoreNumber(ret, Cpu.R_X);

        }
    }
}
