using Rc41.T_Cpu;
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
        public void savep()
        {
            int i;
            int p;
            string progname;
            string filename;
            int start, end;
            int size, regs;
            int addr;
            string alpha = cpu.GetAlpha();
            if (alpha.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            if (alpha.IndexOf(',') >= 0)
            {
                progname = alpha.Substring(0,alpha.IndexOf(","));
                filename = alpha.Substring(alpha.IndexOf(",")+1);
            }
            else
            {
                progname = alpha;
                filename = alpha;
            }
            if (filename.Length > 7) filename = filename.Substring(0, 7);
            while (filename.Length < 7) filename += " ";
            if (progname.Length > 0)
            {
                start = cpu.FindGlobal(progname);
                if (start <= 0)
                {
                    cpu.Message("NAME ERR");
                    cpu.Error();
                    return;
                }
                end = cpu.FindEnd(start);
                start = cpu.FindStart(end);
                end -= 2;
            }
            else
            {
                addr = (cpu.ram[Cpu.REG_B+1] << 8) | cpu.ram[Cpu.REG_B + 0];
                addr = cpu.FromPtr(addr);
                end = cpu.FindEnd(addr);
                start = cpu.FindStart(end);
                end -= 2;
            }
            size = start - end + 1;
            regs = (size + 6) / 7;

            addr = FindFile(filename);
            if (addr >= 0) purfl(filename);

            addr = FindEnd();
            if (addr - (regs + 3) < 0)
            {
                cpu.Message("NO ROOM");
                cpu.Error();
                return;
            }
            addr *= 7;
            p = addr + 6;
            for (i = 0; i < 7; i++) ram[p--] = (byte)filename[i];
            addr -= 7;
            ram[addr + 6] = (byte)'P';
            ram[addr + 5] = (byte)(regs / 256);
            ram[addr + 4] = (byte)(regs % 256);
            ram[addr + 3] = (byte)(size / 256);
            ram[addr + 2] = (byte)(size % 256);
            addr--;
            p = addr - (regs * 7);
            ram[p] = 0xff;
            while (start >= end)
            {
                ram[addr--] = cpu.ram[start--];
            }

        }
    }
}
