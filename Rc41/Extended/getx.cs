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
        public void getx()
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
            addr = currentFile - 1;
            if (ram[addr * 7 + 6] != 'D')
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            size = ram[addr * 7 + 5] * 256 + ram[addr * 7 + 4];
            rec = (ram[addr * 7 + 3] << 8) | ram[addr * 7 + 2];
            if (rec >= size)
            {
                cpu.Message("END OF FL");
                cpu.Error();
                return;
            }
            addr--;
            addr -= rec;
            x = ReadRegister(addr);
            rec++;
            cpu.StoreNumber(x, Cpu.R_X);
            addr = currentFile - 1;
            ram[addr * 7 + 3] = (byte)((rec >> 8) & 0xff);
            ram[addr * 7 + 2] = (byte)(rec & 0xff);
        }
    }
}
