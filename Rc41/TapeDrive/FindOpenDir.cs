using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        int FindOpenDir()
        {
            int s;
            int p;
            int d;
            bool flag;
            if (written) WriteSector(sectorNumber);
            d = 0;
            p = 0;
            s = 2;
            flag = true;
            while (flag)
            {
                ReadSector(s);
                while (p < 256)
                {
                    if (sector[p] == 0x00) return d;
                    if (sector[p] == 0xff) flag = false;
                    p += 32;
                    d++;
                }
                p = 0;
                s++;
            }
            return -1;
        }
    }
}
