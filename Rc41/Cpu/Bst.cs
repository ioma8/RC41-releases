using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public void Bst()
        {
            int line;
            line = ram[REG_E + 0] | ((ram[REG_E + 1] & 0x0f) << 8);
            line--;
            if (line < 1) line = 65535;
            GotoLine(line);
        }
    }
}
