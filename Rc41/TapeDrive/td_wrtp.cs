using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        void td_wrtp(int priv)
        {
            int i;
            int address;
            int end;
            int len;
            int rec;
            int recs;
            int fp;
            int p;
            int dirsec;
            int dirofs;
            int comma;
            string filename;
            string progname;
            progname = cpu.GetAlpha();
            if (progname.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            comma = progname.IndexOf(',');
            if (comma >= 0)
            {
                //                if (comma == progname)
                //                {
                //                    printf("Need to code for ',filename'\n");
                //                    return;
                //                }
                //                else
                //                {
                filename = progname.Substring(comma + 1);
                progname = progname.Substring(0, comma);
                //                }
            }
            else
            {
                filename = progname;
            }
            if (filename.Length > 7) filename = filename.Substring(0, 7);
            if (filename.Length == 0 || progname.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            address = cpu.FindGlobal(progname);
            if (address == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            address = cpu.FindStart(address);
            end = cpu.FindEnd(address);
            end -= 2;
            len = address - end + 1;
            fp = FindFile(filename);
            if (fp >= 0 && sector[fp] != 'P')
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
                rec = FindOpenSectors(recs);
                for (i = 0; i < recs; i++) Allocate(rec + i);
                ReadSector(dirsec);
                sector[dirofs + 10] = (byte)((len + 6) / 7 >> 8 & 0xff);
                sector[dirofs + 11] = (byte)((len + 6) / 7 & 0xff);
                sector[dirofs + 12] = (byte)(len >> 8 & 0xff);
                sector[dirofs + 13] = (byte)(len & 0xff);
                sector[dirofs + 14] = (byte)(rec >> 8 & 0xff);
                sector[dirofs + 15] = (byte)(rec & 0xff);
                sector[dirofs + 16] = (byte)(recs >> 8 & 0xff);
                sector[dirofs + 17] = (byte)(recs & 0xff);
                WriteSector(dirsec);
            }
            else
            {
                rec = CreateFile(filename, len, 'P');
            }
            p = 0;
            while (len > 0)
            {
                sector[p++] = cpu.ram[address--];
                len--;
                if (p == 256)
                {
                    WriteSector(rec);
                    rec++;
                    p = 0;
                }
            }
            if (p > 0) WriteSector(rec);
            fp = FindFile(filename);
            if (fp >= 0)
            {
                if (priv != 0) sector[fp + 9] |= 0x02;
                else sector[fp + 9] &= 0xfd;
                WriteSector(sectorNumber);
            }

        }
    }
}
