using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        int FindOpenSectors(int count)
        {
            int s;
            int p;
            int d;
            int c;
            bool flag;
            if (written) WriteSector(sectorNumber);
            d = 0;
            p = 0;
            s = 0;
            c = 0;
            flag = true;
            while (flag)
            {
                ReadSector(s);
                while (p < 256)
                {
                    if (sector[p] == 0x00) c++;
                    else
                    {
                        d = s * 256 + p + 1;
                    }
                    if (c >= count) return d;
                    p++;
                }
                p = 0;
                s++;
                if (s == 2) flag = false;
            }
            return -1;
        }
    }
}
