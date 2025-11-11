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
        public void posa()
        {
            int i;
            string alpha;
            string a2;
            Number x;
            alpha = cpu.GetAlpha();
            x = cpu.RecallNumber(Cpu.R_X);
            i = -1;
            if (x.sign == 1)
            {
                a2 = x.Alpha();
                i = alpha.IndexOf(a2);
            }
            else
            {
                i = x.Int();
                i = alpha.IndexOf((char)i);
            }
            cpu.StoreNumber(new Number(i), Cpu.R_X);

        }
    }
}
