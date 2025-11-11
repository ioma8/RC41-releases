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
        public void appchr()
        {
            int i;
            int j;
            int addr;
            int size;
            int bottom;
            int end;
            int recs;
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
            end = FindEndAscii();
            if (rec >= recs)
            { 
            }
            if (end - alpha.Length < bottom)
            {
                cpu.Message("END OF FL");
                cpu.Error();
                return;
            }
            if (ram[currentPtr] + alpha.Length > 255)
            {
                cpu.Message("REC TOO LONG");
                cpu.Error();
                return;
            }
            addr = currentPtr - (ram[currentPtr] + 1);
            ram[currentPtr] += (byte)alpha.Length;
            for (i=0; i<alpha.Length; i++)
            {
                for (j = bottom; j < addr; j++) ram[j] = ram[j + 1];
                ram[addr--] = (byte)alpha[i];
            }
        }
    }
}
