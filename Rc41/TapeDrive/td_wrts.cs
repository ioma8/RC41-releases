using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        void td_wrts()
        {
            int i;
            int fp;
            int rec;
            string filename;
            filename = cpu.GetAlpha();
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            fp = FindFile(filename);
            if (fp >= 0 && sector[fp] != 'S')
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
                ReadSector(rec);
                for (i = 0; i < 112; i++) sector[i] = cpu.ram[i];
                WriteSector(rec);
            }
            else
            {
                rec = CreateFile(filename, 112, 'S');
                if (rec > 0)
                {
                    for (i = 0; i < 112; i++) sector[i] = cpu.ram[i];
                    WriteSector(rec);
                }
            }
        }
    }
}
