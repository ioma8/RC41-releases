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
        public void seekpt()
        {
            int addr;
            int len;
            int rec;
            int chr;
            Number x;
            int currentPtr;
            x = cpu.RecallNumber(Cpu.R_X);
            rec = x.Int();
            chr = x.Decimal(0, 3);
            if (currentFile <= 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            addr = currentFile;
            addr--;
            if (rec > 999)
            {
                cpu.Message("DATA ERROR");
                cpu.Error();
                return;
            }
            if (ram[addr * 7 + 6] == 'D')
            {
                len = ram[addr * 7 + 5] * 256 + ram[addr * 7 + 4];
                if (rec >= len)
                {
                    cpu.Message("END OF FL");
                    cpu.Error();
                    return;
                }
                ram[addr * 7 + 3] = (byte)((rec >> 8) & 0xff);
                ram[addr * 7 + 2] = (byte)(rec & 0xff);
            }
            else if (ram[addr * 7 + 6] == 'A')
            {
                currentPtr = CurrentRecord();
                len = RecordCount();
                addr--;
                if (rec > 0 && rec >= len)
                {
                    cpu.Message("END OF FL");
                    cpu.Error();
                    return;
                }
                if (chr > 255)
                {
                    cpu.Message("DATA ERROR");
                    cpu.Error();
                    return;
                }
                currentPtr = currentFile * 7 - 14 + 6;
                while (rec > 0)
                {
                    currentPtr -= (ram[currentPtr] + 1);
                    rec--;
                }
                if (chr > ram[currentPtr]) chr = ram[currentPtr];
                addr = currentFile - 1;
                rec = x.Int();
                ram[addr * 7 + 3] = (byte)((rec >> 8) & 0xff);
                ram[addr * 7 + 2] = (byte)(rec & 0xff);
                ram[addr * 7 + 1] = (byte)((chr >> 8) & 0xff);
                ram[addr * 7 + 0] = (byte)(chr & 0xff);
            }
            else
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }


        }
    }
}
