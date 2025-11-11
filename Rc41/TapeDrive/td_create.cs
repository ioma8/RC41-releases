using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rc41.T_Cpu;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        void td_create()
        {
            string filename;
            int size;
            Number x;
            filename = cpu.GetAlpha();
            if (filename.Length > 7) filename = filename.Substring(0, 7);
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            if (FindFile(filename) >= 0)
            {
                cpu.Message("DUP FL NAME");
                cpu.Error();
                return;
            }
            x = cpu.RecallNumber(Cpu.R_X);
            size = cpu.ToInteger(x);
            CreateFile(filename, size * 7, 'D');
        }
    }
}
