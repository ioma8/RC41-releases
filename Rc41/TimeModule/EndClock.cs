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
        public void EndClock()
        {
            if (cpu.calculatorMode != Cpu.CM_CLOCK) return;
            cpu.calculatorMode = Cpu.CM_DIRECT;
            cpu.ClearFlag(50);
            cpu.window.Display(cpu.Display(), false);
        }
    }
}
