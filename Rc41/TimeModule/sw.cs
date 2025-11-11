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
        public void sw()
        {
            cpu.calculatorMode = Cpu.CM_SW;
            nextSplit = 0;
            nextRcl = 0;
            swMode = 'S';
            swShift = false;
            suppressReg = false;
            deltaMode = false;
            enteringNum = false;
            numEntry = -1;
            SwDisplay();
            window.DisplayTimer.Interval = 100;
        }
    }
}
