using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public void getrec()
        {
            int addr;
            int recs;
            int len;
            int chr;
            int rec;
            int currentPtr;
            string record;
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
            recs = RecordCount();
            rec = ram[addr * 7 + 3] * 256 + ram[addr * 7 + 2];
            chr = ram[addr * 7 + 1] * 256 + ram[addr * 7 + 0];
            if (rec >= recs)
            {
                cpu.Message("END OF FL");
                cpu.Error();
                return;
            }
            len = 24;
            record = "";
            while (len > 0 && chr < ram[currentPtr])
            {
                record += (char)ram[currentPtr - (chr + 1)];
                len--;
                chr++;
            }
            if (chr >= ram[currentPtr])
            {
                rec++;
                currentPtr -= (ram[currentPtr] + 1);
                chr = 0;
                cpu.ClearFlag(17);
            }
            else cpu.SetFlag(17);
            cpu.SetAlpha(record);
            addr = currentFile - 1;
            ram[addr * 7 + 3] = (byte)((rec >> 8) & 0xff);
            ram[addr * 7 + 2] = (byte)(rec & 0xff);
            ram[addr*7+1] = (byte)((chr >> 8) & 0xff);
            ram[addr * 7 + 0] = (byte)(chr & 0xff);
        }

    }
}
