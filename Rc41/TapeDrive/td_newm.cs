using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        void td_newm(int size)
        {
            int i;
            int e;
            e = (size + 7) / 8;
            for (i = 0; i < 256; i++) sector[i] = 0;
            for (i = 0; i < e + 2; i++) sector[i] = 0xff;
            WriteSector(0);
            for (i = 0; i < 256; i++) sector[i] = 0;
            WriteSector(1);
            for (i = 0; i < e; i++)
            {
                if (i == e - 1) sector[7 * 32] = 0xff;
                WriteSector(i + 2);
            }
            for (i = 0; i < 256; i++) sector[i] = 0;
            for (i = e + 2; i < 512; i++) WriteSector(i);
        }
    }
}
