using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        void td_purge()
        {
            int i;
            int fp;
            int rec;
            int recs;
            string filename;
            filename = cpu.GetAlpha();
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
            if ((sector[fp + 9] & 1) != 0)
            {
                cpu.Message("FL SECURED");
                cpu.Error();
                return;
            }
            rec = sector[fp + 14] << 8 | sector[fp + 15];
            recs = sector[fp + 16] << 8 | sector[fp + 17];
            for (i = 0; i < 32; i++) sector[fp + i] = 0;
            WriteSector(sectorNumber);
            for (i = 0; i < 256; i++) sector[i] = 0;
            for (i = 0; i < recs; i++) WriteSector(rec + i);
            for (i = 0; i < recs; i++) Deallocate(rec + i);
        }
    }
}
