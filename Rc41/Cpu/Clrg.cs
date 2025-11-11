using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        void Clrg()
        {
            int b;
            b = ram[REG_C + 2] << 4;
            b |= ((ram[REG_C + 1] >> 4) & 0xf);
            while (b <= 0x1ff)
            {
                StoreNumber(ZERO, b);
                b++;
            }
        }
    }
}
