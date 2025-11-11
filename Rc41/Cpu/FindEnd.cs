using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public int FindEnd(int address)
        {
            while (ram[address] < 0xc0 || ram[address] >= 0xce || ram[address - 2] >= 0xf0)
                address -= isize(address);
            return address;
        }
    }
}
