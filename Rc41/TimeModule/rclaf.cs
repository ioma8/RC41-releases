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
        public void rclaf()
        {
            Number x = new Number();
            cpu.StoreNumber(x, Cpu.R_X);
        }
    }
}
