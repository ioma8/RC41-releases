using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        public List<byte>? LoadData(string filename)
        {
            int p;
            int fp;
            int size;
            int rec;
            int count;
            byte b;
            List<byte> data;
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return null;
            }
            fp = FindFile(filename);
            if (fp < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return null;
            }
            if (sector[fp] != 'D')
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return null;
            }
            if ((sector[fp + 9] & 1) != 0)
            {
                cpu.Message("FL SECURED");
                cpu.Error();
                return null;
            }
            rec = sector[fp + 14] << 8 | sector[fp + 15];
            size = sector[fp + 10] << 8 | sector[fp + 11];
            size *= 7;
            ReadSector(rec);

            b = 0;
            p = 0;
            count = 0;
            data = new List<byte>();
            while (b != 0xff)
            {
                b = sector[p++];
                data.Add(b);
                if (p == 256)
                {
                    rec++;
                    ReadSector(rec);
                    p = 0;
                }
                count++;
                if (count >= size)
                {
                    b = 0xff;
                    data.Add(b);
                }
            }
            return data;
        }
    }
}
