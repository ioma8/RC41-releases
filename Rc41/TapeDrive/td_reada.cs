using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        void td_reada()
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
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            if (fp < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            rec = sector[fp + 14] << 8 | sector[fp + 15];
            p = 0;
            ReadSector(rec);
            for (i = 0; i < 16 * 7; i++) cpu.ram[i] = sector[p++];
            for (i = 0x0c0 * 7; i < 0x200 * 7; i++)
            {
                cpu.ram[i] = sector[p++];
                if (p == 256)
                {
                    rec++;
                    ReadSector(rec);
                    p = 0;
                }
            }
        }
    }
}
