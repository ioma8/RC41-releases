using Rc41.T_Cpu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public void savex()
        {
            int i;
            int size;
            int addr;
            int rec;
            Number x;
            if (currentFile == 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            addr = currentFile-1;
            if (ram[addr*7+6] != 'D')
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            size = ram[addr*7+5] *256 + ram[addr * 7 + 4];
            rec = ram[addr * 7 + 3] * 256 + ram[addr * 7 + 2];
            if (rec >= size)
            {
                cpu.Message("END OF FL");
                cpu.Error();
                return;
            }
            x = cpu.RecallNumber(Cpu.R_X);
            addr--;
            addr -= rec;
            SaveRegister(addr, x);
            rec++;
            addr = currentFile - 1;
            ram[addr * 7 + 3] = (byte)((rec >> 8) & 0xff);
            ram[addr * 7 + 2] = (byte)(rec & 0xff);
        }
    }
}
