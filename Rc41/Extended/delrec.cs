using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public void delrec()
        {
            int i;
            int j;
            int addr;
            int bottom;
            int size;
            int recs;
            int count;
            int chr;
            int rec;
            int currentPtr;
            string alpha = cpu.GetAlpha();
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
            currentPtr = CurrentRecord();
            addr = currentFile - 1;
            size = (ram[addr * 7 + 5] << 8) | ram[addr * 7 + 4];
            recs = RecordCount();
            rec = (ram[addr * 7 + 3] << 8) | ram[addr * 7 + 2];
            bottom = (currentFile - (size + 1)) * 7;
            if (rec >= recs)
            {
                cpu.Message("END OF FL");
                cpu.Error();
                return;
            }
            count = ram[currentPtr] + 1;
            while (count > 0)
            {
                for (i = currentPtr; i > bottom; i--) ram[i] = ram[i - 1];
                ram[bottom] = 0x00;
                count--;
            }

            chr = 0;
            ram[addr * 7 + 1] = (byte)((chr >> 8) & 0xff);
            ram[addr * 7 + 0] = (byte)(chr & 0xff);
        }
    }
}
