using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        // ********************************************************************
        // ***** This function rebuilds the global chain.  This is called *****
        // ***** after a program is inserted or deleted in order to make  *****
        // ***** sure the global chain is valid                           *****
        // ********************************************************************
        public void ReLink()
        {
            int end;
            int count;
            int last;
            int len;
            int regs;
            int bytes;
            int address;
            bool first;
            end = 0;
            count = 0;
            address = (ram[REG_C + 2] << 4) | ((ram[REG_C + 1] >> 4) & 0x0f);
            address *= 7;
            last = address;
            first = true;
            address++;
            while (end == 0)
            {
                if (ram[address] >= 0xc0 && ram[address] <= 0xcd && count > 0)
                {
                    len = last - address;
                    regs = len / 7;
                    bytes = len % 7;
                    len = (bytes << 9) | (regs & 0x1ff);
                    if (first) len = 0;
                    first = false;
                    ram[address] &= 0xf0;
                    ram[address] |= (byte)((len >> 8) & 0x0f);
                    ram[address - 1] = (byte)(len & 0xff);
                    if ((ram[address - 2] & 0xf0) == 0x20) end = -1;
                    else
                    {
                        last = address + 1;
                        count = isize(address);
                        address -= isize(address);
                    }
                }
                else
                {
                    count += isize(address);
                    address -= isize(address);
                }
            }
        }
    }
}
