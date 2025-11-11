using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public void clfl()
        {
            int i;
            int j;
            int addr;
            int len;
            string alpha = cpu.GetAlpha();
            if (alpha.Length > 7) alpha = alpha.Substring(0, 7);
            while (alpha.Length < 7) alpha += " ";
            if (alpha.Equals("       "))
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            addr = FindFile(alpha);
            if (addr < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            addr--;
            if (ram[addr*7+6] != 'D' && ram[addr*7+6] != 'A')
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            len = ram[addr * 7 + 5] * 256 + ram[addr * 7 + 4];
            ram[addr * 7 + 3] = 0x00;
            ram[addr * 7 + 2] = 0x00;
            ram[addr * 7 + 1] = 0x00;
            ram[addr * 7 + 0] = 0x00;
            currentFile = addr + 1;
            addr--;
            for (i=0; i<len; i++)
            {
                for (j = 0; j < 7; j++) ram[addr * 7 + j] = 0;
                addr--;
            }
            addr = currentFile - 1;
            if (ram[addr * 7 + 6] == 'A') ram[addr * 7 - 1] = 0xff;
        }
    }
}
