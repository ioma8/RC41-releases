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
            window.DisplayTimer.Interval = 500;
            window.Display(cpu.Display(), true);
        }
    }
}
