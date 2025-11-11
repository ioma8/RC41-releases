using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public int Skip(int addr)
        {
            while (ram[addr] == 0x00) addr--;
            addr -= isize(addr);
            return addr;
        }
    }
}
