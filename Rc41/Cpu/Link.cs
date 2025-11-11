using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        void Link(int address)
        {
            int adr;
            int reg;
            int byt;
            int next;
            int diff;
            int lnext;
            int prev;
            next = FindNextGlobal(address - 1);
            lnext = ((ram[next] & 0x0f) << 8) | ram[next - 1];
            prev = 0;
            if (lnext != 0)
            {
                prev = ((lnext & 0x1ff) * 7) + ((lnext >> 9) & 0x7) + next + byteCount - 1;
            }
            diff = 1 + address - next;
            reg = diff / 7;
            byt = diff % 7;
            adr = (byt << 9) | reg;
            ram[next] &= 0xf0;
            ram[next] |= (byte)((adr >> 8) & 0x0f);
            ram[next - 1] = (byte)(adr & 0xff);
            if (lnext != 0)
            {
                diff = 1 + prev - address;
                reg = diff / 7;
                byt = diff % 7;
                adr = (byt << 9) | reg;
                ram[address] &= 0xf0;
                ram[address] |= (byte)((adr >> 8) & 0x0f);
                ram[address - 1] = (byte)(adr & 0xff);
            }
            byteCount = 0;
        }
    }
}
