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
        void td_reads()
        {
            int i;
            int rec;
            int fp;
            int r00_a;
            int r00_b;
            string filename;
            filename = cpu.GetAlpha();
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            if (filename.Length > 7) filename = filename.Substring(7);
            fp = FindFile(filename);
            if (fp >= 0 && sector[fp] != 'S')
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
            ReadSector(rec);
            r00_a = cpu.ram[Cpu.REG_C + 2] << 4 | cpu.ram[Cpu.REG_C + 1] >> 4 & 0x0f;
            r00_b = sector[Cpu.REG_C + 2] << 4 | sector[Cpu.REG_C + 1] >> 4 & 0x0f;
            for (i = 0; i < Cpu.REG_P + 3; i++) cpu.ram[i] = sector[i];
            for (i = 2; i <= 6; i++) cpu.ram[Cpu.REG_D + i] = sector[Cpu.REG_D + i];
            cpu.ram[Cpu.REG_D + 1] &= 0x0f;
            cpu.ram[Cpu.REG_D + 1] |= (byte)(sector[Cpu.REG_D + 1] & 0xf0);
            cpu.ram[Cpu.REG_C + 6] = sector[Cpu.REG_C + 6];
            cpu.ram[Cpu.REG_C + 5] = (byte)(cpu.ram[Cpu.REG_C + 5] & 0x0f | sector[Cpu.REG_C + 5] & 0xf0);
            cpu.Resize(r00_a, r00_b);
        }
    }
}
