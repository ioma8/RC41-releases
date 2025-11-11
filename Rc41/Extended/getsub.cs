using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public void getsub()
        {
            int addr;
            List<byte> program;
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
            if (ram[addr * 7 - 1] != 'P')
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            program = readProgram(addr);
            if (program != null)
            {
                cpu.LoadSub(program);
            }
        }
    }
}
