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
        void td_wrtrx()
        {
            int i;
            int adr;
            int r00;
            int size;
            Number x;
            int b, e;
            if (file_rec < 0)
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            if ((file_flags & 1) != 0)
            {
                cpu.Message("FL SECURED");
                cpu.Error();
                return;
            }
            x = cpu.RecallNumber(Cpu.R_X);
            b = 0;
            e = 0;
            cpu.GetBE(x, ref b, ref e);
            if (e < b) e = b;
            r00 = cpu.ram[Cpu.REG_C + 2] << 4 | cpu.ram[Cpu.REG_C + 1] >> 4 & 0x0f;
            size = 0x200 - r00;
            if (b >= size || e >= size)
            {
                cpu.Message("NONEXISTENT");
                cpu.Error();
                return;
            }
            adr = (b + r00) * 7;
            ReadSector(file_rec);
            while (b <= e)
            {
                if (file_reg >= file_regs)
                {
                    if (file_pos != 0) WriteSector(file_rec);
                    cpu.Message("END OF FILE");
                    cpu.Error();
                    return;
                }
                for (i = 6; i >= 0; i--)
                {
                    sector[file_pos++] = cpu.ram[adr + i];
                    if (file_pos == 256)
                    {
                        WriteSector(file_rec);
                        file_rec++;
                        file_pos = 0;
                        ReadSector(file_rec);
                    }
                }
                adr += 7;
                b++;
                file_reg++;
            }
            if (file_pos != 0) WriteSector(file_rec);
        }
    }
}
