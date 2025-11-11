using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public void Sst()
        {
            int addr;
            int adr;
            int byt;
            int reg;
            int line;
            if (FlagSet(52))
            {
                line = ram[REG_E + 0] | ((ram[REG_E + 1] & 0x0f) << 8);
                line++;
                ram[REG_E + 0] = (byte)(line & 0xff);
                ram[REG_E + 1] &= 0xf0;
                ram[REG_E + 1] |= (byte)((line >> 8) & 0x0f);
                if (line == 1) return;
                addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
                reg = (addr & 0xfff);
                byt = (addr >> 12) & 0xf;
                adr = (reg * 7) + byt;
                adr--;
                while (ram[adr] == 0) adr--;
                if (ram[adr] < 0xc0 || ram[adr] >= 0xce || ram[adr - 2] >= 0xf0)
                {
                    adr -= isize(adr);
                    adr++;
                    reg = adr / 7;
                    byt = adr % 7;
                    addr = reg | (byt << 12);
                    ram[REG_B + 0] = (byte)(addr & 0xff);
                    ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
                }
                else
                {
                    GotoLine(1);
                }
            }
            else
            {
                if (FlagSet(F_SYS) && !FlagSet(F_ALPHA)) EndNumber();
                SetFlag(F_SST);
                running = true;
                ram[REG_E + 1] |= 0x0f;
                ram[REG_E + 0] = 0xff;
                addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
                addr = FromPtr(addr) - 1;
                addr = Exec(addr);
                addr = ToPtr(addr + 1);
                ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
                ram[REG_B + 0] = (byte)(addr & 0xff);
                running = false;
                ClearFlag(F_SST);
            }
        }
    }
}
