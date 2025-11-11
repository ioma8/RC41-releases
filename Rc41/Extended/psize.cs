using Rc41.T_Cpu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public void psize()
        {
            int s;
            Number x;
            x = cpu.RecallNumber(Cpu.R_X);
            s = x.Int();
            if (s > 999)
            {
                cpu.Message("DATA ERROR");
                cpu.Error();
                return;
            }
            cpu.Size(s);
        }
    }
}
