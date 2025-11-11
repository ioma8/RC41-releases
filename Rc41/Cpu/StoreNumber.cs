using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public void StoreNumber(Number n, int reg)
        {
            int i;
            if (reg == R_X && ram[LIFT] == 'E')
            {
                for (i = 0; i < 21; i++) ram[i] = ram[i + 7];
                ram[LIFT] = (byte)'D';
            }
            reg *= 7;
            ram[reg + 6] = (byte)((n.sign << 4) | (n.mantissa[0] & 0xf));
            ram[reg + 5] = (byte)((n.mantissa[1] << 4) | (n.mantissa[2] & 0xf));
            ram[reg + 4] = (byte)((n.mantissa[3] << 4) | (n.mantissa[4] & 0xf));
            ram[reg + 3] = (byte)((n.mantissa[5] << 4) | (n.mantissa[6] & 0xf));
            ram[reg + 2] = (byte)((n.mantissa[7] << 4) | (n.mantissa[8] & 0xf));
            ram[reg + 1] = (byte)((n.mantissa[9] << 4) | (n.esign & 0xf));
            ram[reg + 0] = (byte)((n.exponent[0] << 4) | (n.exponent[1] & 0xf));
        }
    }
}
