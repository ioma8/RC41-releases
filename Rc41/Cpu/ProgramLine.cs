using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        string ProgramLine()
        {
            int addr;
            int reg;
            int byt;
            int adr;
            int lineNumber;
            int end;
            string buffer;
            addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
            reg = (addr & 0xfff);
            byt = (addr >> 12) & 0xf;
            adr = (reg * 7) + byt;
            if (FlagSet(F_PRGM) && FlagSet(F_SYS) && FlagSet(F_ALPHA)) adr = alphaPos + 1;
              else if (FlagSet(F_PRGM) && FlagSet(F_PARTIAL) && FlagSet(F_ALPHA)) adr = alphaPos + 1;
            lineNumber = ram[REG_E + 0] + ((ram[REG_E + 1] & 0x0f) << 8);
            if (lineNumber == 0xfff)
            {
                FixLineNumber();
                lineNumber = ram[REG_E + 0] + ((ram[REG_E + 1] & 0x0f) << 8);
            }
            end = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
            if (lineNumber == 0)
            {
                buffer = $"00 REG {end - FindBottom():d2}";
            }
            else
            {
                adr--;
                buffer = ProgramList(lineNumber, adr);
            }

            return buffer;
        }
    }
}
