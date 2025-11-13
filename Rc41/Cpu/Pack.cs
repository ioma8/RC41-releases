using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {

        public int NextGlobal(int addr)
        {
            int next;
            next = ((ram[addr] << 8) | ram[addr - 1]) & 0xfff;
            if (next == 0x000) addr = ((ram[REG_C + 2] << 4) | ((ram[REG_C + 1] >> 4) & 0x0f)) * 7;
            else
                addr += (((next & 0x1ff) * 7) + ((next >> 9) & 0x7)) - 1;
            return addr;
        }

        public void Pack()
        {
            int i;
            int a;
            int addr;
            int prg;
            int end;
            int r00;
            int next;
            int count;
            prg = FromPtr((ram[REG_B + 1] << 8) | ram[REG_B + 0]);
            end = (((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0]) * 7 + 2;
            r00 = ((ram[REG_C + 2] << 4) | ((ram[REG_C + 1] >> 4) & 0x0f));
            r00 = r00 * 7;
            ui.Display("PACKING", false);
            addr = end;
            while (addr < r00)
            {
                count = addr;
                while (ram[addr - 2] < 0xf0 && ((ram[addr-2] & 0x20) == 0) && ram[addr - 3] == 0x00 && --count > end)
                {
                    for (i = addr - 3; i > end + 1; i--)
                        ram[i] = ram[i - 1];
                    if (prg <= addr) prg++;
                    ram[end + 1] = 0x00;
                }
                if ((ram[addr - 2] & 0x04) == 0)
                {
                    next = NextGlobal(addr);
                    while (next < r00 && ram[next - 2] >= 0xf0)
                    {
                        next = NextGlobal(next);
                    }
                    addr = next;
                }
                else
                {
                    ram[addr - 2] &= 0xfb;
                    next = NextGlobal(addr);
                    while (next < r00 && ram[next-2] >= 0xf0)
                    {
                        a = next;
                        while (a > addr)
                        {
                            if (ram[a] >= 0xb1 && ram[a] <= 0xbf)
                            {
                                ram[a - 1] = 0x00;
                            }
                            if (ram[a] >= 0xd0 && ram[a] <= 0xdf)
                            {
                                ram[a] &= 0xf0;
                                ram[a - 1] = 0x00;
                                ram[a - 2] &= 0x7f;
                            }
                            count = a;
                            while (ram[a] == 0x00 && --count > end)
                            {
                                for (i = a; i > end + 1; i--)
                                    ram[i] = ram[i - 1];
                                if (prg <= addr) prg++;
                                ram[end + 1] = 0x00;
                                addr++;
                            }
                            a -= isize(a);
                        }
                        addr = next;
                        next = NextGlobal(next);
                    }
                    addr = next;
                }
            }
            count = r00;
            while (ram[r00-1] == 0x00 && --count > end)
            {
                for (i = r00-1; i > end + 1; i--)
                    ram[i] = ram[i - 1];
                if (prg <= addr) prg++;
                ram[end + 1] = 0x00;
            }
            end -= 2;
            a = 0;
            for (i = 3; i < 13; i++) a += ram[end + i];
            while (a == 0 && end <r00-7)
            {
                for (i=0; i<3; i++)
                {
                    ram[end + 7 + i] = ram[end + i];
                    ram[end + i] = 0x00;
                }
                end += 7;
                a = 0;
                for (i = 0; i < 7; i++) a += ram[end + 7 + i];
            }
            end /= 7;
            ram[REG_C + 1] &= 0xf0;
            ram[REG_C + 1] |= (byte)(((end >> 8) & 0x0f));
            ram[REG_C + 0] = (byte)(end & 0xff);
            if (prg > r00) prg = r00;
            prg = ToPtr(prg);
            ram[REG_B + 1] = (byte)((prg >> 8) & 0xff);
            ram[REG_B + 0] = (byte)((prg & 0xff));
            ReLink();
        }

    }
}
