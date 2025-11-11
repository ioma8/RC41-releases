using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        int FindFile(string filename)
        {
            int s;
            int p;
            int d;
            int i;
            bool flag;
            string fname;
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
                    if (sector[p] != 0x00)
                    {
                        fname = "";
                        i = 1;
                        while (sector[p + i] != 0)
                        {
                            fname += (char)sector[p + i];
                            i++;
                        }
                        if (filename.Equals(fname)) return p;
                    }
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
