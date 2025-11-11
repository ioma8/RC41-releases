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
        public void delchr()
        {
            int i;
            int j;
            int addr;
            int size;
            int bottom;
            int end;
            int recs;
            int chr;
            int rec;
            Number x;
            int count;
            int currentPtr;
            x = cpu.RecallNumber(Cpu.R_X);
            count = x.Int();
            if (currentFile <= 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            if (ram[(currentFile - 1) * 7 + 6] != 'A')
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            addr = currentFile - 1;
            currentPtr = CurrentRecord();
            size = (ram[addr * 7 + 5] << 8) | ram[addr * 7 + 4];
            recs = RecordCount();
            rec = (ram[addr * 7 + 3] << 8) | ram[addr * 7 + 2];
            chr = (ram[addr * 7 + 1] << 8) | ram[addr * 7 + 0];
            bottom = (currentFile - (size + 1)) * 7;
            end = FindEndAscii();
            if (rec >= recs)
            {
            }
            addr = currentPtr - (chr + 1);
            if (chr + count > ram[currentPtr]) count = ram[currentPtr] - chr;
            ram[currentPtr] -= (byte)count;

            for (i = 0; i < count; i++)
            {
                for (j = addr; j > bottom; j--) ram[j] = ram[j - 1];
                ram[bottom] = 0x00;
            }

            addr = currentFile - 1;
            ram[addr * 7 + 1] = (byte)((chr >> 8) & 0xff);
            ram[addr * 7 + 0] = (byte)(chr & 0xff);

        }
    }
}
