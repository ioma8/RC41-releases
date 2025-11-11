using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public Number RecallNumber(int reg)
        {
            Number a = new Number();
            if (reg > 0x1ff)
            {
                Message("NONEXISTENT");
                Error();
                return ZERO;
            }
            reg *= 7;
            a.sign = (byte)((ram[reg + 6] >> 4) & 0xf);
            a.mantissa[0] = (byte)(ram[reg + 6] & 0xf);
            a.mantissa[1] = (byte)((ram[reg + 5] >> 4) & 0xf);
            a.mantissa[2] = (byte)(ram[reg + 5] & 0xf);
            a.mantissa[3] = (byte)((ram[reg + 4] >> 4) & 0xf);
            a.mantissa[4] = (byte)(ram[reg + 4] & 0xf);
            a.mantissa[5] = (byte)((ram[reg + 3] >> 4) & 0xf);
            a.mantissa[6] = (byte)(ram[reg + 3] & 0xf);
            a.mantissa[7] = (byte)((ram[reg + 2] >> 4) & 0xf);
            a.mantissa[8] = (byte)(ram[reg + 2] & 0xf);
            a.mantissa[9] = (byte)((ram[reg + 1] >> 4) & 0xf);
            a.esign = (byte)(ram[reg + 1] & 0xf);
            a.exponent[0] = (byte)((ram[reg + 0] >> 4) & 0xf);
            a.exponent[1] = (byte)(ram[reg + 0] & 0xf);
            return a;
        }
    }
}
