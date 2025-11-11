using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        void Deallocate(int l)
        {
            int sec;
            int ofs;
            sec = l / 256;
            ofs = l & 0xff;
            if (written) WriteSector(sectorNumber);
            ReadSector(sec);
            sector[ofs] = 0x00;
            WriteSector(sec);
        }
    }
}
