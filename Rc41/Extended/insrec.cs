using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public void insrec()
        {
            int addr;
            int bottom;
            int size;
            int recs;
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
            bottom = (currentFile - (size + 1)) * 7;
            addr = FindEndAscii();
            if ((addr - (alpha.Length + 1)) < bottom)
            {
                cpu.Message("END OF FL");
                cpu.Error();
                return;
            }
            for (int i=0; i<alpha.Length+1; i++)
            {
                for (int j=bottom; j<currentPtr; j++) ram[j] = ram[j+1];
            }
            addr = currentPtr;
            ram[addr--] = (byte)alpha.Length;
            for (int i = 0; i < alpha.Length; i++) ram[addr--] = (byte)alpha[i];
            addr = currentFile - 1;
            recs = (ram[addr * 7 + 3] << 8) | ram[addr * 7 + 2];
            recs++;
            ram[addr * 7 + 3] = (byte)((recs >> 8) & 0xff);
            ram[addr * 7 + 2] = (byte)(recs & 0xff);
            ram[addr * 7 + 1] = 0x00;
            ram[addr * 7 + 0] = 0x00;
        }
    }
}
