using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public int ToPtr(int addr)
        {
            int reg;
            int byt;
            reg = addr / 7;
            byt = addr % 7;
            return (reg & 0xfff) | (byt << 12);
        }
    }
}
