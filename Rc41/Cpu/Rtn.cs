using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        int Rtn(int addr)
        {
            int i;
            if (running)
            {
                if (ram[REG_B + 2] == 0x00 && ram[REG_B + 3] == 0x00)
                {
                    running = false;
                    GotoLine(0);
                    for (i = REG_B + 2; i < REG_B + 7; i++) ram[i] = 0x00;
                    for (i = REG_A + 0; i < REG_A + 7; i++) ram[i] = 0x00;
                    ram[REG_E] = 0x00;
                    ram[REG_E + 1] &= 0xf0;
                    return 0;
                }
                for (i = REG_B + 0; i < REG_B + 5; i++) ram[i] = ram[i + 2];
                ram[REG_B + 5] = ram[REG_A + 0];
                ram[REG_B + 6] = ram[REG_A + 1];
                for (i = REG_A + 0; i < REG_A + 5; i++) ram[i] = ram[i + 2];
                ram[REG_A + 5] = 0x00;
                ram[REG_A + 6] = 0x00;
                addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
                addr = FromPtr(addr);
                return addr - 1;
            }
            else
            {
                GotoLine(0);
                for (i = REG_B + 2; i < REG_B + 7; i++) ram[i] = 0x00;
                for (i = REG_A + 0; i < REG_A + 7; i++) ram[i] = 0x00;
            }
            return addr - 1;
        }
    }
}
