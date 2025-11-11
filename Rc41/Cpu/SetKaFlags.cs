using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public void SetKaFlags()
        {
            int i;
            int l;
            int addr;
            int reg;
            int byt;
            for (i = REG_E + 3; i <= REG_E + 6; i++) ram[i] = 0x00;
            for (i = REG_R + 3; i <= REG_R + 6; i++) ram[i] = 0x00;
            ram[REG_E + 2] &= 0x0f;
            ram[REG_R + 2] &= 0x0f;
            addr = 0x0c0 * 7;
            while (ram[addr + 6] == 0xf0)
            {
                if (ram[addr + 4] != 0x00) SetKaFlag(ram[addr + 3], true);
                if (ram[addr + 1] != 0x00) SetKaFlag(ram[addr + 0], true);
                addr += 7;
            }
            addr = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
            addr *= 7;
            addr += 2;
            l = ((ram[addr] & 0x0f) << 8) | ram[addr - 1];
            while (l != 0x000)
            {
                reg = l & 0x1ff;
                byt = (l >> 9) & 0x7;
                addr += (reg * 7 + byt);
                addr--;
                l = ((ram[addr] & 0x0f) << 8) | ram[addr - 1];
                if (ram[addr - 2] >= 0xf0)
                {
                    if (ram[addr - 3] != 0x00) SetKaFlag(ram[addr - 3], true);
                }
            }
        }
    }
}
