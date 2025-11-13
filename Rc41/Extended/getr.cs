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
        public void getr()
        {
            int i;
            int addr;
            int len;
            int size;
            Number x;
            string alpha = cpu.GetAlpha();
            size = (cpu.ram[Cpu.REG_C + 2] << 4) + (cpu.ram[Cpu.REG_C + 1] >> 4 & 0x0f);
            size = Cpu.RAMTOP - size;
            if (alpha.Length > 7) alpha = alpha.Substring(0, 7);
            while (alpha.Length < 7) alpha += " ";
            if (alpha.Equals("       "))
            {
                if (currentFile <= 0)
                {
                    cpu.Message("FL NOT FOUND");
                    cpu.Error();
                    return;
                }
                addr = currentFile;
            }
            else
            {
                addr = FindFile(alpha);
                if (addr < 0)
                {
                    cpu.Message("FL NOT FOUND");
                    cpu.Error();
                    return;
                }
                currentFile = addr;
            }
            addr--;
            if (ram[addr * 7 + 6] != 'D')
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            len = ram[addr * 7 + 5] * 256 + ram[addr * 7 + 4];
            if (len < size) size = len;
            addr--;
            for (i = 0; i < size; i++)
            {
                x = ReadRegister(addr);
                cpu.Sto(x, i);
                addr--;
            }
            addr = currentFile - 1;
            ram[addr * 7 + 3] = (byte)((size >> 8) & 0xff);
            ram[addr * 7 + 2] = (byte)(size & 0xff);
        }
    }
}

