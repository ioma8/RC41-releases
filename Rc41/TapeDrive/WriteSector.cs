using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive;

public partial class TapeDrive
{
    void WriteSector(int s)
    {
        if (s >= 0 && s < 512)
        {
            sectorNumber = s;
            tapefile.Seek(s * 256, SeekOrigin.Begin);
            tapefile.Write(sector);
            written = false;
        }
    }
}
