using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        int GtoAlpha(int address)
        {
            int i;
            int n;
            int addr;
            string token;

            ram[REG_E + 1] |= 0x0f;
            ram[REG_E + 0] = 0xff;
            token = "";
            for (i = 0; i < (ram[address] & 0xf); i++)
                token += ((char)ram[address - 1 - i]).ToString();
            n = FindGlobal(token);
            if (n != 0)
            {
                addr = n;
                n = ToPtr(n);
                ram[REG_B + 1] = (byte)((n >> 8) & 0xff);
                ram[REG_B + 0] = (byte)(n & 0xff);
                return addr;
            }
            Message("NONEXISTENT");
            Error();
            return 0;
        }
    }
}
