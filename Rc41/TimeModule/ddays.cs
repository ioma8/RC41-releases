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
        public void ddays()
        {
            Number x;
            Number y;
            x = cpu.RecallNumber(Cpu.R_X);
            DateTime? dx = todatetime(x);
            if (dx == null) return;
            y = cpu.RecallNumber(Cpu.R_Y);
            DateTime? dy = todatetime(y);
            if (dy == null) return;
            x = new Number((int)(((DateTime)dx).ToOADate()-((DateTime)dy).ToOADate()));
            cpu.StoreNumber(x, Cpu.R_X);
            x = cpu.RecallNumber(Cpu.R_Z);
            cpu.StoreNumber(x, Cpu.R_Y);
            x = cpu.RecallNumber(Cpu.R_T);
            cpu.StoreNumber(x, Cpu.R_Z);
        }
    }
}
