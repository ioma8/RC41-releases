using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive;

public partial class TapeDrive
{
    void ReadSector(int s)
    {
        if (s >= 0 && s < 512)
        {
            sectorNumber = s;
            tapefile!.Seek(s * 256, SeekOrigin.Begin);
#pragma warning disable CA2022 // Partial reads are acceptable for newly created tape files
            tapefile.Read(sector);
#pragma warning restore CA2022
            written = false;
        }
    }
}
