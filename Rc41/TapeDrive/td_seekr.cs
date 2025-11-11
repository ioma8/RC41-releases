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
        void td_seekr()
        {
            int fp;
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
            fp = FindFile(filename);
            if (fp < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            if (sector[fp] != 'D')
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            x = cpu.RecallNumber(Cpu.R_X);
            size = cpu.ToInteger(x);
            file_rec = sector[fp + 14] << 8 | sector[fp + 15];
            file_regs = sector[fp + 10] << 8 | sector[fp + 11];
            if (size >= file_regs)
            {
                file_rec = -1;
                file_regs = -1;
                file_flags = -1;
                file_reg = -1;
                cpu.Message("END OF FILE");
                cpu.Error();
                return;
            }
            file_flags = sector[fp + 9];
            file_reg = size;
            size *= 7;
            file_rec += size / 256;
            file_pos = size & 0xff;
        }
    }
}
