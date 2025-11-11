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
        public void crfld()
        {
            string alpha;
            Number x;
            int len;
            int addr;
            int p;
            int i;
            int j;
            alpha = cpu.GetAlpha();
            if (alpha.Length > 7) alpha = alpha.Substring(0, 7);
            while (alpha.Length < 7) alpha += " ";
            if (alpha.Equals("       "))
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            if (FindFile(alpha) >= 0)
            {
                cpu.Message("DUP FL");
                cpu.Error();
                return;
            }
            x = cpu.RecallNumber(Cpu.R_X);
            len = x.Int();
            if (len == 0)
            {
                cpu.Message("DATA ERROR");
                cpu.Error();
                return;
            }
            addr = FindEnd();
            if (addr - (len + 3) < 0)
            {
                cpu.Message("NO ROOM");
                cpu.Error();
                return;
            }
            currentFile = addr;
            addr *= 7;
            p = addr + 6;
            for (i = 0; i < 7; i++) ram[p--] = (byte)alpha[i];
            addr -= 7;
            ram[addr + 6] = (byte)'D';
            ram[addr + 5] = (byte)(len / 256);
            ram[addr + 4] = (byte)(len % 256);
            ram[addr + 3] = 0x00;
            ram[addr + 2] = 0x00;
            addr -= 7;
            for (i=0; i<len; i++)
            {
                for (j = 0; j < 7; j++) ram[addr + j] = 0x00;
                addr -= 7;
            }
            ram[addr + 6] = 0xff;
        }
    }
}
