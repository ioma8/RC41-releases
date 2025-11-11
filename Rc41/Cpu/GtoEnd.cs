using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public void GtoEnd()
        {
            int n;
            int end;
            int adr;
            int ptr;
            int r00;
            r00 = (ram[REG_C + 2] << 4) | ((ram[REG_C + 1] >> 4) & 0x0f);
            r00 *= 7;
            end = ((ram[REG_C + 1] & 0x0f) << 8) | (ram[REG_C + 0] & 0xff);
            end *= 7;
            adr = end + 3;
            while (ram[adr] == 0x00 && adr < r00) adr++;
            if (adr < r00)
            {
                if (ram[adr + 2] < 0xc0 || ram[adr + 2] > 0xcd || ram[adr] >= 0xf0)
                {
                    ptr = ((ram[end + 2] & 0x0f) << 8) | ram[end + 1];
                    ram[end - 7] = 0x2d;
                    ram[end - 6] = 0x01;
                    ram[end - 5] = 0xc2;
                    ram[end + 2] = (byte)(0xc0 | ((ptr & 0xf00) >> 8));
                    ram[end + 1] = (byte)(ptr & 0xff);
                    ram[end] = 0x0d;
                    end -= 7;
                    end /= 7;
                    ram[REG_C + 1] &= 0xf0;
                    ram[REG_C + 1] |= (byte)(((end >> 8) & 0x0f));
                    ram[REG_C + 0] = (byte)(end & 0xff);
                }
            }
            n = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
            ram[REG_B + 0] = (byte)(n & 0xff);
            ram[REG_B + 1] = (byte)(0x30 | ((n >> 8) & 0x0f));
            ram[REG_E + 0] = 0x00;
            ram[REG_E + 1] &= 0xf0;
        }
    }
}
