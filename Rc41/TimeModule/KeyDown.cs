using Rc41.T_Cpu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rc41.T_TimeModule
{
    public partial class TimeModule
    {
        public void KeyDown(string tag)
        {
            int b;
            if (tag.Equals("04"))                      // ENTER
            {
                swHoldValue = swAccumulated;
                if (swRunning == 'Y')
                {
                    swHoldValue += (DateTime.Now.ToOADate() - swStart);
                }
                Number x = tonumber(swHoldValue);
                b = cpu.ram[Cpu.REG_C + 2] << 4;
                b |= ((cpu.ram[Cpu.REG_C + 1] >> 4) & 0xf);
                b += nextSplit;
                if (b > 0x1ff)
                {
                    cpu.Message("NONEXISTENT");
                    EndSw();
                    return;
                }
                cpu.Sto(x, nextSplit);
                swHold = true;
                SwDisplay();
                ui.Display(cpu.Display(), true);
            }
        }
    }
}
