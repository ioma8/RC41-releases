using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public void Del(int n)
        {
            int addr;
            int s;
            char relink;
            int lineNumber;
            relink = 'N';
            if (FlagSet(52) == false) return;
            lineNumber = ((ram[REG_E + 1] & 0x0f) << 8) | ram[REG_E + 0];
            if (lineNumber == 0xfff)
            {
                FixLineNumber();
                lineNumber = ((ram[REG_E + 1] & 0x0f) << 8) | ram[REG_E + 0];
            }
            if (lineNumber > 0) lineNumber--;
            addr = FromPtr((ram[REG_B + 1] << 8) | ram[REG_B + 0]);
            addr--;
            while (n > 0)
            {
                while (ram[addr] == 0x00) addr--;
                if (ram[addr] >= 0xc0 && ram[addr] <= 0xcd && ram[addr - 2] >= 0xf0) relink = 'Y';
                if (ram[addr] >= 0xc0 && ram[addr] <= 0xcd && ram[addr - 2] < 0xf0) n = 0;
                else
                {
                    s = isize(addr);
                    while (s > 0)
                    {
                        ram[addr--] = 0;
                        s--;
                    }
                    n--;
                }
            }
            if (relink == 'Y') ReLink();
            ram[REG_E + 1] = (byte)((lineNumber >> 8) & 0xff);
            ram[REG_E + 0] = (byte)(lineNumber & 0xff);
            if (lineNumber > 0) GotoLine(lineNumber);
        }
    }
}
