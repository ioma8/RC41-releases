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
        public void saverx()
        {
            int i;
            int b;
            int e;
            int size;
            int addr;
            int count;
            int rec;
            Number x;
            x = cpu.RecallNumber(Cpu.R_X);
            b = x.Int();
            e = x.Decimal(0, 3);
            count = (e-b)+1;
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
            if (rec+count > size)
            {
                cpu.Message("END OF FL");
                cpu.Error();
                return;
            }
            addr--;
            addr -= rec;
            for (i = b; i <= e; i++)
            {
                x = cpu.Rcl(i);
                SaveRegister(addr, x);
                rec++;
                addr--;
            }
            addr = currentFile - 1;
            ram[addr * 7 + 3] = (byte)((rec >> 8) & 0xff);
            ram[addr * 7 + 2] = (byte)(rec & 0xff);
        }

    }
}

