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
        public void dow()
        {
            Number x;
            x = cpu.RecallNumber(Cpu.R_X);
            DateTime? dt = todatetime(x);
            if (dt == null) return;
            x = new Number((int)((DateTime)dt).DayOfWeek);
            cpu.ram[Cpu.LIFT] = (byte)'D';
            cpu.StoreNumber(x, Cpu.R_X);
        }
    }
}
