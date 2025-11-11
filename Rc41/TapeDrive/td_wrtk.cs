using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        void td_wrtk()
        {
            int i;
            int address;
            int rec;
            int recs;
            int fp;
            int p;
            int dirsec;
            int dirofs;
            int size;
            string filename;
            size = 0;
            address = 0x0c0 * 7;
            while (cpu.ram[address + 6] == 0xf0)
            {
                size += 7;
                address += 7;
            }
            if (size == 0)
            {
                cpu.Message("NO KEYS");
                cpu.Error();
                return;
            }
            filename = cpu.GetAlpha();
            if (filename.Length > 7) filename = filename.Substring(0, 7);
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            fp = FindFile(filename);
            if (fp >= 0 && sector[fp] != 'K')
            {
                cpu.Message("DUP FL NAME");
                cpu.Error();
                return;
            }
            if (fp >= 0 && (sector[fp + 9] & 1) != 0)
            {
                cpu.Message("FL SECURED");
                cpu.Error();
                return;
            }
            if (fp > 0)
            {
                dirsec = sectorNumber;
                dirofs = fp;
                rec = sector[fp + 14] << 8 | sector[fp + 15];
                recs = sector[fp + 16] << 8 | sector[fp + 17];
                for (i = 0; i < recs; i++) Deallocate(rec + i);
                recs = (size + 255) / 256;
                rec = FindOpenSectors(recs);
                for (i = 0; i < recs; i++) Allocate(rec + i);
                ReadSector(dirsec);
                sector[dirofs + 10] = (byte)((size + 6) / 7 >> 8 & 0xff);
                sector[dirofs + 11] = (byte)((size + 6) / 7 & 0xff);
                sector[dirofs + 12] = (byte)(size >> 8 & 0xff);
                sector[dirofs + 13] = (byte)(size & 0xff);
                sector[dirofs + 14] = (byte)(rec >> 8 & 0xff);
                sector[dirofs + 15] = (byte)(rec & 0xff);
                sector[dirofs + 16] = (byte)(recs >> 8 & 0xff);
                sector[dirofs + 17] = (byte)(recs & 0xff);
                WriteSector(dirsec);
            }
            else
            {
                rec = CreateFile(filename, size, 'K');
            }
            p = 0;
            address = 0x0c0 * 7;
            while (size > 0)
            {
                sector[p++] = cpu.ram[address++];
                size--;
                if (p == 256)
                {
                    WriteSector(rec);
                    rec++;
                    p = 0;
                }
            }
            if (p > 0) WriteSector(rec);
        }
    }
}
