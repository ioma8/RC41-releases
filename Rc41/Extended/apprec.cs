using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public void apprec()
        {
            int addr;
            int bottom;
            int size;
            int recs;
            string alpha = cpu.GetAlpha();
            if (currentFile <= 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            if (ram[(currentFile-1)*7+6] != 'A')
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
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
            ram[addr--] = (byte)alpha.Length;
            for (int i=0; i<alpha.Length; i++) ram[addr--] = (byte)alpha[i];
            ram[addr] = 0xff;
            addr = currentFile - 1;
            recs = (ram[addr * 7 + 3] << 8) | ram[addr * 7 + 2];
            recs++;
            ram[addr*7+3] = (byte)((recs >> 8) & 0xff);
            ram[addr * 7 + 2] = (byte)(recs & 0xff);
        }
    }
}
