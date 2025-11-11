using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TimeModule
{
    public partial class TimeModule
    {
        public void ClockDisplay()
        {
            string disp;
            int h, m, s, h2;
            DateTime now = DateTime.FromOADate(DateTime.Now.ToOADate() + offset);
            h = now.Hour;
            m = now.Minute;
            s = now.Second;
            h2 = (h < 13) ? h : h - 12;
            if (clktd == 'T')
            {
                if (clkmode == 12)
                {
                    disp = $"{h2}:{m:d02}:{s:d02} ";
                    disp += (h < 12) ? "AM" : "PM";
                }
                else
                {
                    disp = $"{h:d02}:{m:d02}:{s:d02} ";
                }
                cpu.Message(disp);
                return;
            }
            else
            {
                if (clkmode == 12)
                {
                    disp = $"{h2}:{m:d02}";
                    disp += (h < 12) ? "AM " : "PM ";
                }
                else
                {
                    disp = $"{h:d02}:{m:d02}   ";
                }
                if (cpu.FlagSet(31))
                    disp += $"{now.Day:d02}.{now.Month:d02}";
                else
                    disp += $"{now.Month:d02}/{now.Day:d02}";
                cpu.Message(disp);
                return;
            }
        }
    }
}
