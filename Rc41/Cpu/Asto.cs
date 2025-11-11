using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        void Asto(int rreg)
        {
            int i;
            int p;
            int l;
            int reg;
            int b;
            b = ram[REG_C + 2] << 4;
            b |= ((ram[REG_C + 1] >> 4) & 0xf);
            if (rreg < 0x70)
            {
                reg = b + rreg;
            }
            else if (rreg >= 0x70 && rreg <= 0x7f) reg = rreg - 0x70;
            else if (rreg >= 0x80 && rreg <= 0xef)
            {
                reg = b + (rreg - 0x80);
                if (reg > 0x1ff)
                {
                    Message("NONEXISTENT");
                    Error();
                    return;
                }
                reg = b + ToInteger(RecallNumber(reg));
            }
            else if (rreg >= 0xf0 && rreg <= 0xff)
            {
                reg = b + ToInteger(RecallNumber(rreg - 0xf0));
            }
            else reg = 0x200;
            if (reg > 0xfff)
            {
                Message("NONEXISTENT");
                Error();
                return;
            }
            reg *= 7;
            p = (REG_P) + 2;
            while (p > (REG_M) && ram[p] == 0) p--;
            if (p == (REG_M) && ram[p] == 0)
            {
                ram[reg + 6] = 0x10;
                for (i = 0; i < 6; i++) ram[reg + i] = 0x00;
            }
            else
            {
                l = p - (REG_M) + 1;
                if (l > 6) l = 6;
                for (i = 0; i < 6; i++) ram[reg + i] = 0x00;
                i = 5;
                ram[reg + 6] = 0x10;
                while (l > 0)
                {
                    for (i = 5; i > 0; i--) ram[reg + i] = ram[reg + i - 1];
                    ram[reg] = ram[p--];
                    l--;
                }
            }
        }
    }
}
