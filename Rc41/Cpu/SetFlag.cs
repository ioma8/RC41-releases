using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public void SetFlag(byte f)
        {
            int offset;
            offset = 6 - (f / 8);
            f = (byte)(f & 7);
            f = (byte)(0x80 >> f);
            ram[REG_D + offset] |= f;
        }
    }
}
