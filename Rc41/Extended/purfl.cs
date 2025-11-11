using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public void purfl()
        {
            string alpha = cpu.GetAlpha();
            purfl(alpha);
        }

        public void purfl(string alpha)
        {
            int i;
            int addr;
            int len;
            int src;
            int dst;
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
            len = ram[addr * 7 + 5] * 256 + ram[addr * 7 + 4];
            len = (len * 7) + 14;
            addr++;
            dst = addr * 7 + 6;
            src = dst - len;
            while (src >= 0) ram[dst--] = ram[src--];
        }


    }
}
