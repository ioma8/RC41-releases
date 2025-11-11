using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TimeModule
{
    public partial class TimeModule
    {
        public void SwDisplay()
        {
            int d, h, m, s, hn;
            int p;
            double t;
            string disp;
            Number x;
            Number y;
            if (swHold)
            {
                if (deltaMode)
                {
                    x = cpu.Rcl(nextSplit);
                    if (deltaMode && nextSplit > 0)
                    {
                        y = cpu.Rcl(nextSplit - 1);
                        x = cpu.Sub(x, y);
                    }
                    disp = timeToAscii(x);
                }
                else
                {
                    t = swHoldValue;
                    disp = timeToAscii(t);
                }
            }
            else
            {
                if (swMode == 'S')
                {
                    t = swAccumulated;
                    if (swRunning == 'Y')
                    {
                        t += (DateTime.Now.ToOADate() - swStart);
                    }
                    disp = timeToAscii(t);
                }
                else
                {
                    x = cpu.Rcl(nextRcl);
                    if (deltaMode && nextRcl > 0)
                    {
                        y = cpu.Rcl(nextRcl - 1);
                        x = cpu.Sub(x, y);
                    }
                    disp = timeToAscii(x);
                }
            }
            if (swRunning == 'Y' && swHold == false && swMode == 'S')
                disp = disp.Substring(0, disp.Length - 1) + " ";
            if (!suppressReg)
            {
                disp += (swMode == 'S') ? "\x81" : "=";
                if (deltaMode) disp += "D";
                else disp += "R";
                if (enteringNum)
                {
                    if (numEntry == 0) disp += "__";
                    else if (numEntry == 1) disp += $"{number}_";
                }
                else
                {
                    if (swMode == 'S')
                    {
                        disp += $"{nextSplit:d02}";
                    }
                    else
                    {
                        disp += $"{nextRcl:d02}";
                    }
                }
            }
            cpu.Message(disp);
        }
    }
}
