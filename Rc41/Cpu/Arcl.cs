using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        void Arcl(int rreg)
        {
            int i;
            int p;
            int reg;
            int b;
            string buffer;
            Number n;
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
                if (reg > 0xfff)
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
            buffer = "";
            if (ram[reg * 7 + 6] == 0x10)
            {
                p = 0;
                for (i = 5; i >= 0; i--)
                    if (ram[reg * 7 + i] != 0x00)
                        buffer += ((char)ram[reg * 7 + i]).ToString();
            }
            else
            {
                n = RecallNumber(reg);
                buffer = Format(n);
                while (buffer[0] == ' ')
                {
                    buffer = buffer.Substring(1);
                }
            }
            p = 0;
            while (p < buffer.Length && buffer[p] != 0)
            {
                for (i = (REG_P + 2); i > (REG_M); i--) ram[i] = ram[i - 1];
                ram[REG_M] = (byte)buffer[p++];
            }
            if (FlagSet(F_ALPHA) && !running)
            {
                SetFlag(F_ALPHA_IN);
            }
        }
    }
}
