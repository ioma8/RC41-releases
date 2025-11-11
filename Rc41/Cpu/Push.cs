using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        void Push(int address)
        {
            int i;
            address = ToPtr(address);
            for (i = 6; i > 1; i--) ram[REG_A + i] = ram[REG_A + i - 2];
            ram[REG_A + 1] = ram[REG_B + 6];
            ram[REG_A + 0] = ram[REG_B + 5];
            for (i = 6; i > 1; i--) ram[REG_B + i] = ram[REG_B + i - 2];
            ram[REG_B + 3] = (byte)((address >> 8) & 0xff);
            ram[REG_B + 2] = (byte)(address & 0xff);
        }
    }
}
