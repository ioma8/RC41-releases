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
        public void seekpta()
        {
            int addr;
            int pos;
            Number x;
            string alpha = cpu.GetAlpha();
            x = cpu.RecallNumber(Cpu.R_X);
            pos = x.Int();
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
            seekpt();
        }

    }
}
