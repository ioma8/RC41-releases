using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        void td_wrta()
        {
            int i;
            int fp;
            int rec;
            int p;
            string filename;
            filename = cpu.GetAlpha();
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            fp = FindFile(filename);
            if (fp >= 0 && sector[fp] != 'A')
            {
                cpu.Message("DUP FL NAME");
                cpu.Error();
                return;
            }
            if (fp >= 0 && (sector[fp + 9] & 1) != 0)
            {
                cpu.Message("FL SECURED");
                cpu.Error();
                return;
            }
            if (fp >= 0)
            {
                rec = sector[fp + 14] << 8 | sector[fp + 15];
            }
            else
            {
                rec = CreateFile(filename, 2352, 'A');
                if (rec < 0)
                {
                    return;
                }
            }
            p = 0;
            for (i = 0; i < 16 * 7; i++) sector[p++] = cpu.ram[i];
            for (i = 0x0c0 * 7; i < 0x200 * 7; i++)
            {
                sector[p++] = cpu.ram[i];
                if (p == 256)
                {
                    WriteSector(rec);
                    rec++;
                    p = 0;
                }
            }
            if (p != 0) WriteSector(rec);
        }
    }
}
