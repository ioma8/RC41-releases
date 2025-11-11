using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public void Size(int n)
        {
            int adr;
            int end;
            int r00;
            int n00;
            int dst;
            int src;
            int dif;
            int bot;
            int sz;
            bot = 0x0c0;
            while (ram[bot * 7 + 6] != 0x00) bot++;
            end = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
            r00 = (ram[REG_C + 2] << 4) | ((ram[REG_C + 1] & 0xf0) >> 4);
            sz = r00 - end;
            n00 = 0x200 - n;
            dif = n00 - r00;
            if (n00-sz <= bot)
            {
                Message("NO ROOM");
                Error();
                return;
            }
            if (dif > 0)
            {
                dst = 0x1ff * 7 + 6;
                src = (0x1ff - dif) * 7 + 6;
                while (src >= (bot * 7)) ram[dst--] = ram[src--];
                while (dst >= (bot * 7)) ram[dst--] = 0x00;
            }
            else
            {
                dif = -dif;
                dst = bot * 7;
                src = (bot + dif) * 7;
                while (src <= (0x1ff * 7 + 6)) ram[dst++] = ram[src++];
                while (dst <= (0x1ff * 7 + 6)) ram[dst++] = 0x00;
                dif = -dif;
            }
            r00 += dif;
            end += dif;
            ram[REG_C + 2] = (byte)((r00 >> 4) & 0xff);
            ram[REG_C + 1] = (byte)(((r00 & 0x0f) << 4) | ((end >> 8) & 0x0f));
            ram[REG_C + 0] = (byte)(end & 0xff);
            adr = (ram[REG_B+1] << 8) | ram[REG_B + 0];
            adr = FromPtr(adr);
            adr += (dif * 7);
            adr = ToPtr(adr);
            ram[REG_B+1] = (byte)((adr >> 8) & 0xff);
            ram[REG_B + 0] = (byte)(adr & 0xff);

        }
    }
}
