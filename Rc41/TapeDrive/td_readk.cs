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
        void td_readk()
        {
            int i;
            int address;
            int rec;
            int fp;
            int p;
            int regs;
            int size;
            int end;
            string filename;
            size = 0;
            address = 0x0c0 * 7;
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
                cpu.Message("FL TYPE ERR");
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
            end = (cpu.ram[Cpu.REG_C + 1] & 0x0f) << 8 | cpu.ram[Cpu.REG_C + 0];
            if (0x0c0 + regs > end)
            {
                cpu.Message("NO ROOM");
                cpu.Error();
                return;
            }
            address = 0x0c0 * 7;
            while (cpu.ram[address + 6] == 0xf0)
            {
                for (i = 0; i <= 6; i++) cpu.ram[address + i] = 0x00;
                address += 7;
            }
            p = 0;
            address = 0x0c0 * 7;
            ReadSector(rec);
            size = regs * 7;
            while (size > 0)
            {
                cpu.ram[address++] = sector[p++];
                size--;
                if (p == 256)
                {
                    rec++;
                    ReadSector(rec);
                    p = 0;
                }
            }
            cpu.SetKaFlags();
        }
    }
}
