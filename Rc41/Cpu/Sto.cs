using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public void Sto(Number a, int rreg)
        {
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
            if (reg > 0x1ff)
            {
                Message("NONEXISTENT");
                Error();
                return;
            }
            StoreNumber(a, reg);
        }
    }
}
