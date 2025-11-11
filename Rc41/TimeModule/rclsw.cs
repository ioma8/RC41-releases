using Rc41.T_Cpu;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TimeModule
{
    public partial class TimeModule
    {
        public void rclsw()
        {
            int d, h, m, s, hn;
            int p;
            double t;
            Number x;
            t = swAccumulated;
            if (swRunning == 'Y')
            {
                t += (DateTime.Now.ToOADate() - swStart);
            }
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
            x = new Number();
            if (h >= 100)
            {
                h = 99;
                m = 59;
                s = 59;
                hn = 99;
            }
            p = 0;
            if (h > 9)
            {
                x.mantissa[p++] = (byte)(h / 10);
                x.mantissa[p++] = (byte)(h % 10);
                x.exponent[1] = 1;
            }
            else
            {
                x.mantissa[p++] = (byte)h;
            }
            if (p < 10) x.mantissa[p++] = (byte)(m / 10);
            if (p < 10) x.mantissa[p++] = (byte)(m % 10);
            if (p < 10) x.mantissa[p++] = (byte)(s / 10);
            if (p < 10) x.mantissa[p++] = (byte)(s % 10);
            if (p < 10) x.mantissa[p++] = (byte)(hn / 10);
            if (p < 10) x.mantissa[p++] = (byte)(hn % 10);
            cpu.StoreNumber(x, Cpu.R_X);
        }
    }
}
