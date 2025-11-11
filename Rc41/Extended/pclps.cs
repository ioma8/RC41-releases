using Rc41.T_Cpu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public void pclps()
        {
            int i;
            int start;
            int end;
            int prg;
            string alpha = cpu.GetAlpha();
            end = (((cpu.ram[Cpu.REG_C + 1] & 0x0f) << 8) | cpu.ram[Cpu.REG_C + 0]) * 7 + 3;
            prg = cpu.FromPtr((cpu.ram[Cpu.REG_B + 1] << 8) | cpu.ram[Cpu.REG_B + 0]);
            if (alpha.Length > 0)
            {
                start = cpu.FindGlobal(alpha);
                if (start <= 0)
                {
                    cpu.Message("NAME ERR");
                    cpu.Error();
                    return;
                }
                start = cpu.FindStart(start);
            }
            else
            {
                start = (cpu.ram[Cpu.REG_B + 1] << 8) | cpu.ram[Cpu.REG_B + 0];
                start = cpu.FromPtr(start);
                start = cpu.FindEnd(start);
                start = cpu.FindStart(start);
            }
            for (i = end; i <= start; i++) cpu.ram[i] = 0x00;
            cpu.ReLink();
            cpu.Pack();
            if (prg <= start)
            {
                end = (((cpu.ram[Cpu.REG_C + 1] & 0x0f) << 8) | cpu.ram[Cpu.REG_C + 0]) * 7 + 3;
                prg = cpu.ToPtr(end);
                cpu.ram[Cpu.REG_B + 1] = (byte)((prg >> 8) & 0xff);
                cpu.ram[Cpu.REG_B + 0] = (byte)((prg & 0xff));
                cpu.ram[Cpu.REG_E + 1] |= 0x0f;
                cpu.ram[Cpu.REG_E + 0] = 0xff;
            }
        }
    }
}
