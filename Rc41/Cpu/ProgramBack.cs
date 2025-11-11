using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public int ProgramBack()
        {
            int addr;
            int adr;
            int byt;
            int reg;
            addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
            reg = (addr & 0xfff);
            byt = (addr >> 12) & 0xf;
            adr = (reg * 7) + byt;
            adr--;
            while (ram[adr] == 0) adr--;
            while (ram[adr] >= 0x10 && ram[adr] <= 0x1c) adr--;
            adr++;
            ram[adr] = 0x00;
            adr += 2;
            reg = adr / 7;
            byt = adr % 7;
            addr = (byt << 12) | reg;
            ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
            ram[REG_B + 0] = (byte)(addr & 0xff);
            return adr;
        }
    }
}
