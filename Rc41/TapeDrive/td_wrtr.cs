using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rc41.T_Cpu;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        void td_wrtr()
        {
            int i;
            int rec;
            int regs;
            int fp;
            int p;
            int adr;
            int addr;
            string filename;
            filename = cpu.GetAlpha();
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            if (filename.Length > 7) filename = filename.Substring(0, 7);
            fp = FindFile(filename);
            if (fp >= 0 && sector[fp] != 'D')
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
            if (fp < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            rec = sector[fp + 14] << 8 | sector[fp + 15];
            regs = sector[fp + 10] << 8 | sector[fp + 11];
            ReadSector(rec);
            p = 0;
            adr = cpu.ram[Cpu.REG_C + 2] << 4 | cpu.ram[Cpu.REG_C + 1] >> 4 & 0x0f;
            while (adr < 0x200)
            {
                addr = adr * 7;
                if (regs == 0)
                {
                    if (p != 0) WriteSector(rec);
                    cpu.Message("END OF FILE");
                    cpu.Error();
                    return;
                }
                for (i = 6; i >= 0; i--)
                {
                    sector[p++] = cpu.ram[addr + i];
                    if (p == 256)
                    {
                        WriteSector(rec);
                        rec++;
                        p = 0;
                    }
                }
                adr++;
                regs--;
            }
            if (p != 0) WriteSector(rec);
        }
    }
}
