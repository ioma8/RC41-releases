using Rc41.T_Cpu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Printer
{
    public partial class Printer
    {
        public void prkeys()
        {
            int addr;
            int x;
            int y;
            int c;
            int i;
            string buffer;
            Print("", 'L');
            addr = 0x0c0 * 7;
            c = 0;
            while (cpu.ram[addr + 6] == 0xf0)
            {
                if (cpu.ram[addr + 0] != 0)
                {
                    if (c == 0) Print("USER KEYS:", 'L');
                    c++;
                    buffer = "";
                    if ((cpu.ram[addr + 0] & 0x0f) >= 0x01 && (cpu.ram[addr + 0] & 0x0f) <= 0x08)
                    {
                        x = ((cpu.ram[addr + 0] + 0x10) & 0xf0) >> 4;
                        y = (cpu.ram[addr + 0] + 0x10) & 0x0f;
                        if (y == 4 && x > 1) x--;
                        buffer += $" {y}{x}: ";
                    }
                    else
                    {
                        x = (((cpu.ram[addr + 0] & 0xf7)+ 0x10) & 0xf0) >> 4;
                        y = ((cpu.ram[addr + 0] & 0xf7) + 0x10) & 0x0f;
                        if (y == 4 && x > 1) x--;
                        buffer += $"-{y}{x}: ";
                    }
                    buffer += DecodeInstruction(cpu.ram[addr + 2], cpu.ram[addr + 1]);
                    Print(buffer, 'L');
                }
                if (cpu.ram[addr + 3] != 0)
                {
                    if (c == 0) Print("USER KEYS:", 'L');
                    c++;
                    buffer = "";
                    if ((cpu.ram[addr + 3] & 0x0f) >= 0x01 && (cpu.ram[addr + 3] & 0x0f) <= 0x08)
                    {
                        x = ((cpu.ram[addr + 3] + 0x10) & 0xf0) >> 4;
                        y = (cpu.ram[addr + 3] + 0x10) & 0x0f;
                        if (y == 4 && x > 1) x--;
                        buffer += $" {y}{x}: ";
                    }
                    else
                    {
                        x = (((cpu.ram[addr + 3] & 0xf7) + 0x10) & 0xf0) >> 4;
                        y = ((cpu.ram[addr + 3] & 0xf7) + 0x10) & 0x0f;
                        if (y == 4 && x > 1) x--;
                        buffer += $"-{y}{x}: ";
                    }
                    buffer += DecodeInstruction(cpu.ram[addr + 5], cpu.ram[addr + 4]);
                    Print(buffer, 'L');
                }
                addr += 7;
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
                            if (c == 0) Print("USER KEYS:", 'L');
                            c++;
                            buffer = "";
                            if ((cpu.ram[addr - 3] & 0x0f) <= 0x08) buffer += $" {cpu.ram[addr - 3] + 0x10:x02}: \"";
                            else buffer += $"-{(cpu.ram[addr - 3] & 0xf7) + 0x10:x02}: \"";
                            for (i = 1; i < (cpu.ram[addr - 2] & 0x0f); i++)
                                if (cpu.ram[addr - 3 - i] == 0) buffer += "_";
                                else buffer += ((char)cpu.ram[addr - 3 - i]).ToString();
                            Print(buffer, 'L');
                        }
                    }
                }
                addr -= cpu.isize(addr);
            }
            if (c == 0) Print("USER KEYS:NONE", 'L');

        }
    }
}
