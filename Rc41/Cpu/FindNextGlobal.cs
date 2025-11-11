using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        int FindNextGlobal(int address)
        {
            while (ram[address] < 0xc0 || ram[address] >= 0xce)
                address -= isize(address);
            return address;
        }
    }
}
