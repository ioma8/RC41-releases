using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        // This function replaces the last program in memory with the passed program
        public void LoadProg(List<byte> program)
        {
            int addr;
            int l;
            int pend;
            int bottom = FindBottom();
            int regs;
            int reg;
            int byt;
            pend = ((ram[Cpu.REG_C + 1] & 0x0f) << 8) | ram[Cpu.REG_C + 0];
            regs = (program.Count + 6) / 7;
            if (pend - (regs + 1) < bottom)
            {
                Message("NO ROOM");
                Error();
                return;
            }
            addr = pend * 7 + 2;
            l = ((ram[addr] & 0x0f) << 8) | ram[addr - 1];
            reg = l & 0x1ff;
            byt = (l >> 9) & 0x7;
            addr += (reg * 7) + byt - 1;
            if (ram[addr - 2] >= 0xf0)
            {
                // Need code to handle current execution address
                for (int i = bottom * 7; i <= addr; i++) ram[i] = 0x00;
                pend = (addr / 7) -1;
                ram[pend * 7 + 2] = 0xc0;
                ram[pend * 7 + 1] = 0x00;
                ram[pend * 7 + 0] = 0x2d;
                ram[Cpu.REG_C + 1] &= 0xf0;
                ram[Cpu.REG_C + 1] |= (byte)(pend >> 8);
                ram[Cpu.REG_C + 0] = (byte)(pend & 0xff);
            }
            LoadSub(program);
        }
    }
}
