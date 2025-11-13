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
        public void EndSw()
        {
            cpu.calculatorMode = Cpu.CM_DIRECT;
            ui.SetDisplayTimerInterval(500);
            ui.Display(cpu.Display(), true);
        }
    }
}
