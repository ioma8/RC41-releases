using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        int CreateFile(string filename, int size, char typ)
        {
            int i;
            int dir;
            int dirsec;
            int dirofs;
            int rec;
            int recs;
            int regs;
            recs = (size + 255) / 256;
            regs = (size + 6) / 7;
            dir = FindOpenDir();
            if (dir < 0) return -1;
            rec = FindOpenSectors(recs);
            if (rec < 0) return -1;
            dirsec = dir / 8 + 2;
            dirofs = dir % 8 * 32;
            ReadSector(dirsec);
            sector[dirofs] = (byte)typ;
            for (i = 0; i < 7; i++)
                if (i < filename.Length) sector[dirofs + i + 1] = (byte)filename[i];
                else sector[dirofs + i + 1] = 0;
            sector[dirofs + 8] = 0x00;
            sector[dirofs + 9] = 0x00;
            sector[dirofs + 10] = (byte)(regs >> 8 & 0xff);
            sector[dirofs + 11] = (byte)(regs & 0xff);
            sector[dirofs + 12] = (byte)(size >> 8 & 0xff);
            sector[dirofs + 13] = (byte)(size & 0xff);
            sector[dirofs + 14] = (byte)(rec >> 8 & 0xff);
            sector[dirofs + 15] = (byte)(rec & 0xff);
            sector[dirofs + 16] = (byte)(recs >> 8 & 0xff);
            sector[dirofs + 17] = (byte)(recs & 0xff);
            WriteSector(sectorNumber);
            for (i = 0; i < recs; i++) Allocate(rec + i);
            for (i = 0; i < 256; i++) sector[i] = 0;
            for (i = 0; i < recs; i++)
                WriteSector(rec + i);
            return rec;
        }
    }
}
