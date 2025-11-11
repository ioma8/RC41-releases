using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        void Cle()
        {
            int i;
            int b;
            int ofs;
            b = ram[REG_C + 2] << 4;
            b |= ((ram[REG_C + 1] >> 4) & 0xf);
            ofs = ram[REG_C + 6] << 4;
            ofs |= ((ram[REG_C + 5] >> 4) & 0xf);
            b += ofs;
            for (i = 0; i < 6; i++)
                StoreNumber(ZERO, b + i);
        }
    }
}
