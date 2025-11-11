using Rc41.T_Cpu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        public void SaveData(string filename, List<byte>data)
        {
            int i;
            int p;
            int fp;
            int size;
            int rec;
            int count;
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
            if ((sector[fp + 9] & 1) != 0)
            {
                cpu.Message("FL SECURED");
                cpu.Error();
                return;
            }
            rec = sector[fp + 14] << 8 | sector[fp + 15];
            size = sector[fp + 10] << 8 | sector[fp + 11];
            size *= 7;
            ReadSector(rec);
                        
            p = 0;
            count = 0;
            for (i=0; i<data.Count; i++)
            {
                sector[p++] = data[i];
                if (p == 256)
                {
                    WriteSector(rec);
                    rec++;
                    p = 0;
                }
                count++;
                if (count > size)
                {
                    if (p > 0) WriteSector(rec);
                    cpu.Message("END OF FL");
                    cpu.Error();
                    return;
                }
            }
            if (p > 0) WriteSector(rec);

        }
    }
}
