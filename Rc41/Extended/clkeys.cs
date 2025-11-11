using Rc41.T_Cpu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public void clkeys()
        {
            int i;
            int j;
            int addr;
            cpu.ram[Cpu.REG_E + 2] &= 0x0f;
            cpu.ram[Cpu.REG_E + 3] = 0x00;
            cpu.ram[Cpu.REG_E + 4] = 0x00;
            cpu.ram[Cpu.REG_E + 5] = 0x00;
            cpu.ram[Cpu.REG_E + 6] = 0x00;
            cpu.ram[Cpu.REG_R + 2] &= 0x0f;
            cpu.ram[Cpu.REG_R + 3] = 0x00;
            cpu.ram[Cpu.REG_R + 4] = 0x00;
            cpu.ram[Cpu.REG_R + 5] = 0x00;
            cpu.ram[Cpu.REG_R + 6] = 0x00;
            i = 0x0c0 * 7;
            while (cpu.ram[i + 6] == 0xf0)
            {
                for (j = 0; j < 7; j++) cpu.ram[i + j] = 0x00;
                i += 7;
            }
            addr = cpu.ram[Cpu.REG_C + 2] << 4 | (cpu.ram[Cpu.REG_C + 1] & 0xf0) >> 4;
            addr = addr * 7 - 1;
            while (cpu.ram[addr] < 0xc0 || cpu.ram[addr] >= 0xce || (cpu.ram[addr - 2] & 0xf0) != 0x20)
            {
                if (cpu.ram[addr] >= 0xc0 && cpu.ram[addr] < 0xce)
                {
                    if (cpu.ram[addr - 2] >= 0xf0)
                    {
                        if (cpu.ram[addr - 3] != 0x00)
                        {
                            cpu.ram[addr - 3] = 0x00;
                        }
                    }
                }
                addr -= cpu.isize(addr);
            }


        }
    }
}
