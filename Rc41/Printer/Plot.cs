using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Printer
{
    public partial class Printer
    {
        public void Plot(double ymin, double ymax, double value, int be)
        {
            int i;
            int s = be / 1000;
            int e = be % 1000;
            double range = ymax - ymin;
            double col = range / s;
            double x = (value - ymin) / col;
            if (ymin >= ymax || s == 0 || s > 168)
            {
                cpu.Message("DATA ERROR");
                cpu.Error();
                return;
            }
            if (e == 0) e = (int)((0 - ymin) / col);
            i = 0;
            while (i < (int)x - 1)
            {
                if (e != 0 && i == e) PrintToBuffer(0xff);
                else PrintToBuffer(0x80);
                i++;
            }
            PrintToBuffer(20 + 0x80);
            PrintToBuffer(8 + 0x80);
            PrintToBuffer(20 + 0x80);
            i++;
            while (i < s)
            {
                if (e != 0 && i == e) PrintToBuffer(0xff);
                else PrintToBuffer(0x80);
                i++;
            }
            PrintBuffer('R');
        }
    }
}
