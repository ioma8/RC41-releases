using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        void EReg(byte post)
        {
            int b;
            int e;
            b = ram[REG_C + 2] << 4;
            b |= ((ram[REG_C + 1] >> 4) & 0xf);
            e = 0xffff;
            if (post < 0x70) e = post;
            if (post >= 0x80 && post <= 0xef)
            {
                e = ToInteger(RecallNumber(b + (post & 0x7f)));
            }
            if (post >= 0xf0 && post <= 0xff)
            {
                e = ToInteger(RecallNumber(post & 0x0f));
            }
            if (b + e > 0xfff - 6)
            {
                Message("NONEXISTENT");
                Error();
                return;
            }
            ram[REG_C + 6] = (byte)((e >> 4) & 0xff);
            ram[REG_C + 5] &= 0x0f;
            ram[REG_C + 5] |= (byte)((e & 0xf) << 4);
        }
    }
}
