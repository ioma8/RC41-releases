using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        void ClearLinks(int address)
        {
            int pstart;
            int pend;
            if (linksCleared) return;
            linksCleared = true;
            pend = FindEnd(address);
            pstart = FindStart(address);
            while (ram[pstart] == 0x00) pstart--;
            while (pstart > pend)
            {
                if ((ram[pstart] & 0xf0) == 0xb0)
                {
                    ram[pstart - 1] = 0;
                }
                if ((ram[pstart] & 0xf0) == 0xd0)
                {
                    ram[pstart] &= 0xf0;
                    ram[pstart - 1] = 0;
                }
                if ((ram[pstart] & 0xf0) == 0xe0)
                {
                    ram[pstart] &= 0xf0;
                    ram[pstart - 1] = 0;
                }
                pstart -= isize(pstart);
            }
        }
    }
}
