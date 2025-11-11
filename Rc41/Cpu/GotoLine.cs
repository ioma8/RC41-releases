using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public void GotoLine(int line)
        {
            int addr;
            int adr;
            int l;
            addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
            adr = FromPtr(addr);
            adr--;

            adr = FindStart(adr);
            adr++;

            if (line > 1)
            {
                l = line;
                line = 1;
                while (l > 1)
                {
                    while (ram[adr - 1] == 0) adr--;
                    if (ram[adr - 1] >= 0xc0 && ram[adr - 1] < 0xce && ram[adr - 3] < 0xf0) l = 0;
                    else
                    {
                        adr -= isize(adr - 1);
                        line++;
                        l--;
                    }
                }
            }
            addr = ToPtr(adr);
            ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
            ram[REG_B + 0] = (byte)(addr & 0xff);
            ram[REG_E + 0] = (byte)(line & 0xff);
            ram[REG_E + 1] &= 0xf0;
            ram[REG_E + 1] |= (byte)(((line >> 8) & 0x0f));
        }
    }
}
